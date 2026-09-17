using UnityEngine;

public class KeepWorldScale : MonoBehaviour
{
    private Vector3 targetWorldScale;

    private void Start()
    {
        // Remember the particle's original world scale
        targetWorldScale = transform.lossyScale;
    }

    private void LateUpdate()
    {
        if (transform.parent == null)
            return;

        Vector3 parentScale = transform.parent.lossyScale;

        transform.localScale = new Vector3(
            parentScale.x != 0 ? targetWorldScale.x / parentScale.x : targetWorldScale.x,
            parentScale.y != 0 ? targetWorldScale.y / parentScale.y : targetWorldScale.y,
            parentScale.z != 0 ? targetWorldScale.z / parentScale.z : targetWorldScale.z
        );
    }
}