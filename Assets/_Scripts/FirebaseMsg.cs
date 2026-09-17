using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Messaging;
using System;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.UI;

public class FirebaseMsg : MonoBehaviour
{

    private FirebaseFirestore db;
	public string[] codes;
	public static int firebasecheck,codebadge2, codebadge3;
	//public GameObject Cont;
	public GameObject canvas;

    public ContentSizeFitter vLayoutGroup;
    public ContentSizeFitter contentSizeFitter;

   
    // Start is called before the first frame update
    void Start()
    {
        firebasecheck = 0;
        codebadge2 = 0;
        codebadge3 = 0;
        // eventgetter.instance.clubbadge.SetActive(false);
        // eventgetter.instance.clubbadge1.SetActive(false);
        // eventgetter.instance.clubbadge2.SetActive(false);
        // eventgetter.instance.clubbadge3.SetActive(false);
        //Debug.LogError($"Firebase Start");
        if (baselink.firebaseioscheck < 1)
		{
            Firebase.Messaging.FirebaseMessaging.TokenReceived += TokenRecieved;
			Firebase.Messaging.FirebaseMessaging.MessageReceived += MessageRecieved;
			SubscribeFunc();
			baselink.firebaseioscheck += 1;
		}
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            db = FirebaseFirestore.GetInstance(app);
            // Call the method to fetch the field value
          
          

            FetchFieldValue();
        });
    }
   

    public void IncreaseSize()
    {
        //StartCoroutine(IncreaseSizeCoroutine());
    }


    public IEnumerator IncreaseSizeCoroutine()
	{
        vLayoutGroup.enabled = false;
        contentSizeFitter.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        vLayoutGroup.enabled = true;
        contentSizeFitter.gameObject.SetActive(true);

        Debug.Log("refreshed");
    }

	void SubscribeFunc()
	{
		Firebase.Messaging.FirebaseMessaging.SubscribeAsync("/topics/UsamaTKD");
        //Debug.LogError($"SUSCRIBE");
    }

	private void MessageRecieved(object sender, MessageReceivedEventArgs e)
	{
	
		//Debug.LogError("Nitification Recieved From Firebase: " + e.Message);
	}

	private void TokenRecieved(object sender, TokenReceivedEventArgs e)
	{
		//Debug.LogError("Token Recieved From Firebase: " + e.Token);
		
	}

    private string GetSnapshotString(DocumentSnapshot snapshot, string fieldName)
    {
        return snapshot.ContainsField(fieldName) ? snapshot.GetValue<string>(fieldName) : "";
    }

	public void FetchFieldValue2()
	{
    }

	public void FetchFieldValue()
	{
        //Debug.LogError("Inside FetchFieldValue");
        // Replace "ads" with your actual Firestore collection name and "AD" with the document name
        DocumentReference docRef = db.Collection("user").Document(apigetter.email);
		docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
		{
			if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
			{
				DocumentSnapshot snapshot = task.Result;
			
				if (snapshot.Exists)
				{
                    string code = GetSnapshotString(snapshot, "code");
                    string code1 = GetSnapshotString(snapshot, "code1");
                    string code2 = GetSnapshotString(snapshot, "code2");

                    bool hasClubD3 = code == "Club D3" || code1 == "Club D3" || code2 == "Club D3";
                    bool hasSlowAf = code == "SLOWAF" || code1 == "SLOWAF" || code2 == "SLOWAF";
                    bool hasUrc = code == "URC" || code1 == "URC" || code2 == "URC";
                    bool hasAnyClubBadge = hasClubD3 || hasSlowAf || hasUrc;

                    firebasecheck = hasClubD3 ? 1 : 0;
                    codebadge2 = hasSlowAf ? 1 : 0;
                    codebadge3 = hasUrc ? 1 : 0;

                    // eventgetter.instance.clubbadge.SetActive(hasAnyClubBadge);
                    // eventgetter.instance.clubbadge2.SetActive(hasClubD3);
                    // eventgetter.instance.clubbadge3.SetActive(hasSlowAf);
                    // eventgetter.instance.clubbadge1.SetActive(hasUrc);

                    if (hasAnyClubBadge)
                    {
                        IncreaseSize();
                    }
                }
				else
				{
					//Debug.LogError("Document does not exist.");
                    firebasecheck = 0;
                    codebadge2 = 0;
                    codebadge3 = 0;
                    // eventgetter.instance.clubbadge.SetActive(false);
                    // eventgetter.instance.clubbadge2.SetActive(false);
                    // eventgetter.instance.clubbadge3.SetActive(false);
                    // eventgetter.instance.clubbadge1.SetActive(false);
                }
			}
			else
			{
				Debug.LogError("Error fetching document: " + task.Exception);
			}
		});


    }
	public void checkcode()
	{
		if (eventgetter.instance.clubcode.text == codes[0])
		{
			Debug.Log("check answer 1");
			CreateUserInFirestore(codes[0], apigetter.email, apigetter.firstname);
		}
		else if (eventgetter.instance.clubcode.text == codes[1])
		{
			Debug.Log("check answer");
			CreateUserInFirestore2(codes[1], apigetter.email, apigetter.firstname);
		}
		else if (eventgetter.instance.clubcode.text == codes[2])
		{
			Debug.Log("check answer3");
			CreateUserInFirestore3(codes[2], apigetter.email, apigetter.firstname);
		}
		else
		{

			eventgetter.instance.clubcode.text = "wrong code";
            Debug.Log("Wrong Code");

		}


	}


    private async void CreateUserInFirestore(string code, string email, string displayName)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference userRef = db.Collection("user").Document(email);
        DocumentSnapshot snapshot = await userRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            if (snapshot.ContainsField("code1"))
            {
                string temp;

				temp = snapshot.GetValue<string>("code1");
				
                Debug.Log("User already exists in Firestore");

                // Create user document if it doesn't exist
                Dictionary<string, object> user = new Dictionary<string, object>
            {
                { "code", code },
                { "code1", temp }, // ensure we don't lose old data
                { "email", email },
                { "displayName", displayName },
            };

                await userRef.SetAsync(user);
                firebasecheck = 1;
                codebadge2 = 1;
            }
            else
            {
                Debug.Log("User already exists in Firestore");

                // Create user document if it doesn't exist
                Dictionary<string, object> user = new Dictionary<string, object>
                {
                    { "code", code },
                    { "email", email },
                    { "displayName", displayName },
                };

                await userRef.SetAsync(user);
                firebasecheck = 1;
               
            }
        }
        else
        {
            // Create user document if it doesn't exist
            Dictionary<string, object> user = new Dictionary<string, object>
            {
                { "code", code },
                { "email", email },
                { "displayName", displayName },
            };

            await userRef.SetAsync(user);
            Debug.Log("User created in Firestore successfully");
            //eventgetter.instance.clubbadge.SetActive(true);
            firebasecheck = 1;
            Debug.Log("THIS IS THE NEW DEBUG");
            IncreaseSize();
        }
    }

    private async void CreateUserInFirestore2(string code, string email, string displayName)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference userRef = db.Collection("user").Document(email);
        DocumentSnapshot snapshot = await userRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            if (snapshot.ContainsField("code"))
            {
                string temp;

                // Safely get the existing "code1" if it exists

                temp = snapshot.GetValue<string>("code");

                Debug.Log("User already exists in Firestore");

                // Create user document if it doesn't exist
                Dictionary<string, object> user = new Dictionary<string, object>
            {
                { "code", snapshot.GetValue<string>("code") }, // keep old "code"
                { "code1", code }, // set new "code1"
                { "email", email },
                { "displayName", displayName },
            };

                await userRef.SetAsync(user);
                firebasecheck = 1;
                codebadge2 = 1;

            }
            else
            {
                Debug.Log("User already exists in Firestore");

                // Create user document if it doesn't exist
                Dictionary<string, object> user = new Dictionary<string, object>
            {
                { "code1", code },
                { "email", email },
                { "displayName", displayName },
            };

                await userRef.SetAsync(user);
                codebadge2 = 1;

            }
        }
        else
        {
            // Create user document if it doesn't exist
            Dictionary<string, object> user = new Dictionary<string, object>
            {
                { "code1", code },
                { "email", email },
                { "displayName", displayName },
            };

            await userRef.SetAsync(user);
            Debug.Log("User created in Firestore successfully");
            // eventgetter.instance.clubbadge.SetActive(true);
            // eventgetter.instance.clubbadge3.SetActive(true);
            codebadge2 = 1;
            IncreaseSize();
        }
    }
    private async void CreateUserInFirestore3(string code, string email, string displayName)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference userRef = db.Collection("user").Document(email);
        DocumentSnapshot snapshot = await userRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            if (snapshot.ContainsField("code"))
            {
                string temp;

                // Safely get the existing "code1" if it exists

                temp = snapshot.GetValue<string>("code");

                Debug.Log("User already exists in Firestore");

                // Create user document if it doesn't exist
                Dictionary<string, object> user = new Dictionary<string, object>
            {
                { "code", snapshot.GetValue<string>("code") }, // keep old "code"
                { "code1", snapshot.GetValue<string>("code1") }, // keep old "code"
                { "code2", code }, // set new "code1"
                { "email", email },
                { "displayName", displayName },
            };

                await userRef.SetAsync(user);
                firebasecheck = 1;
                codebadge3 = 1;

            }
            else
            {
                Debug.Log("User already exists in Firestore");

                // Create user document if it doesn't exist
                Dictionary<string, object> user = new Dictionary<string, object>
            {
                { "code2", code },
                { "email", email },
                { "displayName", displayName },
            };

                await userRef.SetAsync(user);
                codebadge3 = 1;

            }
        }
        else
        {
            // Create user document if it doesn't exist
            Dictionary<string, object> user = new Dictionary<string, object>
            {
                { "code1", code },
                { "email", email },
                { "displayName", displayName },
            };

            await userRef.SetAsync(user);
            Debug.Log("User created in Firestore successfully");
            // eventgetter.instance.clubbadge.SetActive(true);
            // eventgetter.instance.clubbadge1.SetActive(true);
            codebadge3 = 1;
            IncreaseSize();
        }
    }
    //private async void CreateUserInFirestore2(string newCode, string email, string displayName)
    //{
    //    FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
    //    DocumentReference userRef = db.Collection("user").Document(email);
    //    DocumentSnapshot snapshot = await userRef.GetSnapshotAsync();

    //    Dictionary<string, object> user = new Dictionary<string, object>();

    //    if (snapshot.Exists)
    //    {
    //        bool hasCode = snapshot.ContainsField("code");
    //        bool hasCode1 = snapshot.ContainsField("code1");
    //        bool hasCode2 = snapshot.ContainsField("code2");

    //        // Preserve existing values
    //        if (hasCode) user["code"] = snapshot.GetValue<string>("code");
    //        if (hasCode1) user["code1"] = snapshot.GetValue<string>("code1");
    //        if (hasCode2) user["code2"] = snapshot.GetValue<string>("code2");

    //        // 🔥 Decide where to put the NEW code
    //        if (!hasCode)
    //        {
    //            user["code"] = newCode;
    //        }
    //        else if (!hasCode1)
    //        {
    //            user["code1"] = newCode;
    //        }
    //        else if (!hasCode2)
    //        {
    //            user["code2"] = newCode;
    //        }
    //        else
    //        {
    //            Debug.LogWarning("User already has 3 codes");
    //            return;
    //        }
    //    }
    //    else
    //    {
    //        // New user → first code
    //        user["code"] = newCode;
    //    }

    //    user["email"] = email;
    //    user["displayName"] = displayName;

    //    // ✅ Merge so nothing is deleted
    //    await userRef.SetAsync(user, SetOptions.MergeAll);

    //    Debug.Log("New code added successfully");

    //    firebasecheck = 1;
    //    codebadge2 = 1;
    //    IncreaseSize();
    //}


}
