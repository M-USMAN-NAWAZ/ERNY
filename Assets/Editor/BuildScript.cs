using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;

public static class BuildScript
{
    private const string AndroidOutputPathArgument = "-androidOutputPath";
    private const string IOSOutputPathArgument = "-iosOutputPath";

    private static string[] GetEnabledScenes()
    {
        return EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();
    }

    private static string GetArgumentValue(string argumentName)
    {
        string[] args = Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (string.Equals(args[i], argumentName, StringComparison.OrdinalIgnoreCase))
            {
                return args[i + 1];
            }
        }

        return null;
    }

    private static void BuildPlayer(BuildTarget target, string outputPath, string targetName)
    {
        var scenes = GetEnabledScenes();
        if (scenes.Length == 0)
        {
            throw new Exception("No enabled scenes found in Build Settings.");
        }

        string outputDirectory = Path.GetDirectoryName(outputPath);
        if (string.IsNullOrWhiteSpace(outputDirectory))
        {
            throw new Exception($"Could not resolve {targetName} output directory from {outputPath}");
        }

        Directory.CreateDirectory(outputDirectory);

        var options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = outputPath,
            target = target,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);
        if (report.summary.result != BuildResult.Succeeded)
        {
            throw new Exception($"{targetName} build failed: {report.summary.result}");
        }

        Console.WriteLine($"{targetName} build completed: {options.locationPathName}");
    }

    public static void BuildAndroid()
    {
        string outputPath = GetArgumentValue(AndroidOutputPathArgument);
        if (string.IsNullOrWhiteSpace(outputPath))
        {
            outputPath = "Builds/Android/Erny.apk";
        }

        BuildPlayer(BuildTarget.Android, outputPath, "Android");
    }

    public static void BuildIOS()
    {
        string outputPath = GetArgumentValue(IOSOutputPathArgument);
        if (string.IsNullOrWhiteSpace(outputPath))
        {
            outputPath = "Builds/iOS/Erny";
        }

        BuildPlayer(BuildTarget.iOS, outputPath, "iOS");
    }
}
