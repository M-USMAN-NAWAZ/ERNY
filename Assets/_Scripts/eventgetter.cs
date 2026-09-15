using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
//using AdvancedInputFieldPlugin;
#if (UNITY_IOS && !UNITY_EDITOR)
using System.Runtime.InteropServices;
#endif
using UnityEngine.Android;
using TMPro;
using UnityEngine.SceneManagement;
//using Vuforia;
using System.ComponentModel;
using Unity.Mathematics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.Purchasing.Security;
using AppleAuthSample;
using UnityEngine.XR.ARFoundation;
using Unity.XR.CoreUtils;
using UnityEngine.SocialPlatforms.Impl;
using System.Timers;

public class eventgetter : MonoBehaviour
{
    public GameObject confettieObj;
    public spawnmangr spawnmangr;
    public ARCameraManager cameraManager;
    public GameObject AR;
    public GameObject extraCamera;
    public GameObject arTurnOnAdIoTurnOff;
    public GameObject eventCamera;
    public GameObject loadingscreen, spawnedloader;
    public static int archeck;
    public Text versiondisplay;
    public string[] eventnamechecker;
    public GameObject[] defautart;
    public string[] disney, spartan,disney100medal;
    public Text[] themetext;
    public Text distancethemtext, timethemetext;
    public Sprite[] Disneyart, Spartanart,basicart;
    public Texture disneytexture, spartantexture;
    public GameObject crossbutton,crossbutton2;
    public static int reviewcheck;
    public GameObject freetrialsubscription;
    public Sprite defaulteventiamge, defaultmainscreenimage;
    public GameObject[] themebuttons;
    public pops popuppanel;
    public InputField distancecomplete, timecomplete;
    // public AdvancedInputFieldPlugin
    //   public AdvancedInputField prenots,postnots,nextnots,racenots;
    public GameObject popupclose, popuprefuel;
    public ExpandOnClick expandscript;
    public TMP_InputField pree, postt, nextt, racee;
    //public CameraPreview UPDATECAMERA;
    //condition
    public static int freetrial;
    public static int signuppaid;
    public int checksize;
    public ScrollRect eventscroll;
    public GameObject popup;
    public GameObject[] disableelements;
    /// <summary>
    /// ////
    /// </summary>
    public static string profileimagetext;
    public GameObject[] panels, icons, eventalbum;
    public int sortevent;
    public static int eventhandler;
    public string victorylimit;
    public Animator anim;
    public Text victoylimittext;
    public GameObject loader;
    public static eventgetter instance;
    public string json2send, imagepath;
    public string baseurl;
    public GameObject ARCAMER, ARCANVAS, canv, arsessio, camer, progileedit, cookies, bottom, settingsbutton;
    public GameObject event2spawn;
    public ScrollRect Eventscrol;
    public RawImage profileimage, eventcurrentimage, test;
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    string profileiamgesend;
    //send data
    public InputField Eventname;
    public Dropdown monthh, year, date;
    public InputField cityname;
    public InputField victorycatchphrase;
    public InputField time, distance;
    public InputField goal, agegroup, totalagegroup, gender, totalgender, title, prenotes, race, post, next, notes, result, position, totalpeople;
    public string imagedata;
    public string eventImage;
    public static Texture death, profiletexture;
    public static Texture2D eventimgetexture;
    public RawImage hiimage;
    public string typegetter, themegetter;
    public Texture dd;
    public List<string> mygoals;
    public int goalgetter;
    public int linkgetter;
    public Toggle vr, pr, distanceunit;
    //goals sent data
    public GameObject gaolspawn;
    public ScrollRect goalscroll;
    public Vector2 sizeofgoal;
    public RectTransform goaltranform;
    string getdate;
    //link sent data
    public GameObject linkspawn;
    Vector2 sizeoflink;
    public ScrollRect linkscroll;
    public RectTransform linktransform;
    public Transform linkcontent;
    public int spaxingsize = 25;
    public int countoflinks = 1;
    public int countofgoals = 1;
    public ScrollRect parentscroll;
    string[] datesplit;
    string[] textSplit;
    public string eventdatetoshow;
    //recive data
    public Text name;
    Vector2 singleitemsize;

    public static int currentscreen;
    public int updatescreen;
    //restore event
    Vector2 originalparentscroll;
    Vector2 orignalgaolsizedelata;
    Vector2 originallinkscroldelta;
    //new entries
    public Toggle[] links;
    public InputField[] linksurl;
    public string[] linktitle;
    public GameObject[] linkobj;
    public GameObject[] linksprites;

    //timer stuff
    public InputField time1, time2, time3;
    public int timeone, timetwo, timethree;

    public string firsttime, secondtime, thirdtime;
    int urlcheck;
    public InputField raceno;
    //distance stuff
    public InputField d1, d2, d3;
    //public int one, timetwo, timethree;

    public Button[] todisable;
    public GameObject[] toenable;

    public string firstdistance, seconddistance, thirddistance;

    public static int eventchooser;
    public GameObject[] sorteventobject;

    public GameObject[] ar100medal;

    public static int hintpopup;

    public static int workbadges;
    [Header("IAP buttons")] 
    public Button btn_Monthly;
    public Button btn_Quaterly;
    public Button Btn_Yearly;
    public Button Android_btn_Monthly;
    public Button Android_btn_Quaterly;
    public Button Android_Btn_Yearly;

    public Button save;
    public InputField clubcode;


    public GameObject EditProfileBtn;

	/// <summary>
	/// /profile screen 
	/// </summary>
	public ScrollRect paywalscroll;
    public GameObject restorebutton,paywallscreen,hardpyawall, actualPayWall, profile;





    public GameObject firebaseobject;

    public GameObject clubbadge, clubbadge1, clubbadge2, clubbadge3;
    
    [Header("IAP 50% Off")]
    public GameObject HalfpricePanel;

    [Header("Restrictions Data")]
    public ListOfDataSaved ListOfDataSaved;

    public List<string> savedMonth = new List<string>();
    public List<int> savedYear = new List<int>();

    public bool exists = false;

    public int index;

    public int EventCount;

    public static int yearToCheck;
    public static string monthToCheck;
    public static int currentCount;
    public static string x;

    public GameObject UpgradeBtn;
    public GameObject awodePanel;
    public GameObject events;
     public GameObject eventsCanvasGroup;
    public GameObject help;
    public bool canRefresh = false;

    public Button addBtn;
    public Button saveBtn;
    public Button settingsBtn;
    public Button profileBtn;
    public Button helpBtn;
    public GameObject reStoreBtn;
    public ARSession arSession;
    public XROrigin xrOrigin;
    public ARPlaneManager arPlaneManager;
    public ARCameraManager arCameraManager;
    public ARCameraBackground arCameraBackground;
    public TMP_Text halfPayWallHeadingText;

    public List<ContentSizeFitter> Contents = new List<ContentSizeFitter>();
    public void scrollerrest()
    {

        paywalscroll.verticalNormalizedPosition = 1;

    }

    public void ChangeHalfPayWallTextFormUpdate()
    {
        halfPayWallHeadingText.text =
            "Oh no, you hit the wall! <color=#F7C85A>Go Pro today</color> to capture <u>all</u> your races.";
    }

    public void ChangeHalfPayWallTextFormMembership()
    {
    halfPayWallHeadingText.text =
        "Just a heads up... <color=#F7C85A>Erny is FREE</color> for up to 10 races per year!";
    }

    public void ReserProfileScroller()
    {
        foreach(ContentSizeFitter content in Contents)
        {
            StartCoroutine(ContentReset(content));
        }
    }

    public void EventScreenAlphaDelay()
    {
        StartCoroutine(AlphaDelay());
    }

    IEnumerator AlphaDelay()
    {
        Debug.Log("AHHAHAHAHAAHAHAHAHHAHAHAHAHAHAHAHAAHAHAHAHAHAHH");
        eventsCanvasGroup.GetComponent<CanvasGroup>().alpha = 0f;
        yield return new WaitForSeconds(1.5f);
        eventsCanvasGroup.GetComponent<CanvasGroup>().alpha = 1f;
    }

    IEnumerator ContentReset(ContentSizeFitter content)
    {
        yield return new WaitForSeconds(1f);
        content.enabled = false;
         yield return new WaitForSeconds(0.5f);
         content.enabled = true;
    }

    public void StopCamera()
    {
        if (cameraManager != null && cameraManager.subsystem != null)
            cameraManager.subsystem.Stop(); 
    }

    public void ActivateAR()
    {
        AR.SetActive(false);
        extraCamera.SetActive(true);
    }

    public void DeActiveAr()
    {
        //StartCoroutine(AROfAndOn());
    }

    IEnumerator AROfAndOn()
    {
        yield return new WaitForSeconds(0.5f);
        AR.SetActive(false);
        StopCamera();
        yield return new WaitForSeconds(1f);
        AR.SetActive(true);
        yield return new WaitForSeconds(1f);
        AR.SetActive(false);
        StopCamera();

        Debug.Log("AR object is off now");
    }
    public void CanRefresh()
    {
        canRefresh = true;
    }


    public static int appopened;
    public void CHECKING()
    {
        //UPDATECAMERA.cameraon();





    }
    public string sentence;

	public bool changecheck;
    public void eventnamecheck()
    {
        sentence = Eventname.text;
		eventnamechecker = Eventname.text.Split(char.Parse(" "));

        for (int i = 0; i < eventnamechecker.Length; i++)
        {
            for (int c = 0; c < 1; c++)
            {

                if (eventnamechecker[i].ToLower() == disney100medal[c].ToLower() && year.value == 1)
                {
                  /*  if (eventnamechecker.Length >= i + 1)
                    {
                        if (eventnamechecker[i + 1].ToLower() == disney100medal[c + 1].ToLower())
                        {

                            if (eventnamechecker.Length >= i + 2)
                            {

                                if (eventnamechecker[i + 2].ToLower() == disney100medal[c + 2].ToLower())
                                {
                                    
                                          //  ar100medal[0].SetActive(true);
                                    if (AddressableHandler.Instance.isModelDownloaded[0])
                                    {
                                     //   selecttheme("medaldisney");
                                       // themenumebr(6);
                                    }      return;
                                    
                                }
                            }
                        }
                    }
                    */
                }
                else
                {
                    ar100medal[0].SetActive(false);

                }


            }


            for (int b = 0; b < spartan.Length; b++)
			{
				
				/*foreach (char word in lowerCaseWord)
				{
                    string lowerCaseCurrentWord = disney[b];
					if (lowerCaseCurrentWord.Contains(lowerCaseWord))
					{
						selecttheme("clover");
						themenumebr(1);
                        changecheck = true;
						//return; // Exit the method if the word is found
					}
				}
                if (changecheck == true)
                {
                    return;
                }*/

				string[] words = sentence.Split(' ');

				// Convert the word to lowercase to perform a case-insensitive check
				string lowerCaseWord = disney[0].ToLower();

				// Check each word if it contains the word to check
				foreach (string word in words)
				{
					// Convert the current word to lowercase
					string lowerCaseCurrentWord = word.ToLower();

					// Check if the word to check is part of the current word
					if (lowerCaseCurrentWord.Contains(lowerCaseWord))
					{
						if (AddressableHandler.Instance.isModelDownloaded[1])
						{
							selecttheme("clover");
							themenumebr(1);
						}
						return; // Exit the method if the word is found
					}
				}





				if (eventnamechecker[i].ToLower() == disney[b])
                {
                    if (AddressableHandler.Instance.isModelDownloaded[1])
                    {
                        selecttheme("clover");
                        themenumebr(1);
                    }
                  

                    return;
                }
                else if (eventnamechecker[i].ToLower() == spartan[b])
                {
                    if (AddressableHandler.Instance.isModelDownloaded[3])
                    {

                        selecttheme("baloon");
                        themenumebr(0);
                    }
                    return;
                }
               else
                {

                    selecttheme("baloon");
                   themenumebr(0);

                    ar100medal[0].SetActive(false);
                }
                
            }
            
            
        }
       
    }


