using System.Collections.Generic;
using UnityEditor.Graphs;
using UnityEngine;

namespace SilverTau.iOS 
{
#if UNITY_EDITOR
	
    using UnityEditor;
    using UnityEditor.Callbacks;
    using System.IO;

    #if UNITY_IOS
    using UnityEditor.iOS.Xcode;
    #endif

    public class NSRSettingsProvider : SettingsProvider
    {
#if UNITY_IOS
	    private static string PhotoLibraryAddUsageKey = @"NSPhotoLibraryAddUsageDescription";
	    private static string PhotoLibraryAddUsageDescription = @"Allow this app to save videos to your photo library.";
	    private static string PhotoLibraryUsageKey = @"NSPhotoLibraryUsageDescription";
	    private static string PhotoLibraryUsageDescription = @"Allow this app to save videos to your photo library.";

	    private static string kCustomInterpolatorHelpBox = "If the input field is empty, the parameter is not added to Info.plist.";
#endif
	    
	    private static string kCustomInterpolatorDocumentationURL = "https://silvertau.com";
	    
	    const string TITLE = "NSR - Screen Recorder";
	    const string SUBTITLE = "High-performance video recording for Unity";
	    
	    protected static Texture2D Icon;
	    protected static Texture2D banner;
	    protected static Texture2D Background;
	    protected GUIStyle BackgroundStyle;
	    protected GUIStyle SeparationLineStyle;
	    protected GUIStyle LogoStyle;
	    protected GUIStyle TextStyle;
	    
	    private class Styles
	    {
		    public static readonly GUIContent CustomInterpLabel = L10n.TextContent("Permissions: ", "");
		    public static readonly GUIContent CustomInterpPhotoLibraryAddUsageDescriptionLabel = L10n.TextContent("Photo Library Add Usage Description", $"A message that tells the user why the app is requesting add-only access to the user’s photo library.");
		    public static readonly GUIContent CustomInterpPhotoLibraryUsageDescriptionLabel = L10n.TextContent("Photo Library Usage Description", $"A message that tells the user why the app is requesting access to the user’s photo library. \n\nIf your app only adds assets to the photo library and does not read assets, use the PhotoLibraryAddUsageDescription key instead.");
		    public static readonly GUIContent ReadMore = L10n.IconContent(banner, "Visit the developer's website.");
	    }
	    
	    public NSRSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
	    {
		    Icon = EditorGUIUtility.Load("Packages/com.silvertau.nativescreenrecorder/Editor/Images/icon_settings.png") as Texture2D;
		    if (Icon == null)
		    {
			    Icon = EditorGUIUtility.Load("Assets/Silver Tau/NSR/Editor/Images/icon_settings.png") as Texture2D;
		    }
		    
		    banner = EditorGUIUtility.Load("Packages/com.silvertau.nativescreenrecorder/Editor/Images/banner.png") as Texture2D;
		    if (banner == null)
		    {
			    banner = EditorGUIUtility.Load("Assets/Silver Tau/NSR/Editor/Images/banner.png") as Texture2D;
		    }
		    
		    Background = CreateTexture2D(2, 2, new Color(0.0f, 0.0f, 0.0f, 0.5f));
            
		    BackgroundStyle = new GUIStyle
		    {
			    fixedHeight = 64.0f,
			    stretchWidth = true,
			    normal = new GUIStyleState
			    {
				    background = Background
			    }
		    };
		    
		    SeparationLineStyle = new GUIStyle
		    {
			    fixedHeight = 2.5f,
			    stretchWidth = true,
			    normal = new GUIStyleState
			    {
				    background = Background
			    }
		    };
		    
		    LogoStyle = new GUIStyle
		    {
			    alignment = TextAnchor.MiddleLeft,
			    stretchWidth = true,
			    stretchHeight = true,
			    fixedHeight = 64.0f,
			    fontSize = 21,
			    richText = true
		    };
		    
		    TextStyle = new GUIStyle
		    {
			    alignment = TextAnchor.MiddleCenter,
			    stretchWidth = true,
			    stretchHeight = true,
			    fixedHeight = 64.0f,
			    fontSize = 12,
			    richText = true
		    };
		    
		    guiHandler = OnGUIHandler;
	    }

	    private Texture2D CreateTexture2D(int width, int height, Color col)
	    {
		    var pix = new Color[width * height];
		    for(var i = 0; i < pix.Length; ++i)
		    {
			    pix[i] = col;
		    }
		    var result = new Texture2D(width, height);
		    result.SetPixels(pix);
		    result.Apply();
		    return result;
	    }
	    
