using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LottiePlugin.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.PlayerLoop.EarlyUpdate;
//using AdvancedInputFieldPlugin;
public class recievedata : MonoBehaviour
{
    public GameObject confetti;
    public GameObject confetti2D;
    public float confettiDeactivateDelay = 4.2f;
    public TMP_InputField pree, postt, nextt, racee;

    // public AdvancedInputField ad,racetext,nexttext,postext;// = new AdvancedInputField();
    /*   public ScrollArea prescroll;
       public ScrollArea postscroll;
       public ScrollArea racescroll;
       public ScrollArea nexscroll;
    */
    public Toggle faceoff;
    public GameObject arpopup;
    public updataeven update; 
    public  string eventdatetoshow;
    public Text Eventname;
    public Text Date;
    public Text firstname;
    public Text racerno;
    public Text victorycatchphrase;
    public Text time, distance, unit,city;
    public GameObject goalsobject, mediaobject, portalobject, journalobject;
    public Text goal, agegroup, totalagegroup, gender, totalgender, prenotes, title, race, post, next, notes, result,totalpeople,position;
    public string eventImage;
    public RawImage hiimage;
    public Text type, theme;
    public int goalgetter;
    public Texture dd;
    public string pot, garmin, strava, reddit, youtube, instagram, tiktok, facebook, athlinks, custom1, custom2, custom3;
    // public Text vr, pr;

    //goals sent data
    public LayoutElement em;
    public RectTransform text;
    float size;
    public GameObject gaolspawn,gendeer,totalgendeer;
    public ScrollRect goalscroll;
  public  Vector2 sizeofgoal;
    public RectTransform goaltranform;
    
    //link sent data
    public GameObject linkspawn;
    Vector2 sizeoflink;
    Vector2 originalsize;
    public ScrollRect linkscroll;
    public RectTransform linktransform;
    public Transform linkcontent;
    public int spaxingsize = 25;
    public int countoflinks = 1;
    public int countofgoals = 1;
    public ScrollRect parentscroll;

    public Image virtuaal, personalr;


    public Sprite vrhome, prloc,right,wrong,wrong2,vrsecond,prsecond;
    public arhandler arhandle;
    public GameObject disneybadge,spartan1,spartan2,spartan3;
    public Button[] links;
    public string[] linksurl;

    public static int eventheme;
   private Scene scene;



  
    public turnoffon turn;
    public MicrophonePermissionManager micro;
    public GameObject[] toturnon;
    public GameObject recordbutton;



    public Animator baloonanim;






















    private void Start()
    {
         scene = SceneManager.GetActiveScene();
        eventheme = 0;
        sizeofgoal = goalscroll.content.sizeDelta;
        sizeoflink = goalscroll.content.sizeDelta;
        //  Debug.Log("my reason: " + myreceivedata.result.myGender);
        originalsize = parentscroll.content.sizeDelta;
        originalparentscroll = parentscroll.content.sizeDelta;
        orignalgaolsizedelata = sizeofgoal;
    }
    public void scrollreset()
    {
        parentscroll.verticalNormalizedPosition = 1f;
        //        int c = myDeserializedClass.response.Count;
        //    GameObject obj = Instantiate(gaolspawn, goalscroll.content);
        //gaolspawn.transform.SetParent(goaltranform);

        for (int i=0;i<goalscroll.content.childCount;i++)
        {
            Destroy(goalscroll.content.GetChild(i).gameObject);
        }
        goalsobject.GetComponent<ExpandOnClick>().checkingopenGOAL();

        countofgoals = 1;
        //.SetActive(false);
        portalobject.GetComponent<ExpandOnClick>().checkingopen();
        journalobject.GetComponent<ExpandOnClick>().checkingopen();
        mediaobject.GetComponent<ExpandOnClick>().checkingopen();
        sizeofgoal = orignalgaolsizedelata;
       // parentscroll.content.sizeDelta = new Vector2(parentscroll.content.sizeDelta.x, 3500);
        //originalparentscroll;
        goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight = 180f;
        goaltranform.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(goaltranform.gameObject.GetComponent<RectTransform>().sizeDelta.x, goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight);

        goalscroll.content.sizeDelta = sizeofgoal;
        goalscroll.GetComponent<RectTransform>().sizeDelta = sizeofgoal;
        goalscroll.viewport.sizeDelta = sizeofgoal;
      //  goalsobject.GetComponent<ExpandOnClick>().checkingopenGOAL();


        parentscroll.content.sizeDelta = new Vector2(parentscroll.content.sizeDelta.x, 3540);
        Debug.Log("goals: " + goalgetter);
    }





    public void updategetimagefromtext()
    {
        if (myreceivedata.image != null)
        {
            /*  Texture2D newphoto = new Texture2D(1, 1);

              newphoto.LoadImage(Convert.FromBase64String(myreceivedata.image));
              newphoto.Apply();*/
            float width = Screen.width;
            float height = Screen.height;
            float aspctratio = width / height;



        float updatecheck=    hiimage.texture.width / hiimage.texture.height;
            if(updatecheck==0)
            {
                updatecheck =1f;
            }

            if (hiimage.texture.width > hiimage.texture.height)
            {
                update.hiimage.GetComponent<AspectRatioFitter>().aspectRatio = 1;
            }
            else
            {

                update.hiimage.GetComponent<AspectRatioFitter>().aspectRatio = 1;

            }

            update.hiimage.texture = hiimage.texture;
           
        }
    }





    public void medalar()
    {
        arhandler.selectar = 1;
        newarbutton();
        arhandle.checkARchoice();
        arhandle.artextdisplay[1].SetActive(true);



       


            spartanbadge.instance.spartan1 = Instantiate(AddressableHandler.Instance.currentModels[8], spartanbadge.instance.transform.position, Quaternion.identity);
            spartanbadge.instance.spartan1.transform.SetParent(spartan1.transform);

            spartanbadge.instance.spartan2 = Instantiate(AddressableHandler.Instance.currentModels[8], spartanbadge.instance.transform.position, Quaternion.identity);
            spartanbadge.instance.spartan2.transform.SetParent(spartan2.transform);

            spartanbadge.instance.spartan3 = Instantiate(AddressableHandler.Instance.currentModels[8], spartanbadge.instance.transform.position, Quaternion.identity);
            spartanbadge.instance.spartan3.transform.SetParent(spartan3.transform);


        




        for (int i = 0; i < arhandle.imagetarget.Length; i++)
        {
            if (i == 1)
            {
             //   arhandle.imagetarget[i].SetActive(true);
            }
            else
            {
             //   arhandle.imagetarget[i].SetActive(false);

            }

        }

    }


    public void Plainar()
    {
        arhandler.selectar = 0;
        newarbutton();

        arhandle.checkARchoice();
       
    }

    public void newarbutton()
    {

        turn.turnaron();
        micro.askforpermission();


        for (int i = 0; i < toturnon.Length; i++)
        {

            toturnon[i].SetActive(true);
        
        }
        turn.circel();
        recordbutton.SetActive(false);
    
    
    }
	public string[] eventnamechecker;
	public void scenechange()
    {
		if (myreceivedata.theme != "")
		{
			ardata.artheme = myreceivedata.theme;
		}
		else
		{
			ardata.artheme = "baloon";

		}
		if (AddressableHandler.Instance != null)
		{
			AddressableHandler.Instance.SelectFaceModelForTheme(ardata.artheme);
		}
		eventgetter.archeck = 1;

        faceoff.interactable = true;
        faceoff.isOn = true;
        ardata.ename = myreceivedata.eventName;
        ardata.edistance = myreceivedata.distance.istance;
        ardata.edistenaceunit= myreceivedata.distance.unit;
        ardata.etime = myreceivedata.time;
        ardata.victoryphrase = myreceivedata.victoryCity;
        Debug.Log("theme: "+myreceivedata.theme+ " "+myreceivedata.eventName);
        if (myreceivedata.theme == "medaldisney")
        {
            arhandler.selectar = 1;
            Debug.Log("my check theme: " + myreceivedata.theme + " " + myreceivedata.eventName);
            newarbutton();
           
            arhandle.checkARchoice();

            arhandle.artextdisplay[0].SetActive(true);
            for (int i = 0; i < arhandle.imagetarget.Length; i++)
            {
                if (i == 0)
                {
                  //  arhandle.imagetarget[i].SetActive(true);   
                }
                else
                {// arhandle.imagetarget[i].SetActive(false); 
                
                }
            
            }
            if (AddressableHandler.Instance.isModelDownloaded[0])
            {
            
                
                //getvuforiatarget.instance.target =    Instantiate(AddressableHandler.Instance.currentModels[0], getvuforiatarget.instance.transform.position, Quaternion.identity);
                //getvuforiatarget.instance.target.transform.SetParent(disneybadge.transform);
            
            
            }

        } 
     /*   else if (myreceivedata.theme == "fire")
            {
			eventnamechecker = Eventname.text.Split(char.Parse(" "));

			Debug.Log("my check theme: " + myreceivedata.theme + " " + myreceivedata.eventName);

           

            for (int i = 0; i < eventnamechecker.Length; i++)
            {
                if(eventnamechecker[i].ToLower() =="spartan") 
                {
					arpopup.SetActive(true);

				
					return;

				}

			}

			arhandler.selectar = 0;
			newarbutton();
			arhandle.checkARchoice();





		}*/
		else
            {

                arhandler.selectar = 0;
            newarbutton();
                arhandle.checkARchoice();
            }
          
        
       
       //  SceneManager.LoadScene("test");
    }





