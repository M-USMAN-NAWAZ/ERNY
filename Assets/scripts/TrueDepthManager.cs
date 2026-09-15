using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

#if UNITY_IOS
using UnityEngine.XR.ARKit;
#endif


public class TrueDepthManager : MonoBehaviour
{
    // ============================================================
    // AR REFERENCES
    // ============================================================

    [Header("AR References")]

    [SerializeField]
    private ARSession arSession;

    [SerializeField]
    private ARCameraManager cameraManager;


    // ============================================================
    // MASK FILTER SHADER
    // ============================================================

    [Header("Mask Filter")]

    [Tooltip("Assign TrueDepthMaskFilter.shader here.")]
    [SerializeField]
    private Shader maskFilterShader;


    // ============================================================
    // PERSON MASK
    // ============================================================

    [Header("Smooth Person Mask")]

    [Tooltip("Ignore invalid/extremely close TrueDepth values.")]
    [SerializeField]
    private float minimumPersonDepth = 0.05f;


    [Tooltip("Depth farther than this is treated as background.")]
    [SerializeField]
    private float maximumPersonDepth = 1.8f;


    [Range(0.5f, 2.5f)]
    [Tooltip("Spatial expansion used to fill small TrueDepth holes.")]
    [SerializeField]
    private float spatialRadius = 1.0f;


    [Range(0.01f, 1.0f)]
    [Tooltip("How quickly newly detected foreground appears.")]
    [SerializeField]
    private float maskAttack = 0.90f;


    [Range(0.01f, 1.0f)]
    [Tooltip("How quickly foreground disappears.")]
    [SerializeField]
    private float maskRelease = 0.30f;


    // ============================================================
    // TRUEDEPTH BORDER FIX
    // ============================================================

    [Header("TrueDepth Coverage Edge Fix")]

    [Range(0.0f, 0.15f)]
    [Tooltip(
        "Completely ignores the outer edge of the TrueDepth texture. " +
        "Increase if you still see the rectangular TrueDepth boundary."
    )]
    [SerializeField]
    private float edgeGuard = 0.025f;


    [Range(0.001f, 0.15f)]
    [Tooltip(
        "Smoothly transitions from ignored border into usable depth."
    )]
    [SerializeField]
    private float edgeFade = 0.025f;


    // ============================================================
    // PUBLIC
    // ============================================================

    public Texture2D DepthTexture
    {
        get;
        private set;
    }


    public RenderTexture SmoothMaskTexture
    {
        get;
        private set;
    }


    // ============================================================
    // INTERNAL
    // ============================================================

    private bool sessionConnected = false;

    private bool displayMatrixReceived = false;

    private bool maskReadyLogged = false;


    private Material maskFilterMaterial;

    private RenderTexture maskHistory;

    private RenderTexture maskOutput;


    // ============================================================
    // NATIVE TRUEDEPTH
    // ============================================================

#if UNITY_IOS && !UNITY_EDITOR

    [DllImport("__Internal")]
    private static extern void TrueDepth_SetSession(
        IntPtr session
    );


    [DllImport("__Internal")]
    private static extern int TrueDepth_Update();


    [DllImport("__Internal")]
    private static extern IntPtr TrueDepth_GetBuffer();


    [DllImport("__Internal")]
    private static extern int TrueDepth_GetWidth();


    [DllImport("__Internal")]
    private static extern int TrueDepth_GetHeight();


    [DllImport("__Internal")]
    private static extern void TrueDepth_Destroy();

#endif


#if UNITY_IOS

    [StructLayout(LayoutKind.Sequential)]
    private struct UnityXRNativeSession
    {
        public int version;

        public IntPtr sessionPtr;
    }

#endif


    // ============================================================
    // AWAKE
    // ============================================================

