using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Vuforia;
using UnityEngine.SceneManagement;
public class cmeracontrol : MonoBehaviour
{
    public GameObject vrsoft;



    public void startcamer()
    {
       // VuforiaApplication.Instance.Initialize();

       // target[0].GetComponent<ImageTargetBehaviour>().enabled = true;
        vrsoft.SetActive(true);
    }


    public GameObject[] target;

    public void closecamera()
    {
        vrsoft.SetActive(false);
       // target[0].GetComponent<ImageTargetBehaviour>().enabled = false;
        

    }

    public void restart()
    {


        SceneManager.LoadScene("vuforia");
    
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
