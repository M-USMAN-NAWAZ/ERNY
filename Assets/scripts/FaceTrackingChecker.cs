using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FaceTrackingChecker : MonoBehaviour
{
    [SerializeField] private ARFaceManager faceManager;

    [Header("Tracked Object")]
    [SerializeField] private string trackedObjectTag = "ARFace";

    private bool isFaceTracked = false;

    // Store reference so we can turn it back ON
    private GameObject trackedObject;


    private void OnEnable()
    {
        faceManager.facesChanged += OnFacesChanged;
    }

    private void OnDisable()
    {
        faceManager.facesChanged -= OnFacesChanged;
    }


    private void OnFacesChanged(ARFacesChangedEventArgs args)
    {
        // Face detected for the first time
        foreach (ARFace face in args.added)
        {
            CheckFaceState(face);
        }

        // Face tracking updated
        foreach (ARFace face in args.updated)
        {
            CheckFaceState(face);
        }

        // Face removed
        foreach (ARFace face in args.removed)
        {
            SetFaceTracked(false);
        }
    }


    private void CheckFaceState(ARFace face)
    {
        if (face.trackingState == TrackingState.Tracking)
        {
            SetFaceTracked(true);
        }
        else
        {
            SetFaceTracked(false);
        }
    }


    private void SetFaceTracked(bool tracked)
    {
        // Don't repeat the same action
        if (isFaceTracked == tracked)
            return;

        isFaceTracked = tracked;

        if (isFaceTracked)
        {
            Debug.Log("FACE TRACKED");

            // Turn object back ON
            EnableTrackedObject();
        }
        else
        {
            Debug.Log("FACE LOST / DETRACKED");

            // Turn object OFF
            DisableTrackedObject();
        }
    }


    private void DisableTrackedObject()
    {
        // Find it only if we don't already have the reference
        if (trackedObject == null)
        {
            trackedObject =
                GameObject.FindGameObjectWithTag(trackedObjectTag);
        }

        if (trackedObject != null)
        {
            if (trackedObject.activeInHierarchy)
            {
                Debug.Log(
                    "Tracked object found. Turning OFF: "
                    + trackedObject.name
                );

                trackedObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log(
                "No object found with tag: "
                + trackedObjectTag
            );
        }
    }


    public void DestroyTrackedObject()
    {
        if (trackedObject == null)
        {
            trackedObject =
                GameObject.FindGameObjectWithTag(trackedObjectTag);
        }

        isFaceTracked = false;

        if (trackedObject != null)
        {
            GameObject objectToDestroy = trackedObject;
            trackedObject = null;
            objectToDestroy.SetActive(false);

            Debug.Log(
                "Tracked face object destroyed: "
                + objectToDestroy.name
            );

            Destroy(objectToDestroy);
        }
        else
        {
            Debug.Log(
                "No object found with tag: "
                + trackedObjectTag
            );
        }
    }


    private void EnableTrackedObject()
    {
        // Because FindGameObjectWithTag cannot find inactive objects,
        // we use the saved reference.

        if (trackedObject != null)
        {
            if (!trackedObject.activeSelf)
            {
                trackedObject.SetActive(true);

                Debug.Log(
                    "Tracked object turned ON: "
                    + trackedObject.name
                );
            }
        }
        else
        {
            // First time face is detected, try finding the object
            trackedObject =
                GameObject.FindGameObjectWithTag(trackedObjectTag);

            if (trackedObject != null)
            {
                trackedObject.SetActive(true);

                Debug.Log(
                    "Tracked object found and turned ON: "
                    + trackedObject.name
                );
            }
        }
    }


    public bool IsFaceTracked()
    {
        return isFaceTracked;
    }
}