    //here, splitArray[0] = Give; splitArray[1] = me etc...
    public string example;
    
string[] textSplit ;

    public void edit()
    {

        EventGalleryPicker.Instance?.PrepareForUpdate(
            myreceivedata.galleryImages);

        update.totalpeople.text = myreceivedata.totalpeople;
        update.position.text = myreceivedata.position;
        update.eventImage = myreceivedata.image;
        update.Eventname.text = myreceivedata.eventName;
        update.date.text = eventdatetoshow;//myreceivedata.eventDate;
        //  example = myreceivedata.eventDate;
         textSplit = myreceivedata.time.Split(char.Parse(":"));

        // for (int i = 0; i < textSplit.Length; i++)
        //  {
        ///    Debug.Log("my data: "+textSplit[i]);
        //       }




        update.themegetter = myreceivedata.theme;

        update.cityname.text = myreceivedata.city;
        update.victorycatchphrase.text = myreceivedata.victoryCity;
        
        
        update.time1.text = textSplit[0];
        update.time2.text = textSplit[1];
        update.time3.text = textSplit[2];
        update.timecomplete.text = "" + textSplit[0] + textSplit[1] + textSplit[2];

        update.time1.DeactivateInputField(); 

        update.time2.DeactivateInputField();
        update.time3.DeactivateInputField();


        //Debug.Log("timer " + update.time.text);
        textSplit = myreceivedata.distance.istance.Split(char.Parse("."));
        update.d1.text = textSplit[0];
        update.d2.text = textSplit[1];
        update.d3.text = textSplit[2];

        update.distancecomplete.text =""+ textSplit[0] + textSplit[1] + textSplit[2];
        update.d1.DeactivateInputField();
       
        update.d2.DeactivateInputField();
        update.d3.DeactivateInputField();



        // update.distance.text = myreceivedata.distance.istance;
        update.themegetter = myreceivedata.theme;
        update.type = myreceivedata.eventType;

        update.time3.enabled = false;
        update.time3.enabled = true;

        Debug.Log("event date to show for update is : " + myreceivedata.eventDate);

        //textSplit = myreceivedata.eventdatetoshow.Split(char.Parse("/"));
        textSplit = myreceivedata.eventDate.Split(char.Parse("/"));



            update.dropdate.value = int.Parse(textSplit[1]);

            update.dropmonth.value = int.Parse(textSplit[0]);

        for (int i=0; i<update.dropyear.options.Count; i++)
        {
           
            if (textSplit[2] == update.dropyear.options[i].text)
            {
                    
                Debug.Log("drop year is called");
                update.dropyear.value = i;
            }

          
        }



        if (myreceivedata.distance.unit == "mi")
        {
            update.distanceunit.isOn = true;
        }
        else
        {
            update.distanceunit.isOn = false;

        }
        if(myreceivedata.@virtual=="yes")
        {

            update.vr.isOn = true;

        }
        else
        {
            update.vr.isOn = false;
        }
        if(myreceivedata.personarecord=="yes")
        {
            update.pr.isOn = true;
        }
        else
        {
            update.pr.isOn = false;
        }
        updategetimagefromtext();




        if (myreceivedata.theme == "clover")
        {
            for (int i = 0; i < update.themes.Length; i++)
            {
                if (i == 1)
                {
                    update.themes[i].SetActive(true);
                           }
                else
                {
                    update.themes[i].SetActive(false);
                }
                    
            }
        }

        if (myreceivedata.theme == "winter")
        {
            for (int i = 0; i < update.themes.Length; i++)
            {
                if (i == 2)
                {
                    update.themes[i].SetActive(true);
                }
                else
                {
                    update.themes[i].SetActive(false);
                }

            }
        }


        if (myreceivedata.theme == "fire")
        {
            for (int i = 0; i < update.themes.Length; i++)
            {
                if (i == 3)
                {
                    update.themes[i].SetActive(true);
                }
                else
                {
                    update.themes[i].SetActive(false);
                }

            }
        }

        if (myreceivedata.theme == "baloon")
        {
            for (int i = 0; i < update.themes.Length; i++)
            {
                if (i == 0)
                {
                    update.themes[i].SetActive(true);
                }
                else
                {
                    update.themes[i].SetActive(false);
                }

            }
        }

        if (myreceivedata.theme == "patrik")
        {
            for (int i = 0; i < update.themes.Length; i++)
            {
                if (i == 4)
                {
                    update.themes[i].SetActive(true);
                }
                else
                {
                    update.themes[i].SetActive(false);
                }

            }
        }
        if (myreceivedata.theme == "usa")
        {
            for (int i = 0; i < update.themes.Length; i++)
            {
                if (i == 5)
                {
                    update.themes[i].SetActive(true);
                }
                else
                {
                    update.themes[i].SetActive(false);
                }

            }
        }
        if (myreceivedata.theme == "halloween")
        {
            for (int i = 0; i < update.themes.Length; i++)
            {
                if (i == 7)
                {
                    update.themes[i].SetActive(true);
                }
                else
                {
                    update.themes[i].SetActive(false);
                }

            }
        }
        if (myreceivedata.theme == "thanksgiving")
        {
            for (int i = 0; i < update.themes.Length; i++)
            {
                if (i == 8)
                {
                    update.themes[i].SetActive(true);
                }
                else
                {
                    update.themes[i].SetActive(false);
                }

            }
        }
        if (myreceivedata.theme == "valentine")
        {
            for (int i = 0; i < update.themes.Length; i++)
            {
                if (i == 10)
                {

                    update.themes[i].SetActive(true);

                }
                else
                {

                    update.themes[i].SetActive(false);

                }

            }
		}
		if (myreceivedata.theme == "christmas")
		{
			for (int i = 0; i < update.themes.Length; i++)
			{
				if (i == 9)
				{

					update.themes[i].SetActive(true);

				}
				else
				{

					update.themes[i].SetActive(false);

				}

			}
		}
		if (myreceivedata.theme == "medaldisney")
        {
           // Debug.Log("theme 1: " + myreceivedata.theme + " " + update.themes.Length);
            for (int i = 0; i < update.themebuttons.Length; i++)
            {
                if (i == 6)
                {
                //    Debug.Log("theme: " + myreceivedata.theme);
                    update.ar100medal[0].SetActive(true);
                    update.themebuttons[i].SetActive(true);
                }
                else
                {
                    update.themebuttons[i].SetActive(false);
                }

            }
        }
        //goals


        if (myreceivedata.goals != null)
        {
            update.goalscroll.content.GetComponent<VerticalLayoutGroup>().enabled = false;

            goalgetter++;
            countofgoals++;
            for (int k = 0; k < myreceivedata.goals.Count; k++)
            {
                update.countofgoals++;
                GameObject goalobj = Instantiate(update.gaolspawn, update.goalscroll.content);


                if (k == 0)
                {
                    update.goalscroll.content.GetChild(0).GetChild(2).gameObject.SetActive(false);
                }

                if (myreceivedata.goals[k].achieved == "yes")
                {
                    Debug.Log("Lalal lala la lale oo");
                    update.goalscroll.content.GetChild(k).GetChild(1).GetComponent<imagechange>().check = true;
                    update.goalscroll.content.GetChild(k).GetChild(1).GetChild(0).GetComponent<Image>().sprite = right;
                }
                else
                {
                    update.goalscroll.content.GetChild(k).GetChild(1).GetChild(0).GetComponent<Image>().sprite = wrong;
                }


                if (myreceivedata.goals[k].goal == "no description")
                {

                    update.goalscroll.content.GetChild(k).GetChild(0).GetComponent<InputField>().text = "";
                }
                else
                {
                    update.goalscroll.content.GetChild(k).GetChild(0).GetComponent<InputField>().text = myreceivedata.goals[k].goal;
                }
            }

            RectTransform cre;
            cre = goalscroll.GetComponent<RectTransform>();
            cre.sizeDelta = new Vector2(cre.sizeDelta.x, cre.sizeDelta.y + update.sizeofgoal.y * update.countofgoals);
            update.goalscroll.GetComponent<RectTransform>().sizeDelta = cre.sizeDelta;
            update.goalscroll.viewport.sizeDelta = cre.sizeDelta;
            update.goalscroll.content.sizeDelta = new Vector2(update.sizeofgoal.x, update.sizeofgoal.y * update.countofgoals);
            float temp = update.sizeofgoal.y * update.countofgoals;
            if (update.countofgoals < 2)
            {
                update.goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight = temp ;

            }else
            {
                update.goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight =100+ temp;
            }
            update.goaltranform.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(update.goaltranform.gameObject.GetComponent<RectTransform>().sizeDelta.x, update.goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight);
           
           // update.parentscroll.content.sizeDelta = new Vector2(update.parentscroll.content.sizeDelta.x, update.parentscroll.content.sizeDelta.y + temp);

            


         /*   update.goalscroll.content.GetComponent<VerticalLayoutGroup>().enabled = false;
            RectTransform cre;
            cre = goalscroll.GetComponent<RectTransform>();
            cre.sizeDelta = new Vector2(cre.sizeDelta.x, cre.sizeDelta.y + update.sizeofgoal.y);
            update.goalscroll.GetComponent<RectTransform>().sizeDelta = cre.sizeDelta;
            update.goalscroll.viewport.sizeDelta = cre.sizeDelta;
            update.goalscroll.content.sizeDelta = new Vector2(update.sizeofgoal.x, update.sizeofgoal.y * myreceivedata.goals.Count);

            update.goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight += update.sizeofgoal.y;
            update.goaltranform.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(update.goaltranform.gameObject.GetComponent<RectTransform>().sizeDelta.x, update.goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight);
            update.parentscroll.content.sizeDelta = new Vector2(update.parentscroll.content.sizeDelta.x, update.parentscroll.content.sizeDelta.y + update.sizeofgoal.y);
            
            /* RectTransform cre;
             cre = update.goalscroll.GetComponent<RectTransform>();
             cre.sizeDelta = new Vector2(cre.sizeDelta.x, cre.sizeDelta.y + update.sizeofgoal.y);
             update.goalscroll.GetComponent<RectTransform>().sizeDelta = cre.sizeDelta;
             update.goalscroll.viewport.sizeDelta = cre.sizeDelta; //new Vector2(sizeofgoal.x, sizeofgoal.y * countofgoals);
             update.goalscroll.content.sizeDelta = new Vector2(sizeofgoal.x, sizeofgoal.y * myreceivedata.goals.Count);


             update.goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight += update.sizeofgoal.y; //+ myreceivedata.goals.Count;
             update.goaltranform.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(update.goaltranform.gameObject.GetComponent<RectTransform>().sizeDelta.x, update.goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight);
             update.parentscroll.content.sizeDelta = new Vector2(update.parentscroll.content.sizeDelta.x, update.parentscroll.content.sizeDelta.y + update.sizeofgoal.y);
            */
            update.goalscroll.content.GetComponent<VerticalLayoutGroup>().enabled = true;



            update.parentscroll.content.GetComponent<VerticalLayoutGroup>().enabled = false;
            update.parentscroll.content.GetComponent<VerticalLayoutGroup>().enabled = true;
        }
       /* RectTransform cre;
        cre = update.goalscroll.GetComponent<RectTransform>();
        cre.sizeDelta = new Vector2(cre.sizeDelta.x, cre.sizeDelta.y + update.sizeofgoal.y);
        update.goalscroll.GetComponent<RectTransform>().sizeDelta = new Vector2(update.sizeofgoal.x, update.sizeofgoal.y * myreceivedata.goals.Count); 
        update.goalscroll.content.sizeDelta = new Vector2(update.sizeofgoal.x, update.sizeofgoal.y * myreceivedata.goals.Count);
        update.goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight += update.sizeofgoal.y * myreceivedata.goals.Count;
        update.parentscroll.content.sizeDelta = new Vector2(update.parentscroll.content.sizeDelta.x, update.parentscroll.content.sizeDelta.y + update.sizeofgoal.y);

*/

       
        /*   RectTransform cree;
           cree = update.linkscroll.GetComponent<RectTransform>();
           cree.sizeDelta = new Vector2(cree.sizeDelta.x, cree.sizeDelta.y + update.sizeoflink.y);
           update.linkscroll.GetComponent<RectTransform>().sizeDelta = cree.sizeDelta;
           //gaolspawn.transform.SetParent(goaltranform);
           update.linkscroll.content.sizeDelta = new Vector2(update.sizeoflink.x, update.sizeoflink.y * myreceivedata.links.Count);
           update.linktransform.gameObject.GetComponent<LayoutElement>().preferredHeight += update.sizeoflink.y + myreceivedata.links.Count;
           update.parentscroll.content.sizeDelta = new Vector2(update.parentscroll.content.sizeDelta.x, update.parentscroll.content.sizeDelta.y + update.sizeoflink.y);
        */
    /*    \\\


        if (myreceivedata.links != null)
        {
            
            for (int i = 0; i < myreceivedata.links.Count; i++)
            {
                GameObject linkobj = Instantiate(update.linkspawn, update.linkscroll.content);

                update.linkscroll.content.GetChild(i).GetChild(0).GetComponent<InputField>().text = myreceivedata.links[i].url;
                update.linkscroll.content.GetChild(i).GetChild(1).GetComponent<InputField>().text = myreceivedata.links[i].title;
                
            }


        }

        */
        if (myreceivedata.links != null)
        {
            for (int i = 0; i < myreceivedata.links.Count; i++)
            {

                /// GameObject linkobj = Instantiate(linkspawn, linkscroll.content);

                ///  linkscroll.content.GetChild(i).GetChild(0).GetComponent<Text>().text = myreceivedata.links[i].title + "    " + myreceivedata.links[i].url;

                if (myreceivedata.links[i].title == "proofoftime")
                {

                   update.linksprites[0].SetActive(true);
                   update.linksurl[0].text = myreceivedata.links[i].url;
                }
                if (myreceivedata.links[i].title == "garmin")
                {

                    update.linksprites[1].SetActive(true);
                    update.linksurl[1].text = myreceivedata.links[i].url;
                }
                if (myreceivedata.links[i].title == "starva")
                {

                    update.linksprites[2].SetActive(true);
                    update.linksurl[2].text = myreceivedata.links[i].url;
                }
                if (myreceivedata.links[i].title == "reddit")
                {

                    update.linksprites[3].SetActive(true);
                    update.linksurl[3].text = myreceivedata.links[i].url;

                }
                if (myreceivedata.links[i].title == "youtube")
                {

                    update.linksprites[4].SetActive(true);
                    update.linksurl[4].text = myreceivedata.links[i].url;

                }
                if (myreceivedata.links[i].title == "instagram")
                {

                    update.linksprites[5].SetActive(true);
                    update.linksurl[5].text = myreceivedata.links[i].url;

                }
                if (myreceivedata.links[i].title == "tiktok")
                {

                    update.linksprites[6].SetActive(true);
                    update.linksurl[6].text = myreceivedata.links[i].url;

                }
                if (myreceivedata.links[i].title == "facebook")
                {
                    //Debug.LogError("facebook");

                    update.linksprites[7].SetActive(true);
                    update.linksurl[7].text = myreceivedata.links[i].url;

                }
                if (myreceivedata.links[i].title == "athlinks")
                {

                    update.linksprites[8].SetActive(true);
                    update.linksurl[8].text = myreceivedata.links[i].url;

                }
                if (myreceivedata.links[i].title == "custom1")
                {

                    update.linksprites[9].SetActive(true);
                    update.linksurl[9].text = myreceivedata.links[i].url;

                }
                if (myreceivedata.links[i].title == "custom2")
                {

                    update.linksprites[10].SetActive(true);
                    update.linksurl[10].text = myreceivedata.links[i].url;

                }
                if (myreceivedata.links[i].title == "custom3")
                {

                    update.linksprites[11].SetActive(true);
                    update.linksurl[11].text = myreceivedata.links[i].url;

                }




            }
        }






        

















        update.gender.text = myreceivedata.result.myGender;

        update.totalagegroup.text = myreceivedata.result.totalAgeGroup;
        update.totalgender.text = myreceivedata.result.totalGender;
        update.agegroup.text = myreceivedata.result.myAgeGroup;



        update.postt.text = myreceivedata.journal.post;
        update.pree.text = myreceivedata.journal.pre;
        update.nextt.text = myreceivedata.journal.Next;
        update.notes.text = myreceivedata.journal.Notes;
        update.racee.text = myreceivedata.journal.race;
       // Debug.Log("RACE UPDATE" + update.race.text);

        update.goalgetter = myreceivedata.goals.Count;
        update.linkgetter = myreceivedata.links.Count;
        update.id = myreceivedata._id;

        update.racerno.text = myreceivedata.race;





       




        StartCoroutine(delay());



        Debug.Log("id: " + update.id);
        scrollreset();

     

    }





