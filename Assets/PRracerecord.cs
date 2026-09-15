using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static PRracerecord;


public class PRracerecord : MonoBehaviour
{
    public List<GameObject> allEvents = new List<GameObject>();

    public List<eventdata2send> K5 = new List<eventdata2send>();
    public List<eventdata2send> K10 = new List<eventdata2send>();
    public List<eventdata2send> K15 = new List<eventdata2send>();
    public List<eventdata2send> K13 = new List<eventdata2send>();
    public List<eventdata2send> K26 = new List<eventdata2send>();
    public List<eventdata2send> M10 = new List<eventdata2send>();

    public bool k5Done = false;
    public bool k10Done = false;
    public bool k15Done = false;
    public bool k13Done = false;
    public bool k26Done = false;
    public bool M10Done = false;
     
    public static PRracerecord instance;
    public string baseurl;

    public static int keptdistance;

    public int rewardCount; 

    public GameObject recordata;
    public RectTransform recordspawn;
    public RectTransform recordsize,originalrecordsize;
    public Vector2 sizeofrecord;
    public ScrollRect profilescroll,originalscroll;
    public Toggle kmtoogle;
    List<string> kmdata= new List<string>();
    List<string> milesdata = new List<string>();

    public List<GameObject> instantiatedObjs = new List<GameObject>();

    public Sprite[] recordimages;

    public GameObject firstrecord;
    public List<eventdata2send> reacerecords = new List<eventdata2send>();

    public List<eventdata2send> mydata = new List<eventdata2send>();
    public class Goal
    {
        public string achieved { get; set; }

        public string goal { get; set; }
    }

    public class Distance
    {
        public string istance { get; set; }
        public string unit { get; set; }
    }

    public class Journal
    {
        public string pre { get; set; }
        public string race { get; set; }
        public string post { get; set; }
        public string Next { get; set; }
        public string Notes { get; set; }
    }

    public class Root
    {
        public bool success { get; set; }
        public List<eventdata2send> response { get; set; }
        public object metadata { get; set; }
    }

    public class Link
    {
        public string title { get; set; }
        public string url { get; set; }
    }

    public class eventdata2send
    {
        public string position { get; set; }
        public string totalpeople { get; set; }
        public string _id { get; set; }
        public string eventName { get; set; }
        public string eventDate { get; set; }
        public string city { get; set; }
        public string victoryCity { get; set; }
        public string time { get; set; }
        public string race { get; set; }
        public Distance distance { get; set; }
        public string @virtual { get; set; }
        public string image { get; set; }
        public string theme { get; set; }
        public List<Goal> goals { get; set; }
        public Result result { get; set; }
        public List<Link> links { get; set; }
        public Journal journal { get; set; }
        public string achivement { get; set; }
        public string eventType { get; set; }
        public string personarecord { get; set; }
        public string user { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int __v { get; set; }
        public int value { get; set; }
        public int timevalue { get; set; }
        public string showdistance { get; set; }

    }

    public class timedistance
    {

        public string distance, time;

    }

    public class Result
    {
        public string myAgeGroup { get; set; }
        public string totalAgeGroup { get; set; }
        public string myGender { get; set; }
        public string totalGender { get; set; }
    }

    Root data = new Root();

    public class EVENtreciver
    {
        public string id { get; set; }

    }

    public void senddata()
    {
        RectTransform cre;
        cre = originalrecordsize.GetComponent<RectTransform>();
        //  cre.sizeDelta = new Vector2(cre.sizeDelta.x, cre.sizeDelta.y);
        recordsize.GetComponent<RectTransform>().sizeDelta = new Vector2(cre.sizeDelta.x,190);
        recordsize.gameObject.GetComponent<LayoutElement>().preferredHeight = 190;
        Debug.Log("chcek scroll size: "+ recordsize.gameObject.GetComponent<LayoutElement>().preferredHeight);
        EVENtreciver getdata = new EVENtreciver();
        getdata.id = apigetter.id;
        Debug.Log(apigetter.id);
        string JsonVal = Newtonsoft.Json.JsonConvert.SerializeObject(getdata);
        Debug.Log("json val " + JsonVal);
        StartCoroutine(getevetdata2(baseurl + "/v1/event/user?type=des&sort=eventDate", ""));

    }
    int b;
    string[] textsplit;
    string[] textsplit2;
    IEnumerator getevetdata2(string url, string bodyJsonString)
    {

        var request = new UnityWebRequest(url, "POST");
        request.SetRequestHeader("Authorization", "Bearer " + apigetter.jwt);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        //  Debug.Log("value: " + bodyJsonString);
        //    request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);

        Debug.Log("get data" + request.downloadHandler.text);

       
        int check = 0;
        reacerecords.Clear();
        mydata.Clear();
        kmdata.Clear();
        profilescroll.content.sizeDelta = originalscroll.content.sizeDelta;
        milesdata.Clear();
        Root myDeserializedClass = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(request.downloadHandler.text);
     
        for (int i = 0; i < myDeserializedClass.response.Count; i++)
        {
           
            Root test = new Root();
            test = myDeserializedClass;

           


            check += 1;


            Debug.Log("replacev  "+ test.response[i].eventName);
            mydata.Add(test.response[i]);

            reacerecords.Add(test.response[i]);
            Debug.Log("mydata[i].distance.istance is "+mydata.Count);

            if (test.response[i].distance.istance == "00.00.00")
            {
                string temp = "00.00.00";
                textsplit = temp.Split(char.Parse("."));
            }
            else
            {
                textsplit = test.response[i].distance.istance.Split(char.Parse("."));
            }



            string distance = textsplit[0] + textsplit[1] + textsplit[2];

            bool mile = false;
            if (test.response[i].distance.unit == "mi")
            {
                mile = true;
            }
            else
            {
                mile = false;
            }

            switch (test.response[i].eventType)
            {
                case "COMBO":


                    if (mile && textsplit[1] == "10")
                    {
                        
                    }
                    else if (!mile && textsplit[1] == "16")
                    {
                        
                    }
                    else
                    {
                        if(check <= 0)
                        {
                            mydata.Remove(test.response[i]);
                        }
                        else
                        {
                            check -= 1;
                            mydata.Remove(test.response[i]);
                        }
                    }


                    break;

                case "OBSTACLE":


                    if (mile && textsplit[1] == "10")
                    {
                        
                    }
                    else if (!mile && textsplit[1] == "16")
                    {
                       
                    }
                    else
                    {
                        if (check <= 0)
                        {
                            mydata.Remove(test.response[i]);
                        }
                        else
                        {
                            check -= 1;
                            mydata.Remove(test.response[i]);
                        }
                    }

                    break;

                case "FUN RUN":


                    if (mile && textsplit[1] == "10")
                    {
                        
                    }
                    else if (!mile && textsplit[1] == "16")
                    {
                        
                    }
                    else
                    {
                        if (check <= 0)
                        {
                            mydata.Remove(test.response[i]);
                        }
                        else
                        {
                            check -= 1;
                            mydata.Remove(test.response[i]);
                        }
                    }

                    break;

                case "OTHER":


                    if (mile && textsplit[1] == "10")
                    {
                        
                    }
                    else if (!mile && textsplit[1] == "16")
                    {
                       
                    }
                    else
                    {
                        if (check <= 0)
                        {
                            mydata.Remove(test.response[i]);
                        }
                        else
                        {
                            check -= 1;
                            mydata.Remove(test.response[i]);
                        }
                    }

                    break;



            }


            for (int l = 0; l < recordspawn.childCount; l++)
                {

                    Destroy(recordspawn.GetChild(l).gameObject);
                    instantiatedObjs.Remove(recordspawn.GetChild(l).gameObject);

                }

            
            /*



                  textsplit = myDeserializedClass.response[i].distance.istance.Split(char.Parse("."));
                  string distance = textsplit[0] + textsplit[1] + textsplit[2];
                  b = Convert.ToInt16(distance);

                  for (int j = i; j < myDeserializedClass.response.Count; j++)
                  {


                      textsplit = myDeserializedClass.response[j].distance.istance.Split(char.Parse("."));
                      string distancee = textsplit[0] + textsplit[1] + textsplit[2];
                      int bb = Convert.ToInt16(distancee);
                      Debug.Log("replace: "+bb);
                      textsplit = myDeserializedClass.response[i].distance.istance.Split(char.Parse("."));
                      string distance3 = textsplit[0] + textsplit[1] + textsplit[2];
                      b = Convert.ToInt16(distance3);
                      if (myDeserializedClass.response[j].personarecord == "yes")
                      {
                          int count = 0;
                          if (b == bb)
                          {


                              textsplit = myDeserializedClass.response[i].time.Split(char.Parse(":"));

                              string time = textsplit[0] + textsplit[1] + textsplit[2];
                              int t = Convert.ToInt16(time);



                              textsplit = myDeserializedClass.response[j].time.Split(char.Parse(":"));

                              string time2 = textsplit[0] + textsplit[1] + textsplit[2];
                              int t2 = Convert.ToInt16(time2);

                              if (t > t2)
                              {
                                  count += 1;
                                  Debug.Log("to be replace data: " + t + " " + t2 + "name: " + myDeserializedClass.response[i].eventName);
                                  myDeserializedClass.response.RemoveAt(i);
                                  myDeserializedClass.response[i] = myDeserializedClass.response[j];
                                  textsplit = myDeserializedClass.response[i].distance.istance.Split(char.Parse("."));
                                  string distance4 = textsplit[0] + textsplit[1] + textsplit[2];
                                  b = Convert.ToInt16(distance4);
                                  Debug.Log("to be replace change:  " + myDeserializedClass.response[i].eventName);
                                  mydata.Add(myDeserializedClass.response[i]);
                                  Debug.Log("to be replace added:  " + myDeserializedClass.response[i].eventName + " count: " + mydata.Count);

                              }
                          }
                          if (b > bb)
                          {

                              myDeserializedClass.response[i] = null;

                              myDeserializedClass.response[i] = myDeserializedClass.response[j];
                              Debug.Log("replace data: " + myDeserializedClass.response[i].distance.istance);



                          }


                              if (count == 1)
                              {
                                  mydata.Insert(mydata.Count - 1, myDeserializedClass.response[i]);
                                  Debug.Log("to be replace insert:  " + myDeserializedClass.response[i].eventName + " count: " + mydata.Count);
                              }
                              else
                              {

                                  mydata.Add(myDeserializedClass.response[i]);
                                  Debug.Log("to be replace added:  " + myDeserializedClass.response[i].eventName + " count: " + mydata.Count);

                              }






                      }


                      if (j == myDeserializedClass.response.Count - 1)
                      {



                      }









                  }*/
            /*  if (checksize == 0)
              {
                  if (myDeserializedClass.response.Count > 3)
                  {
                      int c = myDeserializedClass.response.Count;// - 3;

                      Eventscrol.content.sizeDelta = new Vector2(eventscroll.content.sizeDelta.x, singleitemsize.y * c);
                      if (c > 9)
                      {
                          Eventscrol.content.sizeDelta += new Vector2(eventscroll.content.sizeDelta.x, singleitemsize.y * 3);
                      }
                  }
                  Debug.Log("yaho " + singleitemsize.y);
                  checksize = 1;
            */
        }

       

        if (check == 0)
        {
            if (check == 0)
            {
                for (int l = 0; l < recordspawn.childCount; l++)
                {

                    Destroy(recordspawn.GetChild(l).gameObject);
                    instantiatedObjs.Remove(recordspawn.GetChild(l).gameObject);

                }
                Instantiate(firstrecord, recordspawn);
                instantiatedObjs.Add(firstrecord);

            }
            /*
                        GameObject obj = Instantiate(recordata, recordspawn);
                        obj.GetComponent<MyEventData>().recorddata.text = myDeserializedClass.response[i].distance.istance + "         " + myDeserializedClass.response[i].time;
                        obj.GetComponent<MyEventData>().myeventclass = new MyEventData.MyEvent();
                        obj.GetComponent<MyEventData>().myeventclass.links = new List<MyEventData.Link>();
                        obj.GetComponent<MyEventData>().myeventclass.journal = new MyEventData.Journal();
                        obj.GetComponent<MyEventData>().myeventclass.distance = new MyEventData.Distance();
                        obj.GetComponent<MyEventData>().myeventclass.goals = new List<MyEventData.Goal>();
                        obj.GetComponent<MyEventData>().myeventclass.result = new MyEventData.Result();
                        obj.GetComponent<MyEventData>().myeventclass.position = myDeserializedClass.response[i].position;

                        obj.GetComponent<MyEventData>().myeventclass.totalpeople = myDeserializedClass.response[i].totalpeople;
                        obj.GetComponent<MyEventData>().myeventclass.eventType = myDeserializedClass.response[i].eventType;
                        obj.GetComponent<MyEventData>().myeventclass.id = myDeserializedClass.response[i]._id;

                        obj.GetComponent<MyEventData>().myeventclass.city = myDeserializedClass.response[i].city;
                        obj.GetComponent<MyEventData>().myeventclass.race = myDeserializedClass.response[i].race;
                        obj.GetComponent<MyEventData>().myeventclass.result.myGender = myDeserializedClass.response[i].result.myGender;
                        obj.GetComponent<MyEventData>().myeventclass.result.myAgeGroup = myDeserializedClass.response[i].result.myAgeGroup;
                        obj.GetComponent<MyEventData>().myeventclass.result.totalGender = myDeserializedClass.response[i].result.totalGender;
                        obj.GetComponent<MyEventData>().myeventclass.result.totalAgeGroup = myDeserializedClass.response[i].result.totalAgeGroup;



                        obj.GetComponent<MyEventData>().myeventclass.eventName = myDeserializedClass.response[i].eventName;
                        obj.GetComponent<MyEventData>().myeventclass.eventDate = myDeserializedClass.response[i].eventDate;
                        obj.GetComponent<MyEventData>().myeventclass.victoryCity = myDeserializedClass.response[i].victoryCity;
                        obj.GetComponent<MyEventData>().myeventclass.time = myDeserializedClass.response[i].time;
                        obj.GetComponent<MyEventData>().myeventclass.distance.istance = myDeserializedClass.response[i].distance.istance;
                        obj.GetComponent<MyEventData>().myeventclass.personarecord = myDeserializedClass.response[i].personarecord;
                        obj.GetComponent<MyEventData>().myeventclass.distance.unit = myDeserializedClass.response[i].distance.unit;


                        obj.GetComponent<MyEventData>().myeventclass.@virtual = myDeserializedClass.response[i].@virtual;
                        if (myDeserializedClass.response[i].image != "")
                        {
                            Debug.Log("image captured");
                            obj.GetComponent<MyEventData>().myeventclass.image = myDeserializedClass.response[i].image;
                        }
                        obj.GetComponent<MyEventData>().myeventclass.theme = myDeserializedClass.response[i].theme;
                        if (myDeserializedClass.response[i].links != null)
                        {
                            for (int k = 0; k < myDeserializedClass.response[i].links.Count; k++)
                            {
                                MyEventData.Link X = new MyEventData.Link();
                                X.title = myDeserializedClass.response[i].links[k].title;
                                X.url = myDeserializedClass.response[i].links[k].url;
                                obj.GetComponent<MyEventData>().myeventclass.links.Add(X);
                            }
                        }
                        if (myDeserializedClass.response[i].goals != null)
                        {
                            Debug.Log("goals recieve: " + myDeserializedClass.response[i].goals.Count);
                            for (int k = 0; k < myDeserializedClass.response[i].goals.Count; k++)
                            {
                                MyEventData.Goal X = new MyEventData.Goal();
                                X.achieved = myDeserializedClass.response[i].goals[k].achieved;
                                X.goal = myDeserializedClass.response[i].goals[k].goal;
                                obj.GetComponent<MyEventData>().myeventclass.goals.Add(X);
                            }
                        }


                        obj.GetComponent<MyEventData>().myeventclass.journal.pre = myDeserializedClass.response[i].journal.pre;
                        obj.GetComponent<MyEventData>().myeventclass.journal.post = myDeserializedClass.response[i].journal.post;
                        obj.GetComponent<MyEventData>().myeventclass.journal.Next = myDeserializedClass.response[i].journal.Next;
                        obj.GetComponent<MyEventData>().myeventclass.journal.Notes = myDeserializedClass.response[i].journal.Notes;
                        obj.GetComponent<MyEventData>().myeventclass.journal.race = myDeserializedClass.response[i].journal.race;




                        obj.GetComponent<MyEventData>().myeventclass.achivement = myDeserializedClass.response[i].achivement;
                        obj.GetComponent<MyEventData>().myeventclass.user = myDeserializedClass.response[i].user;
                        obj.GetComponent<MyEventData>().myeventclass.createdAt = myDeserializedClass.response[i].createdAt;
                        obj.GetComponent<MyEventData>().myeventclass.updatedAt = myDeserializedClass.response[i].updatedAt;
                        obj.GetComponent<MyEventData>().myeventclass.__v = myDeserializedClass.response[i].__v;

                        */
           
        }
        else if (mydata.Count == 0)
        {
            Instantiate(firstrecord, recordspawn);
            instantiatedObjs.Add(firstrecord);
        }
        else
        {
            shwodata();
        }
    } 

