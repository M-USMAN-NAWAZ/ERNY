using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

//using AdvancedInputFieldPlugin;
public class updataeven : MonoBehaviour
{
    public string[] eventnamechecker;
    public InputField distancecomplete, timecomplete;
    public Sprite defaultpic;
    public GameObject[] themebuttons;
    public TMP_InputField pree, postt, nextt, racee;
    //public AdvancedInputField pretext, posttext, racetext, nexttext;
    public GameObject img1, img2;
    public int imgcheck = 0;
    string[] datesplit;
    public Button save;
    public static updataeven instance;
    public GameObject ARCAMER, ARCANVAS, canv, arsessio, camer, bottom;
    public static Texture2D eventimgetexture;
    public GameObject loader;
    public string id, imagepath;
    public string baseurl;
    public InputField Eventname;
    public string json2send;
    public InputField cityname,date;
    public InputField victorycatchphrase;
    public InputField time, distance;
    public InputField goal, agegroup, totalagegroup, gender, totalgender, title, prenotes, race, post, next, notes, result,position,totalpeople;
    public GameObject[] themes;
    public string eventImage;
    public static Texture death;
    public InputField racerno;
   // public static Texture eventimgetexture;
    public RawImage hiimage;
  public  string typegetter, themegetter;
    public Texture dd;
    public List<string> mygoals;
    public int goalgetter;
    public int linkgetter;
    public Toggle vr, pr, distanceunit;
    public string theme,type;
    //goals sent data
    public GameObject gaolspawn;
    public ScrollRect goalscroll;
    public Vector2 sizeofgoal, goaltransformsize ;
    public RectTransform goaltranform;
    public Dropdown dropyear, dropmonth, dropdate;
    //link sent data
    public GameObject linkspawn;
 public Vector2 sizeoflink;
    public ScrollRect linkscroll;
    public RectTransform linktransform;
    public Transform linkcontent;
    public int spaxingsize = 25;
    public int countoflinks = 1;
    public int countofgoals = 1;
    public ScrollRect parentscroll;
    // Start is called before the first frame update
    public string pot, garmin, strava, reddit, youtube, instagram, tiktok, facebook, athlinks, custom1, custom2, custom3;
    public Text victoylimittext;
    public string victorylimit;
    public GameObject goalsobject, mediaobject, portalobject, journalobject;
    public GameObject[] linksobj;
    public Toggle[] links;
    public InputField[] linksurl;
    public string[] linktitle;
    public GameObject[] linksprites;
    //timer stuff
    public InputField time1, time2, time3;
    public int timeone, timetwo, timethree;

    public string firsttime, secondtime, thirdtime;
    public GameObject[] ar100medal;
    public int MONTHEDIT;

    //distance stuff
    public InputField d1, d2, d3;
    //public int one, timetwo, timethree;

    public string firstdistance, seconddistance, thirddistance;
    private Vector2 orignalgaolsizedelata;
    private Vector2 originalparentscroll;
    public void eventnamecheck()
    {
              eventnamechecker = Eventname.text.Split(char.Parse(" "));
        for (int i = 0; i < eventnamechecker.Length; i++)
        {
            for (int c = 0; c < 1; c++)
            {

                if (eventnamechecker[i].ToLower() ==eventgetter.instance.disney100medal[c].ToLower() && dropyear.value == 1)
                {
                    if (eventnamechecker.Length >= i + 1)
                    {
                        if (eventnamechecker[i + 1].ToLower() == eventgetter.instance.disney100medal[c + 1].ToLower())
                        {

                            if (eventnamechecker.Length >= i + 2)
                            {

                                if (eventnamechecker[i + 2].ToLower() == eventgetter.instance.disney100medal[c + 2].ToLower())
                                {
                                    
                                      //      ar100medal[0].SetActive(true);
                                       ///     selecttheme("medaldisney");
                                        //    themenumebr(6);
                                            return;
                                        
                                    
                                }
                            }
                        }
                    }

                }
                else
                {
                    ar100medal[0].SetActive(false);

                }


            }


            for (int b = 0; b <eventgetter.instance.spartan.Length; b++)
            {
                if (eventnamechecker[i].ToLower() == eventgetter.instance.disney[b])
                {
                    selecttheme("clover");
                    themenumebr(1);

                    return;
                }
                else if (eventnamechecker[i].ToLower() == eventgetter.instance.spartan[b])
                {

                    selecttheme("baloon");
                    themenumebr(0);

                    return;
                }
               else
                {
                    if (themegetter == "")
                    {
                        selecttheme("baloon");
                        themenumebr(0);
                    }
                    ar100medal[0].SetActive(false);
                }
                
            }
            
            
        }

    }
    public void changeurl(int url)
    {
        if (links[url].isOn)
        {
          
            for (int i = 0; i < linksobj.Length; i++)
            {


                if (i == url)
                {
                    linksobj[i].SetActive(true);

                    if (linksurl[i].text != "")
                    {
                        linksprites[i].SetActive(true);
                    }
                    else
                    {
                        linksprites[i].SetActive(false);

                    }
                
                
                }
                else
                {
                    if (linksurl[i].text != "")
                    {
                        linksprites[i].SetActive(true);
                    }
                    else
                    {
                        linksprites[i].SetActive(false);

                    }
                    links[i].isOn = false;
                    linksobj[i].SetActive(false);
                }
            }


        }
    }
    public void changeupper()
    {
        victorycatchphrase.text = victorycatchphrase.text.ToUpper();
    }

    public void onedsitance()
    {
        firstdistance = d1.text.Normalize();
        if (firstdistance.Length == 2)
        {
            d2.ActivateInputField();
        }



    }
    public void kmchange()
    {
        if (distanceunit.isOn)
        {
            if (distancecomplete.text == "500" || distancecomplete.text == "00500")
            {
                distancecomplete.text = "310";
                distancetextcheck();
            }
            if (distancecomplete.text == "1000" || distancecomplete.text == "001000")
            {
                distancecomplete.text = "621";
                distancetextcheck();
            }
            if (distancecomplete.text == "1500" || distancecomplete.text == "001500")
            {
                distancecomplete.text = "932";
                distancetextcheck();
            }
            if (distancecomplete.text == "2109" || distancecomplete.text == "002109")
            {
                distancecomplete.text = "1310";
                distancetextcheck();
            }
            if (distancecomplete.text == "4219" || distancecomplete.text == "004219")
            {
                distancecomplete.text = "2620";
                distancetextcheck();
            }

        }
        else
        {

            if (distancecomplete.text == "310" || distancecomplete.text == "00310")
            {
                distancecomplete.text = "500";
                distancetextcheck();
            }
            if (distancecomplete.text == "621" || distancecomplete.text == "00621")
            {
                distancecomplete.text = "1000";
                distancetextcheck();
            }
            if (distancecomplete.text == "932" || distancecomplete.text == "00932")
            {
                distancecomplete.text = "1500";
                distancetextcheck();
            }
            if (distancecomplete.text == "1310" || distancecomplete.text == "001310")
            {
                distancecomplete.text = "2109";
                distancetextcheck();
            }
            if (distancecomplete.text == "2620" || distancecomplete.text == "002620")
            {
                distancecomplete.text = "4219";
                distancetextcheck();
            }







        }



    }

