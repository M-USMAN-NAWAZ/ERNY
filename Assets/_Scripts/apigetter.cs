using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class apigetter : MonoBehaviour
{
    public static int coneection;
    public static string password;
    public static string id;
    public static string firstname;
    public static string lastname;
    public static string email;
    public static string country;
    public static int countryid;
    public static string profile;
    public static Texture profileimage;

    public static Texture artexture;
    public static DateTime createdtime;
    public static string city;
    public static string province;
    public static string postalcode;
    public static string jwt;
    public static string allofdata;
    // Start is called before the first frame update
    void Awake()
    {

        countryid = PlayerPrefs.GetInt("idd", 0);

        jwt = PlayerPrefs.GetString("jwt");
        email = PlayerPrefs.GetString("email", "");
        password = PlayerPrefs.GetString("pp", "");
        allofdata = PlayerPrefs.GetString("allofdata");
        Debug.Log("jw: " +countryid); Debug.Log("all: " + allofdata);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
