using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

[InitializeOnLoad]
internal static class CodexOneTimeIOSBuild
{
    private const string Request = "Library/CodexIOSBuild.request";
    private const string Result = "Library/CodexIOSBuild.result";

    static CodexOneTimeIOSBuild()
    {
        EditorApplication.update += Run;
    }

    private static void Run()
    {
        if (!File.Exists(Request))
        {
            EditorApplication.update -= Run;
            return;
        }
        if (EditorApplication.isCompiling || EditorApplication.isUpdating ||
            EditorApplication.isPlayingOrWillChangePlaymode || BuildPipeline.isBuildingPlayer)
            return;

        EditorApplication.update -= Run;
        string output = File.ReadAllText(Request).Trim();
        File.Delete(Request);
        try
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS)
                throw new InvalidOperationException("The active build target is not iOS.");
            Debug.Log("Codex iOS export starting: " + output);
            BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled)
                    .Select(scene => scene.path).ToArray(),
                locationPathName = output,
                target = BuildTarget.iOS,
                options = BuildOptions.None
            });
            string result = report.summary.result + " | errors: " + report.summary.totalErrors
                + " | output: " + Path.GetFullPath(output);
            File.WriteAllText(Result, result);
            Debug.Log("Codex iOS export completed: " + result);
        }
        catch (Exception exception)
        {
            File.WriteAllText(Result, exception.ToString());
            Debug.LogException(exception);
        }
    }
}
