using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Android;
using Firebase.Messaging;
using Firebase;
#if UNITY_IOS
     
using UnityEngine.iOS;


using Unity.Notifications.iOS;
#endif
public class permissionM : MonoBehaviour
{
    public Image mic, camera, gallery, NOTIFICATION;
    public Sprite grantedcam, grantedmic, grantedgallery, grantednotification;
    public GameObject permissionpanel, legendpanel, screenLoader;
    public Sprite rejectedcam, rejectedmic, rejectedgallery, rejectednotification;
    // Start is called before the first frame update
    void Start()
    {
        checknotify = 0;

        SplashLoad.camera = 0;
        SplashLoad.gallery = 0;
        SplashLoad.microphone = 0;

        Debug.Log("Tutorial Sceen Loaded");
    }

    public void askformicrophoepermission()
    {
#if UNITY_IOS
        
            Application.RequestUserAuthorization(UserAuthorization.Microphone);
            Invoke("checkpmicermission", 1);

        
#endif
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
            Invoke("checkpmicermission", 1);
        }
#endif
    }






    public void checkpmicermission()
    {
#if UNITY_IOS


        if (Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            mic.sprite = grantedmic;
            SplashLoad.microphone = 1;
            PlayerPrefs.SetInt("mic", SplashLoad.microphone);
            PlayerPrefs.Save();
        }
        else
        {
            mic.sprite = rejectedmic;
            SplashLoad.microphone = 2;
            PlayerPrefs.SetInt("mic", SplashLoad.microphone);
            PlayerPrefs.Save();



        }

#endif

#if PLATFORM_ANDROID

        if (Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            mic.sprite = grantedmic;
            SplashLoad.microphone = 1;
            PlayerPrefs.SetInt("mic", SplashLoad.microphone);
            PlayerPrefs.Save();

            Debug.Log("Mic assess given");
        }
        else
        {
            mic.sprite = rejectedmic;
            SplashLoad.microphone = 2;
            PlayerPrefs.SetInt("mic", SplashLoad.microphone);
            PlayerPrefs.Save();

            Debug.Log("Mic assess denied");

        }

#endif

    }



    public void askforcamerapermission()
    {
#if UNITY_IOS
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            Application.RequestUserAuthorization(UserAuthorization.WebCam);
            Invoke("checkpcampermission", 1);

        }
#endif

#if PLATFORM_ANDROID

        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
            Invoke("checkpcampermission", 1);
        }
        else
        {
            camera.sprite = grantedcam;
            SplashLoad.camera = 1;
            PlayerPrefs.SetInt("camera", SplashLoad.camera);
            PlayerPrefs.Save();
        }
#endif
    }
    public void checkpcampermission()
    {
#if UNITY_IOS


        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            camera.sprite = grantedcam;
            SplashLoad.camera = 1;
            PlayerPrefs.SetInt("camera", SplashLoad.camera);
            PlayerPrefs.Save();
        }
        else
        {
            camera.sprite = rejectedcam;
            SplashLoad.camera = 2;
            PlayerPrefs.SetInt("camera", SplashLoad.camera);
            PlayerPrefs.Save();
        }


#endif
#if PLATFORM_ANDROID

        if (Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            camera.sprite = grantedcam;
            SplashLoad.camera = 1;
            PlayerPrefs.SetInt("camera", SplashLoad.camera);
            PlayerPrefs.Save();

            Debug.Log("Camera assess given");
        }
        else
        {
            camera.sprite = rejectedcam;
            SplashLoad.camera = 2;
            PlayerPrefs.SetInt("camera", SplashLoad.camera);
            PlayerPrefs.Save();
            Debug.Log("Camera assess denied");
        }
#endif


    }

    public void askforgallerymission()
    {



#if PLATFORM_ANDROID

        NativeGallery.RequestPermission(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);
        NativeGallery.RequestPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image);

        Debug.Log("Suepass the first pemission");
        Invoke("checkpgallerypermission", 1.5f);
#endif

#if UNITY_IOS




        NativeGallery.RequestPermission(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);
        NativeGallery.RequestPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image);

        Invoke("checkpgallerypermission", 1);
#endif






    }

    public void checkpgallerypermission()
    {
#if PLATFORM_ANDROID


        NativeGallery.Permission permission = NativeGallery.CheckPermission(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);

        NativeGallery.Permission permissionw = NativeGallery.CheckPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image);
        if (permission == NativeGallery.Permission.Granted && permissionw == NativeGallery.Permission.Granted)
        {
            gallery.sprite = grantedgallery;
            SplashLoad.gallery = 1;
            PlayerPrefs.SetInt("gallery", SplashLoad.gallery);
            PlayerPrefs.Save();

            Debug.Log("Gallery assess given");

        }
        else
        {
            gallery.sprite = rejectedgallery;
            SplashLoad.gallery = 2;
            PlayerPrefs.SetInt("gallery", SplashLoad.gallery);
            PlayerPrefs.Save();

            Debug.Log("gallery assess denied");
        }

