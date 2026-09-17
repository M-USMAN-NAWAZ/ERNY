using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class imagechange : MonoBehaviour
{

    public bool check;

    public Image right;
    public Sprite wrong;
    public Sprite tick;



    public void spritechanger()
    {

        check = !check;

        if(check)
        {
            right.sprite = tick;
        }
        else
        {
            right.sprite = wrong;
        }



    }
    // Start is called before the first frame update
    void Start()
    {
        //check = false;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