    public void twodistance()
    {
        seconddistance = d2.text.Normalize();
        if (seconddistance.Length == 2)
        {
            d3.ActivateInputField();
        }
        else if (seconddistance.Length == 0)
        {

            //d1.ActivateInputField();
        }

    }
    public void threedistance()
    {
        thirddistance = d3.text.Normalize();
        if (thirddistance.Length == 0)
        {
           // d2.ActivateInputField();
        }

    }



    public void onetimer()
    {
        firsttime = time1.text.Normalize();
        if (firsttime.Length == 2)
        {
            time2.ActivateInputField();
        }



    }

    public void onetwo()
    {
        secondtime = time2.text.Normalize();
        if (secondtime.Length == 2)
        {
            time3.ActivateInputField();
        }
        else if (secondtime.Length == 0)
        {

        //    time1.ActivateInputField();
        }
        Debug.Log("" + secondtime.Length);
    }
    public void onethree()
    {
        thirdtime = time3.text.Normalize();
        if (thirdtime.Length == 0)
        {
          //  time2.ActivateInputField();
        }

    }





    public void themenumebr(int num)
    {
        if (num == 11)
        {
            themegetter = "balloon";
        }

        for (int i = 0; i < themebuttons.Length; i++)
        {
            if (i == num)
            {
                themebuttons[i].SetActive(true);
            }
            else
            {
                themebuttons[i].SetActive(false);



            }
        }
    }



    public void goalminuser()
    {
        goalgetter--;
      
            countofgoals--;
            //        int c = myDeserializedClass.response.Count;
            //    GameObject obj = Instantiate(gaolspawn, goalscroll.content);
            //gaolspawn.transform.SetParent(goaltranform);

            RectTransform cre;
            cre = goalscroll.GetComponent<RectTransform>();
            cre.sizeDelta = new Vector2(cre.sizeDelta.x, cre.sizeDelta.y - sizeofgoal.y);
            goalscroll.GetComponent<RectTransform>().sizeDelta = cre.sizeDelta;
            goalscroll.viewport.sizeDelta = cre.sizeDelta;
            goaltranform.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(goaltranform.gameObject.GetComponent<RectTransform>().sizeDelta.x, goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight);
           
                goalscroll.content.sizeDelta = new Vector2(sizeofgoal.x, sizeofgoal.y / countofgoals);
            
            goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight -= sizeofgoal.y ;
        
        parentscroll.content.sizeDelta = new Vector2(parentscroll.content.sizeDelta.x, parentscroll.content.sizeDelta.y - sizeofgoal.y);


        Debug.Log("goals: " + goalgetter);
    }

    public void scrollreset()
    {
        parentscroll.verticalNormalizedPosition = 1f;
        //        int c = myDeserializedClass.response.Count;
        //    GameObject obj = Instantiate(gaolspawn, goalscroll.content);
        //gaolspawn.transform.SetParent(goaltranform);

        for (int i = 0; i < goalscroll.content.childCount; i++)
        {
            Destroy(goalscroll.content.GetChild(i).gameObject);
        }
        countofgoals = 1;
        goalsobject.GetComponent<ExpandOnClick>().checkingopen();//.SetActive(false);
        portalobject.GetComponent<ExpandOnClick>().checkingopen();
        journalobject.GetComponent<ExpandOnClick>().checkingopen();
        mediaobject.GetComponent<ExpandOnClick>().checkingopen();
        sizeofgoal = orignalgaolsizedelata;
        parentscroll.content.sizeDelta = new Vector2(parentscroll.content.sizeDelta.x, 3838f); ;
        goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight = 410;
        goaltranform.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(goaltranform.gameObject.GetComponent<RectTransform>().sizeDelta.x, goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight);

        goalscroll.content.sizeDelta = sizeofgoal;
        goaltranform.gameObject.GetComponent<RectTransform>().sizeDelta=goaltransformsize;

        Debug.Log("goals: " + goalgetter);
    }
    
    void Start()
    {
        instance = this;

        baseurl = baselink.Url;
        sizeofgoal = goalscroll.content.sizeDelta;
        sizeoflink = goalscroll.content.sizeDelta;
        goaltransformsize = goaltranform.gameObject.GetComponent<RectTransform>().sizeDelta;

        imgcheck = 0;
      orignalgaolsizedelata = sizeofgoal ;
      originalparentscroll =  parentscroll.content.sizeDelta ;


        loader = GameObject.FindGameObjectWithTag("loader").GetComponent<dontdestroy>().loader;
    

    }
    public void opencamera()
    {
     //   ARCAMER.SetActive(true);
     //   ARCANVAS.SetActive(true);
    //    arsessio.SetActive(true);

        bottom.SetActive(false);
        canv.SetActive(false);
        camer.SetActive(false);

    }
    public void closecamera()
    {
      //  ARCAMER.SetActive(false);
      //  ARCANVAS.SetActive(false);
     //   arsessio.SetActive(false);

        bottom.SetActive(true);

        canv.SetActive(true);
        camer.SetActive(true);

    }
    public void selecttheme(string type)
    {
        if (type == "baloon" &&
            AddressableHandler.Instance != null &&
            UnityEngine.EventSystems.EventSystem.current != null)
        {
            GameObject selectedObject = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            Button selectedButton = selectedObject != null ? selectedObject.GetComponent<Button>() : null;
            int index = 10;

            bool selectedIndexElevenPicture = AddressableHandler.Instance.upicturebutton != null &&
                                               index < AddressableHandler.Instance.upicturebutton.Length &&
                                               AddressableHandler.Instance.upicturebutton[index] == selectedButton;
            bool selectedIndexElevenToggle = AddressableHandler.Instance.uptogglebutton != null &&
                                              index < AddressableHandler.Instance.uptogglebutton.Length &&
                                              AddressableHandler.Instance.uptogglebutton[index] == selectedButton;

            if (selectedIndexElevenPicture || selectedIndexElevenToggle)
            {
                type = "balloon";
            }
        }

        themegetter = type;

    }
    public void goaladder()
    {
   //  sizeofgoal = goalscroll.content.sizeDelta;
       goalgetter++;
        countofgoals++;
        //        int c = myDeserializedClass.response.Count;
        GameObject obj = Instantiate(gaolspawn, goalscroll.content);
        //gaolspawn.transform.SetParent(goaltranform);
        RectTransform cre;
        cre = goalscroll.GetComponent<RectTransform>();
        cre.sizeDelta = new Vector2(cre.sizeDelta.x, cre.sizeDelta.y + sizeofgoal.y);
        goalscroll.GetComponent<RectTransform>().sizeDelta = cre.sizeDelta;
        goalscroll.viewport.sizeDelta = cre.sizeDelta;
        goalscroll.content.sizeDelta = new Vector2(sizeofgoal.x, sizeofgoal.y * countofgoals);

        goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight += sizeofgoal.y;
        goaltranform.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(goaltranform.gameObject.GetComponent<RectTransform>().sizeDelta.x, goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight);
        parentscroll.content.sizeDelta = new Vector2(parentscroll.content.sizeDelta.x, parentscroll.content.sizeDelta.y + sizeofgoal.y);


    }
    public void lnkadder()
    {
        linkgetter++;

        countoflinks++;
        //        int c = myDeserializedClass.response.Count;
        GameObject obj = Instantiate(linkspawn, linkscroll.content);

        RectTransform cre;
        cre = linkscroll.GetComponent<RectTransform>();
        cre.sizeDelta = new Vector2(cre.sizeDelta.x, cre.sizeDelta.y + sizeoflink.y);
        linkscroll.GetComponent<RectTransform>().sizeDelta = cre.sizeDelta;
        //gaolspawn.transform.SetParent(goaltranform);
        linkscroll.content.sizeDelta = new Vector2(sizeoflink.x, sizeoflink.y * countoflinks);
        linktransform.gameObject.GetComponent<LayoutElement>().preferredHeight += sizeoflink.y + countoflinks;
        parentscroll.content.sizeDelta = new Vector2(parentscroll.content.sizeDelta.x, parentscroll.content.sizeDelta.y * sizeoflink.y);

    }


