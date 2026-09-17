using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class confirmation : MonoBehaviour
{
    public string baseurl;
    public string id;
    public GameObject panel,myevent;
    public class Response
    {
        public bool acknowledged { get; set; }
        public int deletedCount { get; set; }
    }
    public class delrecieve
    {
        public bool success { get; set; }
        public Response response { get; set; }
        public object metadata { get; set; }
    }
    public void deletedata()
    {

        deleteevent deldata = new deleteevent();
        deldata.id = id;
      //  Debug.Log(apigetter.id);
        String JsonVal = Newtonsoft.Json.JsonConvert.SerializeObject(deldata);
        Debug.Log("json val " + JsonVal);
        Debug.Log("json val " + JsonVal);

        StartCoroutine(deletdataa(baseurl + "/v1/event", JsonVal));
    }
    public class deleteevent
    {
        public string id { get; set; }
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

        delrecieve myDeserializedClass = Newtonsoft.Json.JsonConvert.DeserializeObject<delrecieve>(request.downloadHandler.text);

        if (myDeserializedClass.response.deletedCount > 0)
        {
            Destroy(myevent);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

        baseurl = baselink.Url;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
