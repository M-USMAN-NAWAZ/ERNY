using DG.Tweening.Core.Easing;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Google;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;

public class LoginWithGoogle : MonoBehaviour
{
    public static LoginWithGoogle instance;


    public ApiRequestGenerator ApiRequestGenerator;

    public string GoogleAPI =
    "892544376758-7jqorfgn45aik2nphos1c4k6210atalq.apps.googleusercontent.com";


    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    private GoogleSignInConfiguration configuration;

    public TextMeshProUGUI Username;
    public TextMeshProUGUI UserEmail;

    public Image UserProfilePic;
    private string imageUrl;
    private bool isGoogleSignInInitialized = false;


    public bool isCanceled = false;
    public bool isFaulted = false;
    public bool isIdTokenFalse = false;
    public bool isCanceled1 = false;
    public bool isFaulted1 = false;
    private static bool firebaseInitialized = false;

    public TMP_Text erroeTitel;
    public TMP_Text erroeMessage;
    public TMP_Text idToken;
    public GameObject errorNotificationPanel;
    public GameObject SignedInPanel;

    public string tokendata;
    private bool isSigningIn = false;
    private void Awake()
    {

        instance = this;


        configuration = new GoogleSignInConfiguration
        {
            WebClientId = GoogleAPI,
            RequestIdToken = true,
            RequestEmail = true,
            UseGameSignIn = false
        };
    }


    private void Start()
    {
        InitFirebase();
    }