    public void url()
    {

        int c = linkcontent.childCount;
        for (int i = 0; i < c; i++)
        {
            linkcontent.GetChild(i).GetChild(0).gameObject.SetActive(true);

            linkcontent.GetChild(i).GetChild(1).gameObject.SetActive(false);
        }

    }

    public void titlee()
    {
        int c = linkcontent.childCount;
        for (int i = 0; i < c; i++)
        {
            linkcontent.GetChild(i).GetChild(1).gameObject.SetActive(true);

            linkcontent.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }
    }



    List<string> substrings = new List<string>();
    public void distancetextcheck()
    {
        substrings.Clear();
        if (distancecomplete.text.Length > 0)
        {
            if (distancecomplete.text.Length == 3)
            {


                for (int i = 0; i < distancecomplete.text.Length; i += 1)
                {
                    substrings.Add(distancecomplete.text.Substring(i, Mathf.Min(1, distancecomplete.text.Length - i)));

                }


                if (substrings.Count == 3)
                {
                    d1.text = "";
                    d2.text = substrings[0];
                    d3.text = substrings[1] + substrings[2];

                }
            }
            if (distancecomplete.text.Length == 5)
            {


                for (int i = 0; i < distancecomplete.text.Length; i += 1)
                {
                    substrings.Add(distancecomplete.text.Substring(i, Mathf.Min(1, distancecomplete.text.Length - i)));

                }


                if (substrings.Count == 5)
                {
                    d1.text = substrings[0];
                    d2.text = substrings[1] + substrings[2];
                    d3.text = substrings[3] + substrings[4];

                }
            }
            else
            {
                for (int i = 0; i < distancecomplete.text.Length; i += 2)
                {
                    substrings.Add(distancecomplete.text.Substring(i, Mathf.Min(2, distancecomplete.text.Length - i)));

                }
                if (substrings.Count == 1)
                {
                    d3.text = substrings[0];
                    d2.text = "";
                    d1.text = "";
                }
                if (substrings.Count == 2)
                {
                    d1.text = "";
                    d2.text = substrings[0];
                    d3.text = substrings[1];

                }

                if (substrings.Count == 3)
                {
                    d1.text = substrings[0];
                    d2.text = substrings[1];
                    d3.text = substrings[2];

                }

            }
        }
        else
        {
            d1.text = "";
            d2.text = "";
            d3.text = "";

        }


    }

    public void timetextcheck()
    {

       
            if (timecomplete.text.Length > 0)
            {


                if (timecomplete.text.Length == 3)
                {


                    for (int i = 0; i < timecomplete.text.Length; i += 1)
                    {
                        substrings.Add(timecomplete.text.Substring(i, Mathf.Min(1, timecomplete.text.Length - i)));

                    }


                    if (substrings.Count == 3)
                    {
                        time1.text = "";
                        time2.text = substrings[0];
                        time3.text = substrings[1] + substrings[2];

                    }
                }

                if (timecomplete.text.Length == 5)
                {


                    for (int i = 0; i < timecomplete.text.Length; i += 1)
                    {
                        substrings.Add(timecomplete.text.Substring(i, Mathf.Min(1, timecomplete.text.Length - i)));

                    }


                    if (substrings.Count == 5)
                    {
                        time1.text = substrings[0];
                        time2.text = substrings[1] + substrings[2];
                        time3.text = substrings[3] + substrings[4];

                    }
                }
                else
                {
                    for (int i = 0; i < timecomplete.text.Length; i += 2)
                    {
                        substrings.Add(timecomplete.text.Substring(i, Mathf.Min(2, timecomplete.text.Length - i)));

                    }
                    if (substrings.Count == 1)
                    {
                        time3.text = substrings[0];
                        time2.text = "";
                        time1.text = "";
                    }
                    if (substrings.Count == 2)
                    {
                        time1.text = "";
                        time2.text = substrings[0];
                        time3.text = substrings[1];

                    }

                    if (substrings.Count == 3)
                    {
                        time1.text = substrings[0];
                        time2.text = substrings[1];
                        time3.text = substrings[2];

                    }

                }
            }
            else
            {
                time1.text = "";
                time2.text = "";
                time3.text = "";

            }
            substrings.Clear();

        
    }



    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Distance
    {
        public string istance { get; set; }
        public string unit { get; set; }
    }

    public class Event
    {
        public string totalpeople { get; set; }
        public string position { get; set; }
        public string id { get; set; }
        public string eventName { get; set; }
        public string eventDate { get; set; }
        public object city { get; set; }
        public object race { get; set; }
        public string victoryCity { get; set; }
        public string time { get; set; }
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
     //   public DateTime createdAt { get; set; }
      //  public DateTime updatedAt { get; set; }
       // public int __v { get; set; }
    }

    public class Goal
    {
        public string achieved { get; set; }
        public string goal { get; set; }
    }

    public class Journal
    {
        public string pre { get; set; }
        public string race { get; set; }
        public string post { get; set; }
        public string Next { get; set; }
        public string Notes { get; set; }
    }

