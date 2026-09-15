using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableandroid : MonoBehaviour
{
    void Awake()
    {
#if UNITY_IOS
        gameObject.SetActive(false);
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
        gameObject.SetActive(true);
#endif

    }
}