    void InitFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Debug.Log("Inside Initialization");
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("Firebase is initialized and ready to use.");
                // Optional: FirebaseApp app = FirebaseApp.DefaultInstance;
            }
            else
            {
                Debug.LogError("Firebase initialization failed: " + dependencyStatus);
            }
        });
    }

    public void ShowErrorNotification(string _titel, string _message)
    {
        erroeTitel.text = _titel;
        erroeMessage.text = _message;
        errorNotificationPanel.SetActive(true);
    }

    public void GoogleSignInClick()
    {
        if (isSigningIn) return;
        isSigningIn = true;
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;
        Debug.Log("Inside GoogleSignInClick");
        GoogleSignIn.DefaultInstance.
       SignIn().ContinueWithOnMainThread(task =>
               {
                   Debug.Log("Google SignIn task returned");
                   OnGoogleAuthenticatedFinished(task);
               });
    }

    public void AutoLogin()
    {
        if (isSigningIn) return;
        isSigningIn = true;
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.DefaultInstance.SignInSilently()
            .ContinueWithOnMainThread(OnGoogleAuthenticatedFinishedJustForIdToken);
    }

    void OnGoogleAuthenticatedFinished(Task<GoogleSignInUser> task)
    {
        Debug.Log("Inside OnGoogleAuthenticatedFinished");
        try
        {
            if (task.IsCanceled)
            {
                isCanceled = true;
                isSigningIn = false;
                Debug.LogError("Cancelled");
            }
            else if (task.IsFaulted)
            {
                isFaulted = true;
                isSigningIn = false;
                Debug.LogError("Faulted");
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
            }
            else if (task.Result == null || !task.IsCompletedSuccessfully)
            {
                isIdTokenFalse = true;
                isSigningIn = false;
                Debug.LogError("IdToken is null or empty");
                return;
            }
            else
            {
                string gwtToken = task.Result.IdToken;
                Debug.LogError("IdToken inside the google initialize is: " + gwtToken);
                Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
                auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task => {
                    if (task.IsCanceled)
                    {
                        isCanceled1 = true;
                        isSigningIn = false;
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        isFaulted1 = true;
                        isSigningIn = false;
                        Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                        return;
                    }
                    user = auth.CurrentUser;
                    Username.text = user.DisplayName;
                    UserEmail.text = user.Email;
                    idToken.text = gwtToken;
                    tokendata = gwtToken;
                    Debug.Log("User Name: " + user.DisplayName);
                    Debug.Log("User Email: " + user.Email);
                    Debug.Log("User Id Token: " + gwtToken);
                    if (ApiRequestGenerator)
                    {
                        ApiRequestGenerator.googleIdToken = tokendata;
                    }
                    else
                    {
                        Debug.LogError("ApiRequestGenerator is null.");
                    }
                    if (tokendata != null)
                    {
                        ApiRequestGenerator.googleloginapi();
                    }
                    else
                    {
                        Debug.LogError("Token data is null.");
                    }
                        ApiRequestGenerator.loading.SetActive(true);
                    // Load profile picture
                    if (user.PhotoUrl != null)
                    {
                        StartCoroutine(LoadProfileImage(user.PhotoUrl.ToString()));
                    }
                    isSigningIn = false;
                });
            }
        }
        catch (GoogleSignIn.SignInException ex)
        {
            isSigningIn = false;
            Debug.Log("Google SignIn error: " + ex.Status);
        }
        IEnumerator LoadProfileImage(string imageUrl)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
            // Send request and wait for completion
            yield return request.SendWebRequest();
            if (!imageUrl.StartsWith("https"))
            {
                Debug.LogWarning("Insecure URL detected. Updating to HTTPS.");
                imageUrl = imageUrl.Replace("http://", "https://");
            }
            if (request.result == UnityWebRequest.Result.Success)
            {
                // Create texture from downloaded image
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                // Convert texture to Sprite
                Sprite profileSprite = Sprite.Create(texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f));
                // Apply sprite to the UI Image component
                UserProfilePic.sprite = profileSprite;
                Debug.Log("Profile picture loaded successfully.");
                //SignedInPanel.SetActive(true);

               
            }
            else
            {
                Debug.LogError("Failed to load profile picture: " + request.error);
            }
        }
    }
    void OnGoogleAuthenticatedFinishedJustForIdToken(Task<GoogleSignInUser> task)
    {
        try
        {
            if (task.IsCanceled)
            {
                isCanceled = true;
                isSigningIn = false;
                Debug.Log("Silent Google sign-in canceled.");
            }
            else if (task.IsFaulted)
            {
                var signInException = task.Exception?.Flatten().InnerExceptions
                    .FirstOrDefault(ex => ex is GoogleSignIn.SignInException) as GoogleSignIn.SignInException;

                // Silent sign-in commonly fails when there is no cached Google session on the device.
                // Treat Google Sign-In exceptions as a normal "not signed in yet" case and wait
                // for the user to tap the sign-in button instead of surfacing a scene-load error.
                if (signInException != null)
                {
                    isSigningIn = false;
                    Debug.Log("Silent Google sign-in unavailable. Waiting for interactive sign-in.");
                    return;
                }

                isFaulted = true;
                isSigningIn = false;
                Debug.LogError("Google sign-in faulted: " + task.Exception);
            }
            else if (task.Result == null || !task.IsCompletedSuccessfully)
            {
                isIdTokenFalse = true;
                isSigningIn = false;
                Debug.LogError("IdToken is null or empty");
                return;
            }
            else
            {
                string gwtToken = task.Result.IdToken;
                tokendata = gwtToken;
                idToken.text = gwtToken;

                Debug.Log("Google ID Token: " + gwtToken);

                

                // Send token to API
                ApiRequestGenerator.googleIdToken = tokendata;
                if (tokendata != null)
                {
                    ApiRequestGenerator.googleloginapi();
                }
                isSigningIn = false;
            }
        }
        catch (GoogleSignIn.SignInException ex)
        {
            isSigningIn = false;
            Debug.Log("Google SignIn error: " + ex.Status);
        }
    }


    public void Logout()
    {
        auth.SignOut();

        GoogleSignIn.DefaultInstance.SignOut();

        GoogleSignIn.DefaultInstance.Disconnect();

        Debug.Log("User logged out from Firebase and Google.");
    }

}
