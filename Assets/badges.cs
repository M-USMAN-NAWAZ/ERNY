using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static badges;

public class badges : MonoBehaviour
{
    public static badges instance;
    public UnityEngine.UI.Button badgeee;
    CultureInfo provider = CultureInfo.InvariantCulture;
    public string[] Badgesname;
    public GameObject[] Badges;
    public GameObject badgess;
    public ScrollRect badgetospawn;
    public string baseurl;
    public int check;
    public ScrollRect badgesscroll;
    Vector2 singleitemsize;
    public ScrollRect myScrollRect;
    public Text badgesno;
    public RectTransform newscroller;
    public static int previousvalue, currentvalue, condition;
    public AudioSource blingsound;
    public GameObject notifybadge;
    int coondition;

    public static int badgecheck = 0;

    public GameObject defaultBadge;

    public int totalBadges;
    public Text totalBadgesText;

    IEnumerator disable()
    {
        yield return new WaitForSeconds(1.5f);
        badgeee.interactable = true;
    }



    public void badgescroller()
    {
        badgeee.interactable = false;
        if (badgesscroll.content.childCount > 1)
        {
            for (int i = 0; i < badgesscroll.content.childCount; i++)
            {
                Destroy(badgesscroll.content.GetChild(i).gameObject);

            }

            badgesscroll.content.sizeDelta = singleitemsize;

        }
        getbadges();
        StartCoroutine(disable());
    }
    public void deletebadgeevent()
    {

        badgesscroll.content.sizeDelta = singleitemsize;
        if (badgesscroll.content.childCount > 1)
        {
            for (int i = 0; i < badgesscroll.content.childCount; i++)
            {
                Destroy(badgesscroll.content.GetChild(i).gameObject);
            }


            getbadges();
            //        newscroller.sizeDelta = singleitemsize;


        }
    }
    public void badgescrollera()
    {

        badgesscroll.content.sizeDelta = singleitemsize;
        if (badgesscroll.content.childCount > 1)
        {
            for (int i = 0; i < badgesscroll.content.childCount; i++)
            {
                Destroy(badgesscroll.content.GetChild(i).gameObject);
            }


            getbadges();
            //        newscroller.sizeDelta = singleitemsize;
         
            Debug.Log("working sound");

        }

        

        StartCoroutine(delay());



    }
    IEnumerator delay()
    {

        yield return new WaitForSeconds(3f);
        Debug.Log(" delay current value: " + currentvalue + " previous value: " + previousvalue);

        if (currentvalue > previousvalue)
        {
            coondition = 1;
            PlayerPrefs.SetInt("condition", 1);
            PlayerPrefs.Save();
            previousvalue = currentvalue;
            blingsound.Play();
            notifybadge.SetActive(true);
            Debug.Log("working sound");
        }

    

    }
    public class EVENtreciver
    {
        public string id { get; set; }

    }
    public void resetscroll()
    {
        myScrollRect.verticalNormalizedPosition = 1f;
    }





    public void badgecount()
    {
        
        EVENtreciver getdata = new EVENtreciver();
        getdata.id = apigetter.id;
        Debug.Log(apigetter.id);
        String JsonVal = Newtonsoft.Json.JsonConvert.SerializeObject(getdata);
        //  Debug.Log("json val " + JsonVal);
        StartCoroutine(getbadescount(baseurl + "/v1/badges"));
    }





    IEnumerator getbadescount(string url)
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

        //Debug.Log("get data" + request.downloadHandler.text);
        Root myDeserializedClass = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(request.downloadHandler.text);
        Debug.Log("badges: " + myDeserializedClass.response.badges.Count);
        badgesno.text = "" + myDeserializedClass.response.badges.Count;

        currentvalue = myDeserializedClass.response.badges.Count;
        if (condition == 0)
        {
            previousvalue = myDeserializedClass.response.badges.Count;
            condition += 1;
        }


