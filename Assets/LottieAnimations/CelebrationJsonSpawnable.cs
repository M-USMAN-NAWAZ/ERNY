using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class CelebrationJsonSpawnable : MonoBehaviour
{
    private const string VisualObjectName = "Celebrations Begin Animation";

    private class VisualInstance
    {
        public Mesh Mesh;
        public MeshRenderer MeshRenderer;
        public RawImage RawImage;
        public int CurrentFrame = -1;
        public bool IsUi;
    }

    public TextAsset lottieJson;
    public Texture2D spriteSheet;
    public int frameCount = 121;
    public int columns = 11;
    public int rows = 11;
    public float framesPerSecond = 30f;
    public float displayScale = 1.25f;
    public bool loop = true;
    public bool faceCamera = true;
    public bool staggerChildren = true;
    public bool skipPlaneDetection = true;
    public bool renderOnCanvas = false;
    public bool hideWorldRenderersOnCanvas = true;
    public bool hideOtherPrefabRenderers = true;
    public string[] rendererRootNamesToHide =
    {
        "Ballon3",
        "Ballon3 (1)",
        "Ballon3 (2)",
        "Ballloons",
        "Balloon_2 1 1"
    };
    public Vector2 canvasSize = new Vector2(1400f, 1400f);
    public Vector2 canvasAnchoredPosition = Vector2.zero;
    public bool followCamera = true;
    public bool parentToCamera = false;
    public float cameraFollowDistance = 1.5f;
    public Vector3 cameraFollowOffset = Vector3.zero;

    [Header("Color Override")]
    public bool useHexColorOverride = false;
    [Tooltip("Original JSON colors to replace. Leave these as-is unless the source artwork changes.")]
    public string[] sourceHexColors =
    {
        "#FFC343",
        "#4DE1FF",
        "#77DAEE",
        "#FFE600",
        "#307799",
        "#E6A210",
        "#D0A64E",
        "#FECB58"
    };
    [Tooltip("New colors, matched by index with Source Hex Colors. Example: #FF0000")]
    public string[] replacementHexColors =
    {
        "#FFC343",
        "#4DE1FF",
        "#77DAEE",
        "#FFE600",
        "#307799",
        "#E6A210",
        "#D0A64E",
        "#FECB58"
    };
    [Range(0.01f, 0.5f)] public float colorMatchTolerance = 0.12f;
    public bool clearUnmatchedPixels = true;

    private readonly List<VisualInstance> visuals = new List<VisualInstance>();
    private Material runtimeMaterial;
    private double editorStartTime;
    private float playStartTime;
    private int lastFrameCount = -1;
    private int lastColumns = -1;
    private int lastRows = -1;
    private float lastDisplayScale = -1f;
    private Texture2D lastSpriteSheet;
    private int lastVisualCount = -1;
    private Transform originalParent;
    private Transform attachedCamera;
    private Texture2D recoloredSpriteSheet;
    private string generatedRecolorKey;
    private string lastVisualRecolorKey;

    private void Awake()
    {
        originalParent = transform.parent;
        BuildVisuals();
        RestartPlayback();
    }

    private void OnEnable()
    {
        if (originalParent == null && transform.parent != attachedCamera)
        {
            originalParent = transform.parent;
        }

        BuildVisuals();
        RestartPlayback();
        SetFrames(0);
#if UNITY_EDITOR
        EditorApplication.update += EditorTick;
#endif
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        EditorApplication.update -= EditorTick;
#endif
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        EditorApplication.delayCall += RebuildEditorPreview;
#endif
    }

#if UNITY_EDITOR
    private void RebuildEditorPreview()
    {
        if (this == null || Application.isPlaying)
        {
            return;
        }

        BuildVisuals();
        RestartPlayback();
        SetFrames(0);
    }

    private void EditorTick()
    {
        if (this == null || Application.isPlaying || !isActiveAndEnabled)
        {
            return;
        }

        UpdatePlayback();
        SceneView.RepaintAll();
    }
#endif

    private void Update()
    {
        if (Application.isPlaying)
        {
            UpdatePlayback();
        }
    }

    private void OnDestroy()
    {
        if (runtimeMaterial != null)
        {
            if (Application.isPlaying)
            {
                Destroy(runtimeMaterial);
            }
            else
            {
                DestroyImmediate(runtimeMaterial);
            }
        }

        DestroyRecoloredSpriteSheet();

        for (int i = 0; i < visuals.Count; i++)
        {
            if (visuals[i].Mesh == null)
            {
                continue;
            }

            if (Application.isPlaying)
            {
                Destroy(visuals[i].Mesh);
            }
            else
            {
                DestroyImmediate(visuals[i].Mesh);
            }
        }
    }

    private void RestartPlayback()
    {
        playStartTime = Time.realtimeSinceStartup;
#if UNITY_EDITOR
        editorStartTime = EditorApplication.timeSinceStartup;
#endif
    }

    private void BuildVisuals()
    {
        EnsureAtLeastOneVisualChild();

        List<Transform> visualTransforms = GetVisualChildren();
        bool useCanvasRendering = ShouldRenderOnCanvas();
        ApplyRendererVisibility(visualTransforms, useCanvasRendering);
        string currentRecolorKey = BuildRecolorKey();

        bool shouldRebuildMeshes =
            lastFrameCount != frameCount ||
            lastColumns != columns ||
            lastRows != rows ||
            !Mathf.Approximately(lastDisplayScale, displayScale) ||
            lastSpriteSheet != spriteSheet ||
            lastVisualRecolorKey != currentRecolorKey ||
            lastVisualCount != visualTransforms.Count;

        visuals.Clear();

        for (int i = 0; i < visualTransforms.Count; i++)
        {
            VisualInstance visual = useCanvasRendering
                ? BuildCanvasVisual(visualTransforms[i], i)
                : BuildMeshVisual(visualTransforms[i], shouldRebuildMeshes);
            visuals.Add(visual);
        }

        lastFrameCount = frameCount;
        lastColumns = columns;
        lastRows = rows;
        lastDisplayScale = displayScale;
        lastSpriteSheet = spriteSheet;
        lastVisualRecolorKey = currentRecolorKey;
        lastVisualCount = visualTransforms.Count;
    }

    private void EnsureAtLeastOneVisualChild()
    {
        if (GetVisualChildren().Count > 0)
        {
            return;
        }

        GameObject visualObject = new GameObject(VisualObjectName);
        visualObject.transform.SetParent(transform, false);
        visualObject.transform.localPosition = Vector3.zero;
        visualObject.transform.localRotation = Quaternion.identity;
        visualObject.transform.localScale = Vector3.one;
    }

    private List<Transform> GetVisualChildren()
    {
        List<Transform> result = new List<Transform>();

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.name.StartsWith(VisualObjectName) && !child.name.Contains(" UI"))
            {
                result.Add(child);
            }
        }

        result.Sort((left, right) => string.CompareOrdinal(left.name, right.name));
        return result;
    }

    private VisualInstance BuildMeshVisual(Transform visualTransform, bool forceMeshRebuild)
    {
        MeshFilter meshFilter = visualTransform.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = visualTransform.gameObject.AddComponent<MeshFilter>();
        }

        MeshRenderer meshRenderer = visualTransform.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderer = visualTransform.gameObject.AddComponent<MeshRenderer>();
        }

        Mesh mesh = meshFilter.sharedMesh;
        if (mesh == null || forceMeshRebuild)
        {
            mesh = new Mesh { name = visualTransform.name + " Quad" };
            BuildQuadMesh(mesh);
        }

        meshFilter.sharedMesh = mesh;
        Texture activeSpriteSheet = GetActiveSpriteSheet();
        meshRenderer.sharedMaterial = GetMaterial();
        meshRenderer.enabled = true;

        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetTexture("_MainTex", activeSpriteSheet);
        meshRenderer.SetPropertyBlock(propertyBlock);

        return new VisualInstance
        {
            Mesh = mesh,
            MeshRenderer = meshRenderer,
            IsUi = false
        };
    }

    private VisualInstance BuildCanvasVisual(Transform sourceTransform, int index)
    {
        MeshRenderer sourceRenderer = sourceTransform.GetComponent<MeshRenderer>();
        if (sourceRenderer != null)
        {
            sourceRenderer.enabled = false;
        }

        string uiName = sourceTransform.name + " UI";
        Transform uiTransform = transform.Find(uiName);
        GameObject uiObject;

        if (uiTransform == null)
        {
            uiObject = new GameObject(uiName, typeof(RectTransform), typeof(CanvasRenderer), typeof(RawImage));
            uiObject.transform.SetParent(transform, false);
        }
        else
        {
            uiObject = uiTransform.gameObject;
        }

        RectTransform rectTransform = uiObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = canvasAnchoredPosition;
        rectTransform.sizeDelta = canvasSize;
        rectTransform.localScale = Vector3.one;
        rectTransform.SetSiblingIndex(index);

        RawImage rawImage = uiObject.GetComponent<RawImage>();
        rawImage.texture = GetActiveSpriteSheet();
        rawImage.raycastTarget = false;
        rawImage.color = Color.white;

        return new VisualInstance
        {
            RawImage = rawImage,
            IsUi = true
        };
    }

    private void ApplyRendererVisibility(List<Transform> visualTransforms, bool useCanvasRendering)
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>(true);

        for (int i = 0; i < renderers.Length; i++)
        {
            bool isVisualRenderer = IsUnderVisualTransform(renderers[i].transform, visualTransforms);

            if (isVisualRenderer)
            {
                renderers[i].enabled = !useCanvasRendering || !hideWorldRenderersOnCanvas;
            }
            else if (hideOtherPrefabRenderers && IsUnderHiddenRendererRoot(renderers[i].transform))
            {
                renderers[i].enabled = false;
            }
        }
    }

    private bool IsUnderHiddenRendererRoot(Transform target)
    {
        Transform current = target;

        while (current != null)
        {
            for (int i = 0; i < rendererRootNamesToHide.Length; i++)
            {
                if (current.name == rendererRootNamesToHide[i])
                {
                    return true;
                }
            }

            if (current == transform)
            {
                break;
            }

            current = current.parent;
        }

        return false;
    }

    private bool IsUnderVisualTransform(Transform target, List<Transform> visualTransforms)
    {
        for (int i = 0; i < visualTransforms.Count; i++)
        {
            if (target == visualTransforms[i] || target.IsChildOf(visualTransforms[i]))
            {
                return true;
            }
        }

        return false;
    }

    private void BuildQuadMesh(Mesh mesh)
    {
        float safeScale = Mathf.Max(0.01f, displayScale);
        mesh.Clear();
        mesh.vertices = new[]
        {
            new Vector3(-0.5f * safeScale, -0.5f * safeScale, 0f),
            new Vector3(-0.5f * safeScale, 0.5f * safeScale, 0f),
            new Vector3(0.5f * safeScale, 0.5f * safeScale, 0f),
            new Vector3(0.5f * safeScale, -0.5f * safeScale, 0f)
        };
        mesh.triangles = new[] { 0, 2, 1, 0, 3, 2 };
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

    private Material GetMaterial()
    {
        if (runtimeMaterial != null)
        {
            runtimeMaterial.mainTexture = GetActiveSpriteSheet();
            return runtimeMaterial;
        }

        Shader shader = Shader.Find("Unlit/Transparent");
        if (shader == null)
        {
            shader = Shader.Find("Sprites/Default");
        }

        if (shader == null)
        {
            return null;
        }

        runtimeMaterial = new Material(shader)
        {
            name = "Celebrations Begin Runtime Material",
            mainTexture = GetActiveSpriteSheet()
        };

        return runtimeMaterial;
    }

    private int GetCurrentFrame()
    {
        int safeFrameCount = Mathf.Max(1, frameCount);
        float safeFps = Mathf.Max(1f, framesPerSecond);
        float elapsed;

        if (Application.isPlaying)
        {
            elapsed = Time.realtimeSinceStartup - playStartTime;
        }
        else
        {
#if UNITY_EDITOR
            elapsed = (float)(EditorApplication.timeSinceStartup - editorStartTime);
#else
            elapsed = 0f;
#endif
        }

        int frame = Mathf.FloorToInt(elapsed * safeFps);
        return loop ? frame % safeFrameCount : Mathf.Min(frame, safeFrameCount - 1);
    }

    private void UpdatePlayback()
    {
        if (NeedsVisualRebuild())
        {
            BuildVisuals();
        }

        FollowCameraIfNeeded();
        FaceCameraIfNeeded();
        SetFrames(GetCurrentFrame());
    }

    private bool NeedsVisualRebuild()
    {
        return visuals.Count == 0 ||
               lastFrameCount != frameCount ||
               lastColumns != columns ||
               lastRows != rows ||
               !Mathf.Approximately(lastDisplayScale, displayScale) ||
               lastSpriteSheet != spriteSheet ||
               lastVisualRecolorKey != BuildRecolorKey() ||
               lastVisualCount != CountVisualChildren();
    }

    private int CountVisualChildren()
    {
        int count = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.name.StartsWith(VisualObjectName) && !child.name.Contains(" UI"))
            {
                count++;
            }
        }

        return count;
    }

    private void SetFrames(int baseFrame)
    {
        int safeFrameCount = Mathf.Max(1, frameCount);
        int visualCount = Mathf.Max(1, visuals.Count);

        for (int i = 0; i < visuals.Count; i++)
        {
            int offset = staggerChildren ? Mathf.RoundToInt((safeFrameCount / (float)visualCount) * i) : 0;
            int frame = loop ? (baseFrame + offset) % safeFrameCount : Mathf.Min(baseFrame + offset, safeFrameCount - 1);
            SetFrame(visuals[i], frame);
        }
    }

    private void SetFrame(VisualInstance visual, int frame)
    {
        Texture activeSpriteSheet = GetActiveSpriteSheet();

        if (visual == null || activeSpriteSheet == null)
        {
            return;
        }

        int safeColumns = Mathf.Max(1, columns);
        int safeRows = Mathf.Max(1, rows);
        int safeFrameCount = Mathf.Max(1, frameCount);
        int safeFrame = Mathf.Clamp(frame, 0, safeFrameCount - 1);

        if (safeFrame == visual.CurrentFrame)
        {
            return;
        }

        int column = safeFrame % safeColumns;
        int rowFromTop = safeFrame / safeColumns;

        float width = 1f / safeColumns;
        float height = 1f / safeRows;
        float insetX = 0.5f / activeSpriteSheet.width;
        float insetY = 0.5f / activeSpriteSheet.height;
        float xMin = column * width + insetX;
        float yMin = 1f - ((rowFromTop + 1) * height) + insetY;
        float xMax = xMin + width - (insetX * 2f);
        float yMax = yMin + height - (insetY * 2f);

        if (visual.IsUi)
        {
            if (visual.RawImage == null)
            {
                return;
            }

            visual.RawImage.texture = activeSpriteSheet;
            visual.RawImage.uvRect = new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
        }
        else
        {
            if (visual.Mesh == null)
            {
                return;
            }

            visual.Mesh.uv = new[]
            {
                new Vector2(xMin, yMin),
                new Vector2(xMin, yMax),
                new Vector2(xMax, yMax),
                new Vector2(xMax, yMin)
            };

            if (visual.MeshRenderer != null)
            {
                MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
                visual.MeshRenderer.GetPropertyBlock(propertyBlock);
                propertyBlock.SetTexture("_MainTex", activeSpriteSheet);
                visual.MeshRenderer.SetPropertyBlock(propertyBlock);
            }
        }

        visual.CurrentFrame = safeFrame;
    }

    private Texture GetActiveSpriteSheet()
    {
        if (spriteSheet == null)
        {
            return null;
        }

        if (!useHexColorOverride)
        {
            return spriteSheet;
        }

        string recolorKey = BuildRecolorKey();
        if (recoloredSpriteSheet == null || generatedRecolorKey != recolorKey)
        {
            BuildRecoloredSpriteSheet(recolorKey);
        }

        return recoloredSpriteSheet != null ? recoloredSpriteSheet : spriteSheet;
    }

    private void BuildRecoloredSpriteSheet(string recolorKey)
    {
        Texture2D readableSource = CreateReadableCopy(spriteSheet);
        if (readableSource == null)
        {
            return;
        }

        Color32[] pixels = readableSource.GetPixels32();
        Color[] sourceColors = ParseColorList(sourceHexColors);
        Color[] replacementColors = ParseColorList(replacementHexColors);
        float toleranceSquared = colorMatchTolerance * colorMatchTolerance;

        for (int i = 0; i < pixels.Length; i++)
        {
            Color pixel = pixels[i];
            if (pixel.a <= 0.01f)
            {
                pixels[i] = Color.clear;
                continue;
            }

            int sourceIndex = FindClosestSourceColor(pixel, sourceColors, toleranceSquared);
            if (sourceIndex < 0)
            {
                if (clearUnmatchedPixels)
                {
                    pixels[i] = Color.clear;
                }

                continue;
            }

            Color replacement = sourceIndex < replacementColors.Length ? replacementColors[sourceIndex] : sourceColors[sourceIndex];
            replacement.a = pixel.a;
            pixels[i] = replacement;
        }

        DestroyRecoloredSpriteSheet();

        recoloredSpriteSheet = new Texture2D(readableSource.width, readableSource.height, TextureFormat.RGBA32, false)
        {
            name = spriteSheet.name + " Recolored"
        };
        recoloredSpriteSheet.SetPixels32(pixels);
        recoloredSpriteSheet.Apply(false, false);
        generatedRecolorKey = recolorKey;

        DestroyGeneratedTexture(readableSource);
    }

    private Texture2D CreateReadableCopy(Texture2D source)
    {
        RenderTexture previous = RenderTexture.active;
        RenderTexture renderTexture = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGB32);

        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.clear);
        Graphics.Blit(source, renderTexture);

        Texture2D readable = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);
        readable.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0);
        readable.Apply(false, false);

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTexture);

        return readable;
    }

    private Color[] ParseColorList(string[] hexColors)
    {
        if (hexColors == null || hexColors.Length == 0)
        {
            return new[] { Color.white };
        }

        Color[] parsedColors = new Color[hexColors.Length];
        for (int i = 0; i < hexColors.Length; i++)
        {
            if (!ColorUtility.TryParseHtmlString(hexColors[i], out parsedColors[i]))
            {
                parsedColors[i] = Color.white;
            }
        }

        return parsedColors;
    }

    private int FindClosestSourceColor(Color pixel, Color[] sourceColors, float toleranceSquared)
    {
        int closestIndex = -1;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < sourceColors.Length; i++)
        {
            float distance =
                Mathf.Pow(pixel.r - sourceColors[i].r, 2f) +
                Mathf.Pow(pixel.g - sourceColors[i].g, 2f) +
                Mathf.Pow(pixel.b - sourceColors[i].b, 2f);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestDistance <= toleranceSquared ? closestIndex : -1;
    }

    private string BuildRecolorKey()
    {
        if (spriteSheet == null)
        {
            return string.Empty;
        }

        string sourceKey = sourceHexColors == null ? string.Empty : string.Join(",", sourceHexColors);
        string replacementKey = replacementHexColors == null ? string.Empty : string.Join(",", replacementHexColors);
        return spriteSheet.GetInstanceID() + "|" + useHexColorOverride + "|" + colorMatchTolerance + "|" + clearUnmatchedPixels + "|" + sourceKey + "|" + replacementKey;
    }

    private void DestroyRecoloredSpriteSheet()
    {
        if (recoloredSpriteSheet == null)
        {
            return;
        }

        DestroyGeneratedTexture(recoloredSpriteSheet);
        recoloredSpriteSheet = null;
        generatedRecolorKey = null;
    }

    private void DestroyGeneratedTexture(Texture2D texture)
    {
        if (texture == null)
        {
            return;
        }

        if (Application.isPlaying)
        {
            Destroy(texture);
        }
        else
        {
            DestroyImmediate(texture);
        }
    }

    private bool ShouldRenderOnCanvas()
    {
        return renderOnCanvas && GetComponentInParent<Canvas>() != null;
    }

    private void FaceCameraIfNeeded()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        if (ShouldRenderOnCanvas() || parentToCamera || !faceCamera || Camera.main == null)
        {
            return;
        }

        Vector3 direction = Camera.main.transform.position - transform.position;

        if (direction.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
        }
    }

    private void FollowCameraIfNeeded()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        if (ShouldRenderOnCanvas() || Camera.main == null)
        {
            return;
        }

        Transform cameraTransform = Camera.main.transform;

        if (parentToCamera)
        {
            AttachToCamera(cameraTransform);
            return;
        }

        DetachFromCameraIfNeeded();

        if (!followCamera)
        {
            return;
        }

        transform.position = cameraTransform.position +
                             cameraTransform.forward * Mathf.Max(0.01f, cameraFollowDistance) +
                             cameraTransform.TransformVector(cameraFollowOffset);
    }

    private void AttachToCamera(Transform cameraTransform)
    {
        if (transform.parent != cameraTransform)
        {
            if (attachedCamera == null)
            {
                originalParent = transform.parent;
            }

            transform.SetParent(cameraTransform, false);
            attachedCamera = cameraTransform;
        }

        transform.localPosition = Vector3.forward * Mathf.Max(0.01f, cameraFollowDistance) + cameraFollowOffset;
        transform.localRotation = Quaternion.LookRotation(Vector3.back, Vector3.up);
    }

    private void DetachFromCameraIfNeeded()
    {
        if (attachedCamera == null || transform.parent != attachedCamera)
        {
            return;
        }

        transform.SetParent(originalParent, true);
        attachedCamera = null;
    }
}
