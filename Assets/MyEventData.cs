using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class MyEventData : MonoBehaviour
{
public HorizontalLayoutGroup layoutGroup;

    public ScrollRect ss;
    public Text eventname;
    //public GameObject messageContainer;
    public bool checkdel;
    public GameObject delete;
    public Button Delete; 
    public Text eventtype;
    public Text date;
    public Text city;
    string[] datesplit;
    string[] textSplit;
    public string eventdatetoshow;
    public GameObject cofirm;
    public string baseurl;
    public Text recorddata,recordtime;

    public Image rewardImage;

    public string[] currentext;
 
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Distance
    {
        public string istance { get; set; }
        public string unit { get; set; }
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



    public void confirmdata()
    {
        cofirm.GetComponent<confirmation>().panel.SetActive(true);
        cofirm.GetComponent<confirmation>().id = myeventclass.id;
        cofirm.GetComponent<confirmation>().myevent = this.gameObject; 
    }

    public class MyEvent
    {
        public string prchecking { get; set; }
        public string position { get; set; }
        public string totalpeople { get; set; }
        public string id { get; set; }
        public string eventName { get; set; }
        public string eventDate { get; set; }
        public string city { get; set; }
        public string victoryCity { get; set; }
        public string time { get; set; }
        public string race { get; set; }
        public Distance distance { get; set; }
        public string @virtual { get; set; }
        public string image { get; set; }
        public List<string> galleryImages { get; set; } =
        new List<string>();
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
    }

    public class Result
    {
        public string myAgeGroup { get; set; }
        public string totalAgeGroup { get; set; }
        public string myGender { get; set; }
        public string totalGender { get; set; }
    }
    public class deleteevent
    {
        public List<string> id { get; set; }
    }


    // public List<string> galleryImages { get; set; } =
    // new List<string>();


   /* public class deleteevent
    {
       public string id { get; set; }
    }*/    

    public void deletethis()
    {

        if (ss.content.transform.position.x > 100|| ss.content.transform.position.x < -50)
        {
            checkdel = !checkdel;
            if (checkdel)
            {
                delete.SetActive(true);
            }
            else
            {
                delete.SetActive(false);
            }
        }


    }
    
    public IEnumerator ResetLayOutGroup()
    {
        layoutGroup.enabled = false;
        yield return new WaitForSeconds(0.3f);
        layoutGroup.enabled = true;
    }

    public void deletedata()
    {
        List<string> idd = new List<string>();
        deleteevent deldata = new deleteevent();

        idd.Add(myeventclass.id);
        deldata.id = idd;
        //idd.Remove(myeventclass.id);


        Debug.Log(apigetter.id);
        String JsonVal = Newtonsoft.Json.JsonConvert.SerializeObject(deldata);
        Debug.Log("json val " + idd);
        Debug.Log("json va " + myeventclass.id);

        StartCoroutine(deletdataa(baseurl + "/v1/event", JsonVal));



    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
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




    IEnumerator deletdataa(string url, string bodyJsonString)
    {
        eventgetter.currentCount = PlayerPrefs.GetInt(eventgetter.x, 0);

        if(eventgetter.currentCount > 0)
        {
            eventgetter.currentCount -= 1;
            PlayerPrefs.SetInt(eventgetter.x, eventgetter.currentCount);
            PlayerPrefs.Save();
            Debug.LogError("x: " + eventgetter.x);
        }
        Debug.LogError("x: " + eventgetter.x);
        Debug.LogError("currentCount: " + eventgetter.currentCount);
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

        if(myDeserializedClass.response.deletedCount>0)
        {
            for (int l = 0; l < PRracerecord.instance.recordspawn.childCount; l++)
            {

                Destroy(PRracerecord.instance.recordspawn.GetChild(l).gameObject);

            }
            PRracerecord.instance.togglecount = 0;
            PRracerecord.instance.senddata();
            Destroy(gameObject);
            eventgetter.instance.eventno();
            badges.instance.deletebadgecount();
            badges.instance.deletebadgeevent();
            
           // StartCoroutine(checck());
        }
    }
    IEnumerator checck()
    {
        yield return new WaitForSeconds(2);

        badges.previousvalue = badges.currentvalue;
    }

    public MyEvent myeventclass;
    private void Awake()
    {


        baseurl = baselink.Url;

        StartCoroutine(ResetLayOutGroup());
    }


    string sentence;
    void Start()
    {
        Delete.onClick.AddListener(eventgetter.instance.CanRefresh);
        cofirm = GameObject.Find("confirmation").gameObject;
        GameObject obj = GameObject.Find("eventlist");
        eventdatetoshow = myeventclass.eventDate;
        myeventclass.eventName.ToLower();
        eventname.text = myeventclass.eventName;


        currentext = myeventclass.eventName.Split(char.Parse(" "));

        sentence = myeventclass.eventName;


		eventtype.text = myeventclass.eventType;

        Debug.Log("date: " + myeventclass.eventDate);
        textSplit = myeventclass.eventDate.Split(char.Parse("/"));
        textSplit = myeventclass.eventDate.Split(char.Parse("/"));
     
        //   date.text = textSplit[0] + "/" + textSplit[2];
        date.text = myeventclass.eventDate;


        if (myeventclass.@virtual == "yes")
        {
            city.text = "Virtual";
        }
        else
        {
            city.text = myeventclass.city;
        }



        StartCoroutine(refresh());
    }
    IEnumerator refresh()
    {
        //messageContainer.GetComponent<HorizontalLayoutGroup>().childForceExpandWidth = false;
      //  messageContainer.GetComponent<ContentSizeFitter>().enabled = false;
       yield return new WaitForSeconds(0.1f);

        //messageContainer.GetComponent<HorizontalLayoutGroup>().childForceExpandWidth = true;
        //   messageContainer.GetComponent<ContentSizeFitter>().enabled = true;
        // messageContainer.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        eventtype.text = myeventclass.eventType;
    }
    public void RefreshContentSize()
    {   
     
    
    }

    IEnumerator death()
    {
        yield return new WaitForSeconds(1);

        eventgetter.instance.openevent();

    }









    public void readdata()
    {
        PlayerPrefs.SetInt("LogOutSubscription", 0);
        Debug.Log("current text: " +currentext );
       
        for (int a = 0; a < currentext.Length; a++)
        {

            for (int b = 0; b < eventgetter.instance.disney.Length; b++)
			{
				string[] words = sentence.Split(' ');

				// Convert the word to lowercase to perform a case-insensitive check
				string lowerCaseWord = eventgetter.instance.disney[0].ToLower();

                // Check each word if it contains the word to check
                foreach (string word in words)
                {
                    // Convert the current word to lowercase
                    string lowerCaseCurrentWord = word.ToLower();

                    // Check if the word to check is part of the current word
                    if (lowerCaseCurrentWord.Contains(lowerCaseWord))
					{
						eventgetter.instance.defautart[0].GetComponent<Image>().sprite = eventgetter.instance.Disneyart[0];

						eventgetter.instance.defautart[1].GetComponent<RawImage>().texture = eventgetter.instance.Disneyart[1].texture;

						eventgetter.instance.defautart[2].GetComponent<Image>().sprite = eventgetter.instance.Disneyart[2];

						eventgetter.instance.defautart[3].GetComponent<Image>().sprite = eventgetter.instance.Disneyart[3];

						eventgetter.instance.defautart[4].GetComponent<Image>().sprite = eventgetter.instance.Disneyart[4];

						eventgetter.instance.defautart[5].GetComponent<Image>().sprite = eventgetter.instance.Disneyart[5];

						eventgetter.instance.defautart[6].GetComponent<Image>().sprite = eventgetter.instance.Disneyart[6];

						eventgetter.instance.defautart[7].GetComponent<Image>().sprite = eventgetter.instance.Disneyart[7];

						eventgetter.instance.defautart[8].GetComponent<Image>().sprite = eventgetter.instance.Disneyart[8];

						eventgetter.instance.defautart[9].GetComponent<Image>().sprite = eventgetter.instance.Disneyart[9];
						eventgetter.instance.defautart[10].GetComponent<Image>().sprite = eventgetter.instance.Disneyart[10];


						recievedata.eventheme = 1;

						for (int d = 0; d < eventgetter.instance.themetext.Length; d++)
						{
							Debug.Log("waiting on text color1");
							eventgetter.instance.themetext[d].color = new Color32(253, 191, 0, 255);
						}

						eventgetter.instance.distancethemtext.color = Color.white;

						eventgetter.instance.timethemetext.color = Color.white;

                     
					}

                }


			 if (currentext[a].ToLower() == eventgetter.instance.disney[b])
                {


                    eventgetter.instance.defautart[0].GetComponent<Image>().sprite = eventgetter.instance.Disneyart[0];

                    eventgetter.instance.defautart[1].GetComponent<RawImage>().texture = eventgetter.instance.Disneyart[1].texture;

                    eventgetter.instance.defautart[2].GetComponent<Image>().sprite = eventgetter.instance.Disneyart[2];

                    eventgetter.instance.defautart[3].GetComponent<Image>().sprite = eventgetter.instance.Disneyart[3];

                    eventgetter.instance.defautart[4].GetComponent<Image>().sprite = eventgetter.instance.Disneyart[4];

                    eventgetter.instance.defautart[5].GetComponent<Image>().sprite = eventgetter.instance.Disneyart[5];

                    eventgetter.instance.defautart[6].GetComponent<Image>().sprite = eventgetter.instance.Disneyart[6];

                    eventgetter.instance.defautart[7].GetComponent<Image>().sprite = eventgetter.instance.Disneyart[7];

                    eventgetter.instance.defautart[8].GetComponent<Image>().sprite = eventgetter.instance.Disneyart[8];

                    eventgetter.instance.defautart[9].GetComponent<Image>().sprite = eventgetter.instance.Disneyart[9];
                    eventgetter.instance.defautart[10].GetComponent<Image>().sprite = eventgetter.instance.Disneyart[10];


                    recievedata.eventheme = 1;

                    for (int d = 0; d < eventgetter.instance.themetext.Length; d++)
                    {
                        Debug.Log("waiting on text color2");
                        eventgetter.instance.themetext[d].color = new Color32(253, 191, 0, 255);
                    }

                    eventgetter.instance.distancethemtext.color = Color.white;

                    eventgetter.instance.timethemetext.color = Color.white;
                }
                else if(currentext[a].ToLower() == eventgetter.instance.spartan[b])
                {
                    eventgetter.instance.defautart[0].GetComponent<Image>().sprite = eventgetter.instance.basicart[0];



                    eventgetter.instance.defautart[2].GetComponent<Image>().sprite = eventgetter.instance.basicart[2];

                    eventgetter.instance.defautart[3].GetComponent<Image>().sprite = eventgetter.instance.basicart[3];

                    eventgetter.instance.defautart[4].GetComponent<Image>().sprite = eventgetter.instance.basicart[4];

                    eventgetter.instance.defautart[5].GetComponent<Image>().sprite = eventgetter.instance.basicart[5];

                    eventgetter.instance.defautart[6].GetComponent<Image>().sprite = eventgetter.instance.basicart[6];

                    eventgetter.instance.defautart[7].GetComponent<Image>().sprite = eventgetter.instance.basicart[7];

                    eventgetter.instance.defautart[8].GetComponent<Image>().sprite = eventgetter.instance.basicart[8];

                    eventgetter.instance.defautart[9].GetComponent<Image>().sprite = eventgetter.instance.basicart[9];
                    eventgetter.instance.defautart[10].GetComponent<Image>().sprite = eventgetter.instance.basicart[10];
					eventgetter.instance.defautart[11].GetComponent<Image>().sprite = eventgetter.instance.basicart[11];
					eventgetter.instance.defautart[12].GetComponent<Image>().sprite = eventgetter.instance.basicart[12];


					recievedata.eventheme = 0;

                    for (int d = 0; d < eventgetter.instance.themetext.Length; d++)
                    {Debug.Log("waiting on text color2");
                        eventgetter.instance.themetext[d].color = Color.white;
                    }
					eventgetter.instance.themetext[3].color = new Color32(255, 205, 0, 255);
					eventgetter.instance.themetext[5].color = new Color32(255, 205, 0, 255);
					eventgetter.instance.themetext[2].color = new Color32(255, 205, 0, 255);
					eventgetter.instance.themetext[0].color = new Color32(255, 205, 0, 255);
					eventgetter.instance.themetext[6].color = new Color32(255, 205, 0, 255);

					eventgetter.instance.distancethemtext.color = Color.white;

                    eventgetter.instance.timethemetext.color = Color.white;
                }


            }


        }
      
        eventgetter.eventhandler = 2;
        eventgetter.instance.updatescreen = 1;
        StartCoroutine(death());
         GameObject  obj = GameObject.Find("eventlist");

        //obj.GetComponent<recievedata>().sizeofgoal = obj.GetComponent<recievedata>().goalscroll.content.sizeDelta;

      

        obj.GetComponent<recievedata>().myreceivedata = new recievedata.mydata();
        obj.GetComponent<recievedata>().myreceivedata.eventdatetoshow = eventdatetoshow;
        obj.GetComponent<recievedata>().myreceivedata.eventName = myeventclass.eventName;
        obj.GetComponent<recievedata>().myreceivedata.position = myeventclass.position;
        obj.GetComponent<recievedata>().myreceivedata.totalpeople = myeventclass.totalpeople;

        obj.GetComponent<recievedata>().myreceivedata.links = new List<recievedata.Link>();
        obj.GetComponent<recievedata>().myreceivedata.journal = new recievedata.Journal();
        obj.GetComponent<recievedata>().myreceivedata.distance = new recievedata.Distance();
        obj.GetComponent<recievedata>().myreceivedata.result = new recievedata.Result();
        obj.GetComponent<recievedata>().myreceivedata.city = myeventclass.city;
        obj.GetComponent<recievedata>().myreceivedata.goals = new List<recievedata.Goal>();
        obj.GetComponent<recievedata>().myreceivedata.eventType = myeventclass.eventType;
        obj.GetComponent<recievedata>().myreceivedata._id = myeventclass.id;
        obj.GetComponent<recievedata>().myreceivedata.eventName = myeventclass.eventName;
        //  obj.GetComponent<recievedata>().eventdatetoshow = myeventclass.eventDate;

        datesplit = myeventclass.eventDate.Split(char.Parse("/"));

		// obj.GetComponent<recievedata>().myreceivedata.eventDate = datesplit[0]+"/"+datesplit[2];
		obj.GetComponent<recievedata>().myreceivedata.eventDate = myeventclass.eventDate;

		if (myeventclass.race != "")
        {
            obj.GetComponent<recievedata>().myreceivedata.race = myeventclass.race;
        }
        else
        {
            obj.GetComponent<recievedata>().myreceivedata.race = "";


        }


        obj.GetComponent<recievedata>().myreceivedata.victoryCity = myeventclass.victoryCity;
        obj.GetComponent<recievedata>().myreceivedata.time = myeventclass.time;
        
        obj.GetComponent<recievedata>().myreceivedata.distance.istance = myeventclass.distance.istance;
        obj.GetComponent<recievedata>().myreceivedata.personarecord = myeventclass.personarecord;
        obj.GetComponent<recievedata>().myreceivedata.distance.unit = myeventclass.distance.unit;
        obj.GetComponent<recievedata>().myreceivedata.@virtual = myeventclass.@virtual;
        if (myeventclass.image != "")
        {
            Debug.Log("image data: " + myeventclass.image);
            obj.GetComponent<recievedata>().myreceivedata.image = myeventclass.image;
        }
        if (myeventclass.theme != "")
        {
            obj.GetComponent<recievedata>().myreceivedata.theme = myeventclass.theme;
        }
        else
        {

            obj.GetComponent<recievedata>().myreceivedata.theme = "baloon";

        }

        obj.GetComponent<recievedata>().myreceivedata.galleryImages =
        myeventclass.galleryImages ?? new List<string>();

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
        if (myeventclass.links != null)
        {
            for (int k = 0; k < myeventclass.links.Count; k++)
            {
                recievedata.Link X = new recievedata.Link();
                X.title = myeventclass.links[k].title;
                X.url = myeventclass.links[k].url;
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
        int c = myeventclass.goals.Count;
       /// obj.GetComponent<recievedata>().goalscroll.content.sizeDelta = new Vector2(obj.GetComponent<recievedata>().sizeofgoal.x, obj.GetComponent<recievedata>().sizeofgoal.y * c-150);
        Debug.Log("yaho " + obj.GetComponent<recievedata>().sizeofgoal.y);
        if (myeventclass.goals != null)
        {
            for (int k = 0; k < myeventclass.goals.Count; k++)
            {
                Debug.Log("goals event: " + myeventclass.goals.Count);
                recievedata.Goal X = new recievedata.Goal();
                X.achieved = myeventclass.goals[k].achieved;
                X.goal = myeventclass.goals[k].goal;
                obj.GetComponent<recievedata>().myreceivedata.goals.Add(X);
            }
        }


        obj.GetComponent<recievedata>().myreceivedata.journal.pre = myeventclass.journal.pre;
        obj.GetComponent<recievedata>().myreceivedata.journal.post = myeventclass.journal.post;
        obj.GetComponent<recievedata>().myreceivedata.journal.Next = myeventclass.journal.Next;
        obj.GetComponent<recievedata>().myreceivedata.journal.Notes = myeventclass.journal.Notes;
        obj.GetComponent<recievedata>().myreceivedata.journal.race = myeventclass.journal.race;
        
        obj.GetComponent<recievedata>().myreceivedata.result.myGender = myeventclass.result.myGender;

        obj.GetComponent<recievedata>().myreceivedata.result.totalGender = myeventclass.result.totalGender;

        obj.GetComponent<recievedata>().myreceivedata.result.myAgeGroup = myeventclass.result.myAgeGroup;
        obj.GetComponent<recievedata>().myreceivedata.result.totalAgeGroup = myeventclass.result.totalAgeGroup;

        Debug.Log("result: " + obj.GetComponent<recievedata>().myreceivedata.result.totalAgeGroup + " / " + myeventclass.result.totalAgeGroup);


        obj.GetComponent<recievedata>().myreceivedata.achivement = myeventclass.achivement;
        obj.GetComponent<recievedata>().myreceivedata.user = myeventclass.user;
        obj.GetComponent<recievedata>().myreceivedata.createdAt = myeventclass.createdAt;
        obj.GetComponent<recievedata>().myreceivedata.updatedAt = myeventclass.updatedAt;
        obj.GetComponent<recievedata>().myreceivedata.__v = myeventclass.__v;
        obj.GetComponent<recievedata>().getdata();

        //	obj.GetComponent<recievedata>().StartCoroutine(obj.GetComponent<recievedata>().checkbaloon());



    }
	/*   int c = myDeserializedClass.response.Count;
       Eventscrol.content.sizeDelta = new Vector2(singleitemsize.x, singleitemsize.y* c);
       Debug.Log("yaho " + singleitemsize.y);
    */







    public void ShowUploadedGallery()
    {
        if (EventGalleryViewer.Instance == null)
        {
            Debug.LogError("EventGalleryViewer is missing.");
            return;
        }

        RectTransform galleryContent = ExpandOnClick.LastExpandedWindow;

        if (galleryContent == null)
        {
            GameObject selectedButton =
                UnityEngine.EventSystems.EventSystem.current
                    ?.currentSelectedGameObject;
            galleryContent = selectedButton != null
                ? selectedButton.GetComponent<ExpandOnClick>()?.ExpandedWindow
                : null;
        }

        EventGalleryViewer.Instance.ShowUploadedImages(
            myeventclass.galleryImages,
            myeventclass.id,
            urls => myeventclass.galleryImages = urls,
            galleryContent);
    }







   








	// Update is called once per frame
	void Update()
    {
        
    }
}