    public void spritechange()
    {
        eventImage = "";

        newstring = "";
        eventcurrentimage.texture = defaulteventiamge.texture;
    }


    public void mainspritechange()
    {
        imagedata = "";
        PlayerPrefs.SetString("image", imagedata);
        PlayerPrefs.Save();
        hiimage.texture = defaultmainscreenimage.texture;
    }

    List<string> substrings = new List<string>();




    public void textchange(string distance)
    {
        substrings.Clear();
        distancecomplete.text = distance;

        distancetextcheck();



    }
   
    
    public void checkevent(int number)
    {
        eventchooser = number;
    
    }
    public void kmchange()
    {
        if (eventchooser != 1)
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
                if (distancecomplete.text == "4216" || distancecomplete.text == "004216")
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
                    distancecomplete.text = "4216";
                    distancetextcheck();
                }







            }

        }

    }




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
                    d1.text ="";
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
                    d2.text = substrings[1]+substrings[2];
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



    public void OpenAppSettings()
    {
#if UNITY_IOS
  
        string bundleIdentifier = Application.identifier;
        string settingsUrl = string.Format("app-settings:{0}", bundleIdentifier);
        Application.OpenURL(settingsUrl);
#endif



#if PLATFORM_ANDROID



        using var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        using AndroidJavaObject currentActivityObject = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
        string packageName = currentActivityObject.Call<string>("getPackageName");
        using var uriClass = new AndroidJavaClass("android.net.Uri");
        using AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromParts", "package", packageName, null);
        using var intentObject = new AndroidJavaObject("android.content.Intent", "android.settings.APPLICATION_DETAILS_SETTINGS", uriObject);
        intentObject.Call<AndroidJavaObject>("addCategory", "android.intent.category.DEFAULT");
        intentObject.Call<AndroidJavaObject>("setFlags", 0x10000000);
        currentActivityObject.Call("startActivity", intentObject);






        /*
        using (var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (AndroidJavaObject currentActivityObject =
            unityClass.GetStatic<AndroidJavaObject>("currentActivity"))
        using (var intentObject = new AndroidJavaObject(
            "android.content.Intent", "android.settings.WIFI_SETTINGS"))
        {
            currentActivityObject.Call("startActivity", intentObject);
        }
        */

#endif


    }



    public void changetoggle(Toggle check)
    {
    
        

    }
    
	public void LinkHelpEMAILreuqest()
	{
#if UNITY_IPHONE

requestSendEmail();


#endif
#if UNITY_ANDROID
		string t =
		  "mailto: member@blingar.co?subject=";
		Application.OpenURL(t);
#endif
    }
        void requestSendEmail()
		{
			string email = "member@blingar.co";
			string subject = MyEscapeURL("Blingar Support");
			string body = MyEscapeURL("");
			Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
		}


		void SendEmail()
    {
        string email = "support@blingar.co";
        string subject = MyEscapeURL("Blingar Support");
        string body = MyEscapeURL("");
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }
    string MyEscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }

    public void eventdefault()
    {




    }



    public void cleardata()
    {
        newpaywallcheck = 0;
        if (PlayerPrefs.GetInt("google", 0) == 1)
        {
            PlayerPrefs.SetInt("google", 0);
            PlayerPrefs.DeleteAll();
            AddressableHandler.Instance.reassignplayerpref();
            currentscreen = 0;
            SceneManager.LoadScene("3. LoginScene");
            LoginWithGoogle.instance.Logout();
        }
        else if (PlayerPrefs.GetInt("apple", 0) == 1)
        {
            PlayerPrefs.SetInt("apple", 0);
            PlayerPrefs.DeleteAll();
            AddressableHandler.Instance.reassignplayerpref();
            currentscreen = 0;
            SceneManager.LoadScene("3. LoginScene");
            MainMenu.instance.LogoutFromApple();
        }
        else
        {
            PlayerPrefs.DeleteAll();



            AddressableHandler.Instance.reassignplayerpref();


            currentscreen = 0;



            SceneManager.LoadScene("3. LoginScene");
        }

        PlayerPrefs.SetInt("OneTimeAppear", 1);
        PlayerPrefs.SetInt("AutoOnce", 1);
    }
   


    public void changeurl(int url)
    {
        if (links[url].isOn)
        {

            for (int i = 0; i < linkobj.Length; i++)
            {


                if (i == url)
                {
                    linkobj[i].SetActive(true);

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
                    linkobj[i].SetActive(false);
                }
            }

        }
    }
    public void onedsitance()
    {
        firstdistance = d1.text.Normalize();
        if (firstdistance.Length == 2)
        {
            d2.ActivateInputField();
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

            d1.ActivateInputField();
        }

    }
    public void threedistance()
    {
        thirddistance = d3.text.Normalize();
        if (thirddistance.Length == 0)
        {
            d2.ActivateInputField();
        }

    }

    public void onetimer()
    {
        firsttime = time1.text.Normalize();
        if(firsttime.Length==2)
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
        else if(secondtime.Length==0)
        {

            time1.ActivateInputField();
        }
        Debug.Log("" + secondtime.Length);
    }
    public void onethree()
    {
        thirdtime = time3.text.Normalize();
        if (thirdtime.Length == 0)
        {
           time2.ActivateInputField();
        }

    }
    IEnumerator iapsusccessful()
    {
        yield return new WaitForSeconds(1);
        
        SceneManager.LoadScene("4. MainScreens");
    }

    public void RestoreComplete()
    {
        reStoreBtn.SetActive(false);
    }

    public void CheckPurchase()
    {
        ApplySubscriptionStateFromApi();
        if (UserSubscriptionState.IsSubscribed)
        {
            Android_btn_Monthly.interactable = false;
            Android_btn_Quaterly.interactable = false;
            Android_Btn_Yearly.interactable = false;
            btn_Monthly.interactable = false;
            btn_Quaterly.interactable = false;
            Btn_Yearly.interactable = false;

            reStoreBtn.SetActive(false);
        }
        else
        {

            Android_btn_Monthly.interactable = true;
            Android_btn_Quaterly.interactable = true;
            Android_Btn_Yearly.interactable =true;
            btn_Monthly.interactable = true;
            btn_Quaterly.interactable = true;
            Btn_Yearly.interactable = true;
        }
    }

    // Monthly   
    
    public void buysubscibtion()
    {
        if (HalfpricePanel.gameObject.activeInHierarchy)
        {
            HalfpricePanel.SetActive(false);
        }

        actualPayWall.SetActive(false);
        profile.SetActive(true);

        reviewcheck = 1;
           signuppaid = 1;
        subsciptionstring = "Pro";
        UserSubscriptionState.SetSubscribed("Monthly", "");
        subsciptiontext.text = subsciptionstring;
        loader.SetActive(true);
        // pops.instance.pp.Play("popp");
        // popup.SetActive(true);
        //  pops.instance.header.text = "Successfull";
        //  pops.instance.description.text = "Your subscribe was successfull.";
        if (subscriptionapi.subcount > 0)
        {
            DateTime dateTime = DateTime.Parse(subscriptionapi.instance.subscriptionEndDate);
            if (currenttime > dateTime)
            {
                DateTime futureDate = currenttime.AddDays(30);

                subscriptionapi.instance.subscrioptionupdated("Monthly", futureDate.ToString());
            }
            else
            {

                subscriptionapi.instance.subscrioptionupdated("Monthly", subscriptionapi.instance.subscriptionEndDate);
            }
        }
        else
        {

            DateTime futureDate = currenttime.AddDays(30);

            subscriptionapi.instance.subscrioptionbought("Monthly", futureDate.ToString());
        
        }

        Android_btn_Monthly.interactable = false;
        Android_btn_Quaterly.interactable = false;
        Android_Btn_Yearly.interactable = false;
        btn_Monthly.interactable = false;
        btn_Quaterly.interactable = false;
        Btn_Yearly.interactable = false;
        reStoreBtn.SetActive(false);
        StartCoroutine(iapsusccessful());
    }

    // Quaterly 
    public void buyquarterlysubscibtion()
    {
        reviewcheck = 1;
        signuppaid = 1;
        subsciptionstring = "Pro";
        UserSubscriptionState.SetSubscribed("Quarterly", "");
        subsciptiontext.text = subsciptionstring;

        loader.SetActive(true);
        // pops.instance.pp.Play("popp");
        // popup.SetActive(true);
        //  pops.instance.header.text = "Successfull";
        //  pops.instance.description.text = "Your subscribe was successfull.";
        if (subscriptionapi.subcount > 0)
        {
            DateTime dateTime = DateTime.Parse(subscriptionapi.instance.subscriptionEndDate);
            if (currenttime > dateTime)
            {
                DateTime futureDate = currenttime.AddDays(90);

                subscriptionapi.instance.subscrioptionupdated("Quarterly", futureDate.ToString());

                Debug.Log("working iap quaterly");
            }
            else
            {
                Debug.Log("working iap: "+ subscriptionapi.instance.subscriptionEndDate+" quaterly");
                subscriptionapi.instance.subscrioptionupdated("Quarterly", subscriptionapi.instance.subscriptionEndDate);
            }
        }
        else
        {

            DateTime futureDate = currenttime.AddDays(90);

            subscriptionapi.instance.subscrioptionbought("Quarterly", futureDate.ToString());

        }
        Android_btn_Monthly.interactable = false;
        Android_btn_Quaterly.interactable = false;
        Android_Btn_Yearly.interactable = false;

        btn_Monthly.interactable = false;
        btn_Quaterly.interactable = false;
        Btn_Yearly.interactable = false;

        reStoreBtn.SetActive(false);
        StartCoroutine(iapsusccessful());
    }

    // Yearly 

    public void buyyearlysubscibtion()
    {
        reviewcheck = 1;
        signuppaid = 1;
        subsciptionstring = "Pro";
        UserSubscriptionState.SetSubscribed("Yearly", "");
        subsciptiontext.text = subsciptionstring;

        loader.SetActive(true);
        // pops.instance.pp.Play("popp");
        // popup.SetActive(true);
        //  pops.instance.header.text = "Successfull";
        //  pops.instance.description.text = "Your subscribe was successfull.";
        if (subscriptionapi.subcount > 0)
        {
            DateTime dateTime = DateTime.Parse(subscriptionapi.instance.subscriptionEndDate);
            if (currenttime > dateTime)
            {
                DateTime futureDate = currenttime.AddDays(365);
                Debug.Log("working iap yearly");
                subscriptionapi.instance.subscrioptionupdated("Yearly", futureDate.ToString());
            }
            else
            {
                Debug.Log("working iap yearly updated");
                subscriptionapi.instance.subscrioptionupdated("Yearly", subscriptionapi.instance.subscriptionEndDate);
            }
        }
        else
        {

            DateTime futureDate = currenttime.AddDays(365);

            subscriptionapi.instance.subscrioptionbought("Yearly", futureDate.ToString());

        }

        Android_btn_Monthly.interactable = false;
        Android_btn_Quaterly.interactable = false;
        Android_Btn_Yearly.interactable = false;
        btn_Monthly.interactable = false;
        btn_Quaterly.interactable = false;
        Btn_Yearly.interactable = false;

        reStoreBtn.SetActive(false);
        StartCoroutine(iapsusccessful());
    }












    public void subscibtionfailed()
    {

        if (HalfpricePanel.gameObject.activeInHierarchy)
        {
            //HalfpricePanel.SetActive(false);
        }

        //actualPayWall.SetActive(false);
         profile.SetActive(true);

        loader.SetActive(false);
        signuppaid =0;
        UserSubscriptionState.SetUnsubscribed();
        if (value > 15)
        {
            Debug.Log("No of days after buying subscriptionis: " + value);
            subsciptionstring = UserSubscriptionState.DisplayText;
            subsciptiontext.text = "Open";
        }
        else
        { 
        
        
        
        }


        //   pops.instance.pp.Play("popp");
        //   popup.SetActive(true);
        //    pops.instance.header.text = "Oops";
        //    pops.instance.description.text = "Your subscription has not successfull, please subscribe to continue having fun with us.";




        Android_btn_Monthly.interactable = true;
        Android_btn_Quaterly.interactable = true;
        Android_Btn_Yearly.interactable = true;
        btn_Monthly.interactable = true;
        btn_Quaterly.interactable = true;
        Btn_Yearly.interactable = true;

        reStoreBtn.SetActive(true);
    }


    public void firsttimer()
    {
     
            for (int i = 0; i < toenable.Length; i++)
            {
                toenable[i].SetActive(true);


            }

           

    }

    public void unsubscribeduser()
    {
        if (signuppaid == 0 &&  freetrial == 1)
        {
            loader.SetActive(true);

            for (int i = 0; i < toenable.Length; i++)
            {
                toenable[i].SetActive(true);


            }

            for (int i = 0; i < disableelements.Length; i++)
            {
                disableelements[i].SetActive(false);


            }

            loader.SetActive(false);

        }
        else
        {
            for (int i = 0; i < todisable.Length; i++)
            {

                todisable[i].interactable = true;

            }



        }


    }



    public class deldata
    {
        public String id { get; set; }
    }


    public void deletedata()
    {
        deldata deldat = new deldata();
        deldat.id = apigetter.id;
        Debug.Log(apigetter.id);
        String JsonVal = Newtonsoft.Json.JsonConvert.SerializeObject(deldat);
       
        StartCoroutine(deletdataa(baseurl + "/v1/user", JsonVal));



    }
    public class delresponse
    {
        public bool acknowledged { get; set; }
        public int deletedCount { get; set; }
    }

    public class delrecieve
    {
        public bool success { get; set; }
        public delresponse response { get; set; }
        public object metadata { get; set; }
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
            loader.SetActive(true);
            cleardata();
            
        }
    }

    public void cameranotallowedpopup()
    {
        settingsbutton.SetActive(true);
        permissionpopip.instance.popbox.SetActive(true);
        permissionpopip.instance.pp.Play("popp");
        permissionpopip.instance.header.text = "Oops!";
        permissionpopip.instance.description.text = "Permission is required to use this feature. Kindly turn on Camera in device settings to continue.";
        UpgradeBtn.SetActive(false);
    }
    public void ARcameranotallowed()
    {
        settingsbutton.SetActive(true);
        permissionpopip.instance.popbox.SetActive(true);
        permissionpopip.instance.pp.Play("popp");
        permissionpopip.instance.header.text = "Oops!";
        permissionpopip.instance.description.text = "Permission is required to use this feature. Kindly turn on Camera in device settings to continue.";
        UpgradeBtn.SetActive(false);
    }


    public void hintofbadge()
    {
        popup.SetActive(true);
        popuppanel.pp.Play("popp");
        popuppanel.header.text = "Insider tip!";
       // string tex = "Disney";
        string newtext = "To get the correct Blinget, enter the official Event Name and date of your race.";
        popuppanel.description.text = newtext;
        UpgradeBtn.SetActive(false);
    }

    public void ActualAwodes()
    {
               StartCoroutine(Awodes());
    }

    public IEnumerator Awodes()
    {
        addBtn.interactable = false;
        saveBtn.interactable = false;
        settingsBtn.interactable = false;
        profileBtn.interactable = false;
        helpBtn.interactable = false;
        Invoke("SettingsActivate", 1f);
        yield return new WaitForSeconds(1f);
        addBtn.interactable = true;
        saveBtn.interactable = true;
        settingsBtn.interactable = true;
        profileBtn.interactable = true;
        helpBtn.interactable = true;
    }

    void SettingsActivate()
    {
        awodePanel.SetActive(true);
    }
    
    public void Events()
    {
        Invoke("EventDeActivate", 1f);
    }

    void EventDeActivate()
    {
        events.SetActive(false);
    }
    
    public void Help()
    {
        Invoke("HelpDeActivate", 1f);
    }

    void HelpDeActivate()
    {
        help.SetActive(false);
    }
    
    public void Profile()
    {
        Invoke("ProfileDeActivate", 1f);
    }

    void ProfileDeActivate()
    {
        profile.SetActive(false);
    }

    public void delaccountpop()
    {
        popup.SetActive(true);
        popuppanel.pp.Play("popp");
        popuppanel.header.text = "Warning!"; 
        popuppanel.description.text = "Are you sure you want to delete your account and achievements?";
        UpgradeBtn.SetActive(false);
    }

        public void unpaiduser()
    {
        if (freetrial == 1 && signuppaid == 0)
        {
            signuppaid = 1;

            PlayerPrefs.SetInt("OneTimeAppear", 1);

            for (int i = 0; i < todisable.Length; i++)
            {

                todisable[i].interactable = false;

            }
            popup.SetActive(true);
            popuppanel.pp.Play("popp");
            popuprefuel.SetActive(true);
            popupclose.SetActive(false);

            popuppanel.header.text = "Oh no!";
            popuppanel.description.text = "Your subscription is out of juice. Refuel today to stay on course.";
            firsttimer();
            loader.SetActive(false);
            UpgradeBtn.SetActive(false);
        }
        else
        {
            for (int i = 0; i < todisable.Length; i++)
            {

                todisable[i].interactable = true;

            }



        }



    }
    public string subsciptionstring;
    public Text subsciptiontext;


                              
    public Text eventcount;


    public void checkforsubscription()
    {

        //if (value > 0 && signuppaid == 0)
        //{

        //    freetrialsubscription.SetActive(true);


        //}
        //else
        //{
        //    freetrialsubscription.SetActive(false);


        //}


    }

    private void ApplySubscriptionStateFromApi()
    {
        signuppaid = UserSubscriptionState.PaidValue;
        subsciptionstring = UserSubscriptionState.DisplayText;

        if (subsciptiontext != null)
        {
            subsciptiontext.text = UserSubscriptionState.IsSubscribed ? subsciptionstring : "Open";
        }
    }

    public void RefreshSubscriptionStateFromApi()
    {
        ApplySubscriptionStateFromApi();
        CheckPurchase();
    }
    public static int newpaywallcheck;
