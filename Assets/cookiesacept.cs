using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cookiesacept : MonoBehaviour
{
    // Start is called before the first frame update
    public Toggle[] acceptall;
    public int one, two, three, four;
    public Button death;
    public int checktoggle = 0;
    private void Awake()
    {
        checktoggle = PlayerPrefs.GetInt("cc", 0);
        one= PlayerPrefs.GetInt("one", 1);
        two = PlayerPrefs.GetInt("two", 1);
        three = PlayerPrefs.GetInt("three", 1);
        four = PlayerPrefs.GetInt("four", 1);



    }


    public void firsttoggle()
    {
        if (acceptall[0].isOn)
        {

            one = 0;
            if (one == 0 && two == 0 && three == 0 && four == 0)
            {
                checktoggle = 1;
                PlayerPrefs.SetInt("cc", 1);
            }
            PlayerPrefs.SetInt("one", one);
            PlayerPrefs.Save();


        }
        else
        {
            one = 1;
            checktoggle = 0;
            PlayerPrefs.SetInt("cc", 0);
            PlayerPrefs.SetInt("one", one);
            PlayerPrefs.Save();

        }

    }

    public void secondtoggle()
    {
        if (acceptall[1].isOn)
        {

            two = 0;
            if (one == 0 && two == 0 && three == 0 && four == 0)
            {
                checktoggle = 1;
                PlayerPrefs.SetInt("cc", 1);
            }
            PlayerPrefs.SetInt("two", two);
            PlayerPrefs.Save();


        }
        else
        {
            two = 1;
            checktoggle = 0;
            PlayerPrefs.SetInt("cc", 0);
            PlayerPrefs.SetInt("two", two);
            PlayerPrefs.Save();

        }

    }
    public void thirdtoggle()
    {
        if (acceptall[2].isOn)
        {

            three = 0;
            if (one == 0 && two == 0 && three == 0 && four == 0)
            {
                checktoggle = 1;
                PlayerPrefs.SetInt("cc", 1);
            }
            PlayerPrefs.SetInt("three", three);
            PlayerPrefs.Save();


        }
        else
        {
            three = 1;
            checktoggle = 0;
            PlayerPrefs.SetInt("cc", 0);
            PlayerPrefs.SetInt("three", three);
            PlayerPrefs.Save();

        }

    }
    public void fourthtoggle()
    {
        if (acceptall[3].isOn)
        {

            four = 0;
            if (one == 0 && two == 0 && three == 0 && four == 0)
            {
                checktoggle = 1;
                PlayerPrefs.SetInt("cc", 1);
            }
            PlayerPrefs.SetInt("four", four);
            PlayerPrefs.Save();


        }
        else
        {
            four = 1;
            checktoggle = 0;
            PlayerPrefs.SetInt("cc", 0);
            PlayerPrefs.SetInt("four", four);
            PlayerPrefs.Save();

        }

    }
    private void Start()
    {
        if(one==1)
        {
            acceptall[0].isOn = false;

        }
       else
        {
            acceptall[0].isOn = true;

        }


        if (two == 1)
        {
            acceptall[1].isOn = false;

        }
        else
        {
            acceptall[1].isOn = true;

        }
        if (three == 1)
        {
            acceptall[2].isOn = false;

        }
        else
        {
            acceptall[2].isOn = true;
        }

        if (four == 1)
        {
            acceptall[3].isOn = false;

        }else
        {
            acceptall[3].isOn = true;
        }





        if (checktoggle==0)
        {
            
        }
        if(checktoggle==1)
        {
            cookies();
        }







    }

    public void nocookies()
    {
        checktoggle = 0;
        PlayerPrefs.SetInt("cc", checktoggle);
        PlayerPrefs.Save();


        for (int i = 0; i < acceptall.Length; i++)
        {

            acceptall[i].isOn = false;

        }

    }





    public void cookies()
    {
        one = 0; two = 0; three = 0; four = 0;
        checktoggle = 1; PlayerPrefs.SetInt("four", four); PlayerPrefs.SetInt("three", three); PlayerPrefs.SetInt("one", one);
        PlayerPrefs.SetInt("two", two);
        PlayerPrefs.SetInt("cc", checktoggle);
        PlayerPrefs.Save();


        for(int i=0;i<acceptall.Length;i++)
        {

            acceptall[i].isOn = true;

        }

    }


    void Update()
    {
        if(one==0 && two==0 && three ==0&&four==0)
        {
            for (int i = 0; i < acceptall.Length; i++)
            {
                acceptall[i].isOn = true;
            }
            death.interactable = false;
        }else
        { 

            death.interactable = true;

        }


    }









}
