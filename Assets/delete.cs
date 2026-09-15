using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class delete : MonoBehaviour
{


    public class Response
    {
        public bool acknowledged { get; set; }
        public int deletedCount { get; set; }
    }

    public class Root
    {
        public bool success { get; set; }
        public Response response { get; set; }
        public object metadata { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
   
    /*
    public void deletedata()
    {

        Root deldata = new Root();
        deldata._id = "62ea84030fdbca21ee330ea4";
        Debug.Log(apigetter.id);
        String JsonVal = Newtonsoft.Json.JsonConvert.SerializeObject(deldata);
        Debug.Log("json val " + JsonVal);
        StartCoroutine(deletdataa(baseurl + "/v1/badges", JsonVal));
    }



    IEnumerator deletdataa(string url, string bodyJsonString)
    {
        WWWForm form = new WWWForm();
        Dictionary<string, string> headers = form.headers;
        headers["JWT TOKEN"] = apigetter.jwt;
        var request = new UnityWebRequest(url, "DELETE");
        request.SetRequestHeader("Authorization", "Bearer " + apigetter.jwt);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        // request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        //Debug.Log("Status Code: " + request.responseCode);
        Debug.Log(request.downloadHandler.text);
    }*/

    // Start is called before the first frame update
    void Start()
    {

   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