public static int checkappopened;
    void Awake()
    {
        //Debug.Log("I am able to go inside the eventgertter awake");

        instance = this;
        goalgetter = 0;
        linkgetter = 0;

        if (checkappopened == 0)
        {
            Instantiate(loadingscreen, transform.position, transform.rotation);
        
        
        }


        if (ApiRequestGenerator.guestlogin != 0)
        {
            if (newpaywallcheck == 0)
            {

                for (int i = 0; i < panels.Length; i++)
                {
                    if (currentscreen == 4)
                    {
                        if (i == 4)
                        {
                           
                            panels[i].SetActive(true);
                            icons[i].SetActive(true);
                            if (currentscreen == 1)
                            {



                            }
                        }
                        else
                        {

                            panels[i].SetActive(false);
                            icons[i].SetActive(false);
                        }
                    }

                }













            }
            else
            {

                for (int i = 0; i < panels.Length; i++)
                {

                    if (i == currentscreen)
                    {
                        Debug.Log("currentscree in loop " + currentscreen);
                        panels[i].SetActive(true);
                        icons[i].SetActive(true);
                        screencurenter(currentscreen);
                        if (currentscreen == 1)
                        {
                            
                            geteventlist();
                            //EventScreenAlphaDelay();
                        }
                    }
                    else
                    {
                        icons[i].SetActive(false);
                        panels[i].SetActive(false);
                    }
                }
            }
        }

        Debug.Log("new check: "+newpaywallcheck+" "+currentscreen);

        newpaywallcheck = 1;



        loader = GameObject.FindGameObjectWithTag("loader").GetComponent<dontdestroy>().loader;

            checkappopened = 1;
        loader.SetActive(false);

        



        if (ApiRequestGenerator.guestlogin == 0)
        {
            Debug.Log("ApiRequestGenerator.guestlogin " + ApiRequestGenerator.guestlogin);
            sortevent = PlayerPrefs.GetInt("sort", 0);
			for (int i = 0; i < panels.Length; i++)
            {

      //          if (i == currentscreen)
      //          {
      //                  Debug.Log("currentscree in loop " + currentscreen);
      //              panels[i].SetActive(true);
      //              icons[i].SetActive(true);
      //              if (currentscreen == 1)
      //              {
						//geteventlist();

      //              }
      //              else if(currentscreen == 0)
      //              {
      //                  profile.SetActive(false);
      //                  screencurenter(0);
      //              }
      //          }
      //          else
      //          {
      //              icons[i].SetActive(false);
      //              panels[i].SetActive(false);
      //          }
            }
            if (baselink.firebaseioscheck > 0)
            {
                //firebaseobject.SetActive(false);
            }

            baseurl = baselink.Url;

            versiondisplay.text = ApiRequestGenerator.versionupdate;
            eventhandler = 0;
         
            hintpopup = PlayerPrefs.GetInt("hint", 0);

			Debug.Log("sortnumber: " + sortevent);



			for (int k = 0; k < sorteventobject.Length; k++)
            {
                if (k == sortevent)
                {
                    sorteventobject[k].SetActive(true);
                }
                else
                {

                    sorteventobject[k].SetActive(false);


                }
            }
            ApplySubscriptionStateFromApi();
            Debug.Log("check text: " + subsciptionstring);
            eventno();
            if (currentscreen == 1)
            {
                loader.SetActive(true);
            }
            //freetrial = PlayerPrefs.GetInt("freetrial", 0);



            Debug.Log("date: " + apigetter.createdtime);
            Debug.Log("value: " + value);
            currenttime = DateTime.Now;
            Debug.Log("curdate: " + currenttime);
            value = (int)((currenttime - apigetter.createdtime).TotalDays); //currenttime.Subtract(apigetter.createdtime);
            Debug.Log("value: " + value);
            //        if (value > 8)
            //        {
            //            // hardpyawall.SetActive(true);
            //            crossbutton.SetActive(false);
            //            crossbutton2.SetActive(false);
            //freetrial = 1;
            //            PlayerPrefs.SetInt("freetrial", 1);
            //            PlayerPrefs.Save();

            //        }
            //        else
            //        {
            //         // hardpyawall.SetActive(false);
            //            freetrial = 0;
            //            PlayerPrefs.SetInt("freetrial", 0);
            //            PlayerPrefs.Save();
            //        }






            Debug.Log("profprile: " + apigetter.profile);
            if (apigetter.profile != "")
            {
                StartCoroutine(GetTexture());

            }

            timeone = 0;
            timethree = 0;
            timetwo = 0;
            workbadges = PlayerPrefs.GetInt("badge", 0);

            for (int z = 0; z < eventalbum.Length; z++)
            {
                Debug.Log("get event");
                eventalbum[z].GetComponent<Button>().interactable = false;
            }
            eventhandler = 0;
            updatescreen = 0;
           
            imagedata = PlayerPrefs.GetString("image");
          
           

            if (freetrial == 1 && signuppaid == 0)
            {

                unpaiduser();
                subsciptiontext.text = "Open";
                Debug.Log("No of days after buying subscriptionis: " + value);
            }
            else if (subsciptionstring != "")
            {


                subsciptiontext.text = subsciptionstring;
            }
            else
            {
                if(value < 15)
                {
                    subsciptiontext.text = "Pro";
                    Debug.Log("No of days after buying subscriptionis: " + value);
                }
                else
                {
                    subsciptiontext.text = "Open";
                    Debug.Log("No of days after buying subscriptionis: " + value);
                }
                

            }


            closecamera();
        }
        else
        { 
        
        
        
        
        
        }

      


        // if(PlayerPrefs.GetInt("OneTimeAppear", 0) == 1)
        // {
        //     //Debug.Log("one time appear");
        //     //screencurenter(0);
        //     //freetrialsubscription.SetActive(false);
           
        //       Debug.Log("1badges.currentvalue: " + badges.currentvalue);
        // }
        // else if(PlayerPrefs.GetInt("OneTimeAppear", 0) == 0 && badges.currentvalue < 1)
        // {
        //     // currentscreen = 3;
        //      screencurenter(2);
        //     // profile.SetActive(true);
        //     // freetrialsubscription.SetActive(true);
        //     PlayerPrefs.SetInt("OneTimeAppear", 1);
        //     Debug.Log("badges.currentvalue: " + badges.currentvalue);
        // }
        // else
        // {
            
        // }

    }


    public void DeactivateIcons()
    {
         for (int i = 0; i < panels.Length; i++)
        {
            icons[i].SetActive(false);
        }
    }

    public void screencurenter(int screen)
    {  
        Debug.Log("entered screencurenter");
        currentscreen = screen;

        if (freetrial == 1 && signuppaid == 0)
        {
          
        }
        else
        {

            if (currentscreen == 1)
            {
                loader.SetActive(true);
                if (ApiRequestGenerator.guestlogin == 0)
                {
                    Debug.Log("currentscree is 1 now " + currentscreen);
                    SceneManager.LoadSceneAsync("4. MainScreens");
                }
                else
                {
                    
                    Debug.Log("currentscree " + currentscreen);

                    SceneManager.LoadSceneAsync("5.guestlogin");
                    
                }
            }
            Debug.Log("currentscree " + currentscreen);
            PlayerPrefs.GetInt("LogOutSubscription", 0);
        }
    }

    public void closeevent ()
    {

        if (freetrial == 1 && signuppaid == 0)
        {
            unpaiduser();


        }
        else
        {
            if (eventhandler == 1)
            {
                eventhandler = 0;
                //  anim.Play("event off");
                clearevent();
            }
            else if (eventhandler == 2)
            {
                eventhandler = 0;
                //   anim.Play("eventlistoff");
            }
            else if (eventhandler == 3)
            {
                eventhandler = 0;

                //  anim.Play("updateoff");
            }
            if (eventhandler != 1)
            {
                progileedit.SetActive(false);
                cookies.SetActive(false);
            }
            
            
            eventhandler = 0;
            
                for (int i = 0; i < todisable.Length; i++)
                {

                    todisable[i].interactable = true;

                }

        }


    }
  
    

    public void ConfettieOn()
    {
        if (confettieObj == null)
        {
            return;
        }

        UIConfetti confetti = confettieObj.GetComponent<UIConfetti>();
        if (confetti != null)
        {
            confetti.Play();
        }
    }

    private void PlayCurrentEventCardConfetti()
    {
        GameObject eventList = GameObject.Find("eventlist");
        if (eventList == null)
        {
            return;
        }

        recievedata eventCardData = eventList.GetComponent<recievedata>();
        if (eventCardData != null)
        {
            eventCardData.PlayConfetti();
        }
    }


    public void updatedoff()
    {
        anim.Play("updateoff");
    }
    public void openevent()
    {

        if (eventhandler == 1)
        {
            anim.Play("event on");
        }
        else if (eventhandler == 2)
        {
            anim.Play("eventliston");
            PlayCurrentEventCardConfetti();
        }
        else if (eventhandler == 3)
        {

            anim.Play("updateon");
        }
        else if (eventhandler == 4)
        {


        }
        else if (eventhandler == 5)
        {

        }



    }
    
    public void ProfileExtraOff()
    {
        StartCoroutine(profileExtraOff());
    }

    public IEnumerator profileExtraOff()
    {
        yield return new WaitForSeconds(1f);
        profile.SetActive(false);
    }

    IEnumerator checkupdate()
    {
        yield return new WaitForSeconds(0.4f);
        if (ApiRequestGenerator.guestlogin == 0)
        {
            SceneManager.LoadSceneAsync("4. MainScreens");
        }
        else
        {

            SceneManager.LoadSceneAsync("5.guestlogin");

        }
    }
    public void updateison()
    {
        if (freetrial == 1 && signuppaid == 0&&currentscreen!=4&&currentscreen!=3)
        {

            
                unsubscribeduser();
            

        }
        else

        {
            if (updatescreen == 1)
            {
                loader.SetActive(true);
                StartCoroutine(checkupdate());

            }
        }
    }



    public void eventon()
    {
        updatescreen = 1;
        eventhandler = 1;
    }

    public void updateon()
    {
        eventhandler = 3;
        openevent();
    }





    public void opencamera()
    {
      //  ARCAMER.SetActive(true); 
      //  ARCANVAS.SetActive(true);
       // arsessio.SetActive(true);

        bottom.SetActive(false);
        canv.SetActive(false);
        camer.SetActive(false);

    }

    public void closecamera()
    {
       // ARCAMER.SetActive(false);
       /// ARCANVAS.SetActive(false);
        //arsessio.SetActive(false);

        bottom.SetActive(true);

        canv.SetActive(true);
        camer.SetActive(true);

    }




