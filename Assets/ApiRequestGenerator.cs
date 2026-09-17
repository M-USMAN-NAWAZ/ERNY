using AppleAuthSample;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ApiRequestGenerator : MonoBehaviour
{


    public GameObject loading, signin, signup, popup, oneTimePopup, forgott, signinn, popupnew, guestpopupp;
    public GameObject newloader;
    public string baseurl ;
    public string Json2send;
    public Button start;
    public bool verifyemail;
    public bool tt = true;

    

    [Header("SignUp Credentials")]
    public InputField INP_FirstName;
    public InputField INP_LastName;
    public InputField INP_Emailaddress;
    public InputField INP_Password;
    public InputField INP_ConfirmPassword;
    [Header("SignUp Location")]

    public Dropdown Drp_Country,drppp;
    public InputField INP_City;
    public Dropdown Drp_State;
    public InputField INP_PostalCode;

    [Header("SignUp Profile ")]
    public Dropdown Drp_Gender;
    public Dropdown Drp_Age;
    public InputField fogotemail;

    public static string versionupdate;

    [Header("SignIn Credentials")]
    public InputField INP_SIEmailaddress;
    public InputField INP_SIPassword;
    public static int guestlogin;

    public static string versiontosend;
    public string versioncheck;

    [Header("Password On Off")]
    public Button toobleOnOff;
    public Sprite eyeOpenIcon;
    public Sprite eyeClosedIcon;
    public bool isPasswordShown = false;

    [Header("Replace with your actual ID token")]
    public string googleIdToken = "Your Google Id Token Here";
    public int googlesignedin;
    public int appleSignedin;
    public LoginWithGoogle loginWithGoogle;
    public MainMenu MainMenu;

    [Header("Apple Id")]
    private string apiUrl = "https://api-stage.blingar.cloud/v1/user/apple-login";

    public void opennurl(string url)
    {
        Application.OpenURL(url);
    }


    public Animator anim;
    public GameObject animObj;

    public class SendData
    {
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string password { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string postalCode { get; set; }
        public string age { get; set; }
        public string gender { get; set; }

    }
    public GameObject signupscreen;
    public static int signupscreentoshow;
    public static int geustpopup;
	private void Awake()
	{
        geustpopup = 0;
        guestlogin = 0;
        GameObject obj;
        if (signupscreentoshow == 1)
        {
            signupscreen.SetActive(true);
        
        }
        versiontosend = versioncheck;
        signupscreentoshow = 0;

        googlesignedin = PlayerPrefs.GetInt("google",0);
        appleSignedin = PlayerPrefs.GetInt("apple",0);
    }

    private void SaveSubscriptionFromApi(string rawJson)
    {
        if (string.IsNullOrWhiteSpace(rawJson))
        {
            return;
        }

        try
        {
            JToken root = JToken.Parse(rawJson);
            JToken subscriptionToken = FindSubscriptionToken(root);

            if (subscriptionToken == null)
            {
                UserSubscriptionState.SetUnsubscribed();
                return;
            }

            if (subscriptionToken.Type == JTokenType.String)
            {
                UserSubscriptionState.SetFromApi(subscriptionToken.ToString(), "");
                return;
            }

            string subscriptionType = ReadFirstString(subscriptionToken, "subscriptionType", "type", "name", "subscription");
            string subscriptionEndDate = ReadFirstString(subscriptionToken, "subscriptionEndDate", "endDate", "expiresAt", "expiryDate");

            UserSubscriptionState.SetFromApi(subscriptionType, subscriptionEndDate);
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Could not read subscription from login API response: " + ex.Message);
            UserSubscriptionState.SetUnsubscribed();
        }
    }

    private JToken FindSubscriptionToken(JToken token)
    {
        if (token == null)
        {
            return null;
        }

        if (token.Type == JTokenType.Object)
        {
            JObject obj = (JObject)token;
            if (obj["subscriptionType"] != null || obj["subscriptionEndDate"] != null)
            {
                return obj;
            }

            foreach (JProperty property in obj.Properties())
            {
                string propertyName = property.Name.ToLowerInvariant();
                if (propertyName.Contains("subscription"))
                {
                    if (property.Value.Type == JTokenType.Array)
                    {
                        JArray array = (JArray)property.Value;
                        return array.Count > 0 ? array[0] : null;
                    }

                    return property.Value;
                }

                JToken nested = FindSubscriptionToken(property.Value);
                if (nested != null)
                {
                    return nested;
                }
            }
        }

        if (token.Type == JTokenType.Array)
        {
            foreach (JToken item in token)
            {
                JToken nested = FindSubscriptionToken(item);
                if (nested != null)
                {
                    return nested;
                }
            }
        }

        return null;
    }

    private string ReadFirstString(JToken token, params string[] propertyNames)
    {
        foreach (string propertyName in propertyNames)
        {
            JToken value = token[propertyName];
            if (value != null && value.Type != JTokenType.Null)
            {
                return value.ToString();
            }
        }

        return "";
    }


    [System.Serializable]
    public class IdTokenData
    {
        public string idToken;
        public IdTokenData(string token)
        {
            idToken = token;
        }
    }
    public void googleloginapi()
    {
        StartCoroutine(SendIdTokenToServer(googleIdToken));
    }
    IEnumerator SendIdTokenToServer(string idToken)
    {
        // Create a JSON body
        string jsonBody = JsonUtility.ToJson(new IdTokenData(idToken));
        string url = baseurl + "/v1/user/google-login";
        // Create the request
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        // Send the request and wait for a response
        yield return request.SendWebRequest();

        Debug.Log("Response Code: " + request.responseCode);
        Debug.Log("Response Text: " + request.downloadHandler.text);

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Request failed: " + request.error);
            yield break;
        }

        Root myDeserializedClass = null;
        try
        {
            myDeserializedClass = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(request.downloadHandler.text);
        }
        catch (Exception ex)
        {
            Debug.LogError("JSON Deserialization Error: " + ex.Message);
            Debug.LogError("Raw JSON: " + request.downloadHandler.text);
            yield break;
        }

        if (myDeserializedClass == null)
        {
            Debug.LogError("Deserialized object is null");
            yield break;
        }

        Debug.Log("API success flag: " + myDeserializedClass.success);

        if (myDeserializedClass.success)
        {
            SaveSubscriptionFromApi(request.downloadHandler.text);
            googlesignedin = 1;
            PlayerPrefs.SetInt("google",googlesignedin);

            PlayerPrefs.SetString("userEmailGoogle", loginWithGoogle.UserEmail.text);
            PlayerPrefs.Save();

            for (int i = 0; i < Drp_Country.options.Count; i++)
            {

                if (myDeserializedClass.metadata.user.country == drppp.options[i].text.Normalize())
                {

                    apigetter.countryid = i;
                    PlayerPrefs.SetInt("idd", i);
                    Debug.Log("vaalue: " + apigetter.countryid);
                    PlayerPrefs.Save();
                }

                Debug.Log("vaaluee: " + apigetter.countryid);
            }


            apigetter.id = myDeserializedClass.metadata.user._id;
            
                apigetter.allofdata = request.downloadHandler.text;
                apigetter.firstname = myDeserializedClass.metadata.user.firstName;
                apigetter.province = myDeserializedClass.metadata.user.province;
                apigetter.city = myDeserializedClass.metadata.user.city;
                apigetter.lastname = myDeserializedClass.metadata.user.lastName;
                apigetter.profile = myDeserializedClass.metadata.user.profile;
                apigetter.email = myDeserializedClass.metadata.user.email;
                PlayerPrefs.SetString("email", apigetter.email);
                apigetter.postalcode = myDeserializedClass.metadata.user.postalCode;

                apigetter.country = myDeserializedClass.metadata.user.country;






                apigetter.jwt = myDeserializedClass.response.jwt;

                PlayerPrefs.SetString("pp", apigetter.password);

                PlayerPrefs.SetString("allofdata", apigetter.allofdata);
                PlayerPrefs.SetString("jwt", apigetter.jwt);
                PlayerPrefs.Save();

                apigetter.createdtime = myDeserializedClass.metadata.user.createdAt;
                SceneManager.LoadScene("4. MainScreens");
                //SceneManager.LoadScene("5.guestlogin");
            
        }
        else
        {
            popup.SetActive(true);
            pops.instance.header.text = "Oops!";
            pops.instance.description.text = "It appears that something went wrong with google.";
            loading.SetActive(false);
        }
    }






    // Helper class for request body
    [System.Serializable]
    public class IdTokenRequest
    {
        public string idToken;
    }





    public void LoginWithApple(string idToken)
    {
        StartCoroutine(PostAppleLogin(idToken));
    }

    IEnumerator PostAppleLogin(string idToken)
    {
        // Create the request body as JSON
        string jsonBody = JsonUtility.ToJson(new IdTokenRequest { idToken = idToken });

        // Set up the request
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request
        yield return request.SendWebRequest();

        Root myDeserializedClass = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(request.downloadHandler.text);


        if (myDeserializedClass.success)
        {
            SaveSubscriptionFromApi(request.downloadHandler.text);
            appleSignedin = 1;
            PlayerPrefs.SetInt("apple", appleSignedin);

            PlayerPrefs.SetString("userEmailApple", MainMenu.userEmail);
            PlayerPrefs.Save();

            for (int i = 0; i < Drp_Country.options.Count; i++)
            {

                if (myDeserializedClass.metadata.user.country == drppp.options[i].text.Normalize())
                {

                    apigetter.countryid = i;
                    PlayerPrefs.SetInt("idd", i);
                    Debug.Log("vaalue: " + apigetter.countryid);
                    PlayerPrefs.Save();
                }

                Debug.Log("vaaluee: " + apigetter.countryid);
            }


            apigetter.id = myDeserializedClass.metadata.user._id;
           apigetter.allofdata = request.downloadHandler.text;
                apigetter.firstname = myDeserializedClass.metadata.user.firstName;
                apigetter.province = myDeserializedClass.metadata.user.province;
                apigetter.city = myDeserializedClass.metadata.user.city;
                apigetter.lastname = myDeserializedClass.metadata.user.lastName;
                apigetter.profile = myDeserializedClass.metadata.user.profile;
                apigetter.email = myDeserializedClass.metadata.user.email;
                PlayerPrefs.SetString("email", apigetter.email);
                apigetter.postalcode = myDeserializedClass.metadata.user.postalCode;

                apigetter.country = myDeserializedClass.metadata.user.country;






                apigetter.jwt = myDeserializedClass.response.jwt;

                PlayerPrefs.SetString("pp", apigetter.password);

                PlayerPrefs.SetString("allofdata", apigetter.allofdata);
                PlayerPrefs.SetString("jwt", apigetter.jwt);
                PlayerPrefs.Save();

                apigetter.createdtime = myDeserializedClass.metadata.user.createdAt;
                SceneManager.LoadScene("4. MainScreens");
                //SceneManager.LoadScene("5.guestlogin");
            
        }
    }



    public void TogglePassword()
    {
        isPasswordShown = !isPasswordShown;
        HideShowPassword();
    }

    public void HideShowPassword()
    {
        INP_Password.contentType = isPasswordShown ? InputField.ContentType.Standard : InputField.ContentType.Password;
        INP_Password.ForceLabelUpdate();
        toobleOnOff.GetComponent<Image>().sprite = isPasswordShown ? eyeOpenIcon : eyeClosedIcon;
    }


    public void guest()
    {
        loading.SetActive(true);
        geustpopup = 1;
        guestlogin = 1;
        eventgetter.newpaywallcheck = 0;
        SceneManager.LoadSceneAsync("5.guestlogin");
    
    }
	public void DosignUp()
    {

       
        SendData data2send = new SendData();
        data2send.email = INP_Emailaddress.text.Normalize();
        data2send.password = INP_Password.text.Normalize();
        data2send.firstName = INP_FirstName.text.Normalize();
        data2send.lastName = INP_LastName.text.Normalize();
       
        int menuindex = Drp_Country.GetComponent<Dropdown>().value;
        List<Dropdown.OptionData> menuOptions = Drp_Country.GetComponent<Dropdown>().options;

        int ageindex = Drp_Age.GetComponent<Dropdown>().value;
        List<Dropdown.OptionData> ageoption = Drp_Age.GetComponent<Dropdown>().options;


        int genderindex = Drp_Gender.GetComponent<Dropdown>().value;
        List<Dropdown.OptionData> genderoption = Drp_Gender.GetComponent<Dropdown>().options;


        data2send.country = menuOptions[menuindex].text;
        data2send.city = INP_City.text.Normalize();
        data2send.age = ageoption[ageindex].text;
        data2send.gender = genderoption[genderindex].text;


        if (Drp_Country.value == 1 )
        {
            int stateintdex = Drp_State.GetComponent<Dropdown>().value;
            List<Dropdown.OptionData> stateoptions = Drp_State.GetComponent<Dropdown>().options;
            data2send.province = stateoptions[stateintdex].text.Normalize();
            //data2send.gender =""+ stateintdex;


        }
        else
        {
            data2send.province = "State/Province";
           // data2send.gender = "" + 0;

        }





        data2send.postalCode = INP_PostalCode.text.Normalize();
        Json2send = Newtonsoft.Json.JsonConvert.SerializeObject(data2send);
        apigetter.password = data2send.password;
        PlayerPrefs.SetString("pp", apigetter.password);
        PlayerPrefs.Save();
        StartCoroutine(SignUp_Upload(baseurl + "/v1/user/signup", Json2send));
    }
    IEnumerator SignUp_Upload(string url, string bodyJsonString)
    {

       

            loading.SetActive(true);
            var request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            Debug.Log("Status Code: " + request.responseCode);
            Debug.Log(request.downloadHandler.text);


            Root myDeserializedClass = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(request.downloadHandler.text);


        //    Debug.Log("mydeserialize data " + myDeserializedClass.metadata.user.firstName);


        if (myDeserializedClass.success)
        {
            SaveSubscriptionFromApi(request.downloadHandler.text);
            apigetter.countryid = Drp_Country.value;
            apigetter.id = myDeserializedClass.metadata.user._id;
            if (myDeserializedClass.metadata.user.isEmailVerified)
            {
                apigetter.allofdata = request.downloadHandler.text;

                apigetter.createdtime = myDeserializedClass.metadata.user.createdAt;
                apigetter.province = myDeserializedClass.metadata.user.province;
                apigetter.city = myDeserializedClass.metadata.user.city;
                
                Debug.Log("counter value: " + apigetter.countryid + " contry value: " + Drp_Country.value);
                apigetter.id = myDeserializedClass.metadata.user._id;
                apigetter.firstname = myDeserializedClass.metadata.user.firstName;

                apigetter.lastname = myDeserializedClass.metadata.user.lastName;

                apigetter.email = myDeserializedClass.metadata.user.email;

                apigetter.postalcode = myDeserializedClass.metadata.user.postalCode;

                apigetter.country = myDeserializedClass.metadata.user.country;
                apigetter.jwt = myDeserializedClass.response.jwt;
                PlayerPrefs.SetString("jwt", apigetter.jwt);
                PlayerPrefs.SetString("allofdata", apigetter.allofdata);
                PlayerPrefs.SetString("email", apigetter.email);
                PlayerPrefs.SetInt("idd", Drp_Country.value);
                PlayerPrefs.Save();
                SceneManager.LoadScene("4. MainScreens");
                //SceneManager.LoadScene("5.guestlogin");
            }
            else
            {
                verificationemail();
            }
        }
        else
        {
            signup.SetActive(true);
            loading.SetActive(false);



            popup.SetActive(true);
            pops.instance.header.text = "Oops!";
            pops.instance.description.text = "The email you have entered already exists.";
        }
        
        
        
        
        
    }

    IEnumerator off()
    {
        yield return new WaitForSeconds(1);
        loading.SetActive(false);

    }

    #region Sign in data 

    public class Metadata
    {
        public User user { get; set; }
    }

    public class Response
    {
        public string jwt { get; set; }
        public string refreshToken { get; set; }



    }

    public class Root
    {
        public bool success { get; set; }
        public Response response { get; set; }
        public Metadata metadata { get; set; }
    }

    public class User
    {
        public string _id { get; set; }
        public string email { get; set; }
        public string profile { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public bool isEmailVerified { get; set; }
        public string postalCode { get; set; }

        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int __v { get; set; }



    }


    #endregion









    IEnumerator checkInternetConnection(Action<bool> action)
    {
        WWW www = new WWW("http://google.com");
        yield return www;
        if (www.error != null)
        {

            apigetter.coneection = 1;
            action(false);
        }
        else
        {
            apigetter.coneection = 2;
            action(true);
        }
    }
   






    public void verificationemail()
    {
        loading.SetActive(false);
        popupnew.SetActive(true);
        rwesendpopup.instance.header.text = "Almost there!";
        rwesendpopup.instance.description.text = "A verification email has been sent to you. Please verify your address to finish registration.";
      
      
    }







        public void storedata()
    {
        User dd = new User();
        string yoyo = dd.email;
        Debug.Log("fff " + yoyo);
    }

    public class SignInUserData
    {
        public string email { get; set; }
        public string password { get; set; }
    }




    public void resendemail()
    {

        resend resnd = new resend();

        resnd.user = apigetter.id;

        String JsonVal = Newtonsoft.Json.JsonConvert.SerializeObject(resnd);

        StartCoroutine(resenmail(baseurl + "/v1/user/resend-email", JsonVal));
    }


    IEnumerator resenmail(string url, string bodyJsonString)
    {

        loading.SetActive(true);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        Debug.Log("value: " + bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        Debug.Log(request.downloadHandler.text);
        Root myDeserializedClass = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(request.downloadHandler.text);


        if (!myDeserializedClass.success)
        {
            loading.SetActive(false);
            popupnew.SetActive(false);
            popup.SetActive(true);
            pops.instance.header.text = "Email does not exist";
            pops.instance.description.text = "The email you have entered does not exist.";
        }
        else
        {
            popupnew.SetActive(false);
            loading.SetActive(false);
            popup.SetActive(true);
            pops.instance.header.text = "Success!";
            pops.instance.description.text = "A verification email has been resended to your email address.";

        }
    }


    public class forgot
        {
        public string email { get; set; }

    }
    public class resend
    {
        public string user { get; set; }

    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class forgotresponse
    {
        public bool success { get; set; }
        public string response { get; set; }
        public object metadata { get; set; }
    }


    public void forgotpassword()
    {

        forgot forgot = new forgot();

        forgot.email = fogotemail.text.Normalize();
       
        String JsonVal = Newtonsoft.Json.JsonConvert.SerializeObject(forgot);

        StartCoroutine(forget_password(baseurl + "/v1/user/forgot-password", JsonVal));
    }

    IEnumerator forget_password(string url, string bodyJsonString)
    {

        loading.SetActive(true);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        Debug.Log("value: " + bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
         yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        Debug.Log(request.downloadHandler.text);
        forgotresponse myDeserializedClass = Newtonsoft.Json.JsonConvert.DeserializeObject<forgotresponse>(request.downloadHandler.text);


        if(!myDeserializedClass.success)
        {
            loading.SetActive(false);

            popup.SetActive(true);
            pops.instance.header.text = "Oops!";
            pops.instance.description.text = "It appears that you have entered an incorrect email address or password.";
        }
        else
        {
            forgott.SetActive(false);
            signinn.SetActive(true);
            loading.SetActive(false);
            popup.SetActive(true);
            pops.instance.header.text = "Success!";
            pops.instance.description.text = "A password change email has been sent to the address you provided.";



        }
    }





        public void DosignIn()
    {

        SignInUserData suserdata = new SignInUserData();

        suserdata.email =INP_SIEmailaddress.text.Normalize();
        suserdata.password = INP_SIPassword.text.Normalize();
        apigetter.password= INP_SIPassword.text.Normalize();
        PlayerPrefs.SetString("pp", apigetter.password);
        PlayerPrefs.Save();
        String JsonVal = Newtonsoft.Json.JsonConvert.SerializeObject(suserdata); 

        StartCoroutine(SignIN_Upload(baseurl + "/v1/user/login", JsonVal));
    }


    IEnumerator SignIN_Upload(string url, string bodyJsonString)
    {
       
            loading.SetActive(true);
            var request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
            Debug.Log("value: " + bodyJsonString);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            Debug.Log("Status Code: " + request.responseCode);
            Debug.Log(request.downloadHandler.text);
            Root myDeserializedClass = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(request.downloadHandler.text);


        if (myDeserializedClass.success)
        {
            SaveSubscriptionFromApi(request.downloadHandler.text);

            for(int i=0;i<Drp_Country.options.Count;i++)
            {

                if (myDeserializedClass.metadata.user.country == drppp.options[i].text.Normalize())
                {

                    apigetter.countryid = i;
                    PlayerPrefs.SetInt("idd", i);
                    Debug.Log("vaalue: " + apigetter.countryid);
                    PlayerPrefs.Save();
                }

                Debug.Log("vaaluee: " + apigetter.countryid);
            }


            apigetter.id = myDeserializedClass.metadata.user._id;
            if (myDeserializedClass.metadata.user.isEmailVerified)
            {
                apigetter.allofdata = request.downloadHandler.text;
                apigetter.firstname = myDeserializedClass.metadata.user.firstName;
                apigetter.province = myDeserializedClass.metadata.user.province;
                apigetter.city = myDeserializedClass.metadata.user.city;
                apigetter.lastname = myDeserializedClass.metadata.user.lastName;
                apigetter.profile = myDeserializedClass.metadata.user.profile;
                apigetter.email = myDeserializedClass.metadata.user.email;
                PlayerPrefs.SetString("email", apigetter.email);
                apigetter.postalcode = myDeserializedClass.metadata.user.postalCode;

                apigetter.country = myDeserializedClass.metadata.user.country;


                



                apigetter.jwt = myDeserializedClass.response.jwt;

                PlayerPrefs.SetString("pp", apigetter.password);

                PlayerPrefs.SetString("allofdata", apigetter.allofdata);
                PlayerPrefs.SetString("jwt", apigetter.jwt);
                PlayerPrefs.Save();

                apigetter.createdtime = myDeserializedClass.metadata.user.createdAt;
                SceneManager.LoadScene("4. MainScreens");
                //SceneManager.LoadScene("5.guestlogin");
            }
            else
            {
                verificationemail();
            }
        }
        else
        {
            popup.SetActive(true);
            pops.instance.header.text = "Oops!";
            pops.instance.description.text = "It appears that you have entered an incorrect email address or password.";
            loading.SetActive(false);
            signin.SetActive(true);
        }
        
    /*else
            {
            popup.SetActive(true);
            pops.instance.header.text = "No Wifi";
            pops.instance.description.text = "Check your wifi connection.";
            loading.SetActive(false);

        }*/
    }
    public void oldversion()
    {
        if (apigetter.allofdata != "")

        {
            loading.SetActive(false);

            SignInUserData suserdata = new SignInUserData();

            suserdata.email = apigetter.email;
            suserdata.password = apigetter.password;
            String JsonVal = Newtonsoft.Json.JsonConvert.SerializeObject(suserdata);

            StartCoroutine(SignIN_Upload(baseurl + "/v1/user/login", JsonVal));
        }
        else
        {

            loading.SetActive(false);

        }
    }
    IEnumerator test()
    {
        StartCoroutine(versionget(baseurl + "/v1/versionUpdate"));

        yield return new WaitForSeconds(1.3f);

     //   if (versioncontrol)
    //    {
    /*
            if (apigetter.allofdata != "")

            {
                SignInUserData suserdata = new SignInUserData();

                suserdata.email = apigetter.email;
                suserdata.password = apigetter.password;
                String JsonVal = Newtonsoft.Json.JsonConvert.SerializeObject(suserdata);

                StartCoroutine(SignIN_Upload(baseurl + "/v1/user/login", JsonVal));
            }
            else
            {

                loading.SetActive(false);

            }*/
        //     }
        //     else
        //     { 

//        versioncontrol.instance.popbox.SetActive(true);        
  //      }
        /*
         if (apigetter.allofdata != "")
        {
            Root myDeserializedClass = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(apigetter.allofdata);
            // Debug.Log(myDeserializedClass);
            if (myDeserializedClass.success)
            {
                Debug.Log("start");
                apigetter.firstname = myDeserializedClass.metadata.user.firstName;
                apigetter.id = myDeserializedClass.metadata.user._id;
                apigetter.lastname = myDeserializedClass.metadata.user.lastName;
                apigetter.province = myDeserializedClass.metadata.user.province;
                apigetter.city = myDeserializedClass.metadata.user.city;
                apigetter.email = myDeserializedClass.metadata.user.email;

               PlayerPrefs.SetString("email", apigetter.email);
                apigetter.postalcode = myDeserializedClass.metadata.user.postalCode;

                apigetter.country = myDeserializedClass.metadata.user.country;
                apigetter.jwt = myDeserializedClass.response.jwt;

                PlayerPrefs.SetString("allofdata", apigetter.allofdata);
                PlayerPrefs.SetString("jwt", apigetter.jwt);
                PlayerPrefs.Save();

                SceneManager.LoadScene("4. MainScreens");
            }
        }
        else
        {
            loading.SetActive(false);
        }*/

    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    /*public class versioncontrols
    {
        public string _id { get; set; }
        public string versions { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int __v { get; set; }
    }

    public class version
    {
        public bool success { get; set; }
        public List<versioncontrols> response { get; set; }
        public object metadata { get; set; }
    }*/
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class versioncontrols
    {
        public string _id { get; set; }
        public string versions { get; set; }
        public bool test { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int __v { get; set; }
    }

    public class version
    {
        public bool success { get; set; }
        public List<versioncontrols> response { get; set; }
        public object metadata { get; set; }
    }


    public void guestcheck()
    {
        guestpopupp.SetActive(true);
      //  guestpopup.instance.pp.Play("popp");
    }


    public void OneTimePopup()
    {
        loading.SetActive(false);
        oneTimePopup.SetActive(true);   
        PlayerPrefs.SetInt("OnetimePopup", 1);
       
    }

    public void OneTimePopupAutoLogin()
    {
        StartCoroutine(versionget(baseurl + "/v1/versionUpdate"));
    }

    
    IEnumerator versionget(string url)
    {

        loading.SetActive(true);
        var request = new UnityWebRequest(url, "GET");
     //   request.SetRequestHeader("Authorization", "Bearer " + apigetter.jwt);
        //  byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        //  Debug.Log("value: " + bodyJsonString);
        //        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        Debug.Log(request.downloadHandler.text);
        Debug.Log("get data" + request.downloadHandler.text);
        version myDeserializedClass = Newtonsoft.Json.JsonConvert.DeserializeObject<version>(request.downloadHandler.text);
        Debug.Log("version: " + request.downloadHandler.text);


        if (myDeserializedClass != null && myDeserializedClass.success &&
            myDeserializedClass.response != null && myDeserializedClass.response.Count > 0)
        {

            if (versioncheck == myDeserializedClass.response[0].versions || myDeserializedClass.response[0].test)
            {
                if (apigetter.allofdata != "" && googlesignedin==0 && appleSignedin == 0)
                {
                    if(PlayerPrefs.GetInt("OnetimePopup", 0) == 0)
                    {
                        OneTimePopup();
                    }
                    else
                    {
                        SignInUserData suserdata = new SignInUserData();

                        suserdata.email = apigetter.email;
                        suserdata.password = apigetter.password;
                        String JsonVal = Newtonsoft.Json.JsonConvert.SerializeObject(suserdata);

                        StartCoroutine(SignIN_Upload(baseurl + "/v1/user/login", JsonVal));
                    }
                }
                else if (googlesignedin == 1)
                {
                    if(PlayerPrefs.GetInt("OnetimePopup", 0) == 0)
                    {
                        OneTimePopup();
                    }
                    else
                    {
                        Debug.Log("Auto Login");
                        if (loginWithGoogle != null)
                        {
                            loginWithGoogle.AutoLogin();
                        }
                        else
                        {
                            Debug.LogWarning("Auto Google login skipped because LoginWithGoogle reference is missing.");
                            loading.SetActive(false);
                        }
                    }

                }
                else if (appleSignedin == 1)
                {
                    if(PlayerPrefs.GetInt("OnetimePopup", 0) == 0)
                    {
                        OneTimePopup();
                    }
                    else
                    {
                        if (MainMenu != null)
                        {
                            MainMenu.GetAppleIdTokenOnly();
                        }
                        else
                        {
                            Debug.LogWarning("Auto Apple login skipped because MainMenu reference is missing.");
                            loading.SetActive(false);
                        }
                    }

                }
                else
                {
                    if (anim != null)
                    {
                        anim.Play("baloonss");
                    }
                    loading.SetActive(false);

                    newloader = GameObject.FindGameObjectWithTag("loader");



                    if (newloader != null)
                    {
                        newloader.GetComponent<dontdestroy>().loader.SetActive(false);

                    }

                }


            }
            else
            {
                if (versioncontrol.instance != null && versioncontrol.instance.popbox != null)
                {
                    versioncontrol.instance.header.text = "New version available!";
                    versioncontrol.instance.description.text = "Please update app for the latest features, filters and Blingets";
                    versioncontrol.instance.popbox.SetActive(true);
                }
                loading.SetActive(false);
            }



        }
        else
        {
            Debug.LogWarning("Version check response was null or empty.");
            loading.SetActive(false);
        }
    }






    private void Start()
    {

        versionupdate = versioncheck;
        baseurl = baselink.Url;
        // Application.targetFrameRate = 300;
        StartCoroutine(test());




        /* StartCoroutine(checkInternetConnection((isConnected) =>
         {
             // handle connection status here
         }));*/

        HideShowPassword();
    }
    












    private void Update()
    {


        if(INP_FirstName.text!="" && INP_LastName.text!="" /*&& INP_ConfirmPassword.text!=""*/&&INP_Password.text!=""&&INP_Emailaddress.text!=""/*&&INP_PostalCode.text!=""&&INP_City.text!=""*/&& Drp_Country.value!=0/*&&Drp_Age.value!=0 &&Drp_Gender.value!=0*/)
        {   

            start.interactable = true;

        }
        else
        {
            start.interactable = false;

        }

        if (Drp_Country.value != 1)
        {

            Drp_State.interactable = false;

        }
        else
        {

            Drp_State.interactable = true;
            
            
        }









        }

    }
