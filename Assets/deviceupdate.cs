using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class deviceupdate : MonoBehaviour
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Response
    {
        public string device { get; set; }
        public string version { get; set; }
    }

    public class Root
    {
        public bool success { get; set; }
        public Response response { get; set; }
        public object metadata { get; set; }
    }





    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Device
    {
        public string device { get; set; }
        public string version { get; set; }
    }



    string json2send;
    string baseurl;
   
	private void Start()

    {
        baseurl = baselink.Url;

        Device data = new Device();


#if (UNITY_IOS && !UNITY_EDITOR)
        data.device = "IOS";
#endif


#if (PLATFORM_ANDROID || UNITY_EDITOR)
        data.device = "Android";
#endif
       /// data.device = "Android";
        data.version =ApiRequestGenerator.versiontosend;

        json2send = Newtonsoft.Json.JsonConvert.SerializeObject(data);

        StartCoroutine(deviceupload(baseurl + "/v1/user/device", json2send));

        Debug.Log("devia: " +baseurl+ "/v1/user/device");






    }

    IEnumerator deviceupload(string url,string bodyJsonString)
    {

        var request = new UnityWebRequest(url, "PATCH");

        request.SetRequestHeader("Authorization", "Bearer " + apigetter.jwt);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        // request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
    
        Debug.Log("device Status Code: " + request.responseCode);
        Debug.Log("device data: "+request.downloadHandler.text);
        //  Username.text = apigetter.firstname + " " + apigetter.lastname;
       Root myDeserializedClass = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(request.downloadHandler.text);


        Debug.Log("device data: after " + myDeserializedClass.response.device);
    }




}
