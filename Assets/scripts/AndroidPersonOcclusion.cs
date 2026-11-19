#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

internal sealed class AndroidPersonOcclusion : IDisposable
{
    private const float FrameInterval = 0.083f;
    private const float MaskLifetime = 0.35f;
    private const float RequestTimeout = 2f;
    private static readonly int MaskId = Shader.PropertyToID("_AndroidPersonMask");
    private static readonly int DisplayTransformId = Shader.PropertyToID("_AndroidPersonDisplayTransform");
    private static readonly int CropScaleId = Shader.PropertyToID("_AndroidPersonCropScale");
    private static readonly int RotationId = Shader.PropertyToID("_AndroidPersonMaskRotation");
    private static readonly int DepthId = Shader.PropertyToID("_AndroidPersonOccluderDepth");

    private readonly ARCameraManager cameraManager;
    private readonly Camera camera;
    private readonly Canvas captureCanvas;
    private readonly Material material;
    private readonly Mesh fullscreenTriangle;

    private AndroidJavaObject segmenter;
    private NativeArray<sbyte> convertedImage;
    private sbyte[] inputBytes;
    private Texture2D mask;
    private Matrix4x4 displayTransform;
    private Vector2Int previewSize;
    private Vector2 cropScale;
    private float nextFrameTime;
    private float requestTime;
    private float maskTime;
    private float originalCanvasDistance;
    private int geometryVersion;
    private int requestVersion;
    private int requestRotation;
    private bool hasGeometry;
    private bool pending;
    private bool maskReady;
    private bool canvasMoved;
    private bool cpuImageUnavailableLogged;
    private bool failureLogged;

    public AndroidPersonOcclusion(ARCameraManager cameraManager, Shader shader, Canvas captureCanvas)
    {
        this.cameraManager = cameraManager;
        this.captureCanvas = captureCanvas;
        camera = cameraManager.GetComponent<Camera>();
        material = new Material(shader) { hideFlags = HideFlags.HideAndDontSave };
        fullscreenTriangle = new Mesh
        {
            name = "Android person occlusion triangle",
            hideFlags = HideFlags.HideAndDontSave,
            vertices = new[] { Vector3.zero, new Vector3(2, 0, 0), new Vector3(0, 2, 0) },
            triangles = new[] { 0, 1, 2 }
        };
        fullscreenTriangle.UploadMeshData(true);
    }

