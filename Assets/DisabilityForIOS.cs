using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisabilityForIOS : MonoBehaviour
{
     // Start is called before the first frame update
    void Awake()
    {
#if UNITY_IOS
        gameObject.SetActive(true);
#endif
#if UNITY_ANDROID
        gameObject.SetActive(false);
#endif

    }
}
