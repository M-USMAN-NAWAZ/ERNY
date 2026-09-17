using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SilverTau.NSR.EditorTools
{
    public sealed class WelcomeScreenWindow : EditorWindow
    {
        const string TITLE = "NSR - Screen Recorder";
        const string SUBTITLE = "High-performance video recording for Unity";
        static readonly string DESCRIPTION = 
@"NSR - Screen Recorder - Cross-Platform Video Capture allows you to easily record your application's screen in Unity. The plugin provides an api for recording video on iOS, Android, Windows, macOS and is a cross-platform solution.

The plugin has the ability to record any camera layer!

<b>High-Speed:</b> Engineered and extensively optimized for superior performance.
<b>Capture Everything!</b> Capture any visual content that can be transformed into a texture, whether it's a gaming interface, user interface, camera feed, or texture.
<b>Tailored Resolutions:</b> Capture videos with resolutions as high as Full HD (1920x1080) or even higher if your device supports it.
<b>Augmented Reality Support:</b> The package provides complete compatibility with ARFoundation, ARCore, ARKit, and Vuforia.
<b>Concurrent Recording:</b> The package ensures thread safety, enabling recording in worker threads to enhance performance even further.
<b>Share utility:</b> With this utility, you can easily share any files and folders.
<b>Gallery utility:</b> It's a simple and convenient way to save content to your target device.
<b>File manager utility:</b> Manage files from any of your storage locations with ease.
<b>Lightweight Integration:</b> The API is purposefully designed to minimize unnecessary additions or extra burden on your project.

<b>Supported platforms</b>
• iOS
• Android
• macOS
• Windows

---
<b>New in 2.0 - Massive Upgrade to Formats & Codecs:</b>
<b>Android:</b> MP4, WEBM, 3GP | VP8, VP9, AVC (H.264), H.265 (HEVC), MPEG4, H263 | AAC (LC, HE_V1, HE_V2, ELD, XHE), VORBIS, OPUS

<b>iOS & macOS:</b> MP4, MOV, M4V, 3GP, 3GPP, MPEG4 | H.264, H.265 (HEVC), H.265 (HEVCWithAlpha), Apple ProRes Family (4444, XQ, 422, HQ, 422 LT, Proxy), JPEG, JPEGXL, MotionJPEG, MPEG4Video | LinearPCM, AAC (LC, HE_V1, HE_V2, ELD, XHE), Apple Lossless, OPUS

<b>Windows:</b> MP4, M4V | H.264, H.265 (HEVC) | AAC (LC)
---

<b>Highlights</b>
• Easy API for Recording Video
• Full source code included (does not include original source code of native libraries).
• Share any files
• Save pictures and videos to the Gallery

<b>Feature set</b>
• Watermark
• Share any files
• Audio recorder
• Save pictures and videos to the Gallery
• Simple file manager
• Render Texture support
• HDR (high dynamic range) support
• Transparent video recording (Editor Recorder)
• Screen Record with microphone
• Graphic Provider (Screenshot & Image system)
• Editor Screen Record
• Preview Recorded Video
• Get recorded file path
• Save recorded video
• Pause/Resume video recorder
• Automatically pause/resume video recording during program focus/pause
• Custom frame rate
• Edit recorded video (iOS Native Framework)
";

        const string WEBSITE_URL = "https://www.silvertau.com/products/nsr-screen-recorder";
        const string DOCSONLINE_URL = "https://silvertau.s3.eu-central-1.amazonaws.com/NSR-ScreenRecorder/Documentation/docs-page.html#section-1";
        const string DOCSOFFLINE_PATH = "Assets/Silver Tau/NSR/Documentation.pdf";
        const string FAQ_URL = "https://silvertau.s3.eu-central-1.amazonaws.com/NSR-ScreenRecorder/Documentation/docs-page.html#section-6";
        const string CONTACT_URL = "https://www.silvertau.com/#contactus";
        
        const string CHANGELOG_URL = "https://silvertau.s3.eu-central-1.amazonaws.com/NSR-ScreenRecorder/Documentation/docs-page.html#item-5-3";
        const string UPGRADEGUIDE_URL = "https://silvertau.s3.eu-central-1.amazonaws.com/NSR-ScreenRecorder/Documentation/docs-page.html#item-5-2";

        const string ICON_RESOURCE_PATH = "Assets/Silver Tau/NSR/Editor/Images/icon_small.png";
        
        const string USERSET_KEY_SHOW = "SilverTau.NSR.Readme.ShowOnStartup";

        Vector2 _scroll;
        Texture2D _icon;

        [MenuItem("Window/Silver Tau/NSR - Screen Recorder/NSR - Welcome Screen", false, 0)]
        public static void Open()
        {
            var win = GetWindow<WelcomeScreenWindow>("NSR - Welcome Screen", true);
            win.minSize = new Vector2(520, 420);
            win.Show();
        }

        internal static void OpenIfAllowedOnceThisSession()
        {
            var show = EditorUserSettings.GetConfigValue(USERSET_KEY_SHOW);
            bool allow = string.IsNullOrEmpty(show) || show == "1";
            if (!allow) return;

            Open();
        }

        void OnEnable()
        {
            _icon = EditorGUIUtility.Load(ICON_RESOURCE_PATH) as Texture2D;
        }

        void OnGUI()
        {
            DrawHeader();
            EditorGUILayout.Space(8);

            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUILayout.VerticalScope())
                {
                    _scroll = EditorGUILayout.BeginScrollView(_scroll);
                    DrawBody();
                    EditorGUILayout.EndScrollView();
                }

                using (new EditorGUILayout.VerticalScope(GUILayout.Width(220)))
                {
                    DrawActions();
                }
            }
        }

        void DrawHeader()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                GUILayout.Space(8);
                var iconSize = 72f;

                if (_icon != null)
                {
                    GUILayout.Label(_icon, GUILayout.Width(iconSize), GUILayout.Height(iconSize));
                }
                else
                {
                    var rect = GUILayoutUtility.GetRect(iconSize, iconSize, GUILayout.Width(iconSize), GUILayout.Height(iconSize));
                    EditorGUI.DrawRect(rect, new Color(0.17f, 0.17f, 0.17f, 1));
                    GUI.Label(rect, "NSR", EditorStyles.centeredGreyMiniLabel);
                }

                GUILayout.Space(10);

                using (new EditorGUILayout.VerticalScope())
                {
                    GUILayout.Space(6);
                    var titleStyle = new GUIStyle(EditorStyles.label) { fontSize = 20, fontStyle = FontStyle.Bold, wordWrap = true };
                    EditorGUILayout.LabelField(TITLE, titleStyle);

                    if (!string.IsNullOrEmpty(SUBTITLE))
                        EditorGUILayout.LabelField(SUBTITLE, EditorStyles.wordWrappedLabel);
                }
                GUILayout.FlexibleSpace();
            }
        }

        void DrawBody()
        {
            var wordWrappedLabel = EditorStyles.wordWrappedLabel;
            wordWrappedLabel.richText = true;
            
            EditorGUILayout.LabelField("Overview", EditorStyles.boldLabel);
            EditorGUILayout.LabelField(DESCRIPTION, wordWrappedLabel);
        }

        void DrawActions()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Quick Links", EditorStyles.boldLabel);

                LinkButton("Website", WEBSITE_URL);
                LinkButton("Documentation", DOCSONLINE_URL);
                FileButton("Documentation (pdf)", DOCSOFFLINE_PATH);
                LinkButton("FAQ", FAQ_URL);
                LinkButton("Contact / Support", CONTACT_URL);
                
                EditorGUILayout.Space(8);
                
                EditorGUILayout.LabelField("What's new?", EditorStyles.boldLabel);
                
                LinkButton("Changelog", CHANGELOG_URL);

                EditorGUILayout.Space(8);
                
                EditorGUILayout.LabelField("Version History", EditorStyles.boldLabel);
                
                LinkButton("Upgrade guide (1.x > 2.0)", UPGRADEGUIDE_URL);

                EditorGUILayout.Space(8);
                
                EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
                
                FuncButton("Quick Check & Setup", () =>
                {
                    NSREditor.ValidateNSRScreenRecorder();
                });
                
                FuncButton("Sync NSR Define Symbols", () =>
                {
                    NSRDefineSymbolsInstaller.EnsureBasePerGroupDefines(false);
                    EditorUtility.DisplayDialog("NSR - Screen Recorder", "The symbols are synchronized.", "OK");
                });
                
                EditorGUILayout.Space(8);

                bool show = ShouldShowOnStartup();
                bool newShow = EditorGUILayout.ToggleLeft("Show on startup", show);
                if (newShow != show)
                    SetShowOnStartup(newShow);
                    
                EditorGUILayout.Space(8);

                if (GUILayout.Button("Close", EditorStyles.miniButton))
                {
                    NSRDefineSymbolsInstaller.EnsureBasePerGroupDefines(false);
                    Close();
                }
            }
        }

        static void LinkButton(string label, string url)
        {
            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(url)))
            {
                if (GUILayout.Button(label, EditorStyles.miniButton))
                    Application.OpenURL(url);
            }
        }

        static void FileButton(string label, string url)
        {
            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(url)))
            {
                if (GUILayout.Button(label, EditorStyles.miniButton))
                    SelectFile(url);
            }
        }

        static void FuncButton(string label, Action callback = null)
        {
            using (new EditorGUI.DisabledScope(callback == null))
            {
                if (GUILayout.Button(label, EditorStyles.miniButton))
                    callback?.Invoke();
            }
        }

        private static void SelectFile(string assetPath)
        {
            if(string.IsNullOrEmpty(assetPath)) return;
            
            Object selectedAsset = AssetDatabase.LoadMainAssetAtPath(assetPath);

            if (selectedAsset != null)
            {
                Selection.activeObject = selectedAsset;
            }
            else
            {
                Debug.LogWarning($"Asset not found at path: {assetPath}");
            }
        }

        static bool ShouldShowOnStartup()
        {
            var v = EditorUserSettings.GetConfigValue(USERSET_KEY_SHOW);
            return string.IsNullOrEmpty(v) || v == "1";
        }

        static void SetShowOnStartup(bool value)
        {
            EditorUserSettings.SetConfigValue(USERSET_KEY_SHOW, value ? "1" : "0");
        }
    }
    
    [InitializeOnLoad]
    public static class ReadmeWindowViewer
    {
        private const string PREF_SHOW_ON_STARTUP = "SilverTau.NSR.Readme.ShowOnStartup";
        private const string SESSION_SHOWN_THIS_RUN = "SilverTau.NSR.Readme.ShownThisSession";

        static ReadmeWindowViewer()
        {
            EditorApplication.delayCall += TryOpenOncePerEditorSession;
        }

        private static void TryOpenOncePerEditorSession()
        {
            if (SessionState.GetBool(SESSION_SHOWN_THIS_RUN, false))
                return;
            
            var showPref = EditorUserSettings.GetConfigValue(PREF_SHOW_ON_STARTUP);
            bool allowShow = string.IsNullOrEmpty(showPref) || showPref == "1";
            if (!allowShow)
                return;

            SessionState.SetBool(SESSION_SHOWN_THIS_RUN, true);
            WelcomeScreenWindow.Open();
        }
    }

}