    public void OnCameraFrame(ARCameraFrameEventArgs args)
    {
        if (!args.displayMatrix.HasValue || args.textures.Count == 0 || cameraManager.imageStabilizationEnabled)
        {
            InvalidateGeometry();
            return;
        }

        Matrix4x4 transform = args.displayMatrix.Value;
        Texture preview = args.textures[0];
        if (preview == null || preview.width <= 0 || preview.height <= 0)
        {
            InvalidateGeometry();
            return;
        }
        var size = new Vector2Int(preview.width, preview.height);
        if (!hasGeometry || size != previewSize || transform != displayTransform)
        {
            InvalidateGeometry();
            displayTransform = transform;
            previewSize = size;
            hasGeometry = true;
        }

        float now = Time.unscaledTime;
        if (pending || now < nextFrameTime)
        {
            return;
        }

        nextFrameTime = now + FrameInterval;
        if (!cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
        {
            if (!cpuImageUnavailableLogged)
            {
                Debug.LogWarning("Android person occlusion is waiting for ARCore front-camera CPU images.", cameraManager);
                cpuImageUnavailableLogged = true;
            }
            return;
        }

        try
        {
            Submit(image, now);
        }
        catch (Exception exception)
        {
            HandleFailure(exception.Message);
            CloseSegmenter();
        }
        finally
        {
            image.Dispose();
        }
    }

    private void Submit(XRCpuImage image, float now)
    {
        float scale = Mathf.Min(1f, 256f / Mathf.Min(image.width, image.height));
        var dimensions = new Vector2Int(Mathf.RoundToInt(image.width * scale), Mathf.RoundToInt(image.height * scale));
        var conversion = new XRCpuImage.ConversionParams
        {
            inputRect = new RectInt(0, 0, image.width, image.height),
            outputDimensions = dimensions,
            outputFormat = TextureFormat.RGBA32,
            transformation = XRCpuImage.Transformation.None
        };
        int bufferSize = image.GetConvertedDataSize(conversion);
        if (!convertedImage.IsCreated || convertedImage.Length != bufferSize)
        {
            if (convertedImage.IsCreated)
            {
                convertedImage.Dispose();
            }
            convertedImage = new NativeArray<sbyte>(bufferSize, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            inputBytes = new sbyte[bufferSize];
        }
        image.Convert(conversion, new NativeSlice<byte>(convertedImage.Reinterpret<byte>()));
        convertedImage.CopyTo(inputBytes);

        // The display matrix maps preview UVs to sensor axes. Its up vector gives the input rotation without the selfie mirror.
        float upX = displayTransform.m10;
        float upY = displayTransform.m11;
        requestRotation = Mathf.Abs(upX) > Mathf.Abs(upY) ? (upX > 0f ? 270 : 90) : (upY > 0f ? 180 : 0);

        float aspectRatio = previewSize.x * (float)image.height / (previewSize.y * (float)image.width);
        cropScale = new Vector2(Mathf.Min(1f, aspectRatio), Mathf.Min(1f, 1f / aspectRatio));
        segmenter ??= new AndroidJavaObject("com.blingar.PersonSegmenter");
        segmenter.Call("submit", inputBytes, dimensions.x, dimensions.y, requestRotation);
        requestVersion = geometryVersion;
        requestTime = now;
        pending = true;
    }

    public void Render()
    {
        float now = Time.unscaledTime;
        if (pending)
        {
            try
            {
                ReadResult(now);
            }
            catch (Exception exception)
            {
                HandleFailure(exception.Message);
                CloseSegmenter();
            }
        }

        if (!maskReady || now - maskTime > MaskLifetime || !camera.isActiveAndEnabled ||
            cameraManager.currentRenderingMode != XRCameraBackgroundRenderingMode.BeforeOpaques)
        {
            RestoreCanvas();
            return;
        }

        float depth = camera.nearClipPlane + 0.04f;
        // Keep the captured HUD on the same camera and in front of the synthetic person plane.
        if (captureCanvas != null && captureCanvas.renderMode == RenderMode.ScreenSpaceCamera &&
            captureCanvas.worldCamera == camera)
        {
            if (!canvasMoved)
            {
                originalCanvasDistance = captureCanvas.planeDistance;
                canvasMoved = true;
            }
            captureCanvas.planeDistance = Mathf.Min(originalCanvasDistance, camera.nearClipPlane + 0.02f);
        }

        material.SetFloat(DepthId, depth);
        var renderParams = new RenderParams(material)
        {
            camera = camera,
            layer = camera.gameObject.layer,
            shadowCastingMode = ShadowCastingMode.Off,
            receiveShadows = false,
            lightProbeUsage = LightProbeUsage.Off,
            worldBounds = new Bounds(camera.transform.position + camera.transform.forward * depth, Vector3.one)
        };
        Graphics.RenderMesh(renderParams, fullscreenTriangle, 0, Matrix4x4.identity);
    }

    private void ReadResult(float now)
    {
        using AndroidJavaObject result = segmenter.Call<AndroidJavaObject>("takeResult");
        if (result == null)
        {
            if (now - requestTime > RequestTimeout)
            {
                HandleFailure("ML Kit did not complete the frame.");
                CloseSegmenter();
            }
            return;
        }

        pending = false;
        string error = result.Get<string>("error");
        if (!string.IsNullOrEmpty(error))
        {
            HandleFailure(error);
            return;
        }
        if (requestVersion != geometryVersion || now - requestTime > MaskLifetime)
        {
            return;
        }

        int width = result.Get<int>("width");
        int height = result.Get<int>("height");
        sbyte[] confidence = result.Get<sbyte[]>("confidence");
        if (width <= 0 || height <= 0 || confidence == null || confidence.Length != width * height)
        {
            HandleFailure("ML Kit returned an invalid confidence mask.");
            return;
        }

        if (mask == null || mask.width != width || mask.height != height)
        {
            UnityEngine.Object.Destroy(mask);
            mask = new Texture2D(width, height, TextureFormat.R8, false, true)
            {
                name = "Android person confidence",
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Bilinear
            };
        }
        mask.SetPixelData(confidence, 0);
        mask.Apply(false);
        material.SetTexture(MaskId, mask);
        material.SetMatrix(DisplayTransformId, displayTransform);
        material.SetVector(CropScaleId, new Vector4(cropScale.x, cropScale.y, 0f, 0f));
        material.SetInt(RotationId, requestRotation);
        maskTime = requestTime;
        maskReady = true;
    }

    private void InvalidateGeometry()
    {
        hasGeometry = false;
        maskReady = false;
        geometryVersion++;
    }

    private void HandleFailure(string message)
    {
        maskReady = false;
        nextFrameTime = Time.unscaledTime + 1f;
        if (!failureLogged)
        {
            Debug.LogWarning($"Android person occlusion unavailable: {message}", cameraManager);
            failureLogged = true;
        }
    }

    private void RestoreCanvas()
    {
        if (canvasMoved && captureCanvas != null)
        {
            captureCanvas.planeDistance = originalCanvasDistance;
        }
        canvasMoved = false;
    }

    private void CloseSegmenter()
    {
        pending = false;
        if (segmenter == null)
        {
            return;
        }

        try
        {
            segmenter.Call("close");
        }
        catch (AndroidJavaException exception)
        {
            HandleFailure(exception.Message);
        }
        finally
        {
            segmenter.Dispose();
            segmenter = null;
        }
    }

    public void Dispose()
    {
        RestoreCanvas();
        material.SetTexture(MaskId, null);
        UnityEngine.Object.Destroy(mask);
        CloseSegmenter();
        if (convertedImage.IsCreated)
        {
            convertedImage.Dispose();
        }
        UnityEngine.Object.Destroy(material);
        UnityEngine.Object.Destroy(fullscreenTriangle);
    }
}
#endif