    public class Link
    {
        public string title { get; set; }
        public string url { get; set; }
    }

    public class Response
    {
        public Event @event { get; set; }
    }

    public class Result
    {
        public string myAgeGroup { get; set; }
        public string totalAgeGroup { get; set; }
        public string myGender { get; set; }
        public string totalGender { get; set; }
    }

    public class Root
    {
        public bool success { get; set; }
        public Response response { get; set; }
        public object metadata { get; set; }
    }

    public string mystring;
    public string mysecondstring,mythirdstring;
    public void senddata()
    {
        Event eventsenddata = new Event();
        if (racerno.text != null)
        {
            eventsenddata.race = racerno.text;
        }
        else
        {
            eventsenddata.race = "";



        }
       
        eventsenddata.eventName = Eventname.text.Normalize();
        eventsenddata.city = cityname.text.Normalize();
        eventsenddata.victoryCity = victorycatchphrase.text.Normalize();

        guestvictorycatchp = eventsenddata.victoryCity;

        eventsenddata.totalpeople = totalpeople.text;
        eventsenddata.position = position.text;

        if (firsttime != "")
        {
            if (secondtime != "")
            {
                if (thirdtime != "")
                {
                    eventsenddata.time = time1.text.Normalize() + ":" + time2.text.Normalize() + ":" + time3.text.Normalize();
                }
                else
                {
                    eventsenddata.time = time1.text.Normalize() + ":" + time2.text.Normalize() + ":" + "00";
                }

            }
            else if (thirdtime != "")
            {
                eventsenddata.time = time1.text.Normalize() + ":" + "00" + ":" + time3.text.Normalize();

            }
            else
            {
                eventsenddata.time = time1.text.Normalize() + ":" + "00" + ":" + "00";

            }

        }
        else if (secondtime != "")
        {
            if (thirdtime != "")
            {
                eventsenddata.time = "00" + ":" + time2.text.Normalize() + ":" + time3.text.Normalize();

            }
            else
            {
                eventsenddata.time = "00:" + time2.text.Normalize() + ":" + "00";
            }
        }
        else if (thirdtime != "")
        {
            eventsenddata.time = "00:" + "00" + ":" + time3.text.Normalize();

        }
        else
        {
            eventsenddata.time = "00:" + "00" + ":" + "00";
        }



        guesttime = eventsenddata.time;

        // time3.DeactivateInputField();
        eventsenddata.distance = new Distance();





        if (firstdistance != "")
        {
            if (seconddistance != "")
            {
                if (thirddistance != "")
                {
                    eventsenddata.distance.istance = firstdistance + "." + seconddistance + "." + thirddistance;
                }
                else
                {
                    eventsenddata.distance.istance = firstdistance + "." + seconddistance + "." + "00";
                }
            }
            else if (thirddistance != "")
            {
                eventsenddata.distance.istance = firstdistance + "." + "00" + "." + thirddistance;
            }
            else
            {
                eventsenddata.distance.istance = firstdistance + "." + "00" + "." + "00";
            }
        }
        else if (seconddistance != "")
        {
            if (thirddistance != "")
            {
                eventsenddata.distance.istance = "00" + "." + seconddistance + "." + thirddistance;
            }
            else
            {
                eventsenddata.distance.istance = "00." + seconddistance + "." + "00";
            }
            

        }
        else if (thirddistance != "")
        {
            eventsenddata.distance.istance = "00." + "00" + "." + thirddistance;

        }
        else
        {
            eventsenddata.distance.istance = "00." + "00" + "." + "00";
        }

        guestdistance = eventsenddata.distance.istance;






        eventsenddata.journal = new Journal();
        eventsenddata.journal.pre = pree.text;
        eventsenddata.journal.post = postt.text;
        eventsenddata.journal.Next = nextt.text;
        eventsenddata.journal.race = racee.text;
        eventsenddata.journal.Notes = notes.text.Normalize();
        Debug.Log("update event id: " + eventImage);
        eventsenddata.id = id;
        eventsenddata.image = eventImage;
        Debug.Log("update event id: " + eventsenddata.id);
        if (distanceunit.isOn)
        {
            eventsenddata.distance.unit = "mi";
        }
        else
        {

            eventsenddata.distance.unit = "km";
        }


        gdistunit = eventsenddata.distance.unit;



        if (vr.isOn)
        {
            eventsenddata.@virtual = "yes";

        }
        else
        {
            eventsenddata.@virtual = "no";

        }
        gvr = eventsenddata.@virtual;


        if (pr.isOn)
        {
            eventsenddata.personarecord = "yes";
        }
        else
        {
            eventsenddata.personarecord = "no";

        }
        gpr = eventsenddata.personarecord;

        ///  eventsenddata.eventType = typegetter.Normalize();


        if (themegetter != null)
        {
            eventsenddata.theme = themegetter.Normalize();
        }
        else
        {
            eventsenddata.theme = "baloon";
        }
        Debug.Log("date calue:"+eventsenddata.theme);

        eventsenddata.eventType = type;


        int month = dropmonth.GetComponent<Dropdown>().value;
        List<Dropdown.OptionData> menuOptions = dropmonth.GetComponent<Dropdown>().options;


        int yearindex = dropyear.GetComponent<Dropdown>().value;
        List<Dropdown.OptionData> yearoption = dropyear.GetComponent<Dropdown>().options;


        int dateindex = dropdate.GetComponent<Dropdown>().value;
        List<Dropdown.OptionData> dateoption = dropdate.GetComponent<Dropdown>().options;


        if (month != 0)
        {
            MONTHEDIT = month;
        }else
        {
            MONTHEDIT = 1;
        }
        if (ApiRequestGenerator.guestlogin == 0)
        {
            Debug.Log("Edit Date");
            eventsenddata.eventDate =  MONTHEDIT + "/" + dateoption[dateindex].text.Normalize() + "/" + yearoption[yearindex].text.Normalize();
        }
        //   Debug.Log("date: " + eventsenddata.eventDate);

        else
        {
            Debug.Log("Edit Date");
            gusteventdate = month + "/" + dateoption[dateindex].text.Normalize() + "/" + yearoption[yearindex].text.Normalize();
        }

     
        //eventsenddata.eventDate = date.text.Normalize();

        eventsenddata.result = new Result();


        eventsenddata.result.myAgeGroup = agegroup.text.Normalize();
        eventsenddata.result.totalAgeGroup = totalagegroup.text.Normalize();
        eventsenddata.result.myGender = gender.text.Normalize();
        eventsenddata.result.totalGender = totalgender.text.Normalize();







        /*   List<Goal> mtlistgoals = new List<Goal>();

           for(int x = 0; x < mygoals.Count; x++) 
           {

               Goal temp = new Goal();
               temp.title = "";
               temp.goal = mygoals[x];
               mtlistgoals.Add(temp);
           }

           eventsenddata.goals= mtlistgoals;


   */


        //   eventsenddata.goals.Add(goal.text.Normalize());
        //eventsenddata.result = result.text.Normalize();



        List<Goal> mtlistgoals = new List<Goal>();
        for (int i = 0; i < goalscroll.content.childCount; i++)
        {
           




            Goal temp = new Goal();
            if (goalscroll.content.GetChild(i).GetChild(1).GetChild(0).GetComponent<Image>().sprite.name == "yes")
            {
                temp.achieved = "yes";
            }
            else
            {
                temp.achieved = "No";
            }

            if (goalscroll.content.GetChild(i).GetChild(0).GetComponent<InputField>().text == "")
            {
                temp.goal = "Goal";
            }
            else
            {
                temp.goal = goalscroll.content.GetChild(i).GetChild(0).GetComponent<InputField>().text.Normalize();
            }
            mtlistgoals.Add(temp);
        }
        eventsenddata.goals = mtlistgoals;
        guesttlistgoals = eventsenddata.goals;

        //link
        List<Link> linkgoals = new List<Link>();
        /*   for (int i = 0; i <linkscroll.content.childCount; i++)
           {
               Link temp = new Link();
               temp.url = linkscroll.content.GetChild(i).GetChild(0).GetComponent<InputField>().text.Normalize();

               temp.title = linkscroll.content.GetChild(i).GetChild(1).GetComponent<InputField>().text.Normalize();
               linkgoals.Add(temp);
           }*/
        for (int i = 0; i < 12; i++)
        {

            if (links[i].isOn == true || linksprites[i].activeSelf)
            {
                if (linksurl[i].text != "")
                {





                    for (int k = 0; k < 4; k++)
                    {
                        char a = mysecondstring[k];
                        char c = linksurl[i].text[k];
                        if (c == a)
                        {
                            if (k == 3)
                            {
                                linksurl[i].text = "" + linksurl[i].text;

                            }
                        }
                    }
                    for (int j = 0; j < 7; j++)
                    {
                        char b = mystring[j];
                        char c = linksurl[i].text[j];

                        char d = mythirdstring[j];
                        char e = linksurl[i].text[j];

                        if (b == c)
                        {
                            Debug.Log(c);
                        }
                        else if (d==e)
                        {
                            Debug.Log(d);
                        }
                        else
                        {
                            linksurl[i].text = "https://" + linksurl[i].text;
                            break;
                        }
                    }

                    Link temp = new Link();
                    temp.url = linksurl[i].text.Normalize();
                    Debug.LogError("links" + temp.title + " haha " + temp.url);
                    temp.title = linktitle[i];
                    linkgoals.Add(temp);
                }

            }

        }


        eventsenddata.links = linkgoals;

        guestlinkgoals = eventsenddata.links;

        eventsenddata.user = apigetter.id;
        if (ApiRequestGenerator.guestlogin == 0)
        {
            json2send = Newtonsoft.Json.JsonConvert.SerializeObject(eventsenddata);

            StartCoroutine(event_Upload(baseurl + "/v1/event", json2send));
        }
        else
        {

            guestdata();
        
        }



    }