#endif


#if UNITY_IOS

        NativeGallery.Permission permission = NativeGallery.CheckPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image);
        NativeGallery.Permission writepermission = NativeGallery.CheckPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image);



        if (permission == NativeGallery.Permission.Denied || writepermission == NativeGallery.Permission.Denied)
        {
            gallery.sprite = rejectedgallery;
            SplashLoad.gallery = 2;
            PlayerPrefs.SetInt("gallery", SplashLoad.gallery);
            PlayerPrefs.Save();
        }
        else if (permission == NativeGallery.Permission.Granted && writepermission == NativeGallery.Permission.Granted)
        {
            gallery.sprite = grantedgallery;
            SplashLoad.gallery = 1;
            PlayerPrefs.SetInt("gallery", SplashLoad.gallery);
            PlayerPrefs.Save();

        }
        else
        {
            gallery.sprite = rejectedgallery;
            SplashLoad.gallery = 2;
            PlayerPrefs.SetInt("gallery", SplashLoad.gallery);
            PlayerPrefs.Save();



        }

#endif



    }





    public int checknotify;
    public void firebasecheck()
    {



        Firebase.Messaging.FirebaseMessaging.TokenReceived += TokenRecieved;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += MessageRecieved;
        SubscribeFunc();

        StartCoroutine(check());

    }
    void SubscribeFunc()
    {
        Firebase.Messaging.FirebaseMessaging.SubscribeAsync("/topics/UsamaTKD");
        checknotify = 1;

    }

    bool notificationsEnabled;
    IEnumerator check()
    {


        yield return new WaitForSeconds(5);



        if (notificationsEnabled)
        {

            baselink.firebaseioscheck += 1;
            NOTIFICATION.sprite = grantednotification;
            Debug.Log("Notifications are enabled.");
        }
        else
        {
            NOTIFICATION.sprite = rejectednotification;
            Debug.Log("Notifications are disabled.");
        }





    }
    private void OnApplicationFocus(bool focus)
    {
        if (focus && checknotify > 0)
        {

            baselink.firebaseioscheck += 1;
#if UNITY_IOS
            StartCoroutine(RequestAuthorization());

#endif



        }
    }
    private void MessageRecieved(object sender, MessageReceivedEventArgs e)
    {

        Debug.Log("Nitification Recieved: " + e.Message);
    }

    private void TokenRecieved(object sender, TokenReceivedEventArgs e)
    {






        Debug.Log("Token Recieved: " + e.Token);


    }






#if UNITY_IOS


    IEnumerator RequestAuthorization()
    {
        var authorizationOption = AuthorizationOption.ProvidesAppNotificationSettings | AuthorizationOption.Badge|AuthorizationOption.Alert;
        using (var req = new AuthorizationRequest(authorizationOption, true))
        {
            while (!req.IsFinished)
            {
                yield return null;
            };

            string res = "\n RequestAuthorization:";
            res += "\n finished: " + req.IsFinished;
            notificationsEnabled =  req.Granted;
            res += "\n error:  " + req.Error;
            res += "\n deviceToken:  " + req.DeviceToken;
            Debug.Log(res);

        }
        if (notificationsEnabled)
        {
            NOTIFICATION.sprite = grantednotification;
            Debug.Log("Notifications are enabled.");
        }
        else
        {
            NOTIFICATION.sprite = rejectednotification;
            Debug.Log("Notifications are disabled.");
        }
    }
#else
#endif
    // Update is called once per frame
    void Update()
    {
        if ((SplashLoad.camera == 1 || SplashLoad.camera == 2) && (SplashLoad.gallery == 1 || SplashLoad.gallery == 2) && (SplashLoad.microphone == 1 || SplashLoad.microphone == 2))
        {
#if UNITY_IOS
            if (baselink.firebaseioscheck > 0)
            {
                screenLoader.GetComponent<LevelManager>().loadLevelNow(2);
                //legendpanel.SetActive(true);
                //permissionpanel.SetActive(false);
            }
#else

            screenLoader.GetComponent<LevelManager>().loadLevelNow(2);
            //legendpanel.SetActive(true);
            //permissionpanel.SetActive(false);

#endif
        }





    }
}