    private void Awake()
    {
        if (arSession == null)
        {
            arSession =
                FindFirstObjectByType<ARSession>();
        }


        if (cameraManager == null)
        {
            cameraManager =
                FindFirstObjectByType<ARCameraManager>();
        }


        Shader.SetGlobalMatrix(
            "_TrueDepthDisplayMatrix",
            Matrix4x4.identity
        );


        Shader.SetGlobalFloat(
            "_TrueDepthMatrixReady",
            0f
        );


        Shader.SetGlobalFloat(
            "_TrueDepthReady",
            0f
        );


        Shader.SetGlobalFloat(
            "_TrueDepthMaskReady",
            0f
        );


        Shader.SetGlobalTexture(
            "_TrueDepthMaskTexture",
            Texture2D.blackTexture
        );


        // These are also available to the final shaders.
        Shader.SetGlobalFloat(
            "_TrueDepthEdgeGuard",
            edgeGuard
        );


        Shader.SetGlobalFloat(
            "_TrueDepthEdgeFade",
            edgeFade
        );


        CreateMaskMaterial();


        Debug.Log(
            "TRUE DEPTH: ARSession = " +
            (arSession != null ? "FOUND" : "MISSING")
        );


        Debug.Log(
            "TRUE DEPTH: CameraManager = " +
            (cameraManager != null ? "FOUND" : "MISSING")
        );
    }


    // ============================================================
    // CREATE FILTER MATERIAL
    // ============================================================

    private void CreateMaskMaterial()
    {
        if (maskFilterShader == null)
        {
            Debug.LogError(
                "TRUE DEPTH: Mask Filter Shader is NOT assigned!"
            );

            return;
        }


        if (!maskFilterShader.isSupported)
        {
            Debug.LogError(
                "TRUE DEPTH: Mask Filter Shader is NOT supported!"
            );

            return;
        }


        maskFilterMaterial =
            new Material(
                maskFilterShader
            );


        maskFilterMaterial.hideFlags =
            HideFlags.HideAndDontSave;


        Debug.Log(
            "TRUE DEPTH: Mask Filter Material created successfully!"
        );
    }


    // ============================================================
    // ENABLE
    // ============================================================

    private void OnEnable()
    {
        if (cameraManager == null)
        {
            cameraManager =
                FindFirstObjectByType<ARCameraManager>();
        }


        if (cameraManager != null)
        {
            cameraManager.frameReceived +=
                OnCameraFrameReceived;
        }
        else
        {
            Debug.LogError(
                "TRUE DEPTH: ARCameraManager NOT FOUND!"
            );
        }
    }


    // ============================================================
    // DISABLE
    // ============================================================

    private void OnDisable()
    {
        if (cameraManager != null)
        {
            cameraManager.frameReceived -=
                OnCameraFrameReceived;
        }
    }


    // ============================================================
    // UPDATE
    // ============================================================

    private void Update()
    {
        // Keep globals synchronized with Inspector changes.
        Shader.SetGlobalFloat(
            "_TrueDepthEdgeGuard",
            edgeGuard
        );


        Shader.SetGlobalFloat(
            "_TrueDepthEdgeFade",
            edgeFade
        );


#if UNITY_IOS && !UNITY_EDITOR

        if (!sessionConnected)
        {
            ConnectToARKit();

            return;
        }


        UpdateDepthTexture();

#endif
    }


    // ============================================================
    // CAMERA DISPLAY MATRIX
    // ============================================================

    private void OnCameraFrameReceived(
        ARCameraFrameEventArgs args
    )
    {
        if (!args.displayMatrix.HasValue)
        {
            return;
        }


        Shader.SetGlobalMatrix(
            "_TrueDepthDisplayMatrix",
            args.displayMatrix.Value
        );


        Shader.SetGlobalFloat(
            "_TrueDepthMatrixReady",
            1f
        );


        if (!displayMatrixReceived)
        {
            displayMatrixReceived = true;


            Debug.Log(
                "TRUE DEPTH: Display Matrix received!"
            );
        }
    }


#if UNITY_IOS && !UNITY_EDITOR

    // ============================================================
    // CONNECT ARKIT
    // ============================================================