    List<eventdata2send> sortdata = new List<eventdata2send>();

    double g;

    List<string> substrings = new List<string>();
    bool checkdata;
    int checkcount;
    public int togglecount;
    private void Start()
    {
        togglecount = 0;
        checkcount = 0;
        
        if (keptdistance == 0)
        {
            checkdata = false;
            kmtoogle.isOn = false;
            Debug.Log("checking: " + keptdistance);
        }
        else
        {
            checkdata = true;
            kmtoogle.isOn = true;
            Debug.Log("checking: " + keptdistance);


        }
    }

    public bool changecheck;

    public void distancetextcheck()
    {
        
        
        if (togglecount == 0)
        {

            
            checkdata = !checkdata;
                Debug.Log("new data: " + recordspawn.childCount + "KM: " + kmdata.Count + " MI " + milesdata.Count);
                if (checkdata)
                {
                    keptdistance = 1;
                    PlayerPrefs.SetInt("keptdistance", keptdistance);
                    PlayerPrefs.Save();
                if (checkcount == 0)
                {
                    
                        for (int l = 0; l < recordspawn.childCount; l++)
                        {
                        if (recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text == "0.05.00" || recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text == "0.5.00")
                        {
                            recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text = "0.03.10";
                            changecheck = true;

                            milesdata.Add(recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text);

                        }
                        if (recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text == "0.10.00" || recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text == "0.10.00")
                        {
                            changecheck = true;
                            recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text = "0.06.21";

                            milesdata.Add(recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text);

                        }
                        if (recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text == "0.15.00" || recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text == "0.15.00")
                        {
                            changecheck = true;
                            recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text = "0.09.32";
                            milesdata.Add(recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text);

                        }
                        if (recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text == "0.21.09" || recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text == "0.21.09")
                        {
                            changecheck = true;
                            recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text = "0.13.10";

                            milesdata.Add(recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text);

                        }
                        if (recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text == "0.42.16" || recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text == "0.42.16")
                        {
                            changecheck = true;
                            recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text = "0.26.20";
                            milesdata.Add(recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text);

                        }




                        if (!changecheck)
                        {
                            string stemp;

                            stemp = recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text;
                            textsplit = stemp.Split(char.Parse("."));
                            textsplit2 = textsplit[2].Split(char.Parse(":"));
                            Debug.Log("data: " + textsplit[1] + " " + textsplit[2]);
                            if (textsplit[0] == "00" || textsplit[0] == "0")
                            {
                                /*   for (int h = 0; h < textsplit[1].Length; h += 1)
                                   {
                                       substrings.Add(textsplit[1].Substring(h, Mathf.Min(1, textsplit[1].Length - h)));

                                   }
                                   if (substrings[0] == "0")
                                   {
                                       g = float.Parse(substrings[1] + "." + textsplit[2]);
                                   }
                                   else
                                   {*/

                                g = float.Parse(textsplit[1] + "." + textsplit2[0]);

                                Debug.LogError("textsplit[0]: " + textsplit[0]);
                            }
                            else if (textsplit[0].Length > 1)
                            {
                                if (textsplit.Length >= 3)
                                {
                                    int major = int.Parse(textsplit[0]);
                                    int minor = int.Parse(textsplit[1]);
                                    int patch = int.Parse(textsplit2[0]);

                                    g = major + minor * 0.01f + patch * 0.0001f;

                                    Debug.LogError("textsplit[0]: " + textsplit[0]);
                                }

                            }
                            double temp;
                            temp = g * 0.621371;
                            g = temp;

                            stemp = "" + g;

                            textsplit = stemp.Split(char.Parse("."));
                            substrings.Clear();

                            for (int h = 0; h < textsplit[1].Length; h += 1)
                            {
                                substrings.Add(textsplit[1].Substring(h, Mathf.Min(1, textsplit[1].Length - h)));

                            }
                            string newdistance = textsplit[0] + substrings[0] + substrings[1];

                            Debug.Log("checkk: " + newdistance);
                            substrings.Clear();

                            if (newdistance.Length > 0)
                            {


                                if (newdistance.Length == 3)
                                {


                                    for (int h = 0; h < newdistance.Length; h += 1)
                                    {
                                        substrings.Add(newdistance.Substring(h, Mathf.Min(1, newdistance.Length - h)));

                                    }


                                    if (substrings.Count == 3)
                                    {
                                        newdistance = "0" + "." + "0" + substrings[0] + "." + substrings[1] + substrings[2];


                                    }
                                }
                                if (newdistance.Length == 4)
                                {


                                    for (int h = 0; h < newdistance.Length; h += 1)
                                    {
                                        substrings.Add(newdistance.Substring(h, Mathf.Min(1, newdistance.Length - h)));

                                    }


                                    if (substrings.Count == 4)
                                    {
                                        newdistance = "0" + "." + substrings[0] + substrings[1] + "." + substrings[2] + substrings[3];


                                    }
                                }
                                if (newdistance.Length == 5)
                                {


                                    for (int h = 0; h < newdistance.Length; h += 1)
                                    {
                                        substrings.Add(newdistance.Substring(h, Mathf.Min(1, newdistance.Length - h)));

                                    }


                                    if (substrings.Count == 5)
                                    {
                                        if (substrings.Count == 3)
                                        {
                                            newdistance = substrings[0] + "." + substrings[1] + substrings[2] + "." + substrings[3] + substrings[4];


                                        }

                                    }
                                }
                                else
                                {
                                    for (int h = 0; h < newdistance.Length; h += 1)
                                    {
                                        substrings.Add(newdistance.Substring(h, Mathf.Min(1, newdistance.Length - h)));

                                    }
                                    if (substrings.Count == 1)
                                    {
                                        newdistance = "0" + "." + "00" + "." + substrings[0];


                                    }
                                    if (substrings.Count == 2)
                                    {
                                        newdistance = "0" + "." + "00" + "." + substrings[0] + substrings[1];


                                    }

                                    if (substrings.Count == 3)
                                    {
                                        newdistance = "0" + ".0" + substrings[0] + "." + substrings[1] + substrings[2];

                                    }
                                }

                            }
                            newdistance = newdistance + " ";
                            //  Debug.Log("replace conversion data text: " + distance + " " + "length: " + newdistance.Length);
                            recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text = newdistance;
                            milesdata.Add(newdistance);
                        }
                        changecheck = false;
                    }
                   

                }
                if (checkcount != 0)
                {
                    for (int l = 0; l < recordspawn.childCount; l++)
                    {

                        recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text = milesdata[l];

                    }
                }

                }

                else if (!checkdata)

                {
                    keptdistance = 0;
                    PlayerPrefs.SetInt("keptdistance", keptdistance);
                    PlayerPrefs.Save();
                if (checkcount == 0)
                {


                    for (int l = 0; l < recordspawn.childCount; l++)
                    {

                        if (recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text == "0.3.10" || recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text == "0.03.10")
                        {
                            recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text = "0.05.00";
                            checkdata = true;

                            


                        }
                        if (recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text == "0.6.21" || recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text == "0.06.21")
                        {
                            recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text = "0.10.00";
                            checkdata = true;

                            


                        }
                        if (recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text == "0.9.32" || recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text == "0.09.32")
                        {
                            checkdata = true;
                            recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text = "0.15.00";
                           


                        }
                        if (recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text == "0.13.10" || recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text == "0.13.10")
                        {
                            recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text = "0.21.09";
                            checkdata = true;

                           
                        }
                        if (recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text == "0.26.20" || recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text == "0.26.20")
                        {
                            recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text = "0.42.16";
                            checkdata = true;

                            


                        }









                        if (!changecheck)
                        {
                            string stemp;

                            stemp = recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text;
                            textsplit = stemp.Split(char.Parse("."));
                            textsplit2 = textsplit[2].Split(char.Parse(":"));
                            Debug.Log("data: " + textsplit[1] + " " + textsplit[2]);
                            if (textsplit[0] == "00" || textsplit[0] == "0")
                            {
                                /*   for (int h = 0; h < textsplit[1].Length; h += 1)
                                   {
                                       substrings.Add(textsplit[1].Substring(h, Mathf.Min(1, textsplit[1].Length - h)));

                                   }
                                   if (substrings[0] == "0")
                                   {
                                       g = float.Parse(substrings[1] + "." + textsplit[2]);
                                   }
                                   else
                                   {*/

                                g = float.Parse(textsplit[1] + "." + textsplit2[0]);

                                Debug.LogError("textsplit[0]: " + textsplit[0]);
                            }
                            else if (textsplit[0].Length > 1)
                            {
                                if (textsplit.Length >= 3)
                                {
                                    int major = int.Parse(textsplit[0]);
                                    int minor = int.Parse(textsplit[1]);
                                    int patch = int.Parse(textsplit2[0]);

                                    g = major + minor * 0.01f + patch * 0.0001f;

                                    Debug.LogError("textsplit[0]: " + textsplit[0]);
                                }

                            }
                            double temp;
                            temp = g * 1.609344;
                            g = temp;

                            stemp = "" + g;

                            textsplit = stemp.Split(char.Parse("."));
                            substrings.Clear();

                            for (int h = 0; h < textsplit[1].Length; h += 1)
                            {
                                substrings.Add(textsplit[1].Substring(h, Mathf.Min(1, textsplit[1].Length - h)));

                            }
                            string newdistance = textsplit[0] + substrings[0] + substrings[1];

                            Debug.Log("checkk: " + newdistance);
                            substrings.Clear();

                            if (newdistance.Length > 0)
                            {


                                if (newdistance.Length == 3)
                                {


                                    for (int h = 0; h < newdistance.Length; h += 1)
                                    {
                                        substrings.Add(newdistance.Substring(h, Mathf.Min(1, newdistance.Length - h)));

                                    }


                                    if (substrings.Count == 3)
                                    {
                                        newdistance = "0" + "." + "0" + substrings[0] + "." + substrings[1] + substrings[2];


                                    }
                                }
                                if (newdistance.Length == 4)
                                {


                                    for (int h = 0; h < newdistance.Length; h += 1)
                                    {
                                        substrings.Add(newdistance.Substring(h, Mathf.Min(1, newdistance.Length - h)));

                                    }


                                    if (substrings.Count == 4)
                                    {
                                        newdistance = "0" + "." + substrings[0] + substrings[1] + "." + substrings[2] + substrings[3];


                                    }
                                }
                                if (newdistance.Length == 5)
                                {


                                    for (int h = 0; h < newdistance.Length; h += 1)
                                    {
                                        substrings.Add(newdistance.Substring(h, Mathf.Min(1, newdistance.Length - h)));

                                    }


                                    if (substrings.Count == 5)
                                    {
                                        if (substrings.Count == 3)
                                        {
                                            newdistance = substrings[0] + "." + substrings[1] + substrings[2] + "." + substrings[3] + substrings[4];


                                        }

                                    }
                                }
                                else
                                {
                                    for (int h = 0; h < newdistance.Length; h += 1)
                                    {
                                        substrings.Add(newdistance.Substring(h, Mathf.Min(1, newdistance.Length - h)));

                                    }
                                    if (substrings.Count == 1)
                                    {
                                        newdistance = "0" + "." + "00" + "." + substrings[0];


                                    }
                                    if (substrings.Count == 2)
                                    {
                                        newdistance = "0" + "." + "00" + "." + substrings[0] + substrings[1];


                                    }

                                    if (substrings.Count == 3)
                                    {
                                        newdistance = "0" + ".0" + substrings[0] + "." + substrings[1] + substrings[2];

                                    }
                                }

                            }
                            newdistance = newdistance + " ";
                            //  Debug.Log("replace conversion data text: " + distance + " " + "length: " + newdistance.Length);
                            recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text = newdistance;
                            kmdata.Add(newdistance);
                        }
                        changecheck = false;
                    }
                    

                }
                if (checkcount != 0)
                {
                    for (int l = 0; l < recordspawn.childCount; l++)
                    {

                        recordspawn.GetChild(l).GetComponent<MyEventData>().recorddata.text = kmdata[l];

                    }
                }

                }
        }
        else
        {
            togglecount = 0;
        
        }


        Debug.Log("checking: " + keptdistance);

        checkcount = 4;


    }

