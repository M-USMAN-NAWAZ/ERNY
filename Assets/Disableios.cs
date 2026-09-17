using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Disableios : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
#if UNITY_IOS
        gameObject.SetActive(false);
#endif
#if UNITY_ANDROID
        gameObject.SetActive(true);
#endif

    }


}
