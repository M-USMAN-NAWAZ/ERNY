using System;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

public class UGSInitializer : MonoBehaviour
{
    async void Awake()
    {
        // Optional: specify environment, like "production" or "test"
        var options = new InitializationOptions();
        options.SetEnvironmentName("production");

        try
        {
            await UnityServices.InitializeAsync(options);
            Debug.Log("Unity Services initialized.");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to initialize Unity Services: " + e);
        }
    }
}