    IEnumerator delay()
    {
        yield return new WaitForSeconds(1);

        if (myreceivedata.theme == "medaldisney")
        {
            Debug.Log("theme 1: " + myreceivedata.theme+" "+update.themes.Length);
            for (int i = 0; i < update.themes.Length; i++)
            {
                if (i == 6)
                {
                    Debug.Log("theme: " + myreceivedata.theme);
                    update.ar100medal[0].SetActive(true);
                    update.themes[i].SetActive(true);
                }
                else
                {
                    update.themes[i].SetActive(false);
                }

            }
        }
    }






  
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
    public static int goalcounterforexpand;
    public class mydata
    {
        public string position { get; set; }
        public string _id { get; set; }
        public string eventdatetoshow { get; set; }
        public string totalpeople { get; set; }
        public string eventName { get; set; }
        public string eventDate { get; set; }
        public string city { get; set; }
        public string victoryCity { get; set; }
        public string race { get; set; }

        public string time { get; set; }
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
    public mydata myreceivedata;
    private Vector2 originalparentscroll;
    private Vector2 orignalgaolsizedelata;

    

    public class Result
    {
        public string myAgeGroup { get; set; }
        public string totalAgeGroup { get; set; }
        public string myGender { get; set; }
        public string totalGender { get; set; }
    }

