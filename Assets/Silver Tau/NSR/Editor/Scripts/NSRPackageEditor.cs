using UnityEditor;
using UnityEngine;

namespace SilverTau.NSR
{
    public class NSRPackageEditor : Editor
    {
        protected static Texture2D Icon;
        protected static Texture2D IconMini;
        protected static Texture2D Background;
        protected GUIStyle LogoStyle;
        protected GUIStyle BackgroundStyle;
        
        protected GUIStyle helpBox;
        protected GUIStyle labelHeader;
        protected GUIStyle labelHeader2;
        protected GUIStyle labelHeader3;
        protected Color colorDarkBlue;
        protected Color colorBlack;
        protected Color colorDodgerBlue;
        protected Color colorBackgroundDark;
        
        public virtual void Awake()
        {
            Icon = EditorGUIUtility.Load("Packages/com.silvertau.nativescreenrecorder/Editor/Images/icon_small.png") as Texture2D;
            
            if (Icon == null)
            {
                Icon = EditorGUIUtility.Load("Assets/Silver Tau/NSR/Editor/Images/icon_small.png") as Texture2D;
            }
            
            IconMini = EditorGUIUtility.Load("Packages/com.silvertau.nativescreenrecorder/Editor/Images/icon_mini.png") as Texture2D;
            
            if (IconMini == null)
            {
                IconMini = EditorGUIUtility.Load("Assets/Silver Tau/NSR/Editor/Images/icon_mini.png") as Texture2D;
            }
            
            ColorUtility.TryParseHtmlString("#212121", out colorBlack);
            ColorUtility.TryParseHtmlString("#174ead", out colorDodgerBlue);
            ColorUtility.TryParseHtmlString("#122d4a", out colorDarkBlue);
            
            ColorUtility.TryParseHtmlString("#c12648", out colorBackgroundDark);

            //Background = CreateTexture2D(2, 2, new Color(0.0f, 0.0f, 0.0f, 0.5f));
            Background = CreateHorizontalGradientTexture2D(64, 64, colorBlack, colorBackgroundDark, 0.1f);
            
            BackgroundStyle = new GUIStyle
            {
                fixedHeight = 32.0f,
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
                fixedHeight = 32.0f,
                fontSize = 21,
                richText = true
            };
            
            labelHeader = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                fixedHeight = 26.0f,
                richText = true,
                fontSize = 14,
                normal = new GUIStyleState
                {
                    background = CreateTexture2D(2, 2, colorBlack),
                    textColor = Color.white
                }
            };
            
            labelHeader2 = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                fixedHeight = 30.0f,
                richText = true,
                fontSize = 14,
                normal = new GUIStyleState
                {
                    background = CreateTexture2D(2, 2, colorDodgerBlue),
                    textColor = Color.white
                }
            };
            
            labelHeader3 = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                fixedHeight = 30.0f,
                richText = true,
                fontSize = 16,
                normal = new GUIStyleState
                {
                    background = CreateTexture2D(2, 2, colorBlack),
                    textColor = Color.white
                }
            };
        }

        public Texture2D CreateTexture2D(int width, int height, Color col)
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
        
        private Texture2D CreateHorizontalGradientTexture2D(int width, int height, Color left, Color right, float ratio = 0.5f)
        {
            var tex = new Texture2D(width, height, TextureFormat.RGBA32, false, false);
            tex.filterMode = FilterMode.Bilinear;
            tex.wrapMode  = TextureWrapMode.Clamp;

            var pixels = new Color[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float t = Mathf.Clamp01((x + 0.5f) / width);

                    if (t < ratio)
                    {
                        pixels[y * width + x] = left;
                    }
                    else
                    {
                        float blendT = (t - ratio) / (1f - ratio);
                        Color lc = Color.Lerp(left.linear, right.linear, blendT).gamma;
                        pixels[y * width + x] = lc;
                    }
                }
            }

            tex.SetPixels(pixels);
            tex.Apply();
            return tex;
        }

        protected void BoxLogo(MonoBehaviour behaviour = null, string value = "")
        {
            if (Icon != null)
            {
                
#if UNITY_2021_2_OR_NEWER
                if(behaviour) EditorGUIUtility.SetIconForObject(behaviour, Icon);
#endif
            }
            
            EditorGUI.BeginChangeCheck();
            GUILayout.BeginHorizontal(BackgroundStyle);
            GUILayout.Label(IconMini, GUILayout.ExpandWidth(false),  GUILayout.ExpandHeight(true));
            GUILayout.Label("<b><color=#ec3c63>NSR</color></b>" + value, LogoStyle);
		    
            GUILayout.EndHorizontal();
            
            GUILayout.Space(2.5f);
        }
    }
}
