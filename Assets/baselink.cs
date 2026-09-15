using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class baselink : MonoBehaviour
{

    public string liveurl;
    public string testurl = "https://api-stage.blingar.cloud";
    public static int firebaseioscheck;

    public Toggle check;
    public static string Url;
    // Start is called before the first frame update
    void Awake()
    {
        firebaseioscheck = 0;
        
        if (check.isOn)
        {
            Url = liveurl;
        }
        else
        {
            Url = testurl;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
