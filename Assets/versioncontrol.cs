using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class versioncontrol : MonoBehaviour
{

    public static versioncontrol instance;
    public Text header, description;
    public GameObject popbox;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }



    public void openurl()
    {
#if UNITY_IOS

        string appId = "1617612120"; // Replace with your app's App Store ID
        string appStoreURL = "itms-apps://itunes.apple.com/app/id" + appId;

        Application.OpenURL(appStoreURL);
#endif



#if PLATFORM_ANDROID
string packageName = "com.ElytraStudios.Blingar"; // Replace with your app's package name
        string playStoreURL = "market://details?id=" + packageName;

        Application.OpenURL(playStoreURL);
#endif

    }




    // Update is called once per frame
    void Update()
    {

    }
}
