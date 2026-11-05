using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

// Repacking changes ZIP compression, not the signed contents of an Android App Bundle.
public sealed class AndroidAabCompression : IPostprocessBuildWithReport
{
    public int callbackOrder => int.MaxValue;

    public void OnPostprocessBuild(BuildReport report)
    {
        if (report.summary.platform != BuildTarget.Android ||
            !report.summary.outputPath.EndsWith(".aab", StringComparison.OrdinalIgnoreCase)) return;
        Compress(report.summary.outputPath);
    }

    public static void Compress(string bundle)
    {
        string tool = Path.Combine(EditorApplication.applicationContentsPath, "Tools", "7z.exe");
        if (!File.Exists(tool))
            throw new BuildFailedException("The Unity 7-Zip tool is required for lossless AAB compression: " + tool);
        string original = Path.GetFullPath(bundle);
        string temporary = Path.GetFullPath(Path.Combine("Library", "AabCompression", Guid.NewGuid().ToString("N")));
        string input = Path.Combine(temporary, "input");
        string output = Path.Combine(temporary, "compressed.aab");
        Directory.CreateDirectory(input);
        try
        {
            ZipFile.ExtractToDirectory(original, input);
            var start = new ProcessStartInfo(tool,
                "a -tzip -mm=Deflate -mx=9 -mfb=258 -mpass=15 \"" + output + "\" . -bso0 -bsp0")
            {
                WorkingDirectory = input,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using (var process = Process.Start(start))
            {
                if (process == null) throw new BuildFailedException("Could not start AAB compression.");
                process.WaitForExit();
                if (process.ExitCode != 0) throw new BuildFailedException("AAB compression failed; the original is unchanged.");
            }
            // Google Play bundles forbid directory entries, which 7-Zip adds by default.
            using (var archive = ZipFile.Open(output, ZipArchiveMode.Update))
                for (int i = archive.Entries.Count - 1; i >= 0; i--)
                    if (archive.Entries[i].FullName.EndsWith("/")) archive.Entries[i].Delete();
            VerifyContents(original, output);
            if (new FileInfo(output).Length < new FileInfo(original).Length)
                File.Copy(output, original, true);
            double size = new FileInfo(original).Length / (1024d * 1024d);
            UnityEngine.Debug.Log("Losslessly compressed AAB: " + size.ToString("F2") + " MB | " + original);
            if (size >= 150d)
                throw new BuildFailedException("The compressed AAB exceeds the 150 MB limit. The file is preserved, but the build is not approved for release.");
        }
        finally
        {
            // Only this invocation's generated directory is removed.
            Directory.Delete(temporary, true);
        }
    }

    private static void VerifyContents(string original, string compressed)
    {
        using (var before = ZipFile.OpenRead(original))
        using (var after = ZipFile.OpenRead(compressed))
        using (var hash = SHA256.Create())
        {
            int files = 0, copiedFiles = 0;
            foreach (var entry in after.Entries)
                if (!entry.FullName.EndsWith("/")) copiedFiles++;
            foreach (var entry in before.Entries)
            {
                if (entry.FullName.EndsWith("/")) continue;
                files++;
                var copy = after.GetEntry(entry.FullName);
                if (copy == null || copy.Length != entry.Length)
                    throw new BuildFailedException("AAB content verification failed: " + entry.FullName);
                using (var a = entry.Open())
                using (var b = copy.Open())
                    if (Convert.ToBase64String(hash.ComputeHash(a)) != Convert.ToBase64String(hash.ComputeHash(b)))
                        throw new BuildFailedException("AAB file changed during compression: " + entry.FullName);
            }
            if (files != copiedFiles) throw new BuildFailedException("AAB file count changed during compression.");
        }
    }
}