    string gusteventdate, guestdistance, guesttime, guestvictorycatchp, gvr, gpr, gdistunit;
    List<Link> guestlinkgoals = new List<Link>();
    List<Goal> guesttlistgoals = new List<Goal>();




    public void guestdata()
    {

        StartCoroutine(guestdatasent());
    }
    IEnumerator guestdatasent()
    {
        loader.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        //WWWForm form = new WWWForm();
        //Dictionary<string, string> headers = form.headers;
        //  headers["JWT TOKEN"] = apigetter.jwt;
        //  badges.instance.badgescrollera();
        // eventno();
        //   badges.instance.badgecount();
        
        //SceneManager.LoadScene("4. MainScreens");

        eventgetter.eventhandler = 2;
        eventgetter.instance.updatescreen = 1;
        StartCoroutine(deathh());
        GameObject obj = GameObject.Find("eventlist");

        //obj.GetComponent<recievedata>().sizeofgoal = obj.GetComponent<recievedata>().goalscroll.content.sizeDelta;



        obj.GetComponent<recievedata>().myreceivedata = new recievedata.mydata();
        if (racerno.text != null || racerno.text != "")
        {
            obj.GetComponent<recievedata>().myreceivedata.race = racerno.text;
        }
        else
        {
            obj.GetComponent<recievedata>().myreceivedata.race = "000000";



        }


        obj.GetComponent<recievedata>().myreceivedata.eventdatetoshow = gusteventdate;
        obj.GetComponent<recievedata>().myreceivedata.eventName = Eventname.text;
        obj.GetComponent<recievedata>().myreceivedata.position = position.text;
        obj.GetComponent<recievedata>().myreceivedata.totalpeople = totalpeople.text;

        obj.GetComponent<recievedata>().myreceivedata.links = new List<recievedata.Link>();
        obj.GetComponent<recievedata>().myreceivedata.journal = new recievedata.Journal();
        obj.GetComponent<recievedata>().myreceivedata.distance = new recievedata.Distance();
        obj.GetComponent<recievedata>().myreceivedata.result = new recievedata.Result();
        obj.GetComponent<recievedata>().myreceivedata.city = cityname.text;
        obj.GetComponent<recievedata>().myreceivedata.goals = new List<recievedata.Goal>();
        obj.GetComponent<recievedata>().myreceivedata.eventType = typegetter;
        obj.GetComponent<recievedata>().myreceivedata._id = "111";
        obj.GetComponent<recievedata>().myreceivedata.eventName = Eventname.text;
        //  obj.GetComponent<recievedata>().eventdatetoshow = myeventclass.eventDate;
        obj.GetComponent<recievedata>().myreceivedata.eventdatetoshow = gusteventdate;

        Debug.Log("working on date : " + gusteventdate);
        datesplit = gusteventdate.Split(char.Parse("/"));

        obj.GetComponent<recievedata>().myreceivedata.eventDate = datesplit[0] + "/" + datesplit[2];



        obj.GetComponent<recievedata>().myreceivedata.victoryCity = guestvictorycatchp;
        obj.GetComponent<recievedata>().myreceivedata.time = guesttime;

        obj.GetComponent<recievedata>().myreceivedata.distance.istance = guestdistance;
        obj.GetComponent<recievedata>().myreceivedata.personarecord = gpr;
        obj.GetComponent<recievedata>().myreceivedata.distance.unit = gdistunit;
        obj.GetComponent<recievedata>().myreceivedata.@virtual = gvr;
        if (eventImage != "")
        {
            //   Debug.Log("image data: " + myDeserializedClass.response.@event.image);
            obj.GetComponent<recievedata>().myreceivedata.image = eventImage;
        }
        if (newstring != "")
        {
            obj.GetComponent<recievedata>().myreceivedata.image = newstring;

        }
        obj.GetComponent<recievedata>().myreceivedata.theme = themegetter;
        /*for (int x = 0; x < myDeserializedClass.response[i].goals.Count; x++)
        {

            Goal temp = new Goal();
            temp.title = "";
            temp.goal = myDeserializedClass.response[i].goals[x];

            obj.GetComponent<MyEventData>().myeventclass.goals.Add(temp);// = myDeserializedClass.response[i].goals;

            mtlistgoals.Add(temp);

        }*/
        // obj.GetComponent<MyEventData>().myeventclass.result = myDeserializedClass.response[i].result;
        //  Debug.Log("hhh" + myDeserializedClass.response[i].links.Count);
        if (guestlinkgoals != null)
        {
            for (int k = 0; k < guestlinkgoals.Count; k++)
            {
                recievedata.Link X = new recievedata.Link();
                X.title = guestlinkgoals[k].title;
                X.url = guestlinkgoals[k].url;
                obj.GetComponent<recievedata>().myreceivedata.links.Add(X);
            }
        }

        /*
        RectTransform cre;
        cre = obj.GetComponent<recievedata>().goalscroll.GetComponent<RectTransform>();
        /* cre.sizeDelta = new Vector2(cre.sizeDelta.x, cre.sizeDelta.y + obj.GetComponent<recievedata>().sizeofgoal.y);
         obj.GetComponent<recievedata>().goalscroll.GetComponent<RectTransform>().sizeDelta = cre.sizeDelta;
         obj.GetComponent<recievedata>().goalscroll.content.sizeDelta = new Vector2(obj.GetComponent<recievedata>().sizeofgoal.x, obj.GetComponent<recievedata>().sizeofgoal.y + myeventclass.goals.Count);
         obj.GetComponent<recievedata>().goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight += obj.GetComponent<recievedata>().sizeofgoal.y + myeventclass.goals.Count;
         obj.GetComponent<recievedata>().parentscroll.content.sizeDelta = new Vector2(obj.GetComponent<recievedata>().parentscroll.content.sizeDelta.x, obj.GetComponent<recievedata>().parentscroll.content.sizeDelta.y + obj.GetComponent<recievedata>().sizeofgoal.y);
        */
        int c = guesttlistgoals.Count;
        obj.GetComponent<recievedata>().goalscroll.content.sizeDelta = new Vector2(obj.GetComponent<recievedata>().sizeofgoal.x, obj.GetComponent<recievedata>().sizeofgoal.y * c);
        Debug.Log("yaho " + obj.GetComponent<recievedata>().sizeofgoal.y);
        if (guesttlistgoals != null)
        {
            for (int k = 0; k < guesttlistgoals.Count; k++)
            {
                Debug.Log("goals event: " + guesttlistgoals.Count);
                recievedata.Goal X = new recievedata.Goal();
                X.achieved = guesttlistgoals[k].achieved;
                X.goal = guesttlistgoals[k].goal;
                obj.GetComponent<recievedata>().myreceivedata.goals.Add(X);
            }
        }


        obj.GetComponent<recievedata>().myreceivedata.journal.pre = pree.text;
        obj.GetComponent<recievedata>().myreceivedata.journal.post = postt.text;
        obj.GetComponent<recievedata>().myreceivedata.journal.Next = nextt.text;
        obj.GetComponent<recievedata>().myreceivedata.journal.Notes = notes.text;
        obj.GetComponent<recievedata>().myreceivedata.journal.race = racee.text;




        obj.GetComponent<recievedata>().myreceivedata.result.myGender = gender.text;

        obj.GetComponent<recievedata>().myreceivedata.result.totalGender = totalgender.text;

        obj.GetComponent<recievedata>().myreceivedata.result.myAgeGroup = agegroup.text;
        obj.GetComponent<recievedata>().myreceivedata.result.totalAgeGroup = totalagegroup.text;

        //   Debug.Log("result: " + obj.GetComponent<recievedata>().myreceivedata.result.totalAgeGroup + " / " + myDeserializedClass.response.@event.result.totalAgeGroup);

        obj.GetComponent<recievedata>().myreceivedata.personarecord = gpr;
        updateeventguest = 1;
        obj.GetComponent<recievedata>().getguestdata();



        imgcheck += 1;

        eventgetter.instance.updatedoff();

        scrollreset();




        loader.SetActive(false);

    }
    public static int updateeventguest;







