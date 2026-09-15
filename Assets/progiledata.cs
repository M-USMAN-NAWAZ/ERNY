using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class progiledata : MonoBehaviour
{


    public Text Username;
    public InputField password,ps2,email,state,postal,city;
    public Text Address;
    public GameObject loader;
    public Dropdown Drp_Country,province;
    public string baseurl;
    public string json2send,pasword2send;
    public Text statee;

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Response
    {
        public string _id { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string postalCode { get; set; }
        public string image { get; set; }
        public string age { get; set; }
        public string gender { get; set; }
        public string profile { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int __v { get; set; }
    }

    public class Root
    {
        public bool success { get; set; }
        public Response response { get; set; }
        public object metadata { get; set; }
    }


    public void afterupdate()
    {
        if (apigetter.province != "State/Province")
        {
            Username.text = apigetter.firstname + " " + apigetter.lastname;
            Address.text = /*apigetter.city + ", " + apigetter.province + ", " + apigetter.postalcode + "\n" +*/ apigetter.country + "\n" + apigetter.email;
        }
        else
        {
            Username.text = apigetter.firstname + " " + apigetter.lastname;
            Address.text = /*apigetter.city + ", " + apigetter.postalcode + "\n" +*/ apigetter.country + "\n" + apigetter.email;

        }


        //  password.text = apigetter.password;
        //  Drp_Country.value = apigetter.countryid;
        Debug.Log("api: " + apigetter.countryid);
        ps2.text = apigetter.password;
        email.text = apigetter.email;
        //   state.text = apigetter.province;
        postal.text = apigetter.postalcode;
        city.text = apigetter.city;
        StartCoroutine(test());

        Debug.Log("country: " + Drp_Country.value);

        //  int stateintdex = Drp_State.GetComponent<Dropdown>().value;
        //List<Dropdown.OptionData> stateoptions = Drp_State.GetComponent<Dropdown>().options;


        statee.text = apigetter.province;
        int stateintdex = 0;
        List<Dropdown.OptionData> stateoptions = province.GetComponent<Dropdown>().options;
        stateoptions[stateintdex].text = apigetter.province;

        Debug.Log("score: " + apigetter.province);


    }


    // Start is called before the first frame update
    void Start()
    {


        baseurl = baselink.Url;


        loader = GameObject.FindGameObjectWithTag("loader").GetComponent<dontdestroy>().loader;



        if (apigetter.province != "State/Province")
        {
            Username.text = apigetter.firstname + " " + apigetter.lastname;
            Address.text = /*apigetter.city + ", " + apigetter.province + ", " + apigetter.postalcode + "\n" + */apigetter.country + "\n" + apigetter.email;
        }
    else
        {
            Username.text = apigetter.firstname + " " + apigetter.lastname;
            Address.text = /*apigetter.city + ", " + apigetter.postalcode + "\n" + */apigetter.country + "\n" + apigetter.email;

        }


        //  password.text = apigetter.password;
        //  Drp_Country.value = apigetter.countryid;
        Debug.Log("api: " + apigetter.countryid);
        ps2.text = apigetter.password;
        email.text = apigetter.firstname;
     //   state.text = apigetter.province;
        postal.text = apigetter.postalcode;
        city.text = apigetter.city;
        StartCoroutine(test());

        Debug.Log("country: " + Drp_Country.value);
       
            //  int stateintdex = Drp_State.GetComponent<Dropdown>().value;
            //List<Dropdown.OptionData> stateoptions = Drp_State.GetComponent<Dropdown>().options;


            statee.text = apigetter.province;
            int stateintdex = 0;
            List<Dropdown.OptionData> stateoptions = province.GetComponent<Dropdown>().options;
            stateoptions[stateintdex].text = apigetter.province;
           
            Debug.Log("score: " + apigetter.province);

        




    }


    IEnumerator test()
    {
       yield return new WaitForSeconds(2);
     //   Drp_Country.value = 3;
    }



    public void senddata()
    {

        passwordd psd = new passwordd();

        Response myprofile = new Response();
      //  myprofile.province = state.text.Normalize();
        myprofile.city = city.text.Normalize();
        myprofile.postalCode = postal.text;
        myprofile.firstName = email.text.Normalize();
        myprofile.email = apigetter.email;
        int menuindex = Drp_Country.GetComponent<Dropdown>().value;
        List<Dropdown.OptionData> menuOptions = Drp_Country.GetComponent<Dropdown>().options;
        myprofile._id = apigetter.id;
        myprofile.profile = apigetter.profile;
        myprofile.country= menuOptions[menuindex].text.Normalize();
        apigetter.firstname = myprofile.firstName;
        myprofile.lastName = apigetter.lastname;
        myprofile.age = "not specified";
        myprofile.gender = "not specified";

        if (Drp_Country.value == 1 || Drp_Country.value == 229)
        {
            //  int stateintdex = Drp_State.GetComponent<Dropdown>().value;
            //List<Dropdown.OptionData> stateoptions = Drp_State.GetComponent<Dropdown>().options;



            int stateintdex = province.GetComponent<Dropdown>().value; ;
            List<Dropdown.OptionData> stateoptions = province.GetComponent<Dropdown>().options;
            myprofile.province = stateoptions[stateintdex].text.Normalize();// = apigetter.province;



        }
        else
        {
            myprofile.province = "State/Province";
        }
      //  myprofile.country = apigetter.country;


        if(ps2.text != apigetter.password)
        {

            psd.oldPassword = apigetter.password;
            psd.password = ps2.text;
            Debug.Log("death");
            pasword2send = Newtonsoft.Json.JsonConvert.SerializeObject(psd);


            StartCoroutine(pasword_Upload(baseurl + "/v1/user/update-password", pasword2send));

        }


        json2send = Newtonsoft.Json.JsonConvert.SerializeObject(myprofile);

        StartCoroutine(event_Upload(baseurl + "/v1/user/profile", json2send));


        apigetter.city = myprofile.city;
        apigetter.postalcode = myprofile.postalCode;
        apigetter.country = myprofile.country;
        apigetter.countryid = Drp_Country.value;

        //  password.text = apigetter.password;
        if (ps2.text != apigetter.password)
        {
            apigetter.password = ps2.text;
            PlayerPrefs.SetString("pp", apigetter.password);
        }


        apigetter.email = myprofile.email;
        apigetter.province = myprofile.province;
      

        apigetter.countryid = Drp_Country.value;

        PlayerPrefs.SetInt("idd", Drp_Country.value);

        PlayerPrefs.Save();

    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class passwordd
    {
        public string oldPassword { get; set; }
        public string password { get; set; }
    }

    public void showpassword()
    {

        ps2.contentType = InputField.ContentType.Standard;
        ps2.ForceLabelUpdate();
    }

    public void hidepassword()
    {

        ps2.contentType = InputField.ContentType.Password;
        ps2.ForceLabelUpdate();
    }
    IEnumerator pasword_Upload(string url, string bodyJsonString)
    {
        WWWForm form = new WWWForm();
        Dictionary<string, string> headers = form.headers;
        headers["JWT TOKEN"] = apigetter.jwt;
        var request = new UnityWebRequest(url, "POST");
        request.SetRequestHeader("Authorization", "Bearer " + apigetter.jwt);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        // request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        //Debug.Log("Status Code: " + request.responseCode);
        Debug.Log(request.downloadHandler.text);


        //eventdata2send myDeserializedClass = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(request.downloadHandler.text);
    }




    public void loadscene()
    {
        loader.SetActive(true);
        StartCoroutine(profileload());
    }

    IEnumerator profileload()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("4. MainScreens");
    }


    IEnumerator event_Upload(string url, string bodyJsonString)
    {
        WWWForm form = new WWWForm();
        Dictionary<string, string> headers = form.headers;
        headers["JWT TOKEN"] = apigetter.jwt;
        var request = new UnityWebRequest(url, "PATCH");
        request.SetRequestHeader("Authorization", "Bearer " + apigetter.jwt);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        // request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        //Debug.Log("Status Code: " + request.responseCode);
        
        //  Username.text = apigetter.firstname + " " + apigetter.lastname;
        Response myDeserializedClass = Newtonsoft.Json.JsonConvert.DeserializeObject<Response>(request.downloadHandler.text);

        Debug.Log("profile data: " + request.downloadHandler.text);
       
        
        
        
        /*apigetter.city = myDeserializedClass.city;
        apigetter.postalcode = myDeserializedClass.postalCode;
        apigetter.country =myDeserializedClass.country;
        apigetter.countryid = Drp_Country.value;

        //  password.text = apigetter.password;
        if (ps2.text != apigetter.password)
        {
            apigetter.password = ps2.text;
            PlayerPrefs.SetString("pp", apigetter.password);
        }
        
        
        apigetter.email= myDeserializedClass.email;
         apigetter.province = myDeserializedClass.province;
         apigetter.postalcode = myDeserializedClass.postalCode;
         apigetter.city=myDeserializedClass.city;

        apigetter.countryid = Drp_Country.value;

        PlayerPrefs.SetInt("idd", apigetter.countryid);

        PlayerPrefs.Save();
        */
        loadscene();

    }
    // Update is called once per frame
    void Update()
    {
        if (Drp_Country.value != 1 )
        {

            province.interactable = false;

        }
        else
        {

            province.interactable = true;
        }



        
    }
}