        Debug.Log("current value: "+currentvalue+" previous value: "+previousvalue);

    }

    public void deletebadgecount()
    {
        EVENtreciver getdata = new EVENtreciver();
        getdata.id = apigetter.id;
        Debug.Log(apigetter.id);
        String JsonVal = Newtonsoft.Json.JsonConvert.SerializeObject(getdata);
        //  Debug.Log("json val " + JsonVal);
        StartCoroutine(delgetbadescount(baseurl + "/v1/badges"));
    }
    IEnumerator delgetbadescount(string url)
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

       Debug.Log("get data" + request.downloadHandler.text);
        Root myDeserializedClass = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(request.downloadHandler.text);
        Debug.Log("badges: " + myDeserializedClass.response.badges.Count);
        badgesno.text = "" + myDeserializedClass.response.badges.Count;

        currentvalue = myDeserializedClass.response.badges.Count;

        previousvalue = myDeserializedClass.response.badges.Count;

    }





    public void turnoffnotification()
    {
        coondition = 0;
        PlayerPrefs.SetInt("condition", coondition);
        PlayerPrefs.Save();
    }







    public bool showPayWallOnce = false;
    public List<GameObject> fakeBtns = new List<GameObject>();

    public void GetBadgeCount()
    {
        StartCoroutine(GetBadgeCountCoroutine());
    }

    IEnumerator GetBadgeCountCoroutine()
    {
        string url = baseurl + "/v1/badges/count";

        UnityWebRequest request = new UnityWebRequest(url, "GET");

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + apigetter.jwt);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Badge Count Response: " + request.downloadHandler.text);

            BadgeCountResponse data =
                Newtonsoft.Json.JsonConvert.DeserializeObject<BadgeCountResponse>(request.downloadHandler.text);

            totalBadges = data.response.count;
            totalBadgesText.text = totalBadges.ToString();
            Debug.Log("Total Badges: " + data.response.count);
        }
        else
        {
            Debug.Log("Badge Count Error: " + request.error);
        }

         if(PlayerPrefs.GetInt("OneTimeAppear", 0) == 1)
        {
            //Debug.Log("one time appear");
            //screencurenter(0);
            //freetrialsubscription.SetActive(false);
           
              Debug.Log("1badges.currentvalue: " + badges.currentvalue);
        }
        else if(PlayerPrefs.GetInt("OneTimeAppear", 0) == 0 && badges.currentvalue <= 1)
        {
            showPayWallOnce = true;
            // currentscreen = 3;
            eventgetter.instance.screencurenter(2);
            // profile.SetActive(true);
            // freetrialsubscription.SetActive(true);
            PlayerPrefs.SetInt("OneTimeAppear", 1);
            Debug.Log("badges.currentvalue: " + badges.currentvalue);
        }
        else
        {
            
        }
    }


    public void OpenPayWall()
    {
       
            for(int i = 0; i <= fakeBtns.Count-1; i++)
            {
                fakeBtns[i].SetActive(false);
            }
            eventgetter.instance.HalfpricePanel.SetActive(true);
            showPayWallOnce = false;
            
            PlayerPrefs.SetInt("OneTimeAppear", 1);
        
    }






    public void getbadges()
    {
        test = 0;
        EVENtreciver getdata = new EVENtreciver();
        getdata.id = apigetter.id;
        Debug.Log(apigetter.id);
        String JsonVal = Newtonsoft.Json.JsonConvert.SerializeObject(getdata);
      //  Debug.Log("json val " + JsonVal);
        StartCoroutine(getevetdata2(baseurl + "/v1/badges"));
    }
    int test;
   
    IEnumerator getevetdata2(string url)
    {
        int page = 1;
        bool hasNextPage = true;

        while (hasNextPage)
        {
            string finalUrl = url + "?page=" + page + "&limit=40&paginate=true";

            //var request = new UnityWebRequest(url, "GET");
            var request = new UnityWebRequest(finalUrl, "GET");
            request.SetRequestHeader("Authorization", "Bearer " + apigetter.jwt);
          //  byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
            //  Debug.Log("value: " + bodyJsonString);
    //        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
    //        Debug.Log("Status Code: " + request.responseCode);

            Debug.Log("getter data" + request.downloadHandler.text);
            Root myDeserializedClass = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(request.downloadHandler.text);
            currentvalue = myDeserializedClass.response.badges.Count;
            Debug.Log("badges: "+myDeserializedClass.response.badges.Count);
            int c = myDeserializedClass.response.badges.Count;
            for (int i = 0; i < myDeserializedClass.response.badges.Count; i++)
            {

                //  GameObject obj = Instantiate(badgess, badgetospawn.content);
                //          rg = new Root();

                //            rg.response = new List<Response>();

                c++;
                if (c > 2)
                {
                    c = 0;
                    //test = 0;
                    badgesscroll.content.sizeDelta += new Vector2(badgesscroll.content.sizeDelta.x, singleitemsize.y*1.1f);
                    Debug.Log("yaho " + singleitemsize.y);
                }
                bool badgeMatched = false;

                for (int j = 0; j < Badgesname.Length; j++)

                {

                    if (myDeserializedClass.response.badges[i].badgeDefinition != null && Badgesname[j] == myDeserializedClass.response.badges[i].badgeDefinition.displayName)
                    {

                       


                        badgeMatched = true;

                        //defaultBadge = Badges[j];
                        
                        
                        if (Badgesname[j] == "Blingar Sign Up")
                        {
                            GameObject obj = Instantiate(Badges[j], badgetospawn.content);
                            string x = myDeserializedClass.response.badges[i].badgeDate;
                            DateTime result = new DateTime();
                            x = x.Remove(x.IndexOf(" GMT"));
                            result = Convert.ToDateTime(x);
                           obj.transform.GetChild(1).GetComponent<Text>().text = result.Month + "/" + result.Day + "/" + result.Year;
						    //obj.transform.GetChild(1).GetComponent<Text>().text = myDeserializedClass.response[i].badgeDate;

                            if(PlayerPrefs.GetInt("AutoOnce", 0) == 0 && currentvalue <= 1)
                            {
                                eventgetter.instance.screencurenter(2);
                                eventgetter.instance.panels[2].SetActive(true);
                                eventgetter.instance.DeactivateIcons();
                                eventgetter.instance.icons[2].SetActive(true);
                                Debug.Log("currentvalue is: " + currentvalue);
                                obj.GetComponent<badgetexthandler>().showtext();
                                PlayerPrefs.SetInt("AutoOnce", 1);
                                //eventgetter.instance.HalfpricePanel.SetActive(true);
                                for(int k = 0; k <= fakeBtns.Count-1; k++)
                                {
                                    fakeBtns[k].SetActive(true);
                                }
                            }
						   
                        }
                        else
                        {
                            GameObject obj = Instantiate(Badges[j], badgetospawn.content);

                            obj.transform.GetComponent<badgetexthandler>().eventname = myDeserializedClass.response.badges[i].eventName;

                            Debug.Log("Event name is this: " + myDeserializedClass.response.badges[i].eventName);


                            obj.transform.GetChild(1).GetComponent<Text>().text = myDeserializedClass.response.badges[i].badgeDate;

                            if (obj.transform.GetComponent<badgetexthandler>().URL == null)
                            {

                            }
                            else
                            {
                                obj.transform.GetComponent<badgetexthandler>().id = myDeserializedClass.response.badges[i]._id;
                            
                                if (myDeserializedClass.response.badges[i].badgeDefinition != null && myDeserializedClass.response.badges[i].badgeDefinition.category == 1)
                                {
                                    //Debug.Log("This is a category 1 badge!!!");
                                    if (myDeserializedClass.response.badges[i].badgeDefinition != null && myDeserializedClass.response.badges[i].badgeDefinition.image != null)
                                    {
                                        //Debug.Log("image URL is: " + myDeserializedClass.response.badges[i].badgeName);
                                        obj.transform.GetComponent<badgetexthandler>().URL = myDeserializedClass.response.badges[i].badgeDefinition.image;
                                        
                                    }
                                    else if(myDeserializedClass.response.badges[i].badgeDefinition.popupText != null)
                                    {
                                        obj.transform.GetComponent<badgetexthandler>().heading = myDeserializedClass.response.badges[i].badgeDefinition.popupText;
                                    }
                                    else
                                    {
                                        //Debug.Log("no image found for badge: " + myDeserializedClass.response.badges[i].badgeName);
                                    }
                                }
                            }

                        }

                      //      string x = myDeserializedClass.response[i].badgeDate;
                      //      DateTime result = new DateTime();

                      //      string format = "ddd dd MMM yyyy hh:mm";
                      //      x = x.Remove(x.IndexOf(" GMT"));

                    //        result = Convert.ToDateTime(x);
                            //    Debug.Log(result.Day + "  re  " + result.ToShortTimeString());

                            //  Debug.Log("SUCESSFULLY CONVERTED :"+ DateTime.TryParseExact(x, format, provider.DateTimeFormat, DateTimeStyles.AdjustToUniversal, out result));

                         //   Badges[j].transform.GetChild(1).GetComponent<Text>().text = result.Day + "/" + result.Month + "/" + result.Year;

                            //  Debug.Log(result.Day+"    "+ result.ToShortTimeString());
                    }
                    else
                    {
                       
                    }

                

                }

                Debug.Log($"badgeMatched: {badgeMatched}");

                if (myDeserializedClass != null && myDeserializedClass.response.badges[i].badgeDefinition != null)
                {
                    Debug.Log($"myDeserializedClass.response.badges: {myDeserializedClass.response.badges[i]}");
                    Debug.Log($"myDeserializedClass.response.badges.img: {myDeserializedClass.response.badges[i].badgeDefinition.image}");
                }

                if (myDeserializedClass.response.badges[i].badgeDefinition != null && !badgeMatched &&
                    myDeserializedClass.response.badges[i].badgeDefinition.image != null)
                {
                    GameObject obj = Instantiate(defaultBadge, badgetospawn.content);

                    obj.transform.GetComponent<badgetexthandler>().eventname = myDeserializedClass.response.badges[i].eventName;




                    obj.transform.GetChild(1).GetComponent<Text>().text = myDeserializedClass.response.badges[i].badgeDate;

                    if (obj.transform.GetComponent<badgetexthandler>().URL == null)
                    {

                    }
                    else
                    {
                        obj.transform.GetComponent<badgetexthandler>().id = myDeserializedClass.response.badges[i]._id;

                        if (myDeserializedClass.response.badges[i].badgeDefinition != null && myDeserializedClass.response.badges[i].badgeDefinition.category == 1)
                        {
                            Debug.Log("This is a category 1 badge!!!");
                            if (myDeserializedClass.response.badges[i].badgeDefinition != null && myDeserializedClass.response.badges[i].badgeDefinition.image != null)
                            {
                                Debug.Log("Badge name is: " + myDeserializedClass.response.badges[i].badgeName);
                                obj.transform.GetComponent<badgetexthandler>().URL = myDeserializedClass.response.badges[i].badgeDefinition.image;
                                //obj.transform.GetComponent<badgetexthandler>().heading = myDeserializedClass.response.badges[i].badgeDefinition.popupText;

                                if (myDeserializedClass.response.badges[i].badgeDefinition.popupText != null)
                                {
                                    obj.transform.GetComponent<badgetexthandler>().heading = myDeserializedClass.response.badges[i].badgeDefinition.popupText;
                                }
                                    
                                obj.transform.GetComponent<badgetexthandler>().header = obj.transform.GetComponent<badgetexthandler>().heading;
                                string temp = "Congrats on crushing the \r\n" + obj.transform.GetComponent<badgetexthandler>().eventname;
                                obj.transform.GetComponent<badgetexthandler>().explain = temp;
                            }
                            else
                            {
                                Debug.Log("no image found for badge: " + myDeserializedClass.response.badges[i].badgeName);
                            }
                        }
                    }
                }



            }
            // Pagination control
            if (myDeserializedClass.response.pagination != null &&
                myDeserializedClass.response.pagination.hasNextPage)
            {
                page++;
            }
            else
            {
                hasNextPage = false;
            }


        }

        GetBadgeCount();
    }

 

    private void Awake()
    {
        instance = this;

        baseurl = baselink.Url;
        badgecount();
        coondition = PlayerPrefs.GetInt("condition", 0);
        singleitemsize = badgesscroll.content.sizeDelta;
        badgescrollera();
          if(PlayerPrefs.GetInt("OneTimeAppear", 0) == 1)
        {
            for(int i = 0; i <= badges.instance.fakeBtns.Count-1; i++)
            {
                badges.instance.fakeBtns[i].SetActive(false);
            }
        }
       
    }
    // Start is called before the first frame update
    void Start()
    {
        test = 0;
        
      // getbadges();
     }

    public class Badge
    {
        public string _id { get; set; }
        public string badgeName { get; set; }
        public string type { get; set; }
        public string badgeDate { get; set; }
        public string depId { get; set; }
        public string year { get; set; }
        public string user { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int __v { get; set; }
        public string eventName { get; set; }
        public BadgeDefinition badgeDefinition { get; set; }
    }


    [System.Serializable]
    public class BadgeCountResponse
    {
        public bool success;
        public BadgeCountData response;
    }

    [System.Serializable]
    public class BadgeCountData
    {
        public int count;
    }


    public class BadgeDefinition
    {
        public string _id { get; set; }
        public string name { get; set; }
        public string displayName { get; set; }
        public string description { get; set; }
        public string popupText { get; set; }
        public string image { get; set; }
        public int category { get; set; }
    }

    public class Pagination
    {
        public int page { get; set; }
        public int limit { get; set; }
        public int total { get; set; }
        public int totalPages { get; set; }
        public bool hasNextPage { get; set; }
        public bool hasPrevPage { get; set; }
    }


    public class Response
    {
  //      public string _id { get; set; }
  //         public string eventName { get; set; }
		//public string badgeName { get; set; }
  //      public string type { get; set; }
  //      public string badgeDate { get; set; }
  //      public string user { get; set; }
  //      public DateTime createdAt { get; set; }
  //      public DateTime updatedAt { get; set; }
  //      public int __v { get; set; }

        public List<Badge> badges { get; set; }
        public Pagination pagination { get; set; }

    }

    public class Root
    {
        public bool success { get; set; }
        //public List<Response> response { get; set; }
        public Response response { get; set; }

        public object metadata { get; set; }
    }

    public Root rg;

    // Update is called once per frame
    void Update()
    {
        if (coondition == 1)
        {

            notifybadge.SetActive(true);
        }
        else
        {
            notifybadge.SetActive(false);


        }
    }
}
