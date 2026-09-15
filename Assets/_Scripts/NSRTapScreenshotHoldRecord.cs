using System.Collections;
using System.IO;
using SilverTau.NSR.Recorders.Video;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NSRTapScreenshotHoldRecord : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private const float MaxRecordDuration = 10f;

    [Header("Press Timing")]
    [SerializeField] private float holdToRecordSeconds = 0.5f;

    [Header("Video Recording")]
    [SerializeField] private bool recordMicrophone = true;
    [SerializeField] private HoldCountdownVisual holdCountdownVisual;

    [Header("Screenshot")]
    [SerializeField] private string screenshotFileName = "screenshot.png";
    [SerializeField] private string shareTitle = "Share Screenshot";
    [SerializeField] private GameObject[] objectsToHideWhileCapturing;

    private bool isPressed;
    private bool isRecording;
    private Coroutine pressCoroutine;
    private bool[] previousCaptureObjectStates;
    private Button recordButton;

    private IEnumerator Start()
    {
        recordButton = GetComponent<Button>();
        if (holdCountdownVisual == null)
        {
            holdCountdownVisual = GetComponent<HoldCountdownVisual>();
        }

        // NSR adds its old Share callback during Start, so clear this dedicated
        // record button one frame later. Press handling comes from pointer events.
        yield return null;

        if (recordButton != null)
        {
            recordButton.onClick.RemoveAllListeners();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (pressCoroutine != null)
        {
            StopCoroutine(pressCoroutine);
        }

        isPressed = true;
        isRecording = false;
        pressCoroutine = StartCoroutine(HandlePress());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isPressed)
        {
            return;
        }

        isPressed = false;

        if (isRecording)
        {
            StopRecording();
            return;
        }

        if (!isRecording)
        {
            if (pressCoroutine != null)
            {
                StopCoroutine(pressCoroutine);
                pressCoroutine = null;
            }

            StartCoroutine(CaptureScreenshotAndShare());
        }
    }

    private void OnDisable()
    {
        isPressed = false;

        if (isRecording)
        {
            StopRecording();
        }

        if (pressCoroutine != null)
        {
            StopCoroutine(pressCoroutine);
            pressCoroutine = null;
        }
    }

    private IEnumerator HandlePress()
    {
        yield return new WaitForSecondsRealtime(holdToRecordSeconds);

        if (!isPressed)
        {
            pressCoroutine = null;
            yield break;
        }

        Debug.Log("Video hold detected.");
        StartRecording();
        float recordingStartedAt = Time.unscaledTime;

        while (isPressed)
        {
            if (Time.unscaledTime - recordingStartedAt >= MaxRecordDuration)
            {
                isPressed = false;
                StopRecording();
                break;
            }

            yield return null;
        }

        pressCoroutine = null;
    }

    private void StartRecording()
    {
        if (UniversalVideoRecorder.Instance == null || isRecording)
        {
            return;
        }

        UniversalVideoRecorder.Instance.recordMicrophone = recordMicrophone;
        UniversalVideoRecorder.Instance.StartVideoRecorder();
        isRecording = true;
        holdCountdownVisual?.StartCountdown();
    }

    private void StopRecording()
    {
        if (UniversalVideoRecorder.Instance == null)
        {
            isRecording = false;
            return;
        }

        UniversalVideoRecorder.Instance.StopVideoRecorder();
        isRecording = false;
        holdCountdownVisual?.StopCountdown();
    }

    private IEnumerator CaptureScreenshotAndShare()
    {
        SetCaptureObjectsActiveForCapture();
        yield return new WaitForEndOfFrame();

        Texture2D screenshot = CaptureCurrentFrame();

        if (screenshot == null)
        {
            RestoreCaptureObjects();
            Debug.LogError("Screenshot capture failed: no valid render target was available.");
            yield break;
        }

        string path = Path.Combine(Application.temporaryCachePath, screenshotFileName);
        File.WriteAllBytes(path, screenshot.EncodeToPNG());
        Destroy(screenshot);
        RestoreCaptureObjects();

        Debug.Log("Screenshot saved: " + path);

#if UNITY_EDITOR
        SilverTau.NSR.OpenInFileBrowser.OpenFileBrowser(path);
#elif PLATFORM_IOS || PLATFORM_ANDROID || UNITY_IOS || UNITY_ANDROID
        SilverTau.NSR.Utilities.Share.ShareAndroidSetTitle(shareTitle);
        SilverTau.NSR.Utilities.Share.ShareItem(path);
#elif PLATFORM_STANDALONE || UNITY_STANDALONE
        SilverTau.NSR.OpenInFileBrowser.OpenFileBrowser(path);
#endif
    }

    private Texture2D CaptureCurrentFrame()
    {
        RenderTexture previousRenderTexture = RenderTexture.active;
        int captureWidth = previousRenderTexture != null
            ? previousRenderTexture.width
            : 0;
        int captureHeight = previousRenderTexture != null
            ? previousRenderTexture.height
            : 0;

        if (captureWidth <= 0 || captureHeight <= 0)
        {
            Camera captureCamera = Camera.main;
            if (captureCamera != null)
            {
                captureWidth = captureCamera.pixelWidth;
                captureHeight = captureCamera.pixelHeight;
            }
        }

        if (captureWidth <= 0 || captureHeight <= 0)
        {
            captureWidth = Display.main.renderingWidth;
            captureHeight = Display.main.renderingHeight;
        }

        if (captureWidth <= 0 || captureHeight <= 0)
        {
            return null;
        }

        try
        {
            Texture2D screenshot = new Texture2D(
                captureWidth,
                captureHeight,
                TextureFormat.RGB24,
                false);
            screenshot.ReadPixels(new Rect(0, 0, captureWidth, captureHeight), 0, 0);
            screenshot.Apply();
            return screenshot;
        }
        catch (System.Exception exception)
        {
            Debug.LogException(exception);
            return null;
        }
        finally
        {
            RenderTexture.active = previousRenderTexture;
        }
    }

    private void SetCaptureObjectsActiveForCapture()
    {
        if (objectsToHideWhileCapturing == null)
        {
            return;
        }

        previousCaptureObjectStates = new bool[objectsToHideWhileCapturing.Length];

        for (int i = 0; i < objectsToHideWhileCapturing.Length; i++)
        {
            GameObject obj = objectsToHideWhileCapturing[i];
            if (obj != null)
            {
                previousCaptureObjectStates[i] = obj.activeSelf;
                obj.SetActive(false);
            }
        }
    }

    private void RestoreCaptureObjects()
    {
        if (objectsToHideWhileCapturing == null || previousCaptureObjectStates == null)
        {
            return;
        }

        for (int i = 0; i < objectsToHideWhileCapturing.Length && i < previousCaptureObjectStates.Length; i++)
        {
            GameObject obj = objectsToHideWhileCapturing[i];
            if (obj != null)
            {
                obj.SetActive(previousCaptureObjectStates[i]);
            }
        }
    }
}