	    void DrawHeader()
	    {
		    using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
		    {
			    GUILayout.Space(8);
			    var iconSize = 72f;

			    if (Icon != null)
			    {
				    GUILayout.Label(Icon, GUILayout.Width(iconSize), GUILayout.Height(iconSize));
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

	    void OnGUIHandler(string searchContext)
	    {
		    EditorGUI.BeginChangeCheck();

		    DrawHeader();

		    EditorGUILayout.Space(8);
		    
		    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
		    {
			    EditorGUI.indentLevel++;
			    
			    GUILayout.Space(8);
#if UNITY_IOS
			    EditorGUILayout.LabelField(Styles.CustomInterpLabel, EditorStyles.boldLabel);
			    
			    var photoLibraryAddUsageDescription = EditorGUILayout.TextField(Styles.CustomInterpPhotoLibraryAddUsageDescriptionLabel, PhotoLibraryAddUsageDescription);
			    var photoLibraryUsageDescription = EditorGUILayout.TextField(Styles.CustomInterpPhotoLibraryUsageDescriptionLabel, PhotoLibraryUsageDescription);

			    GUILayout.Space(8);
			    
			    using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
			    {
				    EditorGUILayout.Space(8);
			    
				    GUILayout.Label(EditorGUIUtility.IconContent("console.infoicon"), GUILayout.ExpandWidth(false));
				    GUILayout.Box(kCustomInterpolatorHelpBox, EditorStyles.wordWrappedLabel);
		    
				    GUILayout.FlexibleSpace();
			    }
#else
			    GUILayout.Label("<b><color=#919191>These features are designed for the iOS platform." + "\n" + "To enable this feature, switch your platform to iOS.</color></b>", TextStyle);
#endif
			    GUILayout.Space(8);
		    
			    EditorGUI.indentLevel--;
		    
			    if (EditorGUI.EndChangeCheck())
			    {
#if UNITY_IOS
				    ApplyChanges(photoLibraryAddUsageDescription, photoLibraryUsageDescription);
#endif
			    }
		    }
		    
		    EditorGUILayout.Space(8);

		    using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
		    {
			    EditorGUILayout.Space(8);
			    
			    using (new EditorGUILayout.VerticalScope())
			    {
				    EditorGUILayout.Space(8);
			    
				    if (GUILayout.Button(Styles.ReadMore, GUIStyle.none, GUILayout.ExpandWidth(false)))
				    {
					    System.Diagnostics.Process.Start(kCustomInterpolatorDocumentationURL);
				    }
				    EditorGUILayout.Space(8);
			    }
		    
			    GUILayout.FlexibleSpace();
		    }
	    }

	    private void ApplyChanges(string photoLibraryAddUsageDescription, string photoLibraryUsageDescription)
	    {
#if UNITY_IOS
		    PhotoLibraryAddUsageDescription = photoLibraryAddUsageDescription;
		    PhotoLibraryUsageDescription = photoLibraryUsageDescription;
#endif
	    }
	    
        #if UNITY_IOS

		[PostProcessBuild]
		static void SetPermissions (BuildTarget buildTarget, string path) {
			if (buildTarget != BuildTarget.iOS) return;
			if(string.IsNullOrEmpty(PhotoLibraryAddUsageDescription) && string.IsNullOrEmpty(PhotoLibraryUsageDescription)) return;
			
			var plistPath = path + "/Info.plist";
			var plist = new PlistDocument();
			plist.ReadFromString(File.ReadAllText(plistPath));
			var rootDictionary = plist.root;
			
			if(!string.IsNullOrEmpty(PhotoLibraryAddUsageDescription)) rootDictionary.SetString(PhotoLibraryAddUsageKey, PhotoLibraryAddUsageDescription);
			if(!string.IsNullOrEmpty(PhotoLibraryUsageDescription)) rootDictionary.SetString(PhotoLibraryUsageKey, PhotoLibraryUsageDescription);
			
			File.WriteAllText(plistPath, plist.WriteToString());
		}
		#endif
	    
	    
	    [SettingsProvider]
	    public static SettingsProvider NSRSettingsProviderProjectSettingsProvider()
	    {
		    var provider = new NSRSettingsProvider("Project/Silver Tau/NSR - Screen Recorder", SettingsScope.Project);
		    return provider;
	    }
    }

#endif
}
#pragma warning restore 0162, 0429