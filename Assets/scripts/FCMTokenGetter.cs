using UnityEngine;
using Firebase;
using Firebase.Messaging;

public class FirebasePushManager : MonoBehaviour
{
    void Start()
    {
        // Always check Firebase dependencies first
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                Debug.Log("Firebase ready!");

                // Subscribe to token events (fires automatically once APNs+FCM token is ready)
                FirebaseMessaging.TokenReceived += OnTokenReceived;

#if UNITY_IOS
                FirebaseMessaging.RequestPermissionAsync().ContinueWith(permissionTask =>
                {
                    Debug.Log("Requested iOS push notification permission.");

                    FirebaseMessaging.GetTokenAsync().ContinueWith(tokenTask =>
                    {
                        if (tokenTask.IsCompleted && !tokenTask.IsFaulted)
                        {
                            Debug.Log("Initial FCM Token: " + tokenTask.Result);
                        }
                        else
                        {
                            Debug.LogWarning("Could not retrieve FCM token yet. Will receive it via TokenReceived event.");
                        }
                    });
                });
#else
FirebaseMessaging.GetTokenAsync().ContinueWith(tokenTask =>
{
    if (tokenTask.IsCompleted && !tokenTask.IsFaulted)
    {
        Debug.Log("FCM Token: " + tokenTask.Result);
    }
});
#endif
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });
    }

    private void OnTokenReceived(object sender, TokenReceivedEventArgs e)
    {
        Debug.Log("FCM token received (event): " + e.Token);

        // Example: save to PlayerPrefs
        PlayerPrefs.SetString("FCMToken", e.Token);
    }
}