    public bool startcheck;

    public void shwodata()
    {


        Debug.Log("before replace " + mydata.Count);

        string tempdistance;
        if (keptdistance == 0)
        {
            for (int i = 0; i < mydata.Count; i++)
            {
                if (mydata[i].distance.unit == "km")
                {

                    if (mydata[i].distance.istance == "00.5.00" || mydata[i].distance.istance == "0.5.00")
                    {
                        mydata[i].showdistance = "0.05.00";
                        startcheck = true;

                        mydata[i].value = 500;
                    }
                    if (mydata[i].distance.istance == "00.10.00" || mydata[i].distance.istance == "0.10.00")
                    {
                        mydata[i].showdistance = "0.10.00";
                        startcheck = true;

                        mydata[i].value = 1000;

                    }
                    if (mydata[i].distance.istance == "00.15.00" || mydata[i].distance.istance == "0.15.00")
                    {
                        mydata[i].showdistance = "0.15.00";
                        startcheck = true;

                        mydata[i].value = 1500;
                    }
                    if (mydata[i].distance.istance == "00.21.09" || mydata[i].distance.istance == "0.21.09")
                    {
                        mydata[i].showdistance = "0.21.09";
                        startcheck = true;

                        mydata[i].value = 2109;

                    }
                    if (mydata[i].distance.istance == "00.42.16" || mydata[i].distance.istance == "0.42.16")
                    {
                        mydata[i].showdistance = "0.42.16";
                        startcheck = true;

                        mydata[i].value = 4216;

                    }
                    if (mydata[i].distance.istance == "00.00.00" || mydata[i].distance.istance == "0.00.00")
                    {
                        mydata[i].showdistance = "0.00.00";
                        startcheck = true;

                        mydata[i].value = 0000;

                    }
                }
                else
                {
                    if (mydata[i].distance.istance == "00.3.10" || mydata[i].distance.istance == "0.03.10")
                    {
                        mydata[i].showdistance = "0.05.00";
                        startcheck = true;

                        mydata[i].value = 500;


                    }
                    if (mydata[i].distance.istance == "00.6.21" || mydata[i].distance.istance == "0.06.21")
                    {
                        mydata[i].showdistance = "0.10.00";
                        startcheck = true;

                        mydata[i].value = 1000;


                    }
                    if (mydata[i].distance.istance == "00.9.32" || mydata[i].distance.istance == "0.09.32")
                    {
                        startcheck = true;
                        mydata[i].showdistance = "0.15.00";
                        mydata[i].value = 1500;


                    }
                    if (mydata[i].distance.istance == "00.13.10" || mydata[i].distance.istance == "0.13.10")
                    {
                        mydata[i].showdistance = "0.21.09";
                        startcheck = true;

                        mydata[i].value = 2109;
                    }
                    if (mydata[i].distance.istance == "00.26.20" || mydata[i].distance.istance == "0.26.20")
                    {
                        mydata[i].showdistance = "0.42.16";
                        startcheck = true;

                        mydata[i].value = 4216;


                    }




                }
                if (!startcheck)
                {
                    tempdistance = mydata[i].distance.istance;
                    textsplit = mydata[i].distance.istance.Split(char.Parse("."));

                    if (textsplit.Length >= 3)
                    {
                        string distance1 = textsplit[0] + textsplit[1] + textsplit[2];
                    }
                    else
                    {
                        Debug.LogError("textsplit does not have enough elements. Length: " + textsplit.Length + tempdistance);
                    }

                    string distance = textsplit[0] + textsplit[1] + textsplit[2];

                    if (textsplit[0] == "00")
                    {

                        g = float.Parse(textsplit[1] + "." + textsplit[2]);

                    }
                    else if (textsplit[0].Length > 1)
                    {
                        if (textsplit.Length >= 3)
                        {
                            int major = int.Parse(textsplit[0]);
                            int minor = int.Parse(textsplit[1]);
                            int patch = int.Parse(textsplit[2]);

                            g = major + minor * 0.01f + patch * 0.0001f;

                            Debug.Log("Converted Version Number: " + g);
                        }

                    }

                    b = Convert.ToInt32(distance);


                    if (mydata[i].distance.unit != "km")
                    {
                        Debug.Log("replace before conversion data: " + g);
                        double temp;
                        temp = g * 1.609344;
                        g = Convert.ToSingle(temp);

                        tempdistance = "" + g;
                        substrings.Clear();
                        textsplit = tempdistance.Split(char.Parse("."));
                        for (int h = 0; h < textsplit[1].Length; h += 1)
                        {
                            substrings.Add(textsplit[1].Substring(h, Mathf.Min(1, textsplit[1].Length - h)));

                        }
                        string newdistance = textsplit[0] + substrings[0] + substrings[1];
                        substrings.Clear();
                        if (newdistance.Length > 0)
                        {


                            if (newdistance.Length == 3)
                            {


                                for (int h = 0; h < newdistance.Length; h += 1)
                                {
                                    substrings.Add(newdistance.Substring(h, Mathf.Min(1, newdistance.Length - h)));

                                }


                                if (substrings.Count == 3)
                                {
                                    tempdistance = "0" + ".0" + substrings[0] + "." + substrings[1] + substrings[2];


                                }
                            }
                            if (newdistance.Length == 4)
                            {


                                for (int h = 0; h < newdistance.Length; h += 1)
                                {
                                    substrings.Add(newdistance.Substring(h, Mathf.Min(1, newdistance.Length - h)));

                                }


                                if (substrings.Count == 4)
                                {
                                    tempdistance = "0" + "." + substrings[0] + substrings[1] + "." + substrings[2] + substrings[3];


                                }
                            }
                            if (newdistance.Length == 5)
                            {


                                for (int h = 0; h < newdistance.Length; h += 1)
                                {
                                    substrings.Add(newdistance.Substring(h, Mathf.Min(1, newdistance.Length - h)));

                                }


                                if (substrings.Count == 5)
                                {

                                    tempdistance = substrings[0] + "0" + "." + substrings[1] + substrings[2] + "." + substrings[3] + substrings[4];




                                }
                            }
                            else
                            {
                                for (int h = 0; h < newdistance.Length; h += 1)
                                {
                                    substrings.Add(newdistance.Substring(h, Mathf.Min(1, newdistance.Length - h)));

                                }
                                if (substrings.Count == 1)
                                {
                                    tempdistance = "0" + "." + "00" + "." + substrings[0];


                                }
                                if (substrings.Count == 2)
                                {
                                    tempdistance = "0" + "." + "00" + "." + substrings[0] + substrings[1];


                                }

                                if (substrings.Count == 3)
                                {
                                    tempdistance = "0" + ".0" + substrings[0] + "." + substrings[1] + substrings[2];

                                }
                            }

                        }
                        Debug.Log("replace conversion data text: " + distance + " " + "length: " + newdistance.Length);
                        mydata[i].showdistance = tempdistance;
                        textsplit = tempdistance.Split(char.Parse("."));
                        string latestdistance = textsplit[0] + textsplit[1] + textsplit[2];
                        b = Convert.ToInt32(latestdistance);
                        mydata[i].value = b;
                    }

                    else
                    {
                        substrings.Clear();
                        if (distance.Length > 0)
                        {


                            if (distance.Length == 3)
                            {


                                for (int h = 0; h < distance.Length; h += 1)
                                {
                                    substrings.Add(distance.Substring(h, Mathf.Min(1, distance.Length - h)));

                                }


                                if (substrings.Count == 3)
                                {
                                    tempdistance = "0" + ".0" + substrings[0] + "." + substrings[1] + substrings[2];


                                }
                            }
                            if (distance.Length == 4)
                            {


                                for (int h = 0; h < distance.Length; h += 1)
                                {
                                    substrings.Add(distance.Substring(h, Mathf.Min(1, distance.Length - h)));

                                }


                                if (substrings.Count == 4)
                                {
                                    tempdistance = "0" + "." + substrings[0] + substrings[1] + "." + substrings[2] + substrings[3];


                                }
                            }
                            if (distance.Length == 5)
                            {


                                for (int h = 0; h < distance.Length; h += 1)
                                {
                                    substrings.Add(distance.Substring(h, Mathf.Min(1, distance.Length - h)));

                                }


                                if (substrings.Count == 5)
                                {

                                    tempdistance = substrings[0] + "0" + "." + substrings[1] + substrings[2] + "." + substrings[3] + substrings[4];




                                }
                            }
                            else
                            {
                                for (int h = 0; h < distance.Length; h += 1)
                                {
                                    substrings.Add(distance.Substring(h, Mathf.Min(1, distance.Length - h)));

                                }
                                if (substrings.Count == 1)
                                {
                                    tempdistance = "0" + "." + "00" + "." + substrings[0];


                                }
                                if (substrings.Count == 2)
                                {
                                    tempdistance = "0" + "." + "00" + "." + substrings[0] + substrings[1];


                                }

                                if (substrings.Count == 3)
                                {
                                    tempdistance = "0" + ".0" + substrings[0] + "." + substrings[1] + substrings[2];

                                }
                            }

                        }
                        Debug.Log("replace conversion data text: " + distance.Length);

                        mydata[i].showdistance = tempdistance;
                        mydata[i].value = b;

                    }
                }


                Debug.Log("replace conversion data: " + g);


                //if (!startcheck)
                //{
                //    textsplit = mydata[i].time.Split(char.Parse(":"));
                //    string timee = textsplit[0] + textsplit[1] + textsplit[2];
                //    textsplit[0] = "0";
                //    Debug.LogError("textsplit[0]: " + textsplit[0]);
                //    b = Convert.ToInt32(timee);

                //    mydata[i].timevalue = b;
                //}


                if (!startcheck)
                {
                    //textsplit = mydata[i].time.Split(':');

                    //// Keep only digits in each part
                    //for (int j = 0; j < textsplit.Length; j++)
                    //{
                    //    textsplit[j] = Regex.Replace(textsplit[j], @"\D", "");
                    //}

                    //// Special case: if hours are "00", make it just "0"
                    //if (textsplit[0] == "00")
                    //{
                    //    textsplit[0] = "0";
                    //}

                    //// Rebuild the time string (this updates the one shown on screen)
                    //mydata[i].time = string.Join(":", textsplit);

                    //string timee = textsplit[0] + textsplit[1] + textsplit[2];
                    //Debug.LogError("textsplit[0]: " + textsplit[0]);

                    //b = Convert.ToInt32(timee);
                    //mydata[i].timevalue = b;
                }

               

                startcheck = false;

                /*
                for (int j = 1; j < mydata.Count; j++)
                {
                    textsplit = mydata[i].distance.istance.Split(char.Parse("."));
                    string distancee = textsplit[0] + textsplit[1] + textsplit[2];
                    b = Convert.ToInt16(distance);
                    textsplit = mydata[j].distance.istance.Split(char.Parse("."));
                    string distanc2 = textsplit[0] + textsplit[1] + textsplit[2];
                    int bb = Convert.ToInt16(distancee);
                    if (b > bb)
                    {


                        mydata[i] = mydata[j];


                    }


                    if (j == mydata.Count-1)
                    {
                        GameObject obj = Instantiate(recordata, recordspawn);
                        obj.GetComponent<MyEventData>().recorddata.text = mydata[i].distance.istance + "         " + mydata[i].time;
                        obj.GetComponent<MyEventData>().myeventclass = new MyEventData.MyEvent();
                        obj.GetComponent<MyEventData>().myeventclass.links = new List<MyEventData.Link>();
                        obj.GetComponent<MyEventData>().myeventclass.journal = new MyEventData.Journal();
                        obj.GetComponent<MyEventData>().myeventclass.distance = new MyEventData.Distance();
                        obj.GetComponent<MyEventData>().myeventclass.goals = new List<MyEventData.Goal>();
                        obj.GetComponent<MyEventData>().myeventclass.result = new MyEventData.Result();
                        obj.GetComponent<MyEventData>().myeventclass.position = mydata[i].position;

                        obj.GetComponent<MyEventData>().myeventclass.totalpeople = mydata[i].totalpeople;
                        obj.GetComponent<MyEventData>().myeventclass.eventType = mydata[i].eventType;
                        obj.GetComponent<MyEventData>().myeventclass.id = mydata[i]._id;

                        obj.GetComponent<MyEventData>().myeventclass.city = mydata[i].city;
                        obj.GetComponent<MyEventData>().myeventclass.race = mydata[i].race;
                        obj.GetComponent<MyEventData>().myeventclass.result.myGender = mydata[i].result.myGender;
                        obj.GetComponent<MyEventData>().myeventclass.result.myAgeGroup = mydata[i].result.myAgeGroup;
                        obj.GetComponent<MyEventData>().myeventclass.result.totalGender = mydata[i].result.totalGender;
                        obj.GetComponent<MyEventData>().myeventclass.result.totalAgeGroup = mydata[i].result.totalAgeGroup;



                        obj.GetComponent<MyEventData>().myeventclass.eventName = mydata[i].eventName;
                        obj.GetComponent<MyEventData>().myeventclass.eventDate = mydata[i].eventDate;
                        obj.GetComponent<MyEventData>().myeventclass.victoryCity = mydata[i].victoryCity;
                        obj.GetComponent<MyEventData>().myeventclass.time = mydata[i].time;
                        obj.GetComponent<MyEventData>().myeventclass.distance.istance = mydata[i].distance.istance;
                        obj.GetComponent<MyEventData>().myeventclass.personarecord = mydata[i].personarecord;
                        obj.GetComponent<MyEventData>().myeventclass.distance.unit = mydata[i].distance.unit;


                        obj.GetComponent<MyEventData>().myeventclass.@virtual = mydata[i].@virtual;
                        if (mydata[i].image != "")
                        {
                            Debug.Log("image captured");
                            obj.GetComponent<MyEventData>().myeventclass.image = mydata[i].image;
                        }
                        obj.GetComponent<MyEventData>().myeventclass.theme = mydata[i].theme;
                        if (mydata[i].links != null)
                        {
                            for (int k = 0; k < mydata[i].links.Count; k++)
                            {
                                MyEventData.Link X = new MyEventData.Link();
                                X.title = mydata[i].links[k].title;
                                X.url = mydata[i].links[k].url;
                                obj.GetComponent<MyEventData>().myeventclass.links.Add(X);
                            }
                        }
                        if (mydata[i].goals != null)
                        {
                            Debug.Log("goals recieve: " + mydata[i].goals.Count);
                            for (int k = 0; k < mydata[i].goals.Count; k++)
                            {
                                MyEventData.Goal X = new MyEventData.Goal();
                                X.achieved = mydata[i].goals[k].achieved;
                                X.goal = mydata[i].goals[k].goal;
                                obj.GetComponent<MyEventData>().myeventclass.goals.Add(X);
                            }
                        }


                        obj.GetComponent<MyEventData>().myeventclass.journal.pre = mydata[i].journal.pre;
                        obj.GetComponent<MyEventData>().myeventclass.journal.post = mydata[i].journal.post;
                        obj.GetComponent<MyEventData>().myeventclass.journal.Next = mydata[i].journal.Next;
                        obj.GetComponent<MyEventData>().myeventclass.journal.Notes = mydata[i].journal.Notes;
                        obj.GetComponent<MyEventData>().myeventclass.journal.race = mydata[i].journal.race;




                        obj.GetComponent<MyEventData>().myeventclass.achivement = mydata[i].achivement;
                        obj.GetComponent<MyEventData>().myeventclass.user = mydata[i].user;
                        obj.GetComponent<MyEventData>().myeventclass.createdAt = mydata[i].createdAt;
                        obj.GetComponent<MyEventData>().myeventclass.updatedAt = mydata[i].updatedAt;
                        obj.GetComponent<MyEventData>().myeventclass.__v = mydata[i].__v;








                    }
                    /*
                    textsplit = mydata[j].distance.istance.Split(char.Parse("."));
                    string distancee = textsplit[0] + textsplit[1] + textsplit[2];
                    int bb = Convert.ToInt16(distancee);
                    int check = 0; 

                    if (mydata[i].distance.istance == mydata[j].distance.istance)
                    {

                        textsplit = mydata[i].time.Split(char.Parse(":"));
                        string time = textsplit[0] + textsplit[1] + textsplit[2];
                        int t = Convert.ToInt16(time);



                        textsplit = mydata[j].time.Split(char.Parse(":"));
                        string time2 = textsplit[0] + textsplit[1] + textsplit[2];
                        int t2 = Convert.ToInt16(time2);
                        if (t > t2)
                        {


                            mydata[i] = mydata[j];

                        }


                    }
                    else if (b < bb)
                    {
                        mydata[i] = null;
                        mydata[i] = mydata[j];


                    }

                    */
                //  sortdata.Add(mydata[i]);
                Debug.Log("after replace " + mydata[i].eventName);
            }
        }
        else
        {
           

            for (int i = 0; i < mydata.Count; i++)
            {
                if (mydata[i].distance.unit == "km")
                {

                    if (mydata[i].distance.istance == "0.05.00" || mydata[i].distance.istance == "0.5.00")
                    {
                        mydata[i].showdistance = "0.03.10";
                        startcheck = true;

                        mydata[i].value = 310;
                    }
                    if (mydata[i].distance.istance == "0.10.00" || mydata[i].distance.istance == "0.10.00")
                    {
                        startcheck = true;
                        mydata[i].showdistance = "0.06.21";
                        mydata[i].value = 621;

                    }
                    if (mydata[i].distance.istance == "0.15.00" || mydata[i].distance.istance == "0.15.00")
                    {
                        startcheck = true;
                        mydata[i].showdistance = "0.09.32";
                        mydata[i].value = 932;
                    }
                    if (mydata[i].distance.istance == "0.21.09" || mydata[i].distance.istance == "0.21.09")
                    {
                        startcheck = true;
                        mydata[i].showdistance = "0.13.10";
                        mydata[i].value = 1310;

                    }
                    if (mydata[i].distance.istance == "0.42.16" || mydata[i].distance.istance == "0.42.16")
                    {
                        startcheck = true;
                        mydata[i].showdistance = "0.26.20";
                        mydata[i].value = 2620;

                    }
                }
                else
                {
                    if (mydata[i].distance.istance == "0.3.10" || mydata[i].distance.istance == "0.03.10")
                    {
                        mydata[i].showdistance = "0.03.10";
                        startcheck = true;

                        mydata[i].value = 310;


                    }
                    if (mydata[i].distance.istance == "0.6.21" || mydata[i].distance.istance == "0.06.21")
                    {
                        mydata[i].showdistance = "0.06.21";
                        startcheck = true;

                        mydata[i].value = 621;


                    }
                    if (mydata[i].distance.istance == "0.9.32" || mydata[i].distance.istance == "0.09.32")
                    {
                        mydata[i].showdistance = "0.09.32";
                        startcheck = true;

                        mydata[i].value = 932;


                    }
                    if (mydata[i].distance.istance == "0.13.10" || mydata[i].distance.istance == "0.13.10")
                    {
                        mydata[i].showdistance = "0.13.10";
                        startcheck = true;

                        mydata[i].value = 1310;
                    }
                    if (mydata[i].distance.istance == "0.26.20" || mydata[i].distance.istance == "0.26.20")
                    {
                        mydata[i].showdistance = "0.26.20";
                        startcheck = true;

                        mydata[i].value = 2620;


                    }




                }
                if (!startcheck)
                {
                    tempdistance = mydata[i].distance.istance;
                    textsplit = mydata[i].distance.istance.Split(char.Parse("."));
                    string distance = textsplit[0] + textsplit[1] + textsplit[2];
                    if (textsplit[0] == "00")
                    {

                        g = float.Parse(textsplit[1] + "." + textsplit[2]);

                    }
                    else if (textsplit[0].Length > 1)
                    {
                        if (textsplit.Length >= 3)
                        {
                            int major = int.Parse(textsplit[0]);
                            int minor = int.Parse(textsplit[1]);
                            int patch = int.Parse(textsplit[2]);

                            g = major + minor * 0.01f + patch * 0.0001f;

                            Debug.Log("Converted Version Number: " + g);
                        }

                    }

                    b = Convert.ToInt32(distance);


                    if (mydata[i].distance.unit != "mi")
                    {
                        Debug.Log("replace before conversion data: " + g);
                        double temp;
                        temp = g * 0.621371;
                        g = Convert.ToSingle(temp);

                        tempdistance = "" + g;
                        substrings.Clear();
                        textsplit = tempdistance.Split(char.Parse("."));
                        for (int h = 0; h < textsplit[1].Length; h += 1)
                        {
                            substrings.Add(textsplit[1].Substring(h, Mathf.Min(1, textsplit[1].Length - h)));

                        }
                        string newdistance = textsplit[0] + substrings[0] + substrings[1];
                        substrings.Clear();
                        if (newdistance.Length > 0)
                        {


                            if (newdistance.Length == 3)
                            {


                                for (int h = 0; h < newdistance.Length; h += 1)
                                {
                                    substrings.Add(newdistance.Substring(h, Mathf.Min(1, newdistance.Length - h)));

                                }


                                if (substrings.Count == 3)
                                {
                                    tempdistance = "0" + ".0" + substrings[0] + "." + substrings[1] + substrings[2];


                                }
                            }
                            if (newdistance.Length == 4)
                            {


                                for (int h = 0; h < newdistance.Length; h += 1)
                                {
                                    substrings.Add(newdistance.Substring(h, Mathf.Min(1, newdistance.Length - h)));

                                }


                                if (substrings.Count == 4)
                                {
                                    tempdistance = "0" + "." + substrings[0] + substrings[1] + "." + substrings[2] + substrings[3];


                                }
                            }
                            if (newdistance.Length == 5)
                            {


                                for (int h = 0; h < newdistance.Length; h += 1)
                                {
                                    substrings.Add(newdistance.Substring(h, Mathf.Min(1, newdistance.Length - h)));

                                }


                                if (substrings.Count == 5)
                                {

                                    tempdistance = substrings[0] + "0" + "." + substrings[1] + substrings[2] + "." + substrings[3] + substrings[4];




                                }
                            }
                            else
                            {
                                for (int h = 0; h < newdistance.Length; h += 1)
                                {
                                    substrings.Add(newdistance.Substring(h, Mathf.Min(1, newdistance.Length - h)));

                                }
                                if (substrings.Count == 1)
                                {
                                    tempdistance = "0" + "." + "00" + "." + substrings[0];


                                }
                                if (substrings.Count == 2)
                                {
                                    tempdistance = "0" + "." + "00" + "." + substrings[0] + substrings[1];


                                }

                                if (substrings.Count == 3)
                                {
                                    tempdistance = "0" + ".0" + substrings[0] + "." + substrings[1] + substrings[2];

                                }
                            }

                        }
                        Debug.Log("replace conversion data text: " + distance + " " + "length: " + newdistance.Length);
                        mydata[i].showdistance = tempdistance;
                        textsplit = tempdistance.Split(char.Parse("."));
                        string latestdistance = textsplit[0] + textsplit[1] + textsplit[2];
                        b = Convert.ToInt32(latestdistance);
                        mydata[i].value = b;
                    }
                    else
                    {
                        substrings.Clear();
                        if (distance.Length > 0)
                        {


                            if (distance.Length == 3)
                            {


                                for (int h = 0; h < distance.Length; h += 1)
                                {
                                    substrings.Add(distance.Substring(h, Mathf.Min(1, distance.Length - h)));

                                }


                                if (substrings.Count == 3)
                                {
                                    tempdistance = "0" + ".0" + substrings[0] + "." + substrings[1] + substrings[2];


                                }
                            }
                            if (distance.Length == 4)
                            {


                                for (int h = 0; h < distance.Length; h += 1)
                                {
                                    substrings.Add(distance.Substring(h, Mathf.Min(1, distance.Length - h)));

                                }


                                if (substrings.Count == 4)
                                {
                                    tempdistance = "0" + "." + substrings[0] + substrings[1] + "." + substrings[2] + substrings[3];


                                }
                            }
                            if (distance.Length == 5)
                            {


                                for (int h = 0; h < distance.Length; h += 1)
                                {
                                    substrings.Add(distance.Substring(h, Mathf.Min(1, distance.Length - h)));

                                }


                                if (substrings.Count == 5)
                                {

                                    tempdistance = substrings[0] + "0" + "." + substrings[1] + substrings[2] + "." + substrings[3] + substrings[4];




                                }
                            }
                            else
                            {
                                for (int h = 0; h < distance.Length; h += 1)
                                {
                                    substrings.Add(distance.Substring(h, Mathf.Min(1, distance.Length - h)));

                                }
                                if (substrings.Count == 1)
                                {
                                    tempdistance = "0" + "." + "00" + "." + substrings[0];


                                }
                                if (substrings.Count == 2)
                                {
                                    tempdistance = "0" + "." + "00" + "." + substrings[0] + substrings[1];


                                }

                                if (substrings.Count == 3)
                                {
                                    tempdistance = "0" + ".0" + substrings[0] + "." + substrings[1] + substrings[2];

                                }
                            }

                        }
                        Debug.Log("replace conversion data text: " + distance.Length);

                        mydata[i].showdistance = tempdistance;
                        mydata[i].value = b;

                    }


                    Debug.Log("replace conversion data: " + g);



                    //textsplit = mydata[i].time.Split(':');

                    //// Keep only digits in each part
                    //for (int j = 0; j < textsplit.Length; j++)
                    //{
                    //    textsplit[j] = Regex.Replace(textsplit[j], @"\D", "");
                    //}

                    //// Special case: if hours are "00", make it just "0"
                    //if (textsplit[0] == "00")
                    //{
                    //    textsplit[0] = "0";
                    //}

                    //// Rebuild the time string (this updates the one shown on screen)
                    //mydata[i].time = string.Join(":", textsplit);

                    //string timee = textsplit[0] + textsplit[1] + textsplit[2];
                    //Debug.LogError("textsplit[0]: " + textsplit[0]);

                    

                    //b = Convert.ToInt32(timee);
                    //mydata[i].timevalue = b;
                }
                startcheck = false;

                /*
                for (int j = 1; j < mydata.Count; j++)
                {
                    textsplit = mydata[i].distance.istance.Split(char.Parse("."));
                    string distancee = textsplit[0] + textsplit[1] + textsplit[2];
                    b = Convert.ToInt16(distance);
                    textsplit = mydata[j].distance.istance.Split(char.Parse("."));
                    string distanc2 = textsplit[0] + textsplit[1] + textsplit[2];
                    int bb = Convert.ToInt16(distancee);
                    if (b > bb)
                    {


                        mydata[i] = mydata[j];


                    }


                    if (j == mydata.Count-1)
                    {
                        GameObject obj = Instantiate(recordata, recordspawn);
                        obj.GetComponent<MyEventData>().recorddata.text = mydata[i].distance.istance + "         " + mydata[i].time;
                        obj.GetComponent<MyEventData>().myeventclass = new MyEventData.MyEvent();
                        obj.GetComponent<MyEventData>().myeventclass.links = new List<MyEventData.Link>();
                        obj.GetComponent<MyEventData>().myeventclass.journal = new MyEventData.Journal();
                        obj.GetComponent<MyEventData>().myeventclass.distance = new MyEventData.Distance();
                        obj.GetComponent<MyEventData>().myeventclass.goals = new List<MyEventData.Goal>();
                        obj.GetComponent<MyEventData>().myeventclass.result = new MyEventData.Result();
                        obj.GetComponent<MyEventData>().myeventclass.position = mydata[i].position;

                        obj.GetComponent<MyEventData>().myeventclass.totalpeople = mydata[i].totalpeople;
                        obj.GetComponent<MyEventData>().myeventclass.eventType = mydata[i].eventType;
                        obj.GetComponent<MyEventData>().myeventclass.id = mydata[i]._id;

                        obj.GetComponent<MyEventData>().myeventclass.city = mydata[i].city;
                        obj.GetComponent<MyEventData>().myeventclass.race = mydata[i].race;
                        obj.GetComponent<MyEventData>().myeventclass.result.myGender = mydata[i].result.myGender;
                        obj.GetComponent<MyEventData>().myeventclass.result.myAgeGroup = mydata[i].result.myAgeGroup;
                        obj.GetComponent<MyEventData>().myeventclass.result.totalGender = mydata[i].result.totalGender;
                        obj.GetComponent<MyEventData>().myeventclass.result.totalAgeGroup = mydata[i].result.totalAgeGroup;



                        obj.GetComponent<MyEventData>().myeventclass.eventName = mydata[i].eventName;
                        obj.GetComponent<MyEventData>().myeventclass.eventDate = mydata[i].eventDate;
                        obj.GetComponent<MyEventData>().myeventclass.victoryCity = mydata[i].victoryCity;
                        obj.GetComponent<MyEventData>().myeventclass.time = mydata[i].time;
                        obj.GetComponent<MyEventData>().myeventclass.distance.istance = mydata[i].distance.istance;
                        obj.GetComponent<MyEventData>().myeventclass.personarecord = mydata[i].personarecord;
                        obj.GetComponent<MyEventData>().myeventclass.distance.unit = mydata[i].distance.unit;


                        obj.GetComponent<MyEventData>().myeventclass.@virtual = mydata[i].@virtual;
                        if (mydata[i].image != "")
                        {
                            Debug.Log("image captured");
                            obj.GetComponent<MyEventData>().myeventclass.image = mydata[i].image;
                        }
                        obj.GetComponent<MyEventData>().myeventclass.theme = mydata[i].theme;
                        if (mydata[i].links != null)
                        {
                            for (int k = 0; k < mydata[i].links.Count; k++)
                            {
                                MyEventData.Link X = new MyEventData.Link();
                                X.title = mydata[i].links[k].title;
                                X.url = mydata[i].links[k].url;
                                obj.GetComponent<MyEventData>().myeventclass.links.Add(X);
                            }
                        }
                        if (mydata[i].goals != null)
                        {
                            Debug.Log("goals recieve: " + mydata[i].goals.Count);
                            for (int k = 0; k < mydata[i].goals.Count; k++)
                            {
                                MyEventData.Goal X = new MyEventData.Goal();
                                X.achieved = mydata[i].goals[k].achieved;
                                X.goal = mydata[i].goals[k].goal;
                                obj.GetComponent<MyEventData>().myeventclass.goals.Add(X);
                            }
                        }


                        obj.GetComponent<MyEventData>().myeventclass.journal.pre = mydata[i].journal.pre;
                        obj.GetComponent<MyEventData>().myeventclass.journal.post = mydata[i].journal.post;
                        obj.GetComponent<MyEventData>().myeventclass.journal.Next = mydata[i].journal.Next;
                        obj.GetComponent<MyEventData>().myeventclass.journal.Notes = mydata[i].journal.Notes;
                        obj.GetComponent<MyEventData>().myeventclass.journal.race = mydata[i].journal.race;




                        obj.GetComponent<MyEventData>().myeventclass.achivement = mydata[i].achivement;
                        obj.GetComponent<MyEventData>().myeventclass.user = mydata[i].user;
                        obj.GetComponent<MyEventData>().myeventclass.createdAt = mydata[i].createdAt;
                        obj.GetComponent<MyEventData>().myeventclass.updatedAt = mydata[i].updatedAt;
                        obj.GetComponent<MyEventData>().myeventclass.__v = mydata[i].__v;








                    }
                    /*
                    textsplit = mydata[j].distance.istance.Split(char.Parse("."));
                    string distancee = textsplit[0] + textsplit[1] + textsplit[2];
                    int bb = Convert.ToInt16(distancee);
                    int check = 0; 

                    if (mydata[i].distance.istance == mydata[j].distance.istance)
                    {

                        textsplit = mydata[i].time.Split(char.Parse(":"));
                        string time = textsplit[0] + textsplit[1] + textsplit[2];
                        int t = Convert.ToInt16(time);



                        textsplit = mydata[j].time.Split(char.Parse(":"));
                        string time2 = textsplit[0] + textsplit[1] + textsplit[2];
                        int t2 = Convert.ToInt16(time2);
                        if (t > t2)
                        {


                            mydata[i] = mydata[j];

                        }


                    }
                    else if (b < bb)
                    {
                        mydata[i] = null;
                        mydata[i] = mydata[j];


                    }

                    */










                //  sortdata.Add(mydata[i]);





                Debug.Log("after replace " + mydata[i].eventName);





            }


        }
        //    mydata.Sort((x, y) => mydata.value.CompareTo(y.OrderDate));

        mydata.Sort((x, y) => x.value.CompareTo(y.value));

            mydata = mydata
        .OrderBy(x => x.value)     
        .ThenBy(x => x.timevalue)   
        .ToList();

        //mydata.Reverse();



        for (int i = 0; i < mydata.Count; i++)
        {


            /*  if (i + 1 < mydata.Count)
              {
                  if (mydata[i].value == mydata[i + 1].value)
                  {
                      for (int z = i + 1; z < mydata.Count; z++)
                      {
                          if (mydata[i].value == mydata[z].value)
                          {
                              if (mydata[i].timevalue > mydata[z].timevalue)
                              {


                                  mydata[i] = null;
                                  mydata[i] = mydata[z];
                                  mydata[z] = null;


                              }

                          }
                          else
                          {
                              GameObject obj = Instantiate(recordata, recordspawn);
                              obj.GetComponent<MyEventData>().recorddata.text = mydata[i].distance.istance + "         " + mydata[i].time;
                              obj.GetComponent<MyEventData>().myeventclass = new MyEventData.MyEvent();
                              obj.GetComponent<MyEventData>().myeventclass.links = new List<MyEventData.Link>();
                              obj.GetComponent<MyEventData>().myeventclass.journal = new MyEventData.Journal();
                              obj.GetComponent<MyEventData>().myeventclass.distance = new MyEventData.Distance();
                              obj.GetComponent<MyEventData>().myeventclass.goals = new List<MyEventData.Goal>();
                              obj.GetComponent<MyEventData>().myeventclass.result = new MyEventData.Result();
                              obj.GetComponent<MyEventData>().myeventclass.position = mydata[i].position;

                              obj.GetComponent<MyEventData>().myeventclass.totalpeople = mydata[i].totalpeople;
                              obj.GetComponent<MyEventData>().myeventclass.eventType = mydata[i].eventType;
                              obj.GetComponent<MyEventData>().myeventclass.id = mydata[i]._id;

                              obj.GetComponent<MyEventData>().myeventclass.city = mydata[i].city;
                              obj.GetComponent<MyEventData>().myeventclass.race = mydata[i].race;
                              obj.GetComponent<MyEventData>().myeventclass.result.myGender = mydata[i].result.myGender;
                              obj.GetComponent<MyEventData>().myeventclass.result.myAgeGroup = mydata[i].result.myAgeGroup;
                              obj.GetComponent<MyEventData>().myeventclass.result.totalGender = mydata[i].result.totalGender;
                              obj.GetComponent<MyEventData>().myeventclass.result.totalAgeGroup = mydata[i].result.totalAgeGroup;



                              obj.GetComponent<MyEventData>().myeventclass.eventName = mydata[i].eventName;
                              obj.GetComponent<MyEventData>().myeventclass.eventDate = mydata[i].eventDate;
                              obj.GetComponent<MyEventData>().myeventclass.victoryCity = mydata[i].victoryCity;
                              obj.GetComponent<MyEventData>().myeventclass.time = mydata[i].time;
                              obj.GetComponent<MyEventData>().myeventclass.distance.istance = mydata[i].distance.istance;
                              obj.GetComponent<MyEventData>().myeventclass.personarecord = mydata[i].personarecord;
                              obj.GetComponent<MyEventData>().myeventclass.distance.unit = mydata[i].distance.unit;


                              obj.GetComponent<MyEventData>().myeventclass.@virtual = mydata[i].@virtual;
                              if (mydata[i].image != "")
                              {
                                  Debug.Log("image captured");
                                  obj.GetComponent<MyEventData>().myeventclass.image = mydata[i].image;
                              }
                              obj.GetComponent<MyEventData>().myeventclass.theme = mydata[i].theme;
                              if (mydata[i].links != null)
                              {
                                  for (int k = 0; k < mydata[i].links.Count; k++)
                                  {
                                      MyEventData.Link X = new MyEventData.Link();
                                      X.title = mydata[i].links[k].title;
                                      X.url = mydata[i].links[k].url;
                                      obj.GetComponent<MyEventData>().myeventclass.links.Add(X);
                                  }
                              }
                              if (mydata[i].goals != null)
                              {
                                  Debug.Log("goals recieve: " + mydata[i].goals.Count);
                                  for (int k = 0; k < mydata[i].goals.Count; k++)
                                  {
                                      MyEventData.Goal X = new MyEventData.Goal();
                                      X.achieved = mydata[i].goals[k].achieved;
                                      X.goal = mydata[i].goals[k].goal;
                                      obj.GetComponent<MyEventData>().myeventclass.goals.Add(X);
                                  }
                              }


                              obj.GetComponent<MyEventData>().myeventclass.journal.pre = mydata[i].journal.pre;
                              obj.GetComponent<MyEventData>().myeventclass.journal.post = mydata[i].journal.post;
                              obj.GetComponent<MyEventData>().myeventclass.journal.Next = mydata[i].journal.Next;
                              obj.GetComponent<MyEventData>().myeventclass.journal.Notes = mydata[i].journal.Notes;
                              obj.GetComponent<MyEventData>().myeventclass.journal.race = mydata[i].journal.race;




                              obj.GetComponent<MyEventData>().myeventclass.achivement = mydata[i].achivement;
                              obj.GetComponent<MyEventData>().myeventclass.user = mydata[i].user;
                              obj.GetComponent<MyEventData>().myeventclass.createdAt = mydata[i].createdAt;
                              obj.GetComponent<MyEventData>().myeventclass.updatedAt = mydata[i].updatedAt;
                              obj.GetComponent<MyEventData>().myeventclass.__v = mydata[i].__v;

                              break;
                          }


                      }
                  }
                  else if (mydata[i] != null)
                  {
                      GameObject objj = Instantiate(recordata, recordspawn);
                      objj.GetComponent<MyEventData>().recorddata.text = mydata[i].distance.istance + "         " + mydata[i].time;
                      objj.GetComponent<MyEventData>().myeventclass = new MyEventData.MyEvent();
                      objj.GetComponent<MyEventData>().myeventclass.links = new List<MyEventData.Link>();
                      objj.GetComponent<MyEventData>().myeventclass.journal = new MyEventData.Journal();
                      objj.GetComponent<MyEventData>().myeventclass.distance = new MyEventData.Distance();
                      objj.GetComponent<MyEventData>().myeventclass.goals = new List<MyEventData.Goal>();
                      objj.GetComponent<MyEventData>().myeventclass.result = new MyEventData.Result();
                      objj.GetComponent<MyEventData>().myeventclass.position = mydata[i].position;

                      objj.GetComponent<MyEventData>().myeventclass.totalpeople = mydata[i].totalpeople;
                      objj.GetComponent<MyEventData>().myeventclass.eventType = mydata[i].eventType;
                      objj.GetComponent<MyEventData>().myeventclass.id = mydata[i]._id;

                      objj.GetComponent<MyEventData>().myeventclass.city = mydata[i].city;
                      objj.GetComponent<MyEventData>().myeventclass.race = mydata[i].race;
                      objj.GetComponent<MyEventData>().myeventclass.result.myGender = mydata[i].result.myGender;
                      objj.GetComponent<MyEventData>().myeventclass.result.myAgeGroup = mydata[i].result.myAgeGroup;
                      objj.GetComponent<MyEventData>().myeventclass.result.totalGender = mydata[i].result.totalGender;
                      objj.GetComponent<MyEventData>().myeventclass.result.totalAgeGroup = mydata[i].result.totalAgeGroup;



                      objj.GetComponent<MyEventData>().myeventclass.eventName = mydata[i].eventName;
                      objj.GetComponent<MyEventData>().myeventclass.eventDate = mydata[i].eventDate;
                      objj.GetComponent<MyEventData>().myeventclass.victoryCity = mydata[i].victoryCity;
                      objj.GetComponent<MyEventData>().myeventclass.time = mydata[i].time;
                      objj.GetComponent<MyEventData>().myeventclass.distance.istance = mydata[i].distance.istance;
                      objj.GetComponent<MyEventData>().myeventclass.personarecord = mydata[i].personarecord;
                      objj.GetComponent<MyEventData>().myeventclass.distance.unit = mydata[i].distance.unit;


                      objj.GetComponent<MyEventData>().myeventclass.@virtual = mydata[i].@virtual;
                      if (mydata[i].image != "")
                      {
                          Debug.Log("image captured");
                          objj.GetComponent<MyEventData>().myeventclass.image = mydata[i].image;
                      }
                      objj.GetComponent<MyEventData>().myeventclass.theme = mydata[i].theme;
                      if (mydata[i].links != null)
                      {
                          for (int k = 0; k < mydata[i].links.Count; k++)
                          {
                              MyEventData.Link X = new MyEventData.Link();
                              X.title = mydata[i].links[k].title;
                              X.url = mydata[i].links[k].url;
                              objj.GetComponent<MyEventData>().myeventclass.links.Add(X);
                          }
                      }
                      if (mydata[i].goals != null)
                      {
                          Debug.Log("goals recieve: " + mydata[i].goals.Count);
                          for (int k = 0; k < mydata[i].goals.Count; k++)
                          {
                              MyEventData.Goal X = new MyEventData.Goal();
                              X.achieved = mydata[i].goals[k].achieved;
                              X.goal = mydata[i].goals[k].goal;
                              objj.GetComponent<MyEventData>().myeventclass.goals.Add(X);
                          }
                      }


                      objj.GetComponent<MyEventData>().myeventclass.journal.pre = mydata[i].journal.pre;
                      objj.GetComponent<MyEventData>().myeventclass.journal.post = mydata[i].journal.post;
                      objj.GetComponent<MyEventData>().myeventclass.journal.Next = mydata[i].journal.Next;
                      objj.GetComponent<MyEventData>().myeventclass.journal.Notes = mydata[i].journal.Notes;
                      objj.GetComponent<MyEventData>().myeventclass.journal.race = mydata[i].journal.race;




                      objj.GetComponent<MyEventData>().myeventclass.achivement = mydata[i].achivement;
                      objj.GetComponent<MyEventData>().myeventclass.user = mydata[i].user;
                      objj.GetComponent<MyEventData>().myeventclass.createdAt = mydata[i].createdAt;
                      objj.GetComponent<MyEventData>().myeventclass.updatedAt = mydata[i].updatedAt;
                      objj.GetComponent<MyEventData>().myeventclass.__v = mydata[i].__v;






                  }
              }*/

            //if (rewardCount < 7)
            //{

            GameObject obj = Instantiate(recordata, recordspawn);

            instantiatedObjs.Add(obj);
            


            rewardCount += 1;
            //Debug.LogError("mydata.Count: " + mydata.Count);
            //Debug.LogError("rewardCount: " + rewardCount);

            tempdistance = mydata[i].distance.istance;
            textsplit = mydata[i].distance.istance.Split(char.Parse("."));
            string distance = textsplit[0] + textsplit[1] + textsplit[2];

            bool mile = false;
            if (mydata[i].distance.unit == "mi")
            {
                mile = true;
            }
            else
            {
                mile = false;
            }

            switch (mydata[i].eventType)
            {

                case "5K":

                    obj.GetComponent<MyEventData>().rewardImage.sprite = recordimages[0];

                    K5.Add(mydata[i]);

                    break;

                case "10K":

                    obj.GetComponent<MyEventData>().rewardImage.sprite = recordimages[1];
                    K10.Add(mydata[i]);

                    break;

                case "15K":

                    obj.GetComponent<MyEventData>().rewardImage.sprite = recordimages[2];
                    K15.Add(mydata[i]);

                    break;

                case "HALFATHON":

                    obj.GetComponent<MyEventData>().rewardImage.sprite = recordimages[3];
                    K13.Add(mydata[i]);

                    break;

                case "MARATHON":

                    obj.GetComponent<MyEventData>().rewardImage.sprite = recordimages[4];
                    K26.Add(mydata[i]);

                    break;

                case "COMBO":

                    M10.Add(mydata[i]);

                    if (mile && textsplit[1] == "10")
                    {
                        obj.GetComponent<MyEventData>().rewardImage.sprite = recordimages[5];
                    }
                    else if(!mile && textsplit[1] == "16")
                    {
                        obj.GetComponent<MyEventData>().rewardImage.sprite = recordimages[5];
                    }
                    else
                    {
                        Destroy(obj);
                        instantiatedObjs.Remove(obj);
                    }
                        

                    break;

                case "OBSTACLE":

                    M10.Add(mydata[i]);

                    if (mile && textsplit[1] == "10")
                    {
                        obj.GetComponent<MyEventData>().rewardImage.sprite = recordimages[5];
                    }
                    else if (!mile && textsplit[1] == "16")
                    {
                        obj.GetComponent<MyEventData>().rewardImage.sprite = recordimages[5];
                    }
                    else
                    {
                        Destroy(obj);
                        instantiatedObjs.Remove(obj);
                    }

                    break;

                case "FUN RUN":

                    M10.Add(mydata[i]);

                    if (mile && textsplit[1] == "10")
                    {
                        obj.GetComponent<MyEventData>().rewardImage.sprite = recordimages[5];
                    }
                    else if (!mile && textsplit[1] == "16")
                    {
                        obj.GetComponent<MyEventData>().rewardImage.sprite = recordimages[5];
                    }
                    else
                    {
                        Destroy(obj);
                        instantiatedObjs.Remove(obj);
                    }

                    break;

                case "OTHER":

                    M10.Add(mydata[i]);

                    if (mile && textsplit[1] == "10")
                    {
                        obj.GetComponent<MyEventData>().rewardImage.sprite = recordimages[5];
                    }
                    else if (!mile && textsplit[1] == "16")
                    {
                        obj.GetComponent<MyEventData>().rewardImage.sprite = recordimages[5];
                    }
                    else
                    {
                        Destroy(obj);
                        instantiatedObjs.Remove(obj);
                    }

                    break;



            }

           


            foreach (eventdata2send go in mydata)
            {
                textsplit = go.time.Split(':');


                if (textsplit[0] == "00")
                {
                    textsplit[0] = "0";
                }

                go.time = string.Join(":", textsplit);

                string timee = textsplit[0] + textsplit[1] + textsplit[2];
            
                b = Convert.ToInt32(timee);
                mydata[i].timevalue = b;

                Debug.Log("The time We Wanted to see" + mydata[i].time);
            }


            //Debug.Log("length: " + mydata[i].showdistance.Length +" " + mydata[i].showdistance);
            obj.GetComponent<MyEventData>().recorddata.text = mydata[i].showdistance + " ";
            obj.GetComponent<MyEventData>().recordtime.text = mydata[i].time;





            obj.GetComponent<MyEventData>().myeventclass = new MyEventData.MyEvent();
            obj.GetComponent<MyEventData>().myeventclass.links = new List<MyEventData.Link>();
            obj.GetComponent<MyEventData>().myeventclass.journal = new MyEventData.Journal();
            obj.GetComponent<MyEventData>().myeventclass.distance = new MyEventData.Distance();
            obj.GetComponent<MyEventData>().myeventclass.goals = new List<MyEventData.Goal>();
            obj.GetComponent<MyEventData>().myeventclass.result = new MyEventData.Result();
            obj.GetComponent<MyEventData>().myeventclass.position = mydata[i].position;

            obj.GetComponent<MyEventData>().myeventclass.totalpeople = mydata[i].totalpeople;
            obj.GetComponent<MyEventData>().myeventclass.eventType = mydata[i].eventType;
            obj.GetComponent<MyEventData>().myeventclass.id = mydata[i]._id;

            obj.GetComponent<MyEventData>().myeventclass.city = mydata[i].city;
            obj.GetComponent<MyEventData>().myeventclass.race = mydata[i].race;
            obj.GetComponent<MyEventData>().myeventclass.result.myGender = mydata[i].result.myGender;
            obj.GetComponent<MyEventData>().myeventclass.result.myAgeGroup = mydata[i].result.myAgeGroup;
            obj.GetComponent<MyEventData>().myeventclass.result.totalGender = mydata[i].result.totalGender;
            obj.GetComponent<MyEventData>().myeventclass.result.totalAgeGroup = mydata[i].result.totalAgeGroup;



            obj.GetComponent<MyEventData>().myeventclass.eventName = mydata[i].eventName;
            obj.GetComponent<MyEventData>().myeventclass.eventDate = mydata[i].eventDate;
            obj.GetComponent<MyEventData>().myeventclass.victoryCity = mydata[i].victoryCity;



            

            obj.GetComponent<MyEventData>().myeventclass.time = mydata[i].time;

            


            obj.GetComponent<MyEventData>().myeventclass.distance.istance = mydata[i].distance.istance;
            obj.GetComponent<MyEventData>().myeventclass.personarecord = mydata[i].personarecord;
            obj.GetComponent<MyEventData>().myeventclass.distance.unit = mydata[i].distance.unit;






            obj.GetComponent<MyEventData>().myeventclass.@virtual = mydata[i].@virtual;
            if (mydata[i].image != "")
            {
                Debug.Log("image captured");
                obj.GetComponent<MyEventData>().myeventclass.image = mydata[i].image;
            }
            obj.GetComponent<MyEventData>().myeventclass.theme = mydata[i].theme;
            if (mydata[i].links != null)
            {
                for (int k = 0; k < mydata[i].links.Count; k++)
                {
                    MyEventData.Link X = new MyEventData.Link();
                    X.title = mydata[i].links[k].title;
                    X.url = mydata[i].links[k].url;
                    obj.GetComponent<MyEventData>().myeventclass.links.Add(X);
                }
            }
            if (mydata[i].goals != null)
            {
                Debug.Log("goals recieve: " + mydata[i].goals.Count);
                for (int k = 0; k < mydata[i].goals.Count; k++)
                {
                    MyEventData.Goal X = new MyEventData.Goal();
                    X.achieved = mydata[i].goals[k].achieved;
                    X.goal = mydata[i].goals[k].goal;
                    obj.GetComponent<MyEventData>().myeventclass.goals.Add(X);
                }
            }


            obj.GetComponent<MyEventData>().myeventclass.journal.pre = mydata[i].journal.pre;
            obj.GetComponent<MyEventData>().myeventclass.journal.post = mydata[i].journal.post;
            obj.GetComponent<MyEventData>().myeventclass.journal.Next = mydata[i].journal.Next;
            obj.GetComponent<MyEventData>().myeventclass.journal.Notes = mydata[i].journal.Notes;
            obj.GetComponent<MyEventData>().myeventclass.journal.race = mydata[i].journal.race;




            obj.GetComponent<MyEventData>().myeventclass.achivement = mydata[i].achivement;
            obj.GetComponent<MyEventData>().myeventclass.user = mydata[i].user;
            obj.GetComponent<MyEventData>().myeventclass.createdAt = mydata[i].createdAt;
            obj.GetComponent<MyEventData>().myeventclass.updatedAt = mydata[i].updatedAt;
            obj.GetComponent<MyEventData>().myeventclass.__v = mydata[i].__v;

            //K5.Sort((x, y) => x.time.CompareTo(y.time));

            //K10.Sort((x, y) => x.time.CompareTo(y.time));

            //K15.Sort((x, y) => x.time.CompareTo(y.time));

            //K13.Sort((x, y) => x.time.CompareTo(y.time));

            //K26.Sort((x, y) => x.time.CompareTo(y.time));

            //M10.Sort((x, y) => x.time.CompareTo(y.time));

            //M10.Sort((x, y) => x.time.CompareTo(y.time));

            //M10.Sort((x, y) => x.time.CompareTo(y.time));

            //M10.Sort((x, y) => x.time.CompareTo(y.time));


            //allEvents.Clear();

            //allEvents.AddRange(K5);
            //allEvents.AddRange(K10);
            //allEvents.AddRange(K15);
            //allEvents.AddRange(K13);
            //allEvents.AddRange(K26);
            //allEvents.AddRange(M10);

            ////allEvents.Sort((x, y) => x.timevalue.CompareTo(y.timevalue));

            //foreach (var item in K5)
            //{
            //    Debug.Log("items are" + item.time);
            //}

            //if (i + 1 < mydata.Count)
            //{
            //    if (mydata[i].value == mydata[i + 1].value)
            //    {
            //        if (mydata[i].timevalue > mydata[i + 1].timevalue)
            //        {
            //            Debug.Log("obj: " + obj + " is being destroyed and time is " + mydata[i].timevalue);
            //            Destroy(obj);
            //            instantiatedObjs.Remove(obj);
            //            //obj.SetActive(false);

            //        }
            //        if (mydata[i].timevalue == mydata[i + 1].timevalue)
            //        {
            //            Debug.Log("obj: " + obj + " is being destroyed and time is " + mydata[i].timevalue);
            //            Destroy(obj);
            //            instantiatedObjs.Remove(obj);
            //            //obj.SetActive(false);
            //        }

            //    }
            //    if (i > 0)
            //    {
            //        if (mydata[i].value == mydata[i - 1].value)
            //        {
            //            if (mydata[i].timevalue > mydata[i - 1].timevalue)
            //            {
            //                Debug.Log("obj: " + obj + " is being destroyed and time is " + mydata[i - 1].timevalue);
            //                Destroy(obj);
            //                instantiatedObjs.Remove(obj);
            //                //obj.SetActive(false);
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    if (i > 0)
            //    {
            //        if (mydata[i].value == mydata[i - 1].value)
            //        {
            //            if (mydata[i].timevalue > mydata[i - 1].timevalue)
            //            {
            //                Debug.Log("obj: " + obj + " is being destroyed and time is " + mydata[i].timevalue);
            //                Destroy(obj);
            //                instantiatedObjs.Remove(obj);
            //                //obj.SetActive(false);
            //            }
            //        }
            //    }
            //}
        }

        int a = instantiatedObjs.Count;

        
      

     

        Invoke("delay",0f);

        K5.Sort((x, y) => x.time.CompareTo(y.time));

        K10.Sort((x, y) => x.time.CompareTo(y.time));

        K15.Sort((x, y) => x.time.CompareTo(y.time));

        K13.Sort((x, y) => x.time.CompareTo(y.time));

        K26.Sort((x, y) => x.time.CompareTo(y.time));

        M10.Sort((x, y) => x.time.CompareTo(y.time));

        for (int i = 0; i < instantiatedObjs.Count; i++)
        {
            instantiatedObjs[i].gameObject.SetActive(false);

        }


        for (int j = 0; j < instantiatedObjs.Count; j++)
        {
            if (!k5Done)
            {
                if (K5.Count > 0)
                {
                    for (int i = 0; i < K5.Count; i++)
                    {


                        if (instantiatedObjs[j].GetComponent<MyEventData>().recordtime.text == K5[0].time && instantiatedObjs[j].GetComponent<MyEventData>().rewardImage.sprite == recordimages[0])
                        {
                            Debug.Log("iten 0 of 5K is:" + instantiatedObjs[j].GetComponent<MyEventData>().recordtime.text);
                            instantiatedObjs[j].gameObject.SetActive(true);
                            k5Done = true;
                        }

                    }
                }
            }
            if (!k10Done)
            {
                if (K10.Count > 0)
                {
                    for (int i = 0; i < K10.Count; i++)
                    {


                        if (instantiatedObjs[j].GetComponent<MyEventData>().recordtime.text == K10[0].time && instantiatedObjs[j].GetComponent<MyEventData>().rewardImage.sprite == recordimages[1])
                        {
                            Debug.Log("iten 0 of 10K is:" + instantiatedObjs[j].GetComponent<MyEventData>().recordtime.text);
                            instantiatedObjs[j].gameObject.SetActive(true);
                            k10Done = true;
                        }
                    }
                }
            }
            if (!k15Done)
            {
                if (K15.Count > 0)
                {
                    for (int i = 0; i < K15.Count; i++)
                    {


                        if (instantiatedObjs[j].GetComponent<MyEventData>().recordtime.text == K15[0].time && instantiatedObjs[j].GetComponent<MyEventData>().rewardImage.sprite == recordimages[2])
                        {
                            Debug.Log("iten 0 of 15K is:" + instantiatedObjs[j].GetComponent<MyEventData>().recordtime.text);
                            instantiatedObjs[j].gameObject.SetActive(true);
                            k15Done = true;
                        }
                    }
                }
            }
            if (!k13Done)
            {
                if (K13.Count > 0)
                {
                    for (int i = 0; i < K13.Count; i++)
                    {


                        if (instantiatedObjs[j].GetComponent<MyEventData>().recordtime.text == K13[0].time && instantiatedObjs[j].GetComponent<MyEventData>().rewardImage.sprite == recordimages[3])
                        {
                            Debug.Log("iten 0 of 13K is:" + instantiatedObjs[j].GetComponent<MyEventData>().recordtime.text);
                            instantiatedObjs[j].gameObject.SetActive(true);
                            k13Done = true;
                        }
                    }
                }
            }
            if (!k26Done)
            {
                if (K26.Count > 0)
                {
                    for (int i = 0; i < K26.Count; i++)
                    {


                        if (instantiatedObjs[j].GetComponent<MyEventData>().recordtime.text == K26[0].time && instantiatedObjs[j].GetComponent<MyEventData>().rewardImage.sprite == recordimages[4])
                        {
                            Debug.Log("iten 0 of 26K is:" + instantiatedObjs[j].GetComponent<MyEventData>().recordtime.text);
                            instantiatedObjs[j].gameObject.SetActive(true);
                            k26Done = true;
                        }
                    }
                }
            }
            if (!M10Done)
            {
                if (M10.Count > 0)
                {
                    for (int i = 0; i < M10.Count; i++)
                    {


                        if (instantiatedObjs[j].GetComponent<MyEventData>().recordtime.text == M10[0].time && instantiatedObjs[j].GetComponent<MyEventData>().rewardImage.sprite == recordimages[5])
                        {
                            Debug.Log("iten 0 of M10 is:" + instantiatedObjs[j].GetComponent<MyEventData>().recordtime.text);
                            instantiatedObjs[j].gameObject.SetActive(true);
                            k10Done = true;
                        }
                    }
                }
            }
        }
        eventgetter.instance.ReserProfileScroller();
}