    IEnumerator deathh()
    {
        yield return new WaitForSeconds(1);

      //  eventgetter.instance.openevent();
        loader.SetActive(false);
    }




    IEnumerator event_Upload(string url, string bodyJsonString)
    {
        badges.instance.badgecount();
        loader.SetActive(true);
        WWWForm form = new WWWForm();
        // Dictionary<string, string> headers = form.headers;
        //   headers["JWT TOKEN"] = apigetter.jwt;
        Debug.Log("jwt event upload " + apigetter.jwt);
        Debug.Log("Bodyto send" + bodyJsonString);
        var request = new UnityWebRequest(url, "PATCH");
        request.SetRequestHeader("Authorization", "Bearer " + apigetter.jwt);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        // request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
       // Debug.Log("Status Code: " + request.responseCode);
        Debug.Log(request.downloadHandler.text);
        newsavessss myDeserializedClass = Newtonsoft.Json.JsonConvert.DeserializeObject<newsavessss>(request.downloadHandler.text);
        if (request.responseCode == 200)
        {
            StartCoroutine(delaybadges()); // 
            loader.SetActive(false);
            // eventgetter.currentscreen = 1;
           eventgetter.eventhandler = 2;
            eventgetter.instance.updatescreen = 1;
            StartCoroutine(deathh());
            GameObject obj = GameObject.Find("eventlist");

            //obj.GetComponent<recievedata>().sizeofgoal = obj.GetComponent<recievedata>().goalscroll.content.sizeDelta;



            obj.GetComponent<recievedata>().myreceivedata = new recievedata.mydata();
          //  obj.GetComponent<recievedata>().myreceivedata.eventdatetoshow = eventdatetoshow;
            obj.GetComponent<recievedata>().myreceivedata.eventName = myDeserializedClass.response.@event.eventName;
            obj.GetComponent<recievedata>().myreceivedata.position = myDeserializedClass.response.@event.position;
            obj.GetComponent<recievedata>().myreceivedata.totalpeople = myDeserializedClass.response.@event.totalpeople;
            obj.GetComponent<recievedata>().myreceivedata.race = myDeserializedClass.response.@event.race;
            obj.GetComponent<recievedata>().myreceivedata.links = new List<recievedata.Link>();
            obj.GetComponent<recievedata>().myreceivedata.journal = new recievedata.Journal();
            obj.GetComponent<recievedata>().myreceivedata.distance = new recievedata.Distance();
            obj.GetComponent<recievedata>().myreceivedata.result = new recievedata.Result();
            obj.GetComponent<recievedata>().myreceivedata.city = myDeserializedClass.response.@event.city;
            obj.GetComponent<recievedata>().myreceivedata.goals = new List<recievedata.Goal>();
            obj.GetComponent<recievedata>().myreceivedata.eventType = myDeserializedClass.response.@event.eventType;
            obj.GetComponent<recievedata>().myreceivedata._id = myDeserializedClass.response.@event._id;
            obj.GetComponent<recievedata>().myreceivedata.eventName = myDeserializedClass.response.@event.eventName;
            //  obj.GetComponent<recievedata>().eventdatetoshow = myeventclass.eventDate;
            obj.GetComponent<recievedata>().myreceivedata.eventdatetoshow = myDeserializedClass.response.@event.eventDate;
            datesplit = myDeserializedClass.response.@event.eventDate.Split(char.Parse("/"));

            obj.GetComponent<recievedata>().myreceivedata.eventDate = myDeserializedClass.response.@event.eventDate;



            obj.GetComponent<recievedata>().myreceivedata.victoryCity = myDeserializedClass.response.@event.victoryCity;
            obj.GetComponent<recievedata>().myreceivedata.time = myDeserializedClass.response.@event.time;

            obj.GetComponent<recievedata>().myreceivedata.distance.istance = myDeserializedClass.response.@event.distance.istance;
            obj.GetComponent<recievedata>().myreceivedata.personarecord = myDeserializedClass.response.@event.personarecord;
            obj.GetComponent<recievedata>().myreceivedata.distance.unit = myDeserializedClass.response.@event.distance.unit;
            obj.GetComponent<recievedata>().myreceivedata.@virtual = myDeserializedClass.response.@event.@virtual;
            obj.GetComponent<recievedata>().myreceivedata.personarecord = myDeserializedClass.response.@event.personarecord;


            if (myDeserializedClass.response.@event.image != "")
            {
                Debug.Log("image data: " + myDeserializedClass.response.@event.image);
                obj.GetComponent<recievedata>().myreceivedata.image = myDeserializedClass.response.@event.image;
            }
            else
            {

                obj.GetComponent<recievedata>().hiimage.texture = defaultpic.texture;

            }
            obj.GetComponent<recievedata>().myreceivedata.theme = myDeserializedClass.response.@event.theme;
            /*for (int x = 0; x < myDeserializedClass.response[i].goals.Count; x++)
            {

                Goal temp = new Goal();
                temp.title = "";
                temp.goal = myDeserializedClass.response[i].goals[x];

                obj.GetComponent<MyEventData>().myeventclass.goals.Add(temp);// = myDeserializedClass.response[i].goals;

                mtlistgoals.Add(temp);

            }*/
            // obj.GetComponent<MyEventData>().myeventclass.result = myDeserializedClass.response[i].result;
            //  Debug.Log("hhh" + myDeserializedClass.response[i].links.Count);
            if (myDeserializedClass.response.@event.links != null)
            {
                for (int k = 0; k < myDeserializedClass.response.@event.links.Count; k++)
                {
                    recievedata.Link X = new recievedata.Link();
                    X.title = myDeserializedClass.response.@event.links[k].title;
                    X.url = myDeserializedClass.response.@event.links[k].url;
                    obj.GetComponent<recievedata>().myreceivedata.links.Add(X);
                }
            }

            /*
            RectTransform cre;
            cre = obj.GetComponent<recievedata>().goalscroll.GetComponent<RectTransform>();
            /* cre.sizeDelta = new Vector2(cre.sizeDelta.x, cre.sizeDelta.y + obj.GetComponent<recievedata>().sizeofgoal.y);
             obj.GetComponent<recievedata>().goalscroll.GetComponent<RectTransform>().sizeDelta = cre.sizeDelta;
             obj.GetComponent<recievedata>().goalscroll.content.sizeDelta = new Vector2(obj.GetComponent<recievedata>().sizeofgoal.x, obj.GetComponent<recievedata>().sizeofgoal.y + myeventclass.goals.Count);
             obj.GetComponent<recievedata>().goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight += obj.GetComponent<recievedata>().sizeofgoal.y + myeventclass.goals.Count;
             obj.GetComponent<recievedata>().parentscroll.content.sizeDelta = new Vector2(obj.GetComponent<recievedata>().parentscroll.content.sizeDelta.x, obj.GetComponent<recievedata>().parentscroll.content.sizeDelta.y + obj.GetComponent<recievedata>().sizeofgoal.y);
            */
            int c = myDeserializedClass.response.@event.goals.Count;
            obj.GetComponent<recievedata>().goalscroll.content.sizeDelta = new Vector2(obj.GetComponent<recievedata>().sizeofgoal.x, obj.GetComponent<recievedata>().sizeofgoal.y * c);
            Debug.Log("yaho " + obj.GetComponent<recievedata>().sizeofgoal.y);
            if (myDeserializedClass.response.@event.goals != null)
            {
                for (int k = 0; k < myDeserializedClass.response.@event.goals.Count; k++)
                {
                    Debug.Log("goals event: " + myDeserializedClass.response.@event.goals.Count);
                    recievedata.Goal X = new recievedata.Goal();
                    X.achieved = myDeserializedClass.response.@event.goals[k].achieved;
                    X.goal = myDeserializedClass.response.@event.goals[k].goal;
                    obj.GetComponent<recievedata>().myreceivedata.goals.Add(X);
                }
            }


            obj.GetComponent<recievedata>().myreceivedata.journal.pre = myDeserializedClass.response.@event.journal.pre;
            obj.GetComponent<recievedata>().myreceivedata.journal.post = myDeserializedClass.response.@event.journal.post;
            obj.GetComponent<recievedata>().myreceivedata.journal.Next = myDeserializedClass.response.@event.journal.Next;
            obj.GetComponent<recievedata>().myreceivedata.journal.Notes = myDeserializedClass.response.@event.journal.Notes;
            obj.GetComponent<recievedata>().myreceivedata.journal.race = myDeserializedClass.response.@event.journal.race;

            obj.GetComponent<recievedata>().myreceivedata.result.myGender = myDeserializedClass.response.@event.result.myGender;

            obj.GetComponent<recievedata>().myreceivedata.result.totalGender = myDeserializedClass.response.@event.result.totalGender;

            obj.GetComponent<recievedata>().myreceivedata.result.myAgeGroup = myDeserializedClass.response.@event.result.myAgeGroup;
            obj.GetComponent<recievedata>().myreceivedata.result.totalAgeGroup = myDeserializedClass.response.@event.result.totalAgeGroup;

            Debug.Log("result: " + obj.GetComponent<recievedata>().myreceivedata.result.totalAgeGroup + " / " + myDeserializedClass.response.@event.result.totalAgeGroup);


            obj.GetComponent<recievedata>().myreceivedata.achivement = myDeserializedClass.response.@event.achivement;
            obj.GetComponent<recievedata>().myreceivedata.user = myDeserializedClass.response.@event.user;
            obj.GetComponent<recievedata>().myreceivedata.createdAt = myDeserializedClass.response.@event.createdAt;
            obj.GetComponent<recievedata>().myreceivedata.updatedAt = myDeserializedClass.response.@event.updatedAt;
            obj.GetComponent<recievedata>().myreceivedata.__v = myDeserializedClass.response.@event.__v;
            obj.GetComponent<recievedata>().getdata();
			//obj.GetComponent<recievedata>().StartCoroutine(obj.GetComponent<recievedata>().checkbaloonn());

			imgcheck += 1;

            eventgetter.instance.updatedoff();

            scrollreset();
            // SceneManager.LoadScene("4. MainScreens");
        }
        else
        {
            loader.SetActive(false);
        }
       
        //eventdata2send myDeserializedClass = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(request.downloadHandler.text);
    }



