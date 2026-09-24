using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Rendering;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

public class camerarotate : MonoBehaviour
{
    public FaceTrackingChecker FaceTrackingChecker;

    public GameObject image;
    public Toggle imageToggle;
    public ARSession arSession;
    public ARPlaneManager arPlaneManager;
    public ARRaycastManager arRaycastManager;
    public ARFaceManager faceManager;
    public spawnmangr spawnMngr;

    public ARCameraManager cameraManager
    {
        get => m_CameraManager;
        set => m_CameraManager = value;
    }

  //  public ARTrackedImageManager m_ARTrackedImageManager;
//
 //   public GameObject infoCanvas;
  //  public GameObject selfieCanvas;

  //  public ImageTracking imageTracking;

    [SerializeField]
    ARCameraManager m_CameraManager;
    private CameraFacingDirection selectedFacingDirection = CameraFacingDirection.World;
    private Coroutine enableFaceTrackingCoroutine;
    private Coroutine enableWorldTrackingCoroutine;

    private void Start()
    {
        ResolveArComponents();
        SetFaceTrackingEnabled(false);

        if (m_CameraManager != null)
        {
            selectedFacingDirection = CameraFacingDirection.World;
            m_CameraManager.requestedFacingDirection = CameraFacingDirection.World;
        }
    }

    private void OnDisable()
    {
        StopPendingFaceTrackingEnable();
        StopPendingWorldTrackingEnable();
        ResolveArComponents();
        DestroyTrackedFaces();
        SetFaceTrackingEnabled(false);

        selectedFacingDirection = CameraFacingDirection.World;

        if (m_CameraManager != null)
        {
            m_CameraManager.requestedFacingDirection = CameraFacingDirection.World;
        }

        SetWorldTrackingManagersEnabled(true);
        spawnMngr?.SetPlacementInputEnabled(true);
    }

    /// <summary>
    /// On button press callback to toggle the requested camera facing direction.
    /// </summary>
    /// 

    public void UseBackCamera()
    {
        ApplyCameraFacingDirection(CameraFacingDirection.World);
    }

    public void UseFrontCamera()
    {
        ApplyCameraFacingDirection(CameraFacingDirection.User);
    }

    public void imageoff()
    {
        if(image.activeInHierarchy)
        {
            image.SetActive(false);
        }
    }
    public void OnSwapCameraButtonPress()
    {
        Debug.Assert(m_CameraManager != null, "camera manager cannot be null");
        CameraFacingDirection newFacingDirection;
        CameraFacingDirection currentRequestedDirection = GetSelectedFacingDirection();
        switch (currentRequestedDirection)
        {
            case CameraFacingDirection.World:
            case CameraFacingDirection.None:
                newFacingDirection = CameraFacingDirection.User;
                SetCameraChildSpawnedObjectVisible(false);
                SetFaceTrackingEnabled(false);
                AssignSelectedFacePrefab();
                //m_ARTrackedImageManager.enabled = false;
              //  infoCanvas.SetActive(false);
              //  selfieCanvas.SetActive(true);
               // imageTracking.TurnOffAnimals();
                break;
            case CameraFacingDirection.User:
            default:
                newFacingDirection = CameraFacingDirection.World;
                StopPendingFaceTrackingEnable();
                DestroyTrackedFaces();
                SetFaceTrackingEnabled(false);
                SetCameraChildSpawnedObjectVisible(true);
                //m_ARTrackedImageManager.enabled = true;
              //  infoCanvas.SetActive(true);
             //   selfieCanvas.SetActive(false);
                break;
        }

        



        Debug.Log($"Switching ARCameraManager.requestedFacingDirection from {currentRequestedDirection} to {newFacingDirection}");
        ApplyCameraFacingDirection(newFacingDirection);

        if (newFacingDirection == CameraFacingDirection.User)
        {
            enableFaceTrackingCoroutine = StartCoroutine(EnableFaceTrackingWhenFrontCameraIsActive());
        }
    }

    private CameraFacingDirection GetSelectedFacingDirection()
    {
        if (selectedFacingDirection != CameraFacingDirection.None)
        {
            return selectedFacingDirection;
        }

        return m_CameraManager != null ? m_CameraManager.requestedFacingDirection : CameraFacingDirection.None;
    }