    public void getimagefromtext()
    {
       
        if (myreceivedata.image != null)
        {
           
            Texture2D newphoto = new Texture2D((int)1, (int)1);

            newphoto.LoadImage(Convert.FromBase64String(myreceivedata.image));
            float width = Screen.width;
            float height = Screen.height;

            float aspctratio = width / height;
            Debug.Log(aspctratio);
          hiimage.GetComponent<AspectRatioFitter>().aspectRatio = aspctratio;
            Debug.Log("getimage: " + myreceivedata.image);

            newphoto.Apply();

            hiimage.texture = newphoto;
        }
    }
    IEnumerator GetTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(myreceivedata.image);
        yield return www.SendWebRequest();
        Debug.Log("URL: " + myreceivedata.image);
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            float width = Screen.width;
            float height = Screen.height;
            float aspctratio = width / height;

            float twidth = myTexture.width;
            float theight = myTexture.height;


            float ratioo = twidth / theight;
            if (ratioo == 0)
            {
                ratioo = 1f;
            }
            if (aspctratio == 0)
            {
                aspctratio = 1f;
            }
            if (myTexture.width > myTexture.height)
            {
                Debug.LogError("texture");

                hiimage.GetComponent<AspectRatioFitter>().aspectRatio = ratioo;
            }
            else
            {
                
                hiimage.GetComponent<AspectRatioFitter>().aspectRatio = ratioo;

            }
          
            if (myTexture != null)
            {
                Debug.Log("hbjhbhjb");
                apigetter.artexture = myTexture;
            }
            hiimage.texture = myTexture;
         
           


