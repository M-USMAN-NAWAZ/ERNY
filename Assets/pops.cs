using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class pops : MonoBehaviour
{

    public static pops instance;
    public Text header,description;
    public GameObject popbox;
    public Animator pp;
    public GameObject Sharebutton;

    public GameObject badgeIgmDiusplay;

    public GameObject blackFade;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        description.resizeTextMaxSize = 80;
    }



    public void showImg(Sprite spr)
    {
        blackFade.SetActive(true);
        badgeIgmDiusplay.gameObject.SetActive(true);
        badgeIgmDiusplay.GetComponentInChildren<Image>().sprite = spr;
    }

    public void OnButton()
    {
        Sharebutton.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