    private void ConnectToARKit()
    {
        if (arSession == null)
        {
            arSession =
                FindFirstObjectByType<ARSession>();
        }


        if (arSession == null)
        {
            return;
        }


        ARKitSessionSubsystem arKitSession =
            arSession.subsystem
            as ARKitSessionSubsystem;


        if (arKitSession == null)
        {
            return;
        }


        IntPtr nativePtr =
            arKitSession.nativePtr;


        if (nativePtr == IntPtr.Zero)
        {
            return;
        }


        UnityXRNativeSession nativeSession =
            Marshal.PtrToStructure<UnityXRNativeSession>(
                nativePtr
            );


        if (nativeSession.sessionPtr ==
            IntPtr.Zero)
        {
            return;
        }


        TrueDepth_SetSession(
            nativeSession.sessionPtr
        );


        sessionConnected = true;


        Debug.Log(
            "TRUE DEPTH: ARKit session connected!"
        );
    }


    // ============================================================
    // UPDATE RAW TRUEDEPTH
    // ============================================================

    private void UpdateDepthTexture()
    {
        int result =
            TrueDepth_Update();


        if (result == 0)
        {
            return;
        }


        int width =
            TrueDepth_GetWidth();


        int height =
            TrueDepth_GetHeight();


        IntPtr buffer =
            TrueDepth_GetBuffer();


        if (
            buffer == IntPtr.Zero ||
            width <= 0 ||
            height <= 0
        )
        {
            return;
        }


        EnsureDepthTexture(
            width,
            height
        );


        int byteCount =
            width *
            height *
            sizeof(float);


        DepthTexture.LoadRawTextureData(
            buffer,
            byteCount
        );


        DepthTexture.Apply(
            false,
            false
        );


        Shader.SetGlobalTexture(
            "_TrueDepthTexture",
            DepthTexture
        );


        Shader.SetGlobalVector(
            "_TrueDepthTexelSize",
            new Vector4(
                1f / width,
                1f / height,
                width,
                height
            )
        );


        Shader.SetGlobalFloat(
            "_TrueDepthReady",
            1f
        );


        bool maskGenerated =
            UpdateSmoothPersonMask(
                width,
                height
            );


        if (
            maskGenerated &&
            !maskReadyLogged
        )
        {
            maskReadyLogged = true;


            Debug.Log(
                "TRUE DEPTH: DEPTH IS READY!"
            );


            Debug.Log(
                $"TRUE DEPTH SIZE: {width} x {height}"
            );


            Debug.Log(
                "TRUE DEPTH: SMOOTH MASK IS ACTUALLY READY!"
            );
        }
    }


    // ============================================================
    // RAW DEPTH TEXTURE
    // ============================================================

    private void EnsureDepthTexture(
        int width,
        int height
    )
    {
        if (
            DepthTexture != null &&
            DepthTexture.width == width &&
            DepthTexture.height == height
        )
        {
            return;
        }


        if (DepthTexture != null)
        {
            Destroy(
                DepthTexture
            );
        }


        DepthTexture =
            new Texture2D(
                width,
                height,
                TextureFormat.RFloat,
                false,
                true
            );


        DepthTexture.wrapMode =
            TextureWrapMode.Clamp;


        DepthTexture.filterMode =
            FilterMode.Point;


        CreateMaskTextures(
            width,
            height
        );


        Debug.Log(
            $"TRUE DEPTH: Texture created {width} x {height}"
        );
    }


    // ============================================================
    // CREATE MASK TEXTURES
    // ============================================================

    private void CreateMaskTextures(
        int width,
        int height
    )
    {
        ReleaseMaskTextures();


        RenderTextureFormat format =
            SystemInfo.SupportsRenderTextureFormat(
                RenderTextureFormat.R8
            )
            ?
            RenderTextureFormat.R8
            :
            RenderTextureFormat.ARGB32;


        maskHistory =
            new RenderTexture(
                width,
                height,
                0,
                format,
                RenderTextureReadWrite.Linear
            );


        maskOutput =
            new RenderTexture(
                width,
                height,
                0,
                format,
                RenderTextureReadWrite.Linear
            );


        SetupMaskTexture(
            maskHistory
        );


        SetupMaskTexture(
            maskOutput
        );


        ClearRenderTexture(
            maskHistory
        );


        ClearRenderTexture(
            maskOutput
        );


        SmoothMaskTexture =
            maskHistory;
    }


