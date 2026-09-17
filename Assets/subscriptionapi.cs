using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System;
using UnityEngine.Networking;
public class subscriptionapi : MonoBehaviour
{
    public string baseurl, json2send;
    public static subscriptionapi instance;
    public static string subscriptionid;
    public static int subcount;
    public string subscriptionType;
    public string subscriptionEndDate;
    private void Awake()
    {
        instance = this;

        baseurl = baselink.Url;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(getsubsciptiondata(baseurl + "/v1/subscription"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void subscrioptionbought(string name,string enddate)
    {
        UserSubscriptionState.SetSubscribed(name, enddate);
        NotifySubscriptionStateChanged();
        Sub subscription = new Sub();
        subscription.subscriptionEndDate = enddate;
        subscription.subscriptionType = name;
        json2send = Newtonsoft.Json.JsonConvert.SerializeObject(subscription);

        StartCoroutine(create(baseurl + "/v1/subscription", json2send));


    }

    public void subscrioptionupdated(string name, string enddate)
    {
        UserSubscriptionState.SetSubscribed(name, enddate);
        NotifySubscriptionStateChanged();
        Updated update = new Updated();
        update.id = subscriptionid;
        update.subscriptionEndDate = enddate;
        update.subscriptionType = name;
        json2send = Newtonsoft.Json.JsonConvert.SerializeObject(update);
        Debug.Log("Status Code: " + subscriptionid);
        StartCoroutine(subsciptionupdate(baseurl + "/v1/subscription", json2send));


    }
    IEnumerator  subsciptionupdate(string url, string bodyJsonString)
    {

        var request = new UnityWebRequest(url, "PATCH");

        request.SetRequestHeader("Authorization", "Bearer " + apigetter.jwt);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        // request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        Debug.Log("update: "+request.downloadHandler.text);
  //      result myDeserializedClass = Newtonsoft.Json.JsonConvert.DeserializeObject<result>(request.downloadHandler.text);
  //      if (myDeserializedClass.success)
  //      {


  //      }
    }
    IEnumerator create(string url, string bodyJsonString)
    {

        var request = new UnityWebRequest(url, "POST");

        request.SetRequestHeader("Authorization", "Bearer " + apigetter.jwt);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        // request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
      //  Debug.Log("Status Code: " + request.responseCode);
        Debug.Log(request.downloadHandler.text);
        result myDeserializedClass = Newtonsoft.Json.JsonConvert.DeserializeObject<result>(request.downloadHandler.text);
        if (myDeserializedClass.success)
        {

          
        }
    }


    public class Sub
    {
        public string subscriptionType { get; set; }
        public string subscriptionEndDate { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Updated
    {
        public string id { get; set; }
        public string subscriptionType { get; set; }
        public string subscriptionEndDate { get; set; }
    }


    IEnumerator getsubsciptiondata(string url)
    {
        var request = new UnityWebRequest(url, "GET");
            request.SetRequestHeader("Authorization", "Bearer " + apigetter.jwt);
        //  byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        //  Debug.Log("value: " + bodyJsonString);
        //        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        //        Debug.Log("Status Code: " + request.responseCode);

        Root myDeserializedClass = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(request.downloadHandler.text);
        Debug.Log("counter: " +myDeserializedClass.response.Count+ " get data" + request.downloadHandler.text);

        subscriptiondata = new Root();
        subcount = myDeserializedClass.response.Count;
        subscriptiondata.response = new List<Response>();
        if (myDeserializedClass.response.Count > 0)
        {
            for (int i = 0; i < myDeserializedClass.response.Count; i++)
            {


                subscriptionid = myDeserializedClass.response[0]._id;
                subscriptionType = myDeserializedClass.response[0].subscriptionType;
                subscriptionEndDate = myDeserializedClass.response[0].subscriptionEndDate;
                UserSubscriptionState.SetFromApi(subscriptionType, subscriptionEndDate);
                NotifySubscriptionStateChanged();


                /*
                 
                  Response X = new Response();
                    X._id = myDeserializedClass.response[i]._id;
                    X.subscriptionType = myDeserializedClass.response[i].subscriptionType;
                X.subscriptionEndDate = myDeserializedClass.response[i].subscriptionEndDate;
                X.user = myDeserializedClass.response[i].user;
       X.createdAt = myDeserializedClass.response[i].createdAt;
                X.updatedAt = myDeserializedClass.response[i].updatedAt;
                X.__v = myDeserializedClass.response[i].__v;

        subscriptiondata.response.Add(X);
                
                 
                 */
            }
        }
        else
        {
            DateTime current;
            current = DateTime.Now;
            subscriptionEndDate = current.ToString();
            UserSubscriptionState.SetUnsubscribed();
            NotifySubscriptionStateChanged();

        }

    }



    public class result
    {
        public bool success { get; set; }
        public string response { get; set; }
        public object metadata { get; set; }
    }

    private void NotifySubscriptionStateChanged()
    {
        if (eventgetter.instance != null)
        {
            eventgetter.instance.RefreshSubscriptionStateFromApi();
        }
    }

    public class Response
    {
        public string _id { get; set; }
        public string subscriptionType { get; set; }
        public string subscriptionEndDate { get; set; }
        public string user { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int __v { get; set; }
    }


    public Root subscriptiondata;
    public class Root
    {
        public bool success { get; set; }
        public List<Response> response { get; set; }
        public object metadata { get; set; }
    }


}