    private void ApplyCameraFacingDirection(CameraFacingDirection newFacingDirection)
    {
        ResolveArComponents();
        StopPendingWorldTrackingEnable();
        spawnMngr?.SetPlacementInputEnabled(false);
        selectedFacingDirection = newFacingDirection;

        bool sessionWasEnabled = arSession != null && arSession.enabled;
        bool cameraWasEnabled = m_CameraManager != null && m_CameraManager.enabled;

        if (m_CameraManager != null)
        {
            m_CameraManager.enabled = false;
        }

        if (arSession != null)
        {
            arSession.enabled = false;
        }

        if (newFacingDirection == CameraFacingDirection.User)
        {
            SetWorldTrackingManagersEnabled(false);

            if (arSession != null)
            {
                arSession.requestedTrackingMode = TrackingMode.PositionAndRotation;
            }
        }
        else
        {
            SetFaceTrackingEnabled(false);

            if (arSession != null)
            {
                arSession.requestedTrackingMode = TrackingMode.PositionAndRotation;
            }

            SetWorldTrackingManagersEnabled(false);
        }

        if (m_CameraManager != null)
        {
            m_CameraManager.requestedFacingDirection = newFacingDirection;
        }

        if (arSession != null)
        {
            arSession.Reset();
            arSession.enabled = sessionWasEnabled;
        }

        if (m_CameraManager != null)
        {
            m_CameraManager.enabled = cameraWasEnabled;
        }

        StartCoroutine(ApplyCameraFacingDirectionAfterReset(newFacingDirection));

        if (newFacingDirection == CameraFacingDirection.World)
        {
            enableWorldTrackingCoroutine = StartCoroutine(EnableWorldTrackingWhenBackCameraIsActive());
        }
    }

    private IEnumerator EnableFaceTrackingWhenFrontCameraIsActive()
    {
        const int maxWaitFrames = 180;
        yield return null;

        for (int frame = 0; frame < maxWaitFrames; frame++)
        {
            if (selectedFacingDirection != CameraFacingDirection.User)
            {
                enableFaceTrackingCoroutine = null;
                yield break;
            }

            if (m_CameraManager != null &&
                m_CameraManager.currentFacingDirection == CameraFacingDirection.User)
            {
                SetFaceTrackingEnabled(true);
                enableFaceTrackingCoroutine = null;
                yield break;
            }

            yield return null;
        }

        Debug.LogWarning("Front camera did not become active, so ARFaceManager remains disabled.");
        enableFaceTrackingCoroutine = null;
    }

    private void StopPendingFaceTrackingEnable()
    {
        if (enableFaceTrackingCoroutine == null)
        {
            return;
        }

        StopCoroutine(enableFaceTrackingCoroutine);
        enableFaceTrackingCoroutine = null;
    }

    private IEnumerator EnableWorldTrackingWhenBackCameraIsActive()
    {
        const int maxWaitFrames = 180;
        yield return null;

        for (int frame = 0; frame < maxWaitFrames; frame++)
        {
            if (selectedFacingDirection != CameraFacingDirection.World)
            {
                enableWorldTrackingCoroutine = null;
                yield break;
            }

            if (m_CameraManager != null &&
                m_CameraManager.currentFacingDirection == CameraFacingDirection.World)
            {
                SetWorldTrackingManagersEnabled(true);
                spawnMngr?.SetPlacementInputEnabled(true);
                enableWorldTrackingCoroutine = null;
                yield break;
            }

            yield return null;
        }

        Debug.LogWarning("Back camera did not become active before placement was restored.");
        SetWorldTrackingManagersEnabled(true);
        spawnMngr?.SetPlacementInputEnabled(true);
        enableWorldTrackingCoroutine = null;
    }

    private void StopPendingWorldTrackingEnable()
    {
        if (enableWorldTrackingCoroutine == null)
        {
            return;
        }

        StopCoroutine(enableWorldTrackingCoroutine);
        enableWorldTrackingCoroutine = null;
    }

    private IEnumerator ApplyCameraFacingDirectionAfterReset(CameraFacingDirection newFacingDirection)
    {
        yield return null;

        if (m_CameraManager != null)
        {
            m_CameraManager.requestedFacingDirection = newFacingDirection;
        }

        StartCoroutine(LogCameraFacingDirectionAfterSwitch());
    }

