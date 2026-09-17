using System.Collections;
using UnityEngine;

public class EnableChildrenRandomly : MonoBehaviour
{
    public float interval = 2.0f; // Time interval between enabling children

    private void Start()
    {
        StartCoroutine(EnableChildrenWithInterval());
    }

    private IEnumerator EnableChildrenWithInterval()
    {
        Transform parentTransform = transform;
        int childCount = parentTransform.childCount;
        WaitForSeconds waitInterval = new WaitForSeconds(interval);

        while (childCount > 0)
        {
            int randomChildIndex = Random.Range(0, childCount);
            Transform randomChild = parentTransform.GetChild(randomChildIndex);
            randomChild.gameObject.SetActive(false);
            randomChild.gameObject.SetActive(true);

            yield return waitInterval;

            // Disable the child after the interval
          //  randomChild.gameObject.SetActive(false);
        }
    }
}
