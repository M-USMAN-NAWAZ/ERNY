using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class rwesendpopup : MonoBehaviour
{

    public static rwesendpopup instance;
    public Text header, description;
    public GameObject popbox;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