    IEnumerator delaybadges()
    {
        yield return new WaitForSeconds(1);
        badges.instance.badgescrollera();



    }



    public string newstring;
    public void sendcameratext()
    {
        // RawImage newuserphoto = null;
        // Texture2D newphoto = new Texture2D(1, 1);
        //  newphoto.LoadImage(Convert.FromBase64String("string"));

        if (ApiRequestGenerator.guestlogin == 0)
        {
            StartCoroutine(Uploaad());
        }
        else
        {


            newstring = "asdasd";
        }

        Texture2D rawImageTexture = eventimgetexture;

        //   eventImage = Convert.ToBase64String(rawImageTexture.EncodeToJPG());

        float width = Screen.width;
        float height = Screen.height;
        float aspctratio = width / height;
        Debug.Log(aspctratio);



        float check = eventimgetexture.width / eventimgetexture.height;
        if (check == 0)
        {
            check = 1;
        }
        hiimage.GetComponent<AspectRatioFitter>().aspectRatio = 1;
        hiimage.texture = eventimgetexture;




    }


    public void sendconverttext()
    {
        // RawImage newuserphoto = null;
        // Texture2D newphoto = new Texture2D(1, 1);
        //  newphoto.LoadImage(Convert.FromBase64String("string"));
        if (ApiRequestGenerator.guestlogin == 0)
        {
            StartCoroutine(Uploaad());
        }
        else
        {


            newstring = "asdasd";
        }
        Texture2D rawImageTexture = eventimgetexture;

        //   eventImage = Convert.ToBase64String(rawImageTexture.EncodeToJPG());

        float width = Screen.width;
        float height = Screen.height;
        float aspctratio = width / height;
        Debug.Log(aspctratio);



        float check = eventimgetexture.width / eventimgetexture.height;
        if(check==0)
        {
            check = 1;
        }


        if (eventimgetexture.width > eventimgetexture.height)
        {
            hiimage.GetComponent<AspectRatioFitter>().aspectRatio = 1;
        
        }
        else
        {
            hiimage.GetComponent<AspectRatioFitter>().aspectRatio = 1;

        }


        hiimage.texture = eventimgetexture;
        Debug.Log("IMAGE AY HAI YAAH");



    }


