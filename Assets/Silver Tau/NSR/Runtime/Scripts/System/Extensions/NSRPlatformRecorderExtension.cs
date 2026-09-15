using System;
using UnityEngine;
using SilverTau.NSR.Core.Android;
using SilverTau.NSR.Core.iOS;
using SilverTau.NSR.Core.MacOS;
using SilverTau.NSR.Core.Windows;
using SilverTau.NSR.Recorders.Internal;

namespace SilverTau.NSR.Core
{
    public static class NSRPlatformRecorderExtension
    {
        public static void SetPlatformRecorderSettings(this NSR_VideoRecorder nsrVideoRecorder, AndroidRecorderSettings androidRecorderSettings = null, iOSRecorderSettings iOSRecorderSettings = null, MacOSRecorderSettings macOSRecorderSettings = null, WindowsRecorderSettings windowsRecorderSettings = null)
        {
            if (nsrVideoRecorder.settings == null)
            {
                Debug.LogWarning("The recorder settings are not configured. Please configure the settings.");
                return;
            }
            
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    nsrVideoRecorder.settings.mediaSettings = androidRecorderSettings != null ? androidRecorderSettings.GetMediaSettings() : null;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    nsrVideoRecorder.settings.mediaSettings = iOSRecorderSettings != null ? iOSRecorderSettings.GetMediaSettings() : null;
                    break;
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.OSXServer:
                case RuntimePlatform.OSXEditor:
                    nsrVideoRecorder.settings.mediaSettings = macOSRecorderSettings != null ? macOSRecorderSettings.GetMediaSettings() : null;
                    break;
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsServer:
                case RuntimePlatform.WindowsEditor:
                    nsrVideoRecorder.settings.mediaSettings = windowsRecorderSettings != null ? windowsRecorderSettings.GetMediaSettings() : null;
                    break;
                default:
                    return;
            }
        }
    }
}