    public void delay()
    {
        Debug.Log("logger: " + recordspawn.childCount + " " + kmdata.Count);

        for (int i = 1; i < recordspawn.childCount; i++)
        {
            RectTransform cre;
            cre = recordsize.GetComponent<RectTransform>();
            cre.sizeDelta = new Vector2(cre.sizeDelta.x, cre.sizeDelta.y + sizeofrecord.y);
            recordsize.GetComponent<RectTransform>().sizeDelta = cre.sizeDelta;
            recordsize.gameObject.GetComponent<LayoutElement>().preferredHeight += sizeofrecord.y;




            //  profilescroll.content.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(profilescroll.content.gameObject.GetComponent<RectTransform>().sizeDelta.x, recordsize.gameObject.GetComponent<LayoutElement>().preferredHeight);
        }

        for (int i = 0; i < recordspawn.childCount; i++)
        {
            if (keptdistance == 0)
            {
                kmdata.Add(recordspawn.GetChild(i).GetComponent<MyEventData>().recorddata.text);
            }
            else
            {
                milesdata.Add(recordspawn.GetChild(i).GetComponent<MyEventData>().recorddata.text); 
            
            
            }
        }
        Debug.Log("logger: " + recordspawn.childCount+" "+kmdata.Count);
    }

    public void check()
    {
      /*  if (keptdistance == 0)
        {
            kmtoogle.isOn = false;
            Debug.Log("checking: " + keptdistance);
        }
        else
        {
            kmtoogle.isOn = true;
            Debug.Log("checking: " + keptdistance);


        }*/


    }

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        keptdistance = PlayerPrefs.GetInt("keptdistance",0);
        originalrecordsize = recordsize;
        originalscroll = profilescroll;
        baseurl = baselink.Url;
        senddata();
    }

    public void RefreshRecords()
    {
        if (eventgetter.instance.canRefresh)
        {
            SceneManager.LoadScene("4. MainScreens");
            eventgetter.instance.canRefresh = false;
        }
        
    }

    // Update is called once per frame
    void Update()
        {

        }
    }

