using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vrdetect : MonoBehaviour
{

    public GameObject vrrecord,vrtext;
    public int coutn;
    public GameObject model;
    // Start is called before the first frame update
    void Awake()
    {

        coutn = 0;
           
        vrtext = arhandler.instance.vuforiastuff[2].gameObject;

        vrrecord = GameObject.Find("vrrecord");
        vrrecord.SetActive(false);
    }

    public void lost()
    {


        model.SetActive(false);
    }

    public void found()
    {
        coutn = 1;
        model.SetActive(true);
       
       // vrtext = GameObject.Find("vrtext");
       
        if (vrtext != null)
        {
            vrtext.SetActive(false);
        }
        vrrecord.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {

        if (coutn == 1)
        {
            ///rrecord = GameObject.Find("vrrecord");

          //  vrtext = GameObject.Find("vrtext");
            if (vrtext != null)
            {
                vrtext.SetActive(false);
                vrrecord.SetActive(true);
                coutn = 0;
            }

           
        }
    }
}
