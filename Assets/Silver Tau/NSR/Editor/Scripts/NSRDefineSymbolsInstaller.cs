#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace SilverTau.NSR 
{
    internal static class NSR_DefineSymbolsConfig
    {
        public const string IOS = "NSR_PLATFORM_IOS";
        public const string ANDROID = "NSR_PLATFORM_ANDROID";
        public const string WINDOWS = "NSR_PLATFORM_WINDOWS";
        public const string MACOS = "NSR_PLATFORM_MACOS";
        public const string LINUX = "NSR_PLATFORM_LINUX";
        public const string STANDALONE = "NSR_PLATFORM_STANDALONE";
        public const string WEBGL = "NSR_PLATFORM_WEBGL";
    }

    [InitializeOnLoad]
    internal sealed class NSRDefineSymbolsInstaller : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder => -1000;
        
        private static readonly Dictionary<BuildTargetGroup, string> _definesBackup = new();

        static NSRDefineSymbolsInstaller()
        {
            EnsureBasePerGroupDefines();
        }

        [MenuItem("Window/Silver Tau/NSR - Screen Recorder/Sync NSR Define Symbols")]
        private static void ForceSyncNSRDefineSymbols()
        {
            EnsureBasePerGroupDefines(false);
            EditorUtility.DisplayDialog("NSR - Screen Recorder", "The symbols are synchronized.", "OK");
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            EnsureBasePerGroupDefines();

            var group = BuildPipeline.GetBuildTargetGroup(report.summary.platform);
            if (group == BuildTargetGroup.Standalone)
            {
                var toAdd = NSR_DefineSymbolsConfig.STANDALONE;
                string osSpecific = null;

                switch (report.summary.platform)
                {
                    case BuildTarget.StandaloneWindows:
                    case BuildTarget.StandaloneWindows64:
                        osSpecific = NSR_DefineSymbolsConfig.WINDOWS;
                        break;
                    case BuildTarget.StandaloneOSX:
                        osSpecific = NSR_DefineSymbolsConfig.MACOS;
                        break;
                    case BuildTarget.StandaloneLinux64:
                        osSpecific = NSR_DefineSymbolsConfig.LINUX;
                        break;
                }
                
                if (!_definesBackup.ContainsKey(group))
                    _definesBackup[group] = GetDefines(group);

                var defines = SplitDefines(_definesBackup[group]);

                defines.Remove(NSR_DefineSymbolsConfig.WINDOWS);
                defines.Remove(NSR_DefineSymbolsConfig.MACOS);
                defines.Remove(NSR_DefineSymbolsConfig.LINUX);

                defines.Add(toAdd);
                if (!string.IsNullOrEmpty(osSpecific)) defines.Add(osSpecific);

                SetDefines(group, JoinDefines(defines));
            }
            else
            {
                EnsureGroup(report.summary.platform);
            }
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            var group = BuildPipeline.GetBuildTargetGroup(report.summary.platform);
            if (group == BuildTargetGroup.Standalone && _definesBackup.TryGetValue(group, out var prev))
            {
                SetDefines(group, prev);
                _definesBackup.Remove(group);
            }
        }

        public static void EnsureBasePerGroupDefines(bool log = false)
        {
            EnsureGroup(BuildTarget.Android, extra: new[] { NSR_DefineSymbolsConfig.ANDROID });
            EnsureGroup(BuildTarget.iOS, extra: new[] { NSR_DefineSymbolsConfig.IOS });
            EnsureGroup(BuildTarget.StandaloneWindows64, extra: new[] { NSR_DefineSymbolsConfig.STANDALONE });
            EnsureGroup(BuildTarget.WebGL, extra: new[] { NSR_DefineSymbolsConfig.WEBGL });

            if (log) UnityEngine.Debug.Log("[NSR - Screen Recorder] Base per-group symbols ensured.");
        }

        private static void EnsureGroup(BuildTarget target, IEnumerable<string> extra = null)
        {
            var group   = BuildPipeline.GetBuildTargetGroup(target);
            var current = SplitDefines(GetDefines(group));
            
            if (extra != null)
                foreach (var s in extra) current.Add(s);

            SetDefines(group, JoinDefines(current));
        }

        private static HashSet<string> SplitDefines(string defines)
        {
            var set = new HashSet<string>(StringComparer.Ordinal);
            if (!string.IsNullOrEmpty(defines))
            {
                foreach (var d in defines.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                    set.Add(d.Trim());
            }
            return set;
        }

        private static string JoinDefines(HashSet<string> set) =>
            string.Join(";", set.Where(s => !string.IsNullOrWhiteSpace(s)).OrderBy(s => s, StringComparer.Ordinal));

    #if UNITY_2021_2_OR_NEWER
        private static string GetDefines(BuildTargetGroup group)
        {
            NamedBuildTarget nbt = ToNamed(group);
            PlayerSettings.GetScriptingDefineSymbols(nbt, out var defines);
            return (defines == null || defines.Length == 0) ? string.Empty : string.Join(";", defines);
        }

        private static void SetDefines(BuildTargetGroup group, string defines)
        {
            NamedBuildTarget nbt = ToNamed(group);
            PlayerSettings.SetScriptingDefineSymbols(nbt, defines ?? string.Empty);
        }

        private static NamedBuildTarget ToNamed(BuildTargetGroup group)
        {
            return NamedBuildTarget.FromBuildTargetGroup(group);
        }
    #else
        private static string GetDefines(BuildTargetGroup group) =>
            PlayerSettings.GetScriptingDefineSymbolsForGroup(group) ?? string.Empty;

        private static void SetDefines(BuildTargetGroup group, string defines) =>
            PlayerSettings.SetScriptingDefineSymbolsForGroup(group, defines ?? string.Empty);
    #endif
    }
}
#endif