    private IEnumerator LogCameraFacingDirectionAfterSwitch()
    {
        yield return null;
        yield return null;

        if (m_CameraManager != null)
        {
            Debug.Log($"ARCameraManager requested: {m_CameraManager.requestedFacingDirection}, current: {m_CameraManager.currentFacingDirection}");
        }
    }

    private void ResolveArComponents()
    {
        if (arSession == null)
        {
            arSession = FindObjectOfType<ARSession>(true);
        }

        if (arPlaneManager == null)
        {
            arPlaneManager = FindObjectOfType<ARPlaneManager>(true);
        }

        if (arRaycastManager == null)
        {
            arRaycastManager = FindObjectOfType<ARRaycastManager>(true);
        }

        if (spawnMngr == null)
        {
            spawnMngr = FindObjectOfType<spawnmangr>(true);
        }

        if (faceManager == null)
        {
            faceManager = FindObjectOfType<ARFaceManager>(true);
        }

        if (faceManager == null)
        {
            GameObject managerObject = null;

            if (arPlaneManager != null)
            {
                managerObject = arPlaneManager.gameObject;
            }
            else if (arRaycastManager != null)
            {
                managerObject = arRaycastManager.gameObject;
            }

            if (managerObject != null)
            {
                faceManager = managerObject.AddComponent<ARFaceManager>();
            }
        }
    }

    private void SetFaceTrackingEnabled(bool enabled)
    {
        if (faceManager == null)
        {
            return;
        }

        if (enabled)
        {
            faceManager.enabled = true;
        }
        else
        {
            faceManager.enabled = false;
        }
    }

    private void DestroyTrackedFaces()
    {
        if (faceManager != null)
        {
            foreach (ARFace trackedFace in faceManager.trackables)
            {
                if (trackedFace == null)
                {
                    continue;
                }

                trackedFace.gameObject.SetActive(false);
                Destroy(trackedFace.gameObject);
            }
        }

        if (FaceTrackingChecker != null)
        {
            FaceTrackingChecker.DestroyTrackedObject();
        }
    }

    private void AssignSelectedFacePrefab()
    {
        ResolveArComponents();

        if (faceManager == null || AddressableHandler.Instance == null)
        {
            return;
        }

        GameObject selectedFacePrefab = AddressableHandler.Instance.GetSelectedFacePrefab();
        if (selectedFacePrefab != null)
        {
            faceManager.facePrefab = selectedFacePrefab;
        }
    }

    public void SetDefaultFacePrefab()
    {
        ResolveArComponents();

        if (faceManager == null || AddressableHandler.Instance == null)
        {
            return;
        }

        GameObject defaultFacePrefab = AddressableHandler.Instance.GetDefaultFacePrefab();
        if (defaultFacePrefab != null)
        {
            AddressableHandler.Instance.SelectDefaultFace();
            faceManager.facePrefab = defaultFacePrefab;
        }
    }

    private void SetCameraChildSpawnedObjectVisible(bool visible)
    {
        if (spawnMngr == null)
        {
            spawnMngr = FindObjectOfType<spawnmangr>(true);
        }

        if (spawnMngr == null || spawnMngr.confetti == null)
        {
            return;
        }

        spawnMngr.confetti.SetActive(visible);
    }

    private void SetWorldTrackingManagersEnabled(bool enabled)
    {
        if (arPlaneManager != null)
        {
            arPlaneManager.enabled = enabled;
        }

        if (arRaycastManager != null)
        {
            arRaycastManager.enabled = enabled;
        }
    }

    private bool IsImageToggleOn()
    {
        if (imageToggle == null)
        {
            GameObject toggleObject = GameObject.FindGameObjectWithTag("faceoff");

            if (toggleObject != null)
            {
                imageToggle = toggleObject.GetComponent<Toggle>();
            }
        }

        return imageToggle != null && imageToggle.isOn;
    }

    private void SetArProfileImagesVisible(bool visible)
    {
        GameObject[] aritems = GameObject.FindGameObjectsWithTag("ARItem");

        foreach (GameObject x in aritems)
        {
            ardata itemData = x.GetComponent<ardata>();

            if (itemData != null)
            {
                itemData.SetProfileImageVisible(visible);
            }
        }
    }
}
