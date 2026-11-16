using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public sealed class AndroidPhoneBootSettings : IPreprocessBuildWithReport
{
    // Run after the XR package callbacks have written their boot settings.
    public int callbackOrder => int.MaxValue;

    public void OnPreprocessBuild(BuildReport report)
    {
        if (report.summary.platform != BuildTarget.Android)
            return;

        const string settingsKey = "xr-boot-settings";
        string platform = BuildPipeline.GetBuildTargetName(BuildTarget.Android);
        string settings = EditorUserBuildSettings.GetPlatformSettings(platform, settingsKey);
        if (string.IsNullOrEmpty(settings))
            return;

        string filtered = string.Join(";", settings.Split(';').Where(setting =>
        {
            switch (setting.Split(':')[0])
            {
                case "android-device-have-window-support":
                case "android-device-use-null-display-manager":
                case "android-audio-use-MLAudio":
                case "vulkan-force-disable-ETC-support":
                case "vulkan-force-disable-ASTC-support":
                case "vulkan-disable-pre-transform":
                    return false;
                default:
                    return true;
            }
        }));

        if (!string.Equals(settings, filtered, StringComparison.Ordinal))
            EditorUserBuildSettings.SetPlatformSettings(platform, settingsKey, filtered);
    }
}
