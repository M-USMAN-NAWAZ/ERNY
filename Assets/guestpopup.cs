using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class guestpopup : MonoBehaviour
{
    public static guestpopup instance;
    public Text header, description;
    public GameObject popbox;
    public Animator pp;

    // Start is called before the first frame update
    void Awake()
    {           
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void signupscreen()
    {
        guestevent.check = 0;
        eventgetter.newpaywallcheck = 0;
        eventgetter.instance.loader.SetActive(true);
        ApiRequestGenerator.signupscreentoshow = 1;
        SceneManager.LoadSceneAsync("3. LoginScene");
    
    
    }



}