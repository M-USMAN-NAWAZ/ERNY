using UnityEngine;

[DefaultExecutionOrder(100)]
public sealed class FinisherFaceCamera : MonoBehaviour
{
    private Camera arCamera;

    private void OnEnable()
    {
        GameObject cameraObject = GameObject.Find("AR Camera");
        arCamera = cameraObject != null ? cameraObject.GetComponent<Camera>() : Camera.main;
    }

    private void LateUpdate()
    {
        if (arCamera == null)
            arCamera = Camera.main;
        if (arCamera == null)
            return;

        Vector3 direction = arCamera.transform.position - transform.position;
        if (direction.sqrMagnitude < 0.001f)
            return;

        // The imported letters lie in the XZ plane rather than the XY plane.
        transform.rotation = Quaternion.LookRotation(direction, arCamera.transform.up)
            * Quaternion.Euler(-90f, 0f, 0f);
    }
}
