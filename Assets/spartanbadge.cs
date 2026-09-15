using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class spartanbadge : MonoBehaviour
{

    public static spartanbadge instance;

    public GameObject spartan1, spartan2, spartan3;



    private void Start()
    {
        instance = this;
        
    }

    public void onfoundtarget()
    {
        //target = gameObject.transform.GetChild(0).gameObject;
        spartan1.SetActive(true);
        spartan1.transform.GetChild(0).GetComponent<ardata>().checkdata();
        spartan2.SetActive(true);
        spartan2.transform.GetChild(0).GetComponent<ardata>().checkdata();
        spartan3.SetActive(true);
        spartan3.transform.GetChild(0).GetComponent<ardata>().checkdata();



    }





    public void deleteobject()
    {
        if (spartan1 != null)
        {

            Destroy(spartan1);
            Destroy(spartan2);
            Destroy(spartan3);

        }

    }





    public void onlosttarget()
    {
        if (spartan1 != null)
        {
            spartan1.SetActive(false);
            spartan2.SetActive(false);
            spartan3.SetActive(false);

        }



    }
}