        apigetter.profileimage = myTexture;
        }
    }
    public string[] currentext ;
    public string sentence;
    private int confettiPlayId;
    private int confetti2DPlayId;

    public void PlayConfetti()
    {
        StartCoroutine(checkbaloon());
    }

    public IEnumerator checkbaloon()
    {
        int currentConfettiPlayId = ++confettiPlayId;
        int currentConfetti2DPlayId = ++confetti2DPlayId;

        yield return new WaitForSeconds(0.8f);

        SetConfettiActive(confetti2D, true);
        SetConfettiActive(confetti, true);

        PlayConfettiObject(confetti2D);
        PlayConfettiObject(confetti);

        yield return new WaitForSeconds(confettiDeactivateDelay);

        if (currentConfetti2DPlayId == confetti2DPlayId)
        {
            SetConfettiActive(confetti2D, false);
        }

        if (currentConfettiPlayId == confettiPlayId)
        {
            SetConfettiActive(confetti, false);
        }

	}

	public IEnumerator checkbaloonn()
	{
        int currentConfetti2DPlayId = ++confetti2DPlayId;

		yield return new WaitForSeconds(0.2f);

        SetConfettiActive(confetti2D, true);
        PlayConfettiObject(confetti2D);

        yield return new WaitForSeconds(confettiDeactivateDelay);

        if (currentConfetti2DPlayId == confetti2DPlayId)
        {
            SetConfettiActive(confetti2D, false);
        }


	}

    private AnimatedImage GetConfettiAnimation(GameObject confettiObject)
    {
        if (confettiObject == null)
        {
            return null;
        }

        return confettiObject.GetComponent<AnimatedImage>();
    }

    private RawImageSpriteSheetConfetti GetRawImageConfetti(GameObject confettiObject)
    {
        if (confettiObject == null)
        {
            return null;
        }

        RawImageSpriteSheetConfetti rawImageConfetti = confettiObject.GetComponent<RawImageSpriteSheetConfetti>();
        if (rawImageConfetti == null)
        {
            rawImageConfetti = confettiObject.GetComponentInChildren<RawImageSpriteSheetConfetti>(true);
        }

        return rawImageConfetti;
    }

    private void PlayConfettiObject(GameObject confettiObject)
    {
        RawImageSpriteSheetConfetti rawImageConfetti = GetRawImageConfetti(confettiObject);
        if (rawImageConfetti != null)
        {
            rawImageConfetti.enabled = true;
            SetRawImagesEnabled(confettiObject, true);

            rawImageConfetti.Play();
            return;
        }

        PlayConfettiAnimation(GetConfettiAnimation(confettiObject));
    }

    private void PlayConfettiAnimation(AnimatedImage animatedImage)
    {
        if (animatedImage != null)
        {
            animatedImage.Play();
        }
    }

    private void SetConfettiActive(GameObject confettiObject, bool active)
    {
        if (confettiObject == null)
        {
            return;
        }

        RawImageSpriteSheetConfetti rawImageConfetti = GetRawImageConfetti(confettiObject);
        if (rawImageConfetti != null)
        {
            if (!active)
            {
                rawImageConfetti.Stop();
            }

            SetRawImagesEnabled(confettiObject, active);
            rawImageConfetti.enabled = active;
        }

        if (confettiObject.activeSelf != active)
        {
            confettiObject.SetActive(active);
        }
    }

    private void SetRawImagesEnabled(GameObject parent, bool enabled)
    {
        if (parent == null)
        {
            return;
        }

        RawImage[] rawImages = parent.GetComponentsInChildren<RawImage>(true);
        for (int i = 0; i < rawImages.Length; i++)
        {
            rawImages[i].enabled = enabled;
        }
    }

    // public void PlayConfetti()
    // {
    //     RawImageSpriteSheetConfetti rawImageConfetti = null;

    //     if (confetti != null)
    //     {
    //         rawImageConfetti = confetti.GetComponent<RawImageSpriteSheetConfetti>();
    //     }

    //     if (rawImageConfetti == null)
    //     {
    //         rawImageConfetti = GetComponentInChildren<RawImageSpriteSheetConfetti>(true);
    //     }

    //     if (rawImageConfetti != null)
    //     {
    //         rawImageConfetti.gameObject.SetActive(true);
    //         rawImageConfetti.Play();
    //         return;
    //     }

    //     UIConfetti uiConfetti = null;

    //     if (confetti != null)
    //     {
    //         uiConfetti = confetti.GetComponent<UIConfetti>();
    //     }

    //     if (uiConfetti == null)
    //     {
    //         uiConfetti = GetComponentInChildren<UIConfetti>(true);
    //     }

    //     if (uiConfetti != null)
    //     {
    //         uiConfetti.gameObject.SetActive(true);
    //         uiConfetti.Play();
    //     }
    // }


    public static int spartantheme;


	public void getdata()
    {
        spartantheme = 0;

		scrollreset();
        if (myreceivedata.race != "")
        {
            racerno.text = myreceivedata.race;
        }
        else
        {

            racerno.text = "";


        }


        sentence = myreceivedata.eventName;


		currentext = myreceivedata.eventName.Split(char.Parse(" "));
		


		totalpeople.text = myreceivedata.totalpeople;
        position.text = myreceivedata.position;
        city.text = myreceivedata.city;
        Eventname.text = myreceivedata.eventName;
        victorycatchphrase.text = myreceivedata.victoryCity;
        Date.text = myreceivedata.eventDate;
        if (ApiRequestGenerator.guestlogin == 0)
        {
            firstname.text = apigetter.firstname;
        }
        else
        {
            firstname.text = "Guest";
        
        }


        time.text = myreceivedata.time;
        distance.text = myreceivedata.distance.istance;
        unit.text = myreceivedata.distance.unit;
        apigetter.artexture = null;
        if (myreceivedata.image != null)
        {

            StartCoroutine(GetTexture());
        }
        else if (eventheme == 0)
        {
            eventgetter.instance.defautart[1].GetComponent<RawImage>().texture = eventgetter.instance.basicart[1].texture;

        }
        else if (eventheme == 1)
        {
            eventgetter.instance.defautart[1].GetComponent<RawImage>().texture = eventgetter.instance.Disneyart[1].texture;

        }
        else if (eventheme == 2)
        {
            eventgetter.instance.defautart[1].GetComponent<RawImage>().texture = eventgetter.instance.basicart[1].texture;

        }
        gender.text = myreceivedata.result.myGender;
        
        agegroup.text = myreceivedata.result.myAgeGroup;
        totalagegroup.text = myreceivedata.result.totalAgeGroup;
       
            totalgender.text = myreceivedata.result.totalGender;
        

    //    totalgendeer.SetActive(false);
     //   gendeer.SetActive(false);
        
        pree.text = myreceivedata.journal.pre;
        racee.text = myreceivedata.journal.race;
        postt.text = myreceivedata.journal.post;
        nextt.text = myreceivedata.journal.Next;
        notes.text = myreceivedata.journal.Notes;
        Debug.Log("RACE NEXT" + racee.text);


        if (myreceivedata.@virtual == "yes")
        {
            if (eventheme == 1)
            {
                virtuaal.sprite = eventgetter.instance.Disneyart[12];
            }
            else if (eventheme==2)
            {

                virtuaal.sprite = eventgetter.instance.basicart[12];


            }
            else
            {
                virtuaal.sprite = vrhome;
            }
        }
        else
        {
            if (eventheme == 1)
            {
                virtuaal.sprite = eventgetter.instance.Disneyart[5];
            }
            else if (eventheme == 2)
            {

                virtuaal.sprite = eventgetter.instance.basicart[5];


            }
            else
            {
                virtuaal.sprite = vrsecond;
            }
        }



        if (myreceivedata.personarecord == "yes")
        {
            sceneloading.prcheck = 1;


            if (eventheme == 1)
            {
                personalr.sprite = eventgetter.instance.Disneyart[11];
            }
            else if (eventheme == 2)
            {
                personalr.sprite = eventgetter.instance.basicart[11];


            }
            else
            {
                personalr.sprite = prloc;
            }


        }
        else
        {
            sceneloading.prcheck = 0;
            if (eventheme == 1)
            {
                personalr.sprite = eventgetter.instance.Disneyart[6];
            }
            else if (eventheme == 2)
            {
                personalr.sprite = eventgetter.instance.basicart[6];
            }
            else
            {
                personalr.sprite = prsecond;
            }
        
        
        
        
        }
        
        //gaolspawn.transform.SetParent(goaltranform);

       


        if (myreceivedata.goals != null)
        {
            
            for (int k = 0; k < myreceivedata.goals.Count; k++)
            {
                if (k > 0)
                {
                    RectTransform cre;
                    cre = goalscroll.GetComponent<RectTransform>();
                    cre.sizeDelta = new Vector2(cre.sizeDelta.x, cre.sizeDelta.y + sizeofgoal.y);
                    goalscroll.GetComponent<RectTransform>().sizeDelta = cre.sizeDelta;
                    goalscroll.viewport.sizeDelta = cre.sizeDelta;
                    goalscroll.content.sizeDelta = new Vector2(sizeofgoal.x, sizeofgoal.y * myreceivedata.goals.Count);
                    goalcounterforexpand = myreceivedata.goals.Count;
                    goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight += sizeofgoal.y;
                    goaltranform.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(goaltranform.gameObject.GetComponent<RectTransform>().sizeDelta.x, goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight);
                  //  parentscroll.content.sizeDelta = new Vector2(parentscroll.content.sizeDelta.x, parentscroll.content.sizeDelta.y + sizeofgoal.y);
                   
                   /*  RectTransform cre;
                     cre = goalscroll.GetComponent<RectTransform>();
                     cre.sizeDelta = new Vector2(cre.sizeDelta.x, cre.sizeDelta.y + sizeofgoal.y);
                     goalscroll.GetComponent<RectTransform>().sizeDelta = cre.sizeDelta;
                     goalscroll.content.sizeDelta = new Vector2(sizeofgoal.x, sizeofgoal.y * myreceivedata.goals.Count);
                     goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight += sizeofgoal.y + myreceivedata.goals.Count;
                     parentscroll.content.sizeDelta = new Vector2(parentscroll.content.sizeDelta.x, parentscroll.content.sizeDelta.y + sizeofgoal.y);*/
                }
                GameObject goalobj = Instantiate(gaolspawn, goalscroll.content);
                if (myreceivedata.goals[k].achieved == "yes")
                {

                    goalscroll.content.GetChild(k).GetChild(0).GetComponent<Image>().sprite=right ;
                }
                else
                {
                    goalscroll.content.GetChild(k).GetChild(0).GetComponent<Image>().sprite = wrong;
                }
                goalscroll.content.GetChild(k).GetChild(1).GetComponent<Text>().text = myreceivedata.goals[k].goal;
            }



        }






      
        /*
            RectTransform cree;
            cree = linkscroll.GetComponent<RectTransform>();
            cree.sizeDelta = new Vector2(cree.sizeDelta.x, cree.sizeDelta.y + sizeoflink.y);
            linkscroll.GetComponent<RectTransform>().sizeDelta = cree.sizeDelta;
            //gaolspawn.transform.SetParent(goaltranform);
            linkscroll.content.sizeDelta = new Vector2(sizeoflink.x, sizeoflink.y * myreceivedata.links.Count);
            linktransform.gameObject.GetComponent<LayoutElement>().preferredHeight += sizeoflink.y + myreceivedata.links.Count;
            parentscroll.content.sizeDelta = new Vector2(parentscroll.content.sizeDelta.x, parentscroll.content.sizeDelta.y + sizeoflink.y);*/

        if (myreceivedata.links != null)
        {
            for (int i = 0; i < myreceivedata.links.Count; i++)
            {

                /// GameObject linkobj = Instantiate(linkspawn, linkscroll.content);



                ///  linkscroll.content.GetChild(i).GetChild(0).GetComponent<Text>().text = myreceivedata.links[i].title + "    " + myreceivedata.links[i].url;

                if (myreceivedata.links[i].title == "proofoftime")
                {

                    links[0].interactable = true;
                    pot = myreceivedata.links[i].url;
                }
                if (myreceivedata.links[i].title == "garmin")
                {

                    links[1].interactable = true;
                    garmin = myreceivedata.links[i].url;
                }
                if (myreceivedata.links[i].title == "starva")
                {

                    links[2].interactable = true;
                    strava = myreceivedata.links[i].url;
                }
                if (myreceivedata.links[i].title == "reddit")
                {

                    links[3].interactable = true;
                    reddit = myreceivedata.links[i].url;

                }
                if (myreceivedata.links[i].title == "youtube")
                {

                    links[4].interactable = true;
                    youtube = myreceivedata.links[i].url;

                }
                if (myreceivedata.links[i].title == "instagram")
                {

                    links[5].interactable = true;
                    instagram = myreceivedata.links[i].url;

                }
                if (myreceivedata.links[i].title == "tiktok")
                {

                    links[6].interactable = true;
                    tiktok = myreceivedata.links[i].url;

                }
                if (myreceivedata.links[i].title == "facebook")
                {

                    links[7].interactable = true;
                    facebook = myreceivedata.links[i].url;

                }
                if (myreceivedata.links[i].title == "athlinks")
                {

                    links[8].interactable = true;
                    athlinks = myreceivedata.links[i].url;

                }
                if (myreceivedata.links[i].title == "custom1")
                {

                    links[9].interactable = true;
                    custom1 = myreceivedata.links[i].url;

                }
                if (myreceivedata.links[i].title == "custom2")
                {

                    links[10].interactable = true;
                    custom2 = myreceivedata.links[i].url;

                }
                if (myreceivedata.links[i].title == "custom3")
                {

                    links[11].interactable = true;
                    custom3 = myreceivedata.links[i].url;

                }




            }
        }



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



						if (myreceivedata.personarecord == "yes")
						{
							sceneloading.prcheck = 1;


							if (eventheme == 1)
							{
								personalr.sprite = eventgetter.instance.Disneyart[11];
							}
							else if (eventheme == 2)
							{
								personalr.sprite = eventgetter.instance.basicart[11];


							}
							else
							{
								personalr.sprite = prloc;
							}


						}
						else
						{
							sceneloading.prcheck = 0;
							if (eventheme == 1)
							{
								personalr.sprite = eventgetter.instance.Disneyart[6];
							}
							else if (eventheme == 2)
							{
								personalr.sprite = eventgetter.instance.basicart[6];
							}
							else
							{
								personalr.sprite = prsecond;
							}




						}

						if (myreceivedata.@virtual == "yes")
						{
							if (eventheme == 1)
							{
								virtuaal.sprite = eventgetter.instance.Disneyart[12];
							}
							else if (eventheme == 2)
							{
								Debug.Log("spartan wala");
								virtuaal.sprite = eventgetter.instance.basicart[12];


							}
							else
							{
								virtuaal.sprite = vrhome;
							}
						}
						else
						{
							if (eventheme == 1)
							{
								virtuaal.sprite = eventgetter.instance.Disneyart[5];
							}
							else if (eventheme == 2)
							{

								virtuaal.sprite = eventgetter.instance.basicart[5];


							}
							else
							{
								virtuaal.sprite = vrsecond;
							}
						}


						if (myreceivedata.image == null)
						{
							eventgetter.instance.defautart[1].GetComponent<RawImage>().texture = eventgetter.instance.Disneyart[1].texture;
						}










						for (int d = 0; d < eventgetter.instance.themetext.Length; d++)
						{
							eventgetter.instance.themetext[d].color = new Color32(253, 191, 0, 255);
						}

						eventgetter.instance.distancethemtext.color = Color.white;

						eventgetter.instance.timethemetext.color = Color.white;
						return; // Exit the method if the word is found
					}
				}
				if (currentext[a].ToLower() == eventgetter.instance.disney[b])
                {


                    eventgetter.instance.defautart[0].GetComponent<Image>().sprite = eventgetter.instance.Disneyart[0];



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



                    if (myreceivedata.personarecord == "yes")
                    {
                        sceneloading.prcheck = 1;


                        if (eventheme == 1)
                        {
                            personalr.sprite = eventgetter.instance.Disneyart[11];
                        }
                        else if (eventheme == 2)
                        {
                            personalr.sprite = eventgetter.instance.basicart[11];


                        }
                        else
                        {
                            personalr.sprite = prloc;
                        }


                    }
                    else
                    {
                        sceneloading.prcheck = 0;
                        if (eventheme == 1)
                        {
                            personalr.sprite = eventgetter.instance.Disneyart[6];
                        }
                        else if (eventheme == 2)
                        {
                            personalr.sprite = eventgetter.instance.basicart[6];
                        }
                        else
                        {
                            personalr.sprite = prsecond;
                        }




                    }

                    if (myreceivedata.@virtual == "yes")
                    {
                        if (eventheme == 1)
                        {
                            virtuaal.sprite = eventgetter.instance.Disneyart[12];
                        }
                        else if (eventheme == 2)
                        {
                            Debug.Log("spartan wala");
                            virtuaal.sprite = eventgetter.instance.basicart[12];


                        }
                        else
                        {
                            virtuaal.sprite = vrhome;
                        }
                    }
                    else
                    {
                        if (eventheme == 1)
                        {
                            virtuaal.sprite = eventgetter.instance.Disneyart[5];
                        }
                        else if (eventheme == 2)
                        {

                            virtuaal.sprite = eventgetter.instance.basicart[5];


                        }
                        else
                        {
                            virtuaal.sprite = vrsecond;
                        }
                    }


                    if (myreceivedata.image == null)
                    {
                        eventgetter.instance.defautart[1].GetComponent<RawImage>().texture = eventgetter.instance.Disneyart[1].texture;
                    }










                        for (int d = 0; d < eventgetter.instance.themetext.Length; d++)
                    {
                        eventgetter.instance.themetext[d].color = new Color32(253, 191, 0, 255);
					}

                    eventgetter.instance.distancethemtext.color = Color.white;

                    eventgetter.instance.timethemetext.color = Color.white;
                    return;
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



                    if (myreceivedata.personarecord == "yes")
                    {
                        sceneloading.prcheck = 1;


                        if (eventheme == 1)
                        {
                            personalr.sprite = eventgetter.instance.Disneyart[11];
                        }
                        else if (eventheme == 2)
                        {
                            personalr.sprite = eventgetter.instance.basicart[11];


                        }
                        else
                        {
                            personalr.sprite = prloc;
                        }


                    }
                    else
                    {
                        sceneloading.prcheck = 0;
                        if (eventheme == 1)
                        {
                            personalr.sprite = eventgetter.instance.Disneyart[6];
                        }
                        else if (eventheme == 2)
                        {
                            personalr.sprite = eventgetter.instance.basicart[6];
                        }
                        else
                        {
                            personalr.sprite = prsecond;
                        }




                    }

                    if (myreceivedata.@virtual == "yes")
                    {
                        if (eventheme == 1)
                        {
                            virtuaal.sprite = eventgetter.instance.Disneyart[12];
                        }
                        else if (eventheme == 2)
                        {

                            virtuaal.sprite = eventgetter.instance.basicart[12];


                        }
                        else
                        {
                            virtuaal.sprite = vrhome;
                        }
                    }
                    else
                    {
                        if (eventheme == 1)
                        {
                            virtuaal.sprite = eventgetter.instance.Disneyart[5];
                        }
                        else if (eventheme == 2)
                        {

                            virtuaal.sprite = eventgetter.instance.basicart[5];


                        }
                        else
                        {
                            virtuaal.sprite = vrsecond;
                        }
                    }


                    if (myreceivedata.image == null)
                    {
                        eventgetter.instance.defautart[1].GetComponent<RawImage>().texture = eventgetter.instance.basicart[1].texture;
                    }
                    for (int d = 0; d < eventgetter.instance.themetext.Length; d++)
                        {
                            eventgetter.instance.themetext[d].color = Color.white;
                        }

					eventgetter.instance.themetext[3].color = new Color32(255, 205, 0, 255);
					eventgetter.instance.themetext[5].color = new Color32(255, 205, 0, 255);
					eventgetter.instance.themetext[2].color = Color.white;
					eventgetter.instance.themetext[0].color = new Color32(255, 205, 0, 255);
					
					//     eventgetter.instance.themetext[3].color= new Color32(255, 0, 0, 255);
					//	eventgetter.instance.themetext[5].color= new Color32(255, 0, 0, 255);
					//eventgetter.instance.themetext[d].color

					eventgetter.instance.distancethemtext.color = Color.white;
					Debug.Log("working data");


					spartantheme = 0;
					eventgetter.instance.timethemetext.color = Color.white;
                    return;

                }
                else
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



                    if (myreceivedata.personarecord == "yes")
                    {
                        sceneloading.prcheck = 1;


                        if (eventheme == 1)
                        {
                            personalr.sprite = eventgetter.instance.Disneyart[11];
                        }
                        else if (eventheme == 2)
                        {
                            personalr.sprite = eventgetter.instance.basicart[11];

                        }
                        else
                        {
                            personalr.sprite = prloc;
                        }


                    }
                    else
                    {
                        sceneloading.prcheck = 0;
                        if (eventheme == 1)
                        {
                            personalr.sprite = eventgetter.instance.Disneyart[6];
                        }
                        else if (eventheme == 2)
                        {
                            personalr.sprite = eventgetter.instance.basicart[6];
                        }
                        else
                        {
                            personalr.sprite = prsecond;
                        }




                    }


                    if (myreceivedata.@virtual == "yes")
                    {
                        if (eventheme == 1)
                        {
                            virtuaal.sprite = eventgetter.instance.Disneyart[12];
                        }
                        else if (eventheme == 2)
                        {

                            virtuaal.sprite = eventgetter.instance.basicart[12];


                        }
                        else
                        {
                            virtuaal.sprite = vrhome;
                        }
                    }
                    else
                    {
                        if (eventheme == 1)
                        {
                            virtuaal.sprite = eventgetter.instance.Disneyart[5];
                        }
                        else if (eventheme == 2)
                        {

                            virtuaal.sprite = eventgetter.instance.basicart[5];


                        }
                        else
                        {
                            virtuaal.sprite = vrsecond;
                        }
                    }



                    if (myreceivedata.image == null)
                    {
                        eventgetter.instance.defautart[1].GetComponent<RawImage>().texture = eventgetter.instance.basicart[1].texture;
                    }
                    for (int d = 0; d < eventgetter.instance.themetext.Length; d++)
                    {
                        eventgetter.instance.themetext[d].color = Color.white;
                    }
					eventgetter.instance.themetext[3].color = new Color32(255, 205, 0, 255);
					eventgetter.instance.themetext[5].color = new Color32(255, 205, 0, 255);
					eventgetter.instance.themetext[0].color = new Color32(255, 205, 0, 255);
					eventgetter.instance.themetext[6].color = new Color32(255, 205, 0, 255);


					eventgetter.instance.distancethemtext.color = Color.white;

                    eventgetter.instance.timethemetext.color = Color.white;
				}


            }

    
        }

        StartCoroutine(checkbaloon());
        //     getimagefromtext();
        //

    }
    public void getguestdata()
	{
		
		scrollreset();
        if (myreceivedata.race != "")
        {
            racerno.text = myreceivedata.race;
        }
        else
        {

            racerno.text = "";


        }
        sentence = myreceivedata.eventName;




		currentext = myreceivedata.eventName.Split(char.Parse(" "));


        totalpeople.text = myreceivedata.totalpeople;
        position.text = myreceivedata.position;
        city.text = myreceivedata.city;
        Eventname.text = myreceivedata.eventName;
        victorycatchphrase.text = myreceivedata.victoryCity;
        Date.text = myreceivedata.eventDate;
        if (ApiRequestGenerator.guestlogin == 0)
        {
            firstname.text = apigetter.firstname;
        }
        else
        {
            firstname.text = "Guest";
        
        }
        time.text = myreceivedata.time;
        distance.text = myreceivedata.distance.istance;
        unit.text =   myreceivedata.distance.unit ;
        apigetter.artexture = null;
        if (myreceivedata.image != null)
        {
            if (ApiRequestGenerator.guestlogin == 0)
            {
                StartCoroutine(GetTexture());
            }
            else
            {
                
                if (updataeven.updateeventguest==0)
                {
                    apigetter.artexture = eventgetter.instance.eventcurrentimage.texture;

                    hiimage.texture = eventgetter.instance.eventcurrentimage.texture;
                }
                if (updataeven.updateeventguest == 1)
                {
                    updataeven.updateeventguest = 0;
                    apigetter.artexture = updataeven.instance.hiimage.texture;

                    hiimage.texture = updataeven.instance.hiimage.texture;


                }
            }
           
        }
        
        else if (eventheme == 0)
        {
            eventgetter.instance.defautart[1].GetComponent<RawImage>().texture = eventgetter.instance.basicart[1].texture;

        }
        else if (eventheme == 1)
        {
            eventgetter.instance.defautart[1].GetComponent<RawImage>().texture = eventgetter.instance.Disneyart[1].texture;

        }
        else if (eventheme == 2)
        {
            eventgetter.instance.defautart[1].GetComponent<RawImage>().texture = eventgetter.instance.basicart[1].texture;

        }
        gender.text = myreceivedata.result.myGender;

        agegroup.text = myreceivedata.result.myAgeGroup;
        totalagegroup.text = myreceivedata.result.totalAgeGroup;

        totalgender.text = myreceivedata.result.totalGender;


        //    totalgendeer.SetActive(false);
        //   gendeer.SetActive(false);

        pree.text = myreceivedata.journal.pre;
        racee.text = myreceivedata.journal.race;
        postt.text = myreceivedata.journal.post;
        nextt.text = myreceivedata.journal.Next;
        notes.text = myreceivedata.journal.Notes;
        Debug.Log("RACE NEXT" + racee.text);


        if (myreceivedata.@virtual == "yes")
        {
            if (eventheme == 1)
            {
                virtuaal.sprite = eventgetter.instance.Disneyart[12];
            }
            else if (eventheme == 2)
            {

                virtuaal.sprite = eventgetter.instance.basicart[12];


            }
            else
            {
                virtuaal.sprite = vrhome;
            }
        }
        else
        {
            if (eventheme == 1)
            {
                virtuaal.sprite = eventgetter.instance.Disneyart[5];
            }
            else if (eventheme == 2)
            {

                virtuaal.sprite = eventgetter.instance.basicart[5];


            }
            else
            {
                virtuaal.sprite = vrsecond;
            }
        }



        if (myreceivedata.personarecord == "yes")
        {
            sceneloading.prcheck = 1;


            if (eventheme == 1)
            {
                personalr.sprite = eventgetter.instance.Disneyart[11];
            }
            else if (eventheme == 2)
            {
                personalr.sprite = eventgetter.instance.basicart[11];


            }
            else
            {
                personalr.sprite = prloc;
            }


        }
        else
        {
            sceneloading.prcheck = 0;
            if (eventheme == 1)
            {
                personalr.sprite = eventgetter.instance.Disneyart[6];
            }
            else if (eventheme == 2)
            {
                personalr.sprite = eventgetter.instance.basicart[6];
            }
            else
            {
                personalr.sprite = prsecond;
            }




        }

        //gaolspawn.transform.SetParent(goaltranform);




        if (myreceivedata.goals != null)
        {

            for (int k = 0; k < myreceivedata.goals.Count; k++)
            {
                if (k > 0)
                {
                    RectTransform cre;
                    cre = goalscroll.GetComponent<RectTransform>();
                    cre.sizeDelta = new Vector2(cre.sizeDelta.x, cre.sizeDelta.y + sizeofgoal.y);
                    goalscroll.GetComponent<RectTransform>().sizeDelta = cre.sizeDelta;
                    goalscroll.viewport.sizeDelta = cre.sizeDelta;
                    goalscroll.content.sizeDelta = new Vector2(sizeofgoal.x, sizeofgoal.y * myreceivedata.goals.Count);
                    goalcounterforexpand = myreceivedata.goals.Count;
                    goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight += sizeofgoal.y;
                    goaltranform.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(goaltranform.gameObject.GetComponent<RectTransform>().sizeDelta.x, goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight);
                    //  parentscroll.content.sizeDelta = new Vector2(parentscroll.content.sizeDelta.x, parentscroll.content.sizeDelta.y + sizeofgoal.y);

                    /*  RectTransform cre;
                      cre = goalscroll.GetComponent<RectTransform>();
                      cre.sizeDelta = new Vector2(cre.sizeDelta.x, cre.sizeDelta.y + sizeofgoal.y);
                      goalscroll.GetComponent<RectTransform>().sizeDelta = cre.sizeDelta;
                      goalscroll.content.sizeDelta = new Vector2(sizeofgoal.x, sizeofgoal.y * myreceivedata.goals.Count);
                      goaltranform.gameObject.GetComponent<LayoutElement>().preferredHeight += sizeofgoal.y + myreceivedata.goals.Count;
                      parentscroll.content.sizeDelta = new Vector2(parentscroll.content.sizeDelta.x, parentscroll.content.sizeDelta.y + sizeofgoal.y);*/
                }
                GameObject goalobj = Instantiate(gaolspawn, goalscroll.content);
                if (myreceivedata.goals[k].achieved == "yes")
                {

                    goalscroll.content.GetChild(k).GetChild(0).GetComponent<Image>().sprite = right;
                }
                else
                {
                    goalscroll.content.GetChild(k).GetChild(0).GetComponent<Image>().sprite = wrong;
                }
                goalscroll.content.GetChild(k).GetChild(1).GetComponent<Text>().text = myreceivedata.goals[k].goal;
            }



        }







        /*
            RectTransform cree;
            cree = linkscroll.GetComponent<RectTransform>();
            cree.sizeDelta = new Vector2(cree.sizeDelta.x, cree.sizeDelta.y + sizeoflink.y);
            linkscroll.GetComponent<RectTransform>().sizeDelta = cree.sizeDelta;
            //gaolspawn.transform.SetParent(goaltranform);
            linkscroll.content.sizeDelta = new Vector2(sizeoflink.x, sizeoflink.y * myreceivedata.links.Count);
            linktransform.gameObject.GetComponent<LayoutElement>().preferredHeight += sizeoflink.y + myreceivedata.links.Count;
            parentscroll.content.sizeDelta = new Vector2(parentscroll.content.sizeDelta.x, parentscroll.content.sizeDelta.y + sizeoflink.y);*/

        if (myreceivedata.links != null)
        {
            for (int i = 0; i < myreceivedata.links.Count; i++)
            {

                /// GameObject linkobj = Instantiate(linkspawn, linkscroll.content);



                ///  linkscroll.content.GetChild(i).GetChild(0).GetComponent<Text>().text = myreceivedata.links[i].title + "    " + myreceivedata.links[i].url;

                if (myreceivedata.links[i].title == "proofoftime")
                {

                    links[0].interactable = true;
                    pot = myreceivedata.links[i].url;
                }
                if (myreceivedata.links[i].title == "garmin")
                {

                    links[1].interactable = true;
                    garmin = myreceivedata.links[i].url;
                }
                if (myreceivedata.links[i].title == "starva")
                {

                    links[2].interactable = true;
                    strava = myreceivedata.links[i].url;
                }
                if (myreceivedata.links[i].title == "reddit")
                {

                    links[3].interactable = true;
                    reddit = myreceivedata.links[i].url;

                }
                if (myreceivedata.links[i].title == "youtube")
                {

                    links[4].interactable = true;
                    youtube = myreceivedata.links[i].url;

                }
                if (myreceivedata.links[i].title == "instagram")
                {

                    links[5].interactable = true;
                    instagram = myreceivedata.links[i].url;

                }
                if (myreceivedata.links[i].title == "tiktok")
                {

                    links[6].interactable = true;
                    tiktok = myreceivedata.links[i].url;

                }
                if (myreceivedata.links[i].title == "facebook")
                {

                    links[7].interactable = true;
                    facebook = myreceivedata.links[i].url;

                }
                if (myreceivedata.links[i].title == "athlinks")
                {

                    links[8].interactable = true;
                    athlinks = myreceivedata.links[i].url;

                }
                if (myreceivedata.links[i].title == "custom1")
                {

                    links[9].interactable = true;
                    custom1 = myreceivedata.links[i].url;

                }
                if (myreceivedata.links[i].title == "custom2")
                {

                    links[10].interactable = true;
                    custom2 = myreceivedata.links[i].url;

                }
                if (myreceivedata.links[i].title == "custom3")
                {

                    links[11].interactable = true;
                    custom3 = myreceivedata.links[i].url;

                }




            }
        }



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



						if (myreceivedata.personarecord == "yes")
						{
							sceneloading.prcheck = 1;


							if (eventheme == 1)
							{
								personalr.sprite = eventgetter.instance.Disneyart[11];
							}
							else if (eventheme == 2)
							{
								personalr.sprite = eventgetter.instance.basicart[11];


							}
							else
							{
								personalr.sprite = prloc;
							}


						}
						else
						{
							sceneloading.prcheck = 0;
							if (eventheme == 1)
							{
								personalr.sprite = eventgetter.instance.Disneyart[6];
							}
							else if (eventheme == 2)
							{
								personalr.sprite = eventgetter.instance.basicart[6];
							}
							else
							{
								personalr.sprite = prsecond;
							}




						}

						if (myreceivedata.@virtual == "yes")
						{
							if (eventheme == 1)
							{
								virtuaal.sprite = eventgetter.instance.Disneyart[12];
							}
							else if (eventheme == 2)
							{
								Debug.Log("spartan wala");
								virtuaal.sprite = eventgetter.instance.basicart[12];


							}
							else
							{
								virtuaal.sprite = vrhome;
							}
						}
						else
						{
							if (eventheme == 1)
							{
								virtuaal.sprite = eventgetter.instance.Disneyart[5];
							}
							else if (eventheme == 2)
							{

								virtuaal.sprite = eventgetter.instance.basicart[5];


							}
							else
							{
								virtuaal.sprite = vrsecond;
							}
						}


						if (myreceivedata.image == null)
						{
							eventgetter.instance.defautart[1].GetComponent<RawImage>().texture = eventgetter.instance.Disneyart[1].texture;
						}


						for (int d = 0; d < eventgetter.instance.themetext.Length; d++)
						{
							eventgetter.instance.themetext[d].color = new Color32(217, 166, 9, 255);
						}

						eventgetter.instance.distancethemtext.color = Color.white;

						eventgetter.instance.timethemetext.color = Color.white;
						return;



					}
                }
















						if (currentext[a].ToLower() == eventgetter.instance.disney[b])
                {


                    eventgetter.instance.defautart[0].GetComponent<Image>().sprite = eventgetter.instance.Disneyart[0];



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



                    if (myreceivedata.personarecord == "yes")
                    {
                        sceneloading.prcheck = 1;


                        if (eventheme == 1)
                        {
                            personalr.sprite = eventgetter.instance.Disneyart[11];
                        }
                        else if (eventheme == 2)
                        {
                            personalr.sprite = eventgetter.instance.basicart[11];


                        }
                        else
                        {
                            personalr.sprite = prloc;
                        }


                    }
                    else
                    {
                        sceneloading.prcheck = 0;
                        if (eventheme == 1)
                        {
                            personalr.sprite = eventgetter.instance.Disneyart[6];
                        }
                        else if (eventheme == 2)
                        {
                            personalr.sprite = eventgetter.instance.basicart[6];
                        }
                        else
                        {
                            personalr.sprite = prsecond;
                        }




                    }

                    if (myreceivedata.@virtual == "yes")
                    {
                        if (eventheme == 1)
                        {
                            virtuaal.sprite = eventgetter.instance.Disneyart[12];
                        }
                        else if (eventheme == 2)
                        {
                            Debug.Log("spartan wala");
                            virtuaal.sprite = eventgetter.instance.basicart[12];


                        }
                        else
                        {
                            virtuaal.sprite = vrhome;
                        }
                    }
                    else
                    {
                        if (eventheme == 1)
                        {
                            virtuaal.sprite = eventgetter.instance.Disneyart[5];
                        }
                        else if (eventheme == 2)
                        {

                            virtuaal.sprite = eventgetter.instance.basicart[5];


                        }
                        else
                        {
                            virtuaal.sprite = vrsecond;
                        }
                    }


                    if (myreceivedata.image == null)
                    {
                        eventgetter.instance.defautart[1].GetComponent<RawImage>().texture = eventgetter.instance.Disneyart[1].texture;
                    }










                    for (int d = 0; d < eventgetter.instance.themetext.Length; d++)
                    {
                        eventgetter.instance.themetext[d].color = new Color32(217, 166, 9, 255);
                    }

                    eventgetter.instance.distancethemtext.color = Color.white;

                    eventgetter.instance.timethemetext.color = Color.white;
                    return;
                }
                else if (currentext[a].ToLower() == eventgetter.instance.spartan[b])
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



                    if (myreceivedata.personarecord == "yes")
                    {
                        sceneloading.prcheck = 1;


                        if (eventheme == 1)
                        {
                            personalr.sprite = eventgetter.instance.Disneyart[11];
                        }
                        else if (eventheme == 2)
                        {
                            personalr.sprite = eventgetter.instance.basicart[11];


                        }
                        else
                        {
                            personalr.sprite = prloc;
                        }


                    }
                    else
                    {
                        sceneloading.prcheck = 0;
                        if (eventheme == 1)
                        {
                            personalr.sprite = eventgetter.instance.Disneyart[6];
                        }
                        else if (eventheme == 2)
                        {
                            personalr.sprite = eventgetter.instance.basicart[6];
                        }
                        else
                        {
                            personalr.sprite = prsecond;
                        }




                    }

                    if (myreceivedata.@virtual == "yes")
                    {
                        if (eventheme == 1)
                        {
                            virtuaal.sprite = eventgetter.instance.Disneyart[12];
                        }
                        else if (eventheme == 2)
                        {

                            virtuaal.sprite = eventgetter.instance.basicart[12];


                        }
                        else
                        {
                            virtuaal.sprite = vrhome;
                        }
                    }
                    else
                    {
                        if (eventheme == 1)
                        {
                            virtuaal.sprite = eventgetter.instance.Disneyart[5];
                        }
                        else if (eventheme == 2)
                        {

                            virtuaal.sprite = eventgetter.instance.basicart[5];


                        }
                        else
                        {
                            virtuaal.sprite = vrsecond;
                        }
                    }


                    if (myreceivedata.image == null)
                    {
                        eventgetter.instance.defautart[1].GetComponent<RawImage>().texture = eventgetter.instance.basicart[1].texture;
                    }
                  
					for (int d = 0; d < eventgetter.instance.themetext.Length; d++)
					{
						eventgetter.instance.themetext[d].color = Color.white;
					}

					eventgetter.instance.themetext[3].color = new Color32(255, 205, 0, 255);
					eventgetter.instance.themetext[5].color = new Color32(255, 205, 0, 255);
					eventgetter.instance.themetext[2].color = new Color32(255, 205, 0, 255);
					eventgetter.instance.themetext[0].color = new Color32(255, 205, 0, 255);
					eventgetter.instance.themetext[4].color = new Color32(255, 205, 0, 255);
					//     eventgetter.instance.themetext[3].color= new Color32(255, 0, 0, 255);
					//	eventgetter.instance.themetext[5].color= new Color32(255, 0, 0, 255);
					//eventgetter.instance.themetext[d].color

					eventgetter.instance.distancethemtext.color = Color.white;
					Debug.Log("working data");


					spartantheme = 0;
					eventgetter.instance.timethemetext.color = Color.white;


















					return;

                }
                else
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



                    if (myreceivedata.personarecord == "yes")
                    {
                        sceneloading.prcheck = 1;


                        if (eventheme == 1)
                        {
                            personalr.sprite = eventgetter.instance.Disneyart[11];
                        }
                        else if (eventheme == 2)
                        {
                            personalr.sprite = eventgetter.instance.basicart[11];


                        }
                        else
                        {
                            personalr.sprite = prloc;
                        }


                    }
                    else
                    {
                        sceneloading.prcheck = 0;
                        if (eventheme == 1)
                        {
                            personalr.sprite = eventgetter.instance.Disneyart[6];
                        }
                        else if (eventheme == 2)
                        {
                            personalr.sprite = eventgetter.instance.basicart[6];
                        }
                        else
                        {
                            personalr.sprite = prsecond;
                        }




                    }


                    if (myreceivedata.@virtual == "yes")
                    {
                        if (eventheme == 1)
                        {
                            virtuaal.sprite = eventgetter.instance.Disneyart[12];
                        }
                        else if (eventheme == 2)
                        {

                            virtuaal.sprite = eventgetter.instance.basicart[12];


                        }
                        else
                        {
                            virtuaal.sprite = vrhome;
                        }
                    }
                    else
                    {
                        if (eventheme == 1)
                        {
                            virtuaal.sprite = eventgetter.instance.Disneyart[5];
                        }
                        else if (eventheme == 2)
                        {

                            virtuaal.sprite = eventgetter.instance.basicart[5];


                        }
                        else
                        {
                            virtuaal.sprite = vrsecond;
                        }
                    }



                    if (myreceivedata.image == null)
                    {
                        eventgetter.instance.defautart[1].GetComponent<RawImage>().texture = eventgetter.instance.basicart[1].texture;
                    }
                   


					for (int d = 0; d < eventgetter.instance.themetext.Length; d++)
					{
						eventgetter.instance.themetext[d].color = Color.white;
					}
					eventgetter.instance.themetext[3].color = new Color32(255, 205, 0, 255);
					eventgetter.instance.themetext[5].color = new Color32(255, 205, 0, 255);
					eventgetter.instance.themetext[0].color = new Color32(255, 205, 0, 255);
					eventgetter.instance.themetext[6].color = new Color32(255, 205, 0, 255);
					eventgetter.instance.themetext[2].color = new Color32(255, 205, 0, 255);
					eventgetter.instance.themetext[4].color = new Color32(255, 205, 0, 255);


					eventgetter.instance.distancethemtext.color = Color.white;

					eventgetter.instance.timethemetext.color = Color.white;







				}


            }


        }
		StartCoroutine(checkbaloon());
		//     getimagefromtext();
		//

	}

    public void test()
    {
        
    }



    public void proofoftimee()
    {
        Application.OpenURL(pot);
    }
    public void garminn()
    {
        Application.OpenURL(garmin);
    }
    public void stravaa()
    {
        Application.OpenURL(strava);
    }
    public void redditt()
    {
        Application.OpenURL(reddit);
    }
    public void youtubee()
    {
        Application.OpenURL(youtube);
    }
    public void instagramm()
    {
        Application.OpenURL(instagram);
    }
    public void tiktokk()
    {
        Application.OpenURL(tiktok);
    }
    public void facebokk()
    {
        Application.OpenURL(facebook);
    }
    public void athlinkss()
    {
        Application.OpenURL(athlinks);
    }
    public void customone()
    {
        Application.OpenURL(custom1);
    }
    public void customtwo()
    {
        Application.OpenURL(custom2);
    }
    public void cusomthree()
    {
        Application.OpenURL(custom3);
    }









    public void ShowUploadedGallery()
    {
        if (EventGalleryViewer.Instance == null)
        {
            Debug.LogError("EventGalleryViewer is missing.");
            return;
        }

        int count = myreceivedata?.galleryImages?.Count ?? 0;
        Debug.Log("Current event gallery image count: " + count);

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
            myreceivedata?.galleryImages,
            myreceivedata?._id,
            urls => myreceivedata.galleryImages = urls,
            galleryContent);
    }





}