    private void SetupMaskTexture(
        RenderTexture texture
    )
    {
        texture.wrapMode =
            TextureWrapMode.Clamp;


        texture.filterMode =
            FilterMode.Bilinear;


        texture.useMipMap =
            false;


        texture.autoGenerateMips =
            false;


        texture.Create();
    }


    private void ClearRenderTexture(
        RenderTexture texture
    )
    {
        RenderTexture old =
            RenderTexture.active;


        RenderTexture.active =
            texture;


        GL.Clear(
            false,
            true,
            Color.black
        );


        RenderTexture.active =
            old;
    }


    // ============================================================
    // SMOOTH PERSON MASK
    // ============================================================

    private bool UpdateSmoothPersonMask(
        int width,
        int height
    )
    {
        if (
            maskFilterMaterial == null ||
            maskHistory == null ||
            maskOutput == null ||
            DepthTexture == null
        )
        {
            Shader.SetGlobalFloat(
                "_TrueDepthMaskReady",
                0f
            );


            return false;
        }


        maskFilterMaterial.SetTexture(
            "_PreviousMask",
            maskHistory
        );


        maskFilterMaterial.SetVector(
            "_DepthTexelSize",
            new Vector4(
                1f / width,
                1f / height,
                width,
                height
            )
        );


        maskFilterMaterial.SetFloat(
            "_MinDepth",
            minimumPersonDepth
        );


        maskFilterMaterial.SetFloat(
            "_MaxDepth",
            maximumPersonDepth
        );


        maskFilterMaterial.SetFloat(
            "_SpatialRadius",
            spatialRadius
        );


        maskFilterMaterial.SetFloat(
            "_Attack",
            maskAttack
        );


        maskFilterMaterial.SetFloat(
            "_Release",
            maskRelease
        );


        // NEW BORDER PARAMETERS
        maskFilterMaterial.SetFloat(
            "_EdgeGuard",
            edgeGuard
        );


        maskFilterMaterial.SetFloat(
            "_EdgeFade",
            edgeFade
        );


        Graphics.Blit(
            DepthTexture,
            maskOutput,
            maskFilterMaterial,
            0
        );


        // --------------------------------------------------------
        // Ping-pong
        // --------------------------------------------------------

        RenderTexture generated =
            maskOutput;


        maskOutput =
            maskHistory;


        maskHistory =
            generated;


        SmoothMaskTexture =
            maskHistory;


        Shader.SetGlobalTexture(
            "_TrueDepthMaskTexture",
            SmoothMaskTexture
        );


        Shader.SetGlobalFloat(
            "_TrueDepthMaskReady",
            1f
        );


        return true;
    }

#endif


    // ============================================================
    // RELEASE MASKS
    // ============================================================

    private void ReleaseMaskTextures()
    {
        if (maskHistory != null)
        {
            maskHistory.Release();


            Destroy(
                maskHistory
            );


            maskHistory =
                null;
        }


        if (maskOutput != null)
        {
            maskOutput.Release();


            Destroy(
                maskOutput
            );


            maskOutput =
                null;
        }


        SmoothMaskTexture =
            null;
    }


    // ============================================================
    // DESTROY
    // ============================================================

    private void OnDestroy()
    {
        Shader.SetGlobalFloat(
            "_TrueDepthReady",
            0f
        );


        Shader.SetGlobalFloat(
            "_TrueDepthMaskReady",
            0f
        );


        Shader.SetGlobalFloat(
            "_TrueDepthMatrixReady",
            0f
        );


        Shader.SetGlobalTexture(
            "_TrueDepthMaskTexture",
            Texture2D.blackTexture
        );


#if UNITY_IOS && !UNITY_EDITOR

        TrueDepth_Destroy();

#endif


        if (DepthTexture != null)
        {
            Destroy(
                DepthTexture
            );


            DepthTexture =
                null;
        }


        ReleaseMaskTextures();


        if (maskFilterMaterial != null)
        {
            Destroy(
                maskFilterMaterial
            );


            maskFilterMaterial =
                null;
        }
    }
}