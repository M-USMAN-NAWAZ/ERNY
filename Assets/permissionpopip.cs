using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class permissionpopip : MonoBehaviour
{
    public static permissionpopip instance;
    public Text header, description;
    public GameObject popbox;
    public Animator pp;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
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
