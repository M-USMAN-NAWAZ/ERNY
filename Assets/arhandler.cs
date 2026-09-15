using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
//using Vuforia;
public class arhandler : MonoBehaviour
{


    public static arhandler instance;
    public GameObject todestrou;
    public GameObject vuforiaobject;
    public static int selectar;
    public GameObject[] arcorestuff, vuforiastuff,vuforiaimagetarget;
    public static int check;
    public GameObject[] artextdisplay,imagetarget;

    public GameObject checkimagetearget;
    // Start is called before the first frame update
    void Start()
    {
       
        check = 0;   

    }





    public void turntargeton()
    {

        for (int i = 0; i < artextdisplay.Length; i++)
        {
         //   imagetarget[i].SetActive(false);
        }


    }



    public void turndisplayoff()
    {

        for (int i = 0; i < artextdisplay.Length; i++)
        {
            artextdisplay[i].SetActive(false);
        }
    
    
    }
    private void Awake()
    {
        instance = this;
        //if (!VuforiaApplication.Instance.IsInitialized)
        //{
        // //   VuforiaApplication.Instance.Initialize();
        //}
    }
    public void closevuforia()
    {

/*
       if (VuforiaApplication.Instance.IsInitialized)
        {
          
            
         //   VuforiaApplication.Instance.Deinit();
            //
        //    Destroy(todestrou);
          //      Instantiate(vuforiaobject, transform.position, Quaternion.identity);
            
        }*/
     //   VuforiaBehaviour.Instance.enabled = false;

        // vuforiaimagetarget[0].GetComponent<ImageTargetBehaviour>().enabled = false;
        check += 1;
      

        for (int i = 0; i < arcorestuff.Length; i++)
        {
            if (i == 0)
            {
                arcorestuff[i].GetComponent<spawnmangr>().enabled = true;
                arcorestuff[i].GetComponent<XROrigin>().enabled = true;
                arcorestuff[i].GetComponent<ARPlaneManager>().enabled = true;
                arcorestuff[i].GetComponent<ARRaycastManager>().enabled = true;
            }

            arcorestuff[i].SetActive(true);

        }

        for (int i = 0; i < vuforiastuff.Length; i++)
        {
            vuforiastuff[i].SetActive(false);

        }



    }
    public void checkARchoice()
    {
        if (selectar == 0)
        {

          //  vuforiaimagetarget[0].GetComponent<ImageTargetBehaviour>().enabled = false;

              // VuforiaBehaviour.Instance.enabled = false;
            for (int i = 0; i < arcorestuff.Length; i++)
            {
                if (i == 0)
                {
                    arcorestuff[i].GetComponent<spawnmangr>().enabled = true;

                    arcorestuff[i].GetComponent<XROrigin>().enabled = true;
                    arcorestuff[i].GetComponent<ARPlaneManager>().enabled = true;
                    arcorestuff[i].GetComponent<ARRaycastManager>().enabled = true;
                }
                arcorestuff[i].SetActive(true);

            }

            for (int i = 0; i < vuforiastuff.Length; i++)
            {
                vuforiastuff[i].SetActive(false);

            }

        }
        else
        {
            //   vuforiaimagetarget[0].GetComponent<ImageTargetBehaviour>().enabled = true;
            vuforiaimagetarget[0].SetActive(false);

            for (int i = 0; i < arcorestuff.Length; i++)
            {
                if (i == 0)
                {
                    arcorestuff[i].GetComponent<spawnmangr>().enabled = false;
                  //  arcorestuff[i].GetComponent<XROrigin>().enabled = false;
                   // arcorestuff[i].GetComponent<ARPlaneManager>().enabled = false;
                  //  arcorestuff[i].GetComponent<ARRaycastManager>().enabled = false;
                }
              else if(i==1)
                { }
                else
                {
                    arcorestuff[i].SetActive(false);


                }
            }

            for (int i = 0; i < vuforiastuff.Length; i++)
            {
                vuforiastuff[i].SetActive(true);

            }

         //  
       //   if (!VuforiaApplication.Instance.IsInitialized)
       //   {
       ////        VuforiaApplication.Instance.Initialize();
       //   }
            
            //todestrou = GameObject.FindGameObjectWithTag("target");


            //  VuforiaBehaviour.Instance.enabled = true;
            // Vuforia..Instance.InitVuforia();
            //    VuforiaApplication.Instance.Initialize();
            //   vuforiaobject.GetComponent<ImageTargetBehaviour>().enabled = true;
        }
            
    
    
    
    
    
    
    
    }








    // Update is called once per frame
    void Update()
    { //if (checkimagetearget.activeInHierarchy)
       // {
        ///    Debug.Log("DEATH: "+checkimagetearget.name+" "+checkimagetearget.transform.childCount);
         //           }
    
    }
}
