using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class MicrophonePermissionManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }



    public void askforpermission()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
           // Permission.RequestUserPermission(Permission.Microphone);
        }

    }

}
