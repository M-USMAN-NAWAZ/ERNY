using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttonoff : MonoBehaviour
{
    public GameObject NewSigIn;
    public GameObject OldSigIn;
    public GameObject Guest;
    public GameObject ChangeTextIOS;
    public GameObject ChangeTextAndriod;
    void Awake()
    {
#if UNITY_IOS
        NewSigIn.gameObject.SetActive(false);
        OldSigIn.gameObject.SetActive(true);
        //Guest.gameObject.SetActive(true);
        ChangeTextIOS.gameObject.SetActive(true);
         ChangeTextAndriod.gameObject.SetActive(false);
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
   OldSigIn.gameObject.SetActive(false);
    //Guest.gameObject.SetActive(false);
    NewSigIn.gameObject.SetActive(true);
     ChangeTextIOS.gameObject.SetActive(false);
         ChangeTextAndriod.gameObject.SetActive(true);
#endif

    }
}