#if (UNITY_IOS && !UNITY_EDITOR)
    [DllImport("__Internal")]
    private static extern void requestReview();
#endif

    IEnumerator checkreview()
    {


        yield return new WaitForSeconds(4);
        reviewcheck = 0;
        PlayerPrefs.SetInt("reviewcheck",0);
        PlayerPrefs.Save();

#if (UNITY_IOS && !UNITY_EDITOR)
        Debug.Log("Trying to request the review window.");
        requestReview();
#endif


    }




    public RectTransform eventsize;

    int value;

   DateTime currenttime;

    private void Start()
    {
        
        //Debug.Log("I am able to go inside the eventgertter start");
        Invoke("DeActiveAr", 5f);

        index = PlayerPrefs.GetInt("EventCount", 0);
        //GetEvent();
         
        spawnmangr.objectSpawned = false;
        sizeofgoal = goalscroll.content.sizeDelta;
        sizeoflink = goalscroll.content.sizeDelta;
        singleitemsize = eventsize.sizeDelta;

        orignalgaolsizedelata = sizeofgoal;
        originalparentscroll = parentscroll.content.sizeDelta;
        originallinkscroldelta = sizeoflink;
        if (ApiRequestGenerator.guestlogin==0)
        {

            archeck = 0;
            reviewcheck = PlayerPrefs.GetInt("reviewcheck", 1);


            if (reviewcheck == 1)
            {
                StartCoroutine(checkreview());

            }


            checksize = 0;
            ApplySubscriptionStateFromApi();
            CheckPurchase();

            profileimagetext = PlayerPrefs.GetString("profileimage");


            imagedata = PlayerPrefs.GetString("image");
            if (imagedata != "")
            {
                fromconvertimage();
                // hiimage.texture = death;
            }
            Debug.Log("date: " + profileimagetext);
            if (profileimagetext != "")
            {
                Debug.Log("date: " + " yecallhuwa hai");

                sendconverttextprofile();
            }
            death = dd;
            name.text = "Hi " + apigetter.firstname;

            Debug.Log("" + apigetter.jwt);



            if (signuppaid == 0 && appopened == 0 && freetrial == 0)
            {
                for (int i = 0; i < toenable.Length; i++)
                {
                    toenable[i].SetActive(true);


                }

                Debug.Log("checking the panel" + signuppaid + " " + freetrial + " " + appopened);
                //   freetrialsubscription.SetActive(true);

            }
            Debug.Log("checking the panel" + signuppaid + " " + freetrial + " " + appopened);
            Destroy(Eventscrol.content.GetChild(0).gameObject);
            appopened++;
            
        }

        // if (PlayerPrefs.GetInt("OneTimeAppear", 0) == 1)
        // {
        //     freetrialsubscription.SetActive(false);
        //     if (PlayerPrefs.GetInt("ProfileShownOnce", 0) == 0)
        //     {
        //         PlayerPrefs.SetInt("ProfileShownOnce", 1);

        //         profile.SetActive(false);
        //         Debug.Log("Profile False");
        //     }
        //     else
        //     {
        //         profile.SetActive(true);
        //         canv.SetActive(true);
        //         //screencurenter(0);
        //         Debug.Log("Profile True");
        //     }

        // }
        // else
        // {
        //     freetrialsubscription.SetActive(true);
        // }

        for (int i = 0; i < panels.Length; i++)
        {

            if (i == currentscreen)
            {
                
                panels[i].SetActive(true);
                icons[i].SetActive(true);
                if (currentscreen == 1)
                {
                    geteventlist();

                }
                else if (currentscreen == 0)
                {
                    Debug.Log("currentscree in loop " + currentscreen);
                    profile.SetActive(false);
                    screencurenter(0);
                }
            }
            else
            {
                icons[i].SetActive(false);
                panels[i].SetActive(false);
            }
        }
        ReserProfileScroller();
    }


    public void ProfileOff()
    {
        PlayerPrefs.SetInt("ProfileShownOnce", 0);
    }

    public void ProfileOn()
    {
        PlayerPrefs.SetInt("ProfileShownOnce", 1);
        profile.SetActive(true);
    }


    public void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("ProfileShownOnce", 0);
    }

    public void clearevent()
    {

      /*  for(int i=1;i<goalscroll.content.childCount;i++)
        {
          goalscroll.content.GetChild(i).gameObject.SetActive(false);
        }
        goalscroll.content.sizeDelta = new Vector2(orignalgaolsizedelata.x, orignalgaolsizedelata.y);

        goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight =200;

        for (int i = 1; i < linkscroll.content.childCount; i++)
        {
        linkscroll.content.GetChild(i).gameObject.SetActive(false);
        }

        linkscroll.content.sizeDelta = new Vector2(originallinkscroldelta.x, originallinkscroldelta.y);

        linktransform.gameObject.GetComponent<LayoutElement>().preferredHeight =500;
        parentscroll.content.sizeDelta = new Vector2(originalparentscroll.x, originalparentscroll.y);*/

    }

    public void destroyevent()
    {
       
            for (int i = 0; i < Eventscrol.content.childCount; i++)
        {
            Debug.Log("event destrou");
            Eventscrol.content.GetChild(i).gameObject.SetActive(false);

            }
        
    }




    public void goalsadder()
    {
        mygoals.Add(goal.text.Normalize());




        Debug.Log("count: " + mygoals.Count);
    }



    public void testlist()
    {
       

        
     

        Debug.Log("count: " + mygoals.Count);
    }

    public class profilee
    {
        public string profile { get; set; }
    }


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

    public class Link
    {
        public string title { get; set; }
        public string url { get; set; }
    }

    public class eventdata2send
    {
        public string position { get; set; }
        public string totalpeople { get; set; }
        public string eventType { get; set; }
        public string eventName { get; set; }
        public string eventDate { get; set; }
        public string city { get; set; }
        public string victoryCity { get; set; }
        public string time { get; set; }
        public Distance distance { get; set; }
        public string @virtual { get; set; }
        public string race { get; set; }
        public string personarecord { get; set; }
        public string image { get; set; }
        public string theme { get; set; }
        public List<Goal> goals { get; set; }
        public List<Link> links { get; set; }
        public Journal journal { get; set; }
        public Result result { get; set; }
    }

    public class Result
    {
        public string myAgeGroup { get; set; }
        public string totalAgeGroup { get; set; }
        public string myGender { get; set; }
        public string totalGender { get; set; }
    }
    public void selectevent(string type)
    {

        typegetter = type;
     
        if (ApiRequestGenerator.guestlogin == 0)
        {
            if (hintpopup == 0)
            {
                hintofbadge();
            }

            hintpopup = 1;
            PlayerPrefs.SetInt("hint", hintpopup);
            PlayerPrefs.Save();
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

    public void selecttheme(string type)
    {

        themegetter = type;

      

    }

    /* public void getgoals()
     {
         List<Goal> mtlistgoals = new List<Goal>();
         for (int i =0;i<=goalgetter;i++)
         {
             Goal temp = new Goal();
            if( goalscroll.content.GetChild(i).GetChild(1).GetChild(0).GetComponent<Image>().sprite.name=="yes")
             {
                 temp.achieved = "yes";
             }
            else
             {
                 temp.achieved = "No";
             }
             temp.goal = goalscroll.content.GetChild(i).GetChild(0).GetComponent<InputField>().text.Normalize();
             mtlistgoals.Add(temp);
         }


     //    Debug.LogError("checking" + mtlistgoals);
         foreach (var x in mtlistgoals)
         {
             Debug.Log("checking"+x.goal.ToString());
         }

     }*/




    public void sortlinks(int number)
    {
        for(int i = 0 ;i<links.Length;i++)
        {
            if(i==number)
            {
                linkobj[i].SetActive(true);
            }
            else
            {
                linkobj[i].SetActive(false);

            }

        }



    }




    public void opennurl(string url)
    {
       Application.OpenURL(url);
    }
    public void scrollreset()
    {
       
        //        int c = myDeserializedClass.response.Count;
        //    GameObject obj = Instantiate(gaolspawn, goalscroll.content);
        //gaolspawn.transform.SetParent(goaltranform);

       

        sizeofgoal = orignalgaolsizedelata ;
         parentscroll.content.sizeDelta = originalparentscroll ;
        goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight = 200;
       


        Debug.Log("goals: " + goalgetter);
    }
    public void goalminuser()
    {
        goalgetter--;
        countofgoals--;
        expandscript.goalchec--;
        //        int c = myDeserializedClass.response.Count;
        //    GameObject obj = Instantiate(gaolspawn, goalscroll.content);
        //gaolspawn.transform.SetParent(goaltranform);

        RectTransform cre;
        cre = goalscroll.GetComponent<RectTransform>();
        cre.sizeDelta = new Vector2(cre.sizeDelta.x, cre.sizeDelta.y - sizeofgoal.y);
        goalscroll.GetComponent<RectTransform>().sizeDelta = cre.sizeDelta;
        goalscroll.viewport.sizeDelta = cre.sizeDelta;

        goalscroll.content.sizeDelta = new Vector2(sizeofgoal.x, sizeofgoal.y / countofgoals);
        goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight -= sizeofgoal.y;
        goaltranform.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(goaltranform.gameObject.GetComponent<RectTransform>().sizeDelta.x, goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight);
        parentscroll.content.sizeDelta = new Vector2(parentscroll.content.sizeDelta.x, parentscroll.content.sizeDelta.y - sizeofgoal.y);


        Debug.Log("goals: " + goalgetter);
    }
    public void goaladder()
    {
       goalgetter++;
        countofgoals++;
        //        int c = myDeserializedClass.response.Count;
        GameObject obj = Instantiate(gaolspawn, goalscroll.content);
        //gaolspawn.transform.SetParent(goaltranform);
      
        RectTransform cre;
        cre =  goalscroll.GetComponent<RectTransform>();
        cre.sizeDelta = new Vector2(cre.sizeDelta.x, cre.sizeDelta.y + sizeofgoal.y);
        goalscroll.GetComponent<RectTransform>().sizeDelta = cre.sizeDelta;
        goalscroll.viewport.sizeDelta = cre.sizeDelta;
        goalscroll.content.sizeDelta = new Vector2(sizeofgoal.x, sizeofgoal.y *countofgoals);
  
        goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight += sizeofgoal.y;
        goaltranform.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(goaltranform.gameObject.GetComponent<RectTransform>().sizeDelta.x, goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight);
        parentscroll.content.sizeDelta = new Vector2(parentscroll.content.sizeDelta.x,  parentscroll.content.sizeDelta.y + sizeofgoal.y);


        Debug.Log("goals: " + goalgetter);
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
        parentscroll.content.sizeDelta = new Vector2(parentscroll.content.sizeDelta.x, parentscroll.content.sizeDelta.y + sizeoflink.y);

    }
    public void testingprofile()
    {
        apigetter.profile = "https://api-dev.blingar.cloud/uploads/IMG_20200326_232747.jpg";

        profilee pp = new profilee();

        pp.profile = apigetter.profile;

        profileiamgesend = Newtonsoft.Json.JsonConvert.SerializeObject(pp);
   
        
        StartCoroutine(profileiamgeupload(baseurl + "/v1/user/profile", profileiamgesend));

        Debug.Log("button is calling;");
    
    }

    public void url()
    {

        int c = linkcontent.childCount;
        for(int i=0;i<c;i++)
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
    IEnumerator profileiamgeupload(string url, string bodyJsonString)
    {
        Debug.Log("coroutine started is calling;");

        WWWForm form = new WWWForm();
        Dictionary<string, string> headers = form.headers;
        headers["JWT TOKEN"] = apigetter.jwt;
        var request = new UnityWebRequest(url, "PATCH");
        request.SetRequestHeader("Authorization", "Bearer " + apigetter.jwt);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        // request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        //Debug.Log("Status Code: " + request.responseCode);
        Debug.Log("profile image test: "+request.downloadHandler.text);
        Debug.Log(apigetter.profile);
        StartCoroutine(GetTexture());
    }



    public void changeupper()
    {
     victorycatchphrase.text=   victorycatchphrase.text.ToUpper();
    }

    public void evententrycondiiton()
    {
        if (monthh.value != 0 && year.value != 0 && date.value != 0 && cityname.text != "" )
        {
            
            yearToCheck = int.Parse(year.options[year.value].text);
            monthToCheck = monthh.options[monthh.value].text;

            checkcounter = 0;
            x = "No_of_Entries_In_Year_" + yearToCheck;
            for (int i = 0; i < PRracerecord.instance.reacerecords.Count; i++)
            {
                string x = PRracerecord.instance.reacerecords[i].eventDate;
                DateTime result = new DateTime();
                 

                Debug.Log("reacerecords is: " + PRracerecord.instance.reacerecords[i].eventDate + "Event name is: " + PRracerecord.instance.reacerecords[i].eventName);
                  
                result = Convert.ToDateTime(x);
              string tempdate =  "" + result.Year;
                
                if (tempdate == yearToCheck.ToString())
                {
                    checkcounter++;

                }



            }

            if (checkcounter > 9 && subsciptiontext.text == "Open")
            {
                popup.SetActive(true);
                pops.instance.pp.Play("popp");
                pops.instance.header.text = "Uh oh!";
                pops.instance.description.text = "You’ve reached your Open plan limit of 10 races per calendar year.";
                UpgradeBtn.SetActive(true);
            }
            else
            {
                
                evententry();
                save.interactable = false;
                Debug.Log("Saved: " + yearToCheck + " " + monthToCheck);
            }
                





        }
        else
        {
            popup.SetActive(true);
            UpgradeBtn.SetActive(false);
            pops.instance.pp.Play("popp");
            pops.instance.header.text = "Uh oh!";
            pops.instance.description.text = "Your Event Name, Date and City are required to advance on the course.";
        }
    }

    




    //guestdata
    string gusteventdate, guestdistance, guesttime, guestvictorycatchp, gvr, gpr, gdistunit;
    List<Link> guestlinkgoals = new List<Link>();
    List<Goal> guesttlistgoals = new List<Goal>();
    public void evententry()
  
    {
    //    picture pics = new picture();
      //  pics.response = new getpicture();
        eventdata2send eventsenddata = new eventdata2send();
        if (victorycatchphrase.text != "")
        {
            eventsenddata.victoryCity = victorycatchphrase.text.Normalize();
        }
        else
        {
            eventsenddata.victoryCity = "I DID IT!";
        }

        guestvictorycatchp = eventsenddata.victoryCity;

        eventsenddata.position = position.text.Normalize();
        eventsenddata.totalpeople = totalpeople.text.Normalize();
        eventsenddata.eventName = Eventname.text.Normalize();
        eventsenddata.city = cityname.text.Normalize();

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
        else if (thirdtime.Length == 1)
        {
            eventsenddata.time = "00:" + "00" + ":" + "0" + time3.text.Normalize();

        }
        else
        {
            eventsenddata.time = "00:" + "00" + ":" + "00";
        }
        guesttime = eventsenddata.time;

        eventsenddata.distance = new Distance();


        if (raceno.text != null)
        {
            eventsenddata.race = raceno.text;
        }
        else
        {
            eventsenddata.race = "000000";



        }
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


        if (thirdtime.Length == 1)
        {
            eventsenddata.time = "00:" + "00" + ":" + "0" + time3.text.Normalize();
            Debug.Log("Data wanted is " +  eventsenddata.time);
        }


        guestdistance = eventsenddata.distance.istance;


        // eventsenddata.distance.istance = distance.text.Normalize();
        eventsenddata.journal = new Journal();
        eventsenddata.journal.pre = pree.text;//.text.Normalize();
        eventsenddata.journal.post = postt.text;
        eventsenddata.journal.Next = nextt.text;
        eventsenddata.journal.race = racee.text;
        eventsenddata.journal.Notes = notes.text;

        workbadges = 2;
        PlayerPrefs.SetInt("badge", workbadges);
        PlayerPrefs.Save();



        if (eventImage != "")
        {
            Debug.Log("picture got sent");
            eventsenddata.image = eventImage;//eventImage.Normalize();


            if (ApiRequestGenerator.guestlogin == 0)
            {
                apigetter.profile = eventImage;
                profilee pp = new profilee();

                pp.profile = apigetter.profile;

                profileiamgesend = Newtonsoft.Json.JsonConvert.SerializeObject(pp);


                StartCoroutine(profileiamgeupload(baseurl + "/v1/user/profile", profileiamgesend));
            }
        }





        if (ApiRequestGenerator.guestlogin != 0)
        { 
        
        
        
        
        
        }





        Debug.Log("eventimage:" + eventImage);
        Debug.Log("test: " + imagepath);
        if (distanceunit.isOn)
        {
            Debug.Log("this was called.");
            eventsenddata.distance.unit = "mi";
        }
       else
        {

            eventsenddata.distance.unit = "km";
        }

        gdistunit = eventsenddata.distance.unit;

        int month = monthh.GetComponent<Dropdown>().value;
        List<Dropdown.OptionData> menuOptions = monthh.GetComponent<Dropdown>().options;
   

        int yearindex = year.GetComponent<Dropdown>().value;
        List<Dropdown.OptionData> yearoption = year.GetComponent<Dropdown>().options;


        int dateindex = date.GetComponent<Dropdown>().value;
        List<Dropdown.OptionData> dateoption = date.GetComponent<Dropdown>().options;
        if (ApiRequestGenerator.guestlogin == 0)
        {
            eventsenddata.eventDate = menuOptions[month].text + "/" + dateoption[dateindex].text.Normalize() + "/" + yearoption[yearindex].text.Normalize();
            //savedMonth = menuOptions[month].text;
        }
        //   Debug.Log("date: " + eventsenddata.eventDate);

        else
        {
            gusteventdate = month + "/" + dateoption[dateindex].text.Normalize() + "/" + yearoption[yearindex].text.Normalize();
        }
     /*   string x = menuOptions[monthindex].text.Normalize() + "/" + dateoption[dateindex].text.Normalize() + "/" + yearoption[yearindex].text.Normalize();
        DateTime result = new DateTime();
     F
        
      //  x = x.Remove(x.IndexOf(" GMT"));

        result = Convert.ToDateTime(x);

        Debug.Log("fight: " + result.Day + "/" + result.Month + "/" + result.Year);*/


        if (vr.isOn)
        {
            eventsenddata.@virtual = "yes";
        
        }
        else
        {
            eventsenddata.@virtual = "no";

        }
        gvr = eventsenddata.@virtual;


        //eventsenddata.personarecord = "yes";

        if (pr.isOn)
        {
            eventsenddata.personarecord = "yes";
        }
        else
        {
            eventsenddata.personarecord = "no";

        }

        gpr = eventsenddata.personarecord;
        eventsenddata.eventType = typegetter.Normalize();


        if (themegetter != null )
        {
            eventsenddata.theme = themegetter.Normalize();
        }
        else
        {

            eventsenddata.theme = "baloon";
            }
        Debug.Log("theme data: " + themegetter);



        eventsenddata.result = new Result();


        eventsenddata.result.myAgeGroup = agegroup.text.Normalize();
        eventsenddata.result.totalAgeGroup = totalagegroup.text.Normalize();
        eventsenddata.result.myGender = gender.text.Normalize();
        eventsenddata.result.totalGender = totalgender.text.Normalize();




        Debug.Log("firsttime: " + eventsenddata.time);

        



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
        for (int i = 0; i <= goalgetter; i++)
        {
            
            Goal temp = new Goal();
            if (goalscroll.content.GetChild(i).GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite.name == "yes")
            {
                temp.achieved = "yes";
                Debug.Log("YYYYEEEESSSS");
            }
            else
            {
                temp.achieved = "No";
                Debug.Log("NNNNNNOOOOOOO");
            }
            if (goalscroll.content.GetChild(i).GetChild(0).GetComponent<InputField>().text=="")
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
        guesttlistgoals=eventsenddata.goals;

       //link
        List<Link> linkgoals = new List<Link>();
        for (int i = 0; i < 12; i++)
        {
            
            if (links[i].isOn==true|| linksprites[i].activeSelf)
            {



                if (linksurl[i].text != "")
                {





                    for(int k=0;k<4;k++)
                    {
                        char a = mysecondstring[k];
                        char c = linksurl[i].text[k];
                        if (c == a)
                        {
                            if(k==3)
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

                            Debug.Log(c);

                        }
                        else
                        {
                            linksurl[i].text = "https://" + linksurl[i].text;
                            break;
                        }
                    }

                    //   char c = s[i];
                    //    Debug.Log(c);




                    Link temp = new Link();
                    temp.url = linksurl[i].text.Normalize();
                    temp.title = linktitle[i];
                    linkgoals.Add(temp);
                }

            } 
        
        }




        eventsenddata.links = linkgoals;

        guestlinkgoals = eventsenddata.links;
        if (ApiRequestGenerator.guestlogin == 0)
        {

            json2send = Newtonsoft.Json.JsonConvert.SerializeObject(eventsenddata);

            StartCoroutine(event_Upload(baseurl + "/v1/event", json2send));
        }
        else
        { guestdata(); }


        Debug.Log("date: " + eventsenddata.eventDate);

    }


    public void Active50PayWall()
    {
        // if (PlayerPrefs.GetInt("OneTimeAppear", 0) == 0)
        // {
        //     HalfpricePanel.SetActive(true);
        // }
        // else
        // {
        //     profile.SetActive(true);
        //     //SceneManager.LoadScene("4. MainScreens");
        // }
    }


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
            currentscreen = 1;
            //SceneManager.LoadScene("4. MainScreens");

            eventgetter.eventhandler = 2;
            eventgetter.instance.updatescreen = 1;
            StartCoroutine(deathh());
            GameObject obj = GameObject.Find("eventlist");

            //obj.GetComponent<recievedata>().sizeofgoal = obj.GetComponent<recievedata>().goalscroll.content.sizeDelta;



            obj.GetComponent<recievedata>().myreceivedata = new recievedata.mydata();
            if (raceno.text != null || raceno.text != "")
            {
                obj.GetComponent<recievedata>().myreceivedata.race = raceno.text;
            }
            else
            {
         
            
                obj.GetComponent<recievedata>().myreceivedata.race = "";


    
            }


        obj.GetComponent<recievedata>().myreceivedata.eventdatetoshow = eventdatetoshow;
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

        obj.GetComponent<recievedata>().myreceivedata.eventDate = gusteventdate;//datesplit[0] + "/" + datesplit[2];




			obj.GetComponent<recievedata>().myreceivedata.victoryCity = guestvictorycatchp;
            obj.GetComponent<recievedata>().myreceivedata.time =guesttime;

            obj.GetComponent<recievedata>().myreceivedata.distance.istance = guestdistance;
            obj.GetComponent<recievedata>().myreceivedata.personarecord =gpr;
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
   
            obj.GetComponent<recievedata>().getguestdata();






        

    }


    public void deathtomyself()
    {

        Debug.Log("calling is working");
        Toconvertimage();
     //  hiimage.texture = death;
    }
    public string checkstring,mystring,mysecondstring,mythirdstring;
    public void checking()
    {
        for (int j = 0; j < 12; j++)
        {


            char b = mystring[j];
            char c = checkstring[j];

            if (b == c)
            {
                Debug.Log(c);
            }
        }
    }

    IEnumerator event_Upload(string url, string bodyJsonString)
    {
        badges.instance.badgecount();

        loader.SetActive(true);
        //WWWForm form = new WWWForm();
        //Dictionary<string, string> headers = form.headers;
      //  headers["JWT TOKEN"] = apigetter.jwt;
        var request = new UnityWebRequest(url, "POST");
        request.SetRequestHeader("Authorization", "Bearer " + apigetter.jwt);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
       // request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        Debug.Log(request.downloadHandler.text);
        newsavessss myDeserializedClass = Newtonsoft.Json.JsonConvert.DeserializeObject<newsavessss>(request.downloadHandler.text);
        //eventdata2send myDeserializedClass = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(request.downloadHandler.text);
        if (request.responseCode==200)
        {
            
            eventno();
            badges.instance.badgecount();
            currentscreen = 1;
    //SceneManager.LoadScene("4. MainScreens");

            eventgetter.eventhandler = 2;
            eventgetter.instance.updatescreen = 1;
            StartCoroutine(deathh());
            StartCoroutine(delaybadges());
            int selectedYear = int.Parse(year.options[year.value].text);

            if ((time1.text == "" && time2.text == "" && time3.text == "") || selectedYear > 2026)
            {
                PlayerPrefs.SetString("DisableEventButtonId", myDeserializedClass.response.@event._id);
                PlayerPrefs.Save();
                screencurenter(1);
            }
            else
            {
                GameObject obj = GameObject.Find("eventlist");

                //obj.GetComponent<recievedata>().sizeofgoal = obj.GetComponent<recievedata>().goalscroll.content.sizeDelta;



                obj.GetComponent<recievedata>().myreceivedata = new recievedata.mydata();
                if (myDeserializedClass.response.@event.race != null || myDeserializedClass.response.@event.race != "")
                {
                    obj.GetComponent<recievedata>().myreceivedata.race = myDeserializedClass.response.@event.race;
                }
                else
                {
                    obj.GetComponent<recievedata>().myreceivedata.race = "000000";



                }

                obj.GetComponent<recievedata>().myreceivedata.eventdatetoshow = eventdatetoshow;
                obj.GetComponent<recievedata>().myreceivedata.eventName = myDeserializedClass.response.@event.eventName;
                obj.GetComponent<recievedata>().myreceivedata.position = myDeserializedClass.response.@event.position;
                obj.GetComponent<recievedata>().myreceivedata.totalpeople = myDeserializedClass.response.@event.totalpeople;

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

                obj.GetComponent<recievedata>().myreceivedata.eventDate = myDeserializedClass.response.@event.eventDate;//datesplit[0] + "/" + datesplit[2];



				obj.GetComponent<recievedata>().myreceivedata.victoryCity = myDeserializedClass.response.@event.victoryCity;
                obj.GetComponent<recievedata>().myreceivedata.time = myDeserializedClass.response.@event.time;

                obj.GetComponent<recievedata>().myreceivedata.distance.istance = myDeserializedClass.response.@event.distance.istance;
                obj.GetComponent<recievedata>().myreceivedata.personarecord = myDeserializedClass.response.@event.personarecord;
                obj.GetComponent<recievedata>().myreceivedata.distance.unit = myDeserializedClass.response.@event.distance.unit;
                obj.GetComponent<recievedata>().myreceivedata.@virtual = myDeserializedClass.response.@event.@virtual;
                if (myDeserializedClass.response.@event.image != "")
                {
                    Debug.Log("image data: " + myDeserializedClass.response.@event.image);
                    obj.GetComponent<recievedata>().myreceivedata.image = myDeserializedClass.response.@event.image;
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

                obj.GetComponent<recievedata>().myreceivedata.personarecord = myDeserializedClass.response.@event.personarecord;
                obj.GetComponent<recievedata>().myreceivedata.achivement = myDeserializedClass.response.@event.achivement;
                obj.GetComponent<recievedata>().myreceivedata.user = myDeserializedClass.response.@event.user;
                obj.GetComponent<recievedata>().myreceivedata.createdAt = myDeserializedClass.response.@event.createdAt;
                obj.GetComponent<recievedata>().myreceivedata.updatedAt = myDeserializedClass.response.@event.updatedAt;
                obj.GetComponent<recievedata>().myreceivedata.__v = myDeserializedClass.response.@event.__v;


                recievedata eventCardData = obj.GetComponent<recievedata>();
                eventCardData.getdata();


                
            }
        
        
        }
        else
        {
            loader.SetActive(false);
        }
       
    }

    IEnumerator deathh()
    {
        yield return new WaitForSeconds(1);

        eventgetter.instance.openevent();
        loader.SetActive(false);
    }
    IEnumerator delaybadges()
    {
       

        yield return new WaitForSeconds(1);
        badges.instance.badgescrollera();


    }

    //62ebf7dcb4e20ce7938d1fccs

    public class EVENtreciver
    {
        public string id { get; set; }

    }


    public void sortname()
    {

        Debug.Log("get event from sortname");
        // StopAllCoroutines();

        for (int i=0;i<eventalbum.Length;i++)
        {
            eventalbum[i].GetComponent<Button>().interactable = false;
        }
        
        sortevent = 0;
        PlayerPrefs.SetInt("sort",sortevent);
        PlayerPrefs.Save();
        destroyevent();

        StartCoroutine(namee());
    }

    public void sortdate()
    {
        Debug.Log("get event from sortdate");
        for (int i = 0; i < eventalbum.Length; i++)
        {
            
            eventalbum[i].GetComponent<Button>().interactable = false;
        }
        sortevent = 1;

        PlayerPrefs.SetInt("sort", sortevent);
        PlayerPrefs.Save();
        destroyevent();
        StartCoroutine(getecentdataa());
    }
    public void sortcity()
    {
        Debug.Log("get event from sortcity");
        for (int i = 0; i < eventalbum.Length; i++)
        {
            Debug.Log("get event");
            eventalbum[i].GetComponent<Button>().interactable = false;
        }
        sortevent = 2;

        PlayerPrefs.SetInt("sort", sortevent);
        PlayerPrefs.Save();
        destroyevent();
        StartCoroutine(geteventdatacityy());

    }
    public void sorttype()
    {
        Debug.Log("get event from sorttype");
        for (int i = 0; i < eventalbum.Length; i++)
        {
            Debug.Log("get event");
            eventalbum[i].GetComponent<Button>().interactable = false;
        }


        sortevent = 3;

        PlayerPrefs.SetInt("sort", sortevent);
        PlayerPrefs.Save();
        destroyevent();
        StartCoroutine(geteventdatatypee());
    }


    IEnumerator namee()
    {
        yield return new WaitForSeconds(0.5f);


        geteventdataName();
        yield return new WaitForSeconds(0.3f);
    


    }


    IEnumerator getecentdataa()
    {
        yield return new WaitForSeconds(0.5f);


        getecentdata();


        yield return new WaitForSeconds(0.3f);
    
    }



    IEnumerator geteventdatacityy()
    {
        yield return new WaitForSeconds(0.5f);


        geteventdatacity();


        yield return new WaitForSeconds(0.3f);
     
    }

    IEnumerator geteventdatatypee()
    {
        yield return new WaitForSeconds(0.5f);
        geteventdatatype();



        yield return new WaitForSeconds(0.3f);
    

    }



    public void eventno()
    {
        EVENtreciver getdata = new EVENtreciver();
        getdata.id = apigetter.id;
        Debug.Log(apigetter.id);
        String JsonVal = Newtonsoft.Json.JsonConvert.SerializeObject(getdata);
        Debug.Log("json val " + JsonVal);
        StartCoroutine(getevenetnumber(baseurl + "/v1/event/user?type=des&sort=eventDate", ""));

    }

    IEnumerator getevenetnumber(string url, string bodyJsonString)
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

      
        Root myDeserializedClass = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(request.downloadHandler.text);
     //   Debug.Log(myDeserializedClass.response.Count);
        eventcount.text = "" + myDeserializedClass.response.Count;
    }





    public void geteventlist()
    {
        Debug.Log("get event from geteventlist");
        if (freetrial == 1 && signuppaid == 0)
        {
            unsubscribeduser();
        }
        else
        {
            if (sortevent == 0)
            {
                destroyevent();
                StartCoroutine(namee());
            }
            else if (sortevent == 1)
            {
                destroyevent();
                StartCoroutine(getecentdataa());
            }
            else if (sortevent == 2)
            {
                destroyevent();
                StartCoroutine(geteventdatacityy());

            }
            else if (sortevent == 3)
            {
                destroyevent();

                StartCoroutine(geteventdatatypee());
            }

            Debug.Log("sortnumber: "+sortevent);


        }
        EventScreenAlphaDelay();
    }


    public void getecentdata()
    {
        EVENtreciver getdata = new EVENtreciver();
        getdata.id = apigetter.id;
        Debug.Log(apigetter.id);
        String JsonVal = Newtonsoft.Json.JsonConvert.SerializeObject(getdata);
        Debug.Log("json val "+JsonVal);
            StartCoroutine(getevetdata2(baseurl + "/v1/event/user?type=des&sort=eventDate", ""));
    }
    
    
    public void geteventdatacity()
    {
        EVENtreciver getdata = new EVENtreciver();
        getdata.id = apigetter.id;
        Debug.Log(apigetter.id);
        String JsonVal = Newtonsoft.Json.JsonConvert.SerializeObject(getdata);
        Debug.Log("json val " + JsonVal);
        StartCoroutine(getevetdata2(baseurl + "/v1/event/user?type=asc&sort=city", ""));
    }

    public void geteventdataName()
    {
        EVENtreciver getdata = new EVENtreciver();
        getdata.id = apigetter.id;
        Debug.Log(apigetter.id);
        String JsonVal = Newtonsoft.Json.JsonConvert.SerializeObject(getdata);
        Debug.Log("json val " + JsonVal);
        StartCoroutine(getevetdata2(baseurl + "/v1/event/user?type=asc&sort=eventName", ""));
    }
    public void geteventdatatype()
    {
        EVENtreciver getdata = new EVENtreciver();
        getdata.id = apigetter.id;
        Debug.Log(apigetter.id);
        String JsonVal = Newtonsoft.Json.JsonConvert.SerializeObject(getdata);
        Debug.Log("json val " + JsonVal);
        StartCoroutine(getevetdata2(baseurl + "/v1/event/user?type=asc&sort=eventType", ""));
    }


    IEnumerator getevetdata2(string url, string bodyJsonString)
    {
        eventscroll.verticalNormalizedPosition = 1f;
        loader.SetActive(true);
        var request = new UnityWebRequest(url, "POST");
        request.SetRequestHeader("Authorization", "Bearer " + apigetter.jwt);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        //  Debug.Log("value: " + bodyJsonString);
        //    request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);

        Debug.Log("get eventdata" + request.downloadHandler.text);
        Root myDeserializedClass = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(request.downloadHandler.text);
        Debug.Log(myDeserializedClass.response.Count);

        loader.SetActive(false);
        for (int i = 0; i < myDeserializedClass.response.Count; i++)
        {
            for (int z = 0; z < eventalbum.Length; z++)
            {
                Debug.Log("get event");
                eventalbum[z].GetComponent<Button>().interactable = true;
            }


            GameObject obj = Instantiate(event2spawn, Eventscrol.content);




            string disableId = PlayerPrefs.GetString("DisableEventButtonId", "");
            bool disableSavedEvent = myDeserializedClass.response[i]._id == disableId;
            bool disableFutureEvent = IsFutureYearEvent(myDeserializedClass.response[i].eventDate);

            if (disableSavedEvent || disableFutureEvent)
            {
                SetEventCardButtonsInteractable(obj, false);
            }

            if (disableSavedEvent)
            {
                PlayerPrefs.DeleteKey("DisableEventButtonId");
                PlayerPrefs.Save();
            }


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


           obj.GetComponent<MyEventData>().myeventclass.id = myDeserializedClass.response[i]._id;

            // string disableId = PlayerPrefs.GetString("DisableEventButtonId", "");

            // if (myDeserializedClass.response[i]._id == disableId)
            // {
            //     Button btn = obj.GetComponentInChildren<Button>(true);

            //     if (btn != null)
            //     {
            //         btn.interactable = false;
            //     }

            //     PlayerPrefs.DeleteKey("DisableEventButtonId");
            //     PlayerPrefs.Save();
            // }


            obj.GetComponent<MyEventData>().myeventclass.@virtual = myDeserializedClass.response[i].@virtual;
            if (myDeserializedClass.response[i].image != "")
            {
                Debug.Log("image captured");
                obj.GetComponent<MyEventData>().myeventclass.image = myDeserializedClass.response[i].image;
            }
            obj.GetComponent<MyEventData>().myeventclass.theme = myDeserializedClass.response[i].theme;























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





        }

        if (checksize == 0)
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
    }
    }








    private bool IsFutureYearEvent(string eventDate)
    {
        if (string.IsNullOrEmpty(eventDate))
        {
            return false;
        }

        DateTime parsedDate;
        if (DateTime.TryParse(eventDate, out parsedDate))
        {
            return parsedDate.Year > 2026;
        }

        string[] dateParts = eventDate.Split(new[] { '/', '-', '.', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = dateParts.Length - 1; i >= 0; i--)
        {
            int year;
            if (dateParts[i].Length == 4 && int.TryParse(dateParts[i], out year))
            {
                return year > 2026;
            }
        }

        return false;
    }

    private void SetEventCardButtonsInteractable(GameObject eventCard, bool interactable)
    {
        if (eventCard == null)
        {
            return;
        }

        Button button = eventCard.GetComponentInChildren<Button>(true);
        if (button != null)
        {
            button.interactable = interactable;
        }
    }

    public class Response
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
    }

    public class Root
    {
        public bool success { get; set; }
        public List<Response> response { get; set; }
        public object metadata { get; set; }
    }

public   int  checkcounter = 0;

    public void Toconvertprofileimage()
    {
        // RawImage newuserphoto = null;
        // Texture2D newphoto = new Texture2D(1, 1);
        //  newphoto.LoadImage(Convert.FromBase64String("string"));

     //   Texture2D rawImageTexture = (Texture2D)profiletexture;
    ///    profileimagetext = Convert.ToBase64String(rawImageTexture.EncodeToJPG());
      //  Debug.Log("image: " + profileimagetext);
  //      PlayerPrefs.SetString("profileimage", profileimagetext);
//        PlayerPrefs.Save();

    }

    public void sendconverttextprofile()
    {
      /*  float width = Screen.width;
        float height = Screen.height;

        Texture2D newphoto = new Texture2D((int)1, (int)1);
       
        newphoto.LoadImage(Convert.FromBase64String(profileimagetext));
        float aspctratio = width / height;
        Debug.Log(aspctratio);
      //  hiimage.GetComponent<AspectRatioFitter>().aspectRatio = aspctratio;
     //  profileimage.GetComponent<AspectRatioFitter>().aspectRatio = aspctratio;
        //    profileimage.GetComponent<AspectRatioFitter>().aspectRatio = aspctratio;
        newphoto.Apply();

     //   hiimage.texture = newphoto;
        profileimage.texture = newphoto;
      */

    }













    //my testing area


    public void sendcameratext()
    {
        // RawImage newuserphoto = null;
        // Texture2D newphoto = new Texture2D(1, 1);
        //  newphoto.LoadImage(Convert.FromBase64String("string"));
        if (ApiRequestGenerator.guestlogin == 0)
        {
            Debug.Log("checking up on this");
            StartCoroutine(Uploaad());
        }
        else
        {
            newstring = "asd";



        }
        Texture2D rawImageTexture = eventimgetexture;

        //   eventImage = Convert.ToBase64String(rawImageTexture.EncodeToJPG());

        float width = Screen.width;
        float height = Screen.height;
        float aspctratio = width / height;
        Debug.Log(aspctratio);




        //  eventcurrentimage.GetComponent<AspectRatioFitter>().aspectRatio = aspctratio;


        float ratop = eventimgetexture.width / eventimgetexture.height;

        if (ratop == 0)
        {
            ratop = 1;
        }
        Debug.Log("aspect ratio: " + ratop);




        eventcurrentimage.GetComponent<AspectRatioFitter>().aspectRatio = 1;

        eventcurrentimage.texture = eventimgetexture;




    }
    public string newstring;
    public void sendconverttext()
    {
        // RawImage newuserphoto = null;
        // Texture2D newphoto = new Texture2D(1, 1);
        //  newphoto.LoadImage(Convert.FromBase64String("string"));
        if (ApiRequestGenerator.guestlogin == 0)
        {
            Debug.Log("checking up on this");
            StartCoroutine(Uploaad());
        }
        else
        {
            newstring = "asd";



        }
        Texture2D rawImageTexture = eventimgetexture;

     //   eventImage = Convert.ToBase64String(rawImageTexture.EncodeToJPG());

        float width = Screen.width;
        float height = Screen.height;
        float aspctratio = width / height;
        Debug.Log(aspctratio);




      //  eventcurrentimage.GetComponent<AspectRatioFitter>().aspectRatio = aspctratio;


        float ratop = eventimgetexture.width / eventimgetexture.height;

        if(ratop==0)
        {
            ratop = 1;
        }



        if (eventimgetexture.width > eventimgetexture.height)
        {
            Debug.Log("aspect ratio: " + ratop);
            eventcurrentimage.GetComponent<AspectRatioFitter>().aspectRatio = 1;
        
        }
        else
        {
            eventcurrentimage.GetComponent<AspectRatioFitter>().aspectRatio = 1;

        }
        eventcurrentimage.texture = eventimgetexture;
    



    }

    public void Toconvertimage()
    {
     
        
        
        // RawImage newuserphoto = null;
        // Texture2D newphoto = new Texture2D(1, 1);
        //  newphoto.LoadImage(Convert.FromBase64String("string"));

        Texture2D rawImageTexture = (Texture2D)death;
        imagedata = Convert.ToBase64String(rawImageTexture.EncodeToJPG());
        Debug.Log("image: " + imagedata);
        PlayerPrefs.SetString("image", imagedata);
        PlayerPrefs.Save();




    }


    public void SubscriptionDeActivate()
    {
        PlayerPrefs.SetInt("LogOutSubscription", 0);
        PlayerPrefs.SetInt("OneTimeAppear", 1);
        PlayerPrefs.SetInt("ProfileShownOnce", 1);
    }


    private void Update()
    {

        //Debug.Log("I am able to go inside the eventgertter update");
        victorylimit = victorycatchphrase.text.Normalize();
        victoylimittext.text = victorylimit.Length + "/" + "12";

        if (FirebaseMsg.firebasecheck == 1)
        { 
        
            clubbadge.SetActive(true);
			clubbadge2.SetActive(true);

		}

		if (FirebaseMsg.codebadge2 == 1)
		{

			clubbadge.SetActive(true);

			clubbadge3.SetActive(true);
		}
		if (FirebaseMsg.codebadge3 == 1)
		{

			clubbadge.SetActive(true);

			clubbadge1.SetActive(true);
		}
        //if (PlayerPrefs.GetInt("LogOutSubscription", 0) == 1)
        //{
        //    freetrialsubscription.SetActive(true);

        //    profile.SetActive(true);
        //    Debug.Log("HIIIIIIIIIIIIIIIIIIIIIIIIIIII");

        //}

        //if (PlayerPrefs.GetInt("OneTimeAppear", 0) == 1)
        //{
        //    freetrialsubscription.SetActive(false);
        //}
        //else
        //{
        //    freetrialsubscription.SetActive(true);
        //}

    }

	public void  fromconvertimage()
    {

        // RawImage newuserphoto = null;
       float width = Screen.width;
        float height = Screen.height;

        Texture2D newphoto = new Texture2D((int) 1,(int) 1);
      
        newphoto.LoadImage(Convert.FromBase64String(imagedata));
        float aspctratio = newphoto.height/newphoto.width;
        if(aspctratio==0)
        {
            aspctratio = 1.25f;
        }
        Debug.Log(aspctratio);



        hiimage.GetComponent<AspectRatioFitter>().aspectRatio = aspctratio;
     //   profileimage.GetComponent<AspectRatioFitter>().aspectRatio = aspctratio;
       //    profileimage.GetComponent<AspectRatioFitter>().aspectRatio = aspctratio;
        newphoto.Apply();

        hiimage.texture = newphoto;
     //   profileimage.texture = newphoto;



    }

    IEnumerator Uploaad()
    {
        save.interactable = false;
        WWWForm form = new WWWForm();

        // eventimgetexture.isReadable =    true;
        byte[] bytes = eventimgetexture.EncodeToJPG();
        form.AddBinaryData("image", bytes,imagepath);

        // Dictionary<string, string> headers = form.headers;
        // headers["JWT TOKEN"] = apigetter.jwt;
        UnityWebRequest www = UnityWebRequest.Post(baseurl+ "/v1/user/upload-image", form);
        // www.SetRequestHeader("Authorization", "Bearer " + apigetter.jwt);
        // www.SetRequestHeader("Content-Type", "application/json");
        // byte[] myData = System.Text.Encoding.UTF8.GetBytes("This is some test data");


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

         eventImage =  baseurl+"/"+  myDeserializedClass.response.url;
            Debug.Log("eventimage 2:" + myDeserializedClass.response.url);
            save.interactable = true;


        }
    }

    IEnumerator GetTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(apigetter.profile);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
         //   Texture2D newphoto = new Texture2D((int)1, (int)1);
         //   newphoto.LoadImage(Convert.FromBase64String(profileimagetext));
            float aspctratio = myTexture.width /myTexture.height;
            if(aspctratio==0)
            {
                aspctratio = 0.5f;
            }
            float aspectw = Screen.width;
            float aspecth =Screen.height;
            float aspect;
            aspect = aspectw / aspecth;
            Debug.Log(aspctratio);
            if (myTexture.width > myTexture.height)
            {
                Debug.Log("here");
                profileimage.GetComponent<AspectRatioFitter>().aspectRatio = aspctratio;
            }
            else
            {
                Debug.Log("there");
                profileimage.GetComponent<AspectRatioFitter>().aspectRatio = aspctratio;
            }
         //     profileimage.GetComponent<AspectRatioFitter>().aspectRatio = aspctratio;
         //     profileimage.GetComponent<AspectRatioFitter>().aspectRatio = aspctratio;
         //     newphoto.Apply();
         //     hiimage.texture = newphoto;
            profileimage.texture = myTexture;
        }
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


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
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
        public string victoryCity { get; set; }
        public string time { get; set; }
        public nDistances distance { get; set; }
        public string @virtual { get; set; }
        public string image { get; set; }
        public string race { get; set; }
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

}

[System.Serializable]
public class ListOfDataSaved 
{
    public List<DataToSave> dataToSave = new List<DataToSave>();
}

[System.Serializable]
public class DataToSave
{
    public int yearToCheck;
    public string monthToCheck;
}