    IEnumerator Uploaad()
    {
        save.interactable = false;
        WWWForm form = new WWWForm();
        byte[] bytes = eventimgetexture.EncodeToPNG();
        form.AddBinaryData("image", bytes, imagepath);

        // Dictionary<string, string> headers = form.headers;
        //headers["JWT TOKEN"] = apigetter.jwt;
        UnityWebRequest www = UnityWebRequest.Post(baseurl + "/v1/user/upload-image", form);
        //www.SetRequestHeader("Authorization", "Bearer " + apigetter.jwt);
        //www.SetRequestHeader("Content-Type", "application/json");
        //byte[] myData = System.Text.Encoding.UTF8.GetBytes("This is some test data");


        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);

            save.interactable = true;
        }
        else
        {
            Debug.Log("Form upload complete! " + form.data.ToString());
            
            Debug.Log("Form upload complete! " + www.downloadHandler.text);

            picture myDeserializedClass = Newtonsoft.Json.JsonConvert.DeserializeObject<picture>(www.downloadHandler.text);

            eventImage = baseurl + "/" + myDeserializedClass.response.url;

            save.interactable = true;


        }
    }


    public void changesprite()
    {
        newstring = "";
        eventImage = "";
        hiimage.texture = defaultpic.texture;
    }



    public class getpicture
    {
        public string url { get; set; }
        public string message { get; set; }
    }

    public class picture
    {
        public bool success { get; set; }
        public getpicture response { get; set; }
        public object metadata { get; set; }
    }
    public class nDistances
    {
        public string istance { get; set; }
        public string unit { get; set; }
    }

    public class nEvent
    {
        public string eventName { get; set; }
        public string eventDate { get; set; }
        public string city { get; set; }
        public string race { get; set; }
        public string victoryCity { get; set; }
        public string time { get; set; }
        public nDistances distance { get; set; }
        public string @virtual { get; set; }
        public string image { get; set; }
        public string theme { get; set; }
        public List<Goal> goals { get; set; }
        public nResult result { get; set; }
        public List<Link> links { get; set; }
        public nJournal journal { get; set; }
        public string achivement { get; set; }
        public string eventType { get; set; }
        public string personarecord { get; set; }
        public string position { get; set; }
        public string totalpeople { get; set; }
        public string user { get; set; }
        public string _id { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int __v { get; set; }
    }

    public class nGoal
    {
        public string achieved { get; set; }
        public string goal { get; set; }
    }

    public class nJournal
    {
        public string pre { get; set; }
        public string race { get; set; }
        public string post { get; set; }
        public string Next { get; set; }
        public string Notes { get; set; }
    }

    public class nResponse
    {
        public string msg { get; set; }
        public nEvent @event { get; set; }
    }

    public class nResult
    {
        public string myAgeGroup { get; set; }
        public string totalAgeGroup { get; set; }
        public string myGender { get; set; }
        public string totalGender { get; set; }
    }

    public class newsavessss
    {
        public bool success { get; set; }
        public nResponse response { get; set; }
        public object metadata { get; set; }
    }

   
    // Update is called once per frame
    void Update()
    {


        firstdistance = d1.text;
        seconddistance = d2.text;
        thirddistance = d3.text;


        firsttime = time1.text;
        secondtime = time2.text;
        thirdtime = time3.text;

        victorylimit = victorycatchphrase.text.Normalize();
        victoylimittext.text = victorylimit.Length + "/" + "12";
        if(imgcheck!=0)
        {
            img2.SetActive(true);

            img1.SetActive(false);

        }



    }
}
