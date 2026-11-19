using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

internal static class ErnyLocalPurchaseBuild
{
    [MenuItem("Tools/Build ERNY Purchase Test AAB")]
    private static void Build()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode || BuildPipeline.isBuildingPlayer)
        {
            Debug.LogError("Leave Play Mode before starting the purchase test build.");
            return;
        }

        const string path = "Builds/Android/ErnyAR_purchasefix_78.aab";
        if (File.Exists(path))
        {
            Debug.LogError($"The build output already exists at {path}.");
            return;
        }

        Directory.CreateDirectory(Path.GetDirectoryName(path));
        EditorUserBuildSettings.buildAppBundle = true;
        PlayerSettings.Android.bundleVersionCode = Mathf.Max(PlayerSettings.Android.bundleVersionCode, 78);
        AssetDatabase.SaveAssets();
        BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray(),
            locationPathName = path,
            target = BuildTarget.Android,
            options = BuildOptions.CompressWithLz4
        });
        Debug.Log($"ERNY purchase test bundle {report.summary.result} at {report.summary.outputPath}");
    }
}
