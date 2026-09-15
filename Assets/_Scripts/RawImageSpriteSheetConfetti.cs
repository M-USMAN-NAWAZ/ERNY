using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class RawImageSpriteSheetConfetti : MonoBehaviour
{
    [Header("Sprite Sheet")]
    public Texture2D spriteSheet;
    public int frameCount = 121;
    public int columns = 11;
    public int rows = 11;

    [Header("Playback")]
    public float framesPerSecond = 30f;
    public bool playOnEnable = true;
    public bool loop = false;
    public bool hideWhenFinished = false;
    public bool useUnscaledTime = false;

    [Header("Color Override")]
    public bool useHexColorOverride = false;
    [Tooltip("Original JSON colors to replace. Leave these as-is unless the source artwork changes.")]
    public string[] sourceHexColors =
    {
        "#FFC343",
        "#4DE1FF",
        "#77DAEE",
        "#FFE600",
        "#307799",
        "#E6A210",
        "#D0A64E",
        "#FECB58"
    };
    [Tooltip("New colors, matched by index with Source Hex Colors. Example: #FF0000")]
    public string[] replacementHexColors =
    {
        "#FFC343",
        "#4DE1FF",
        "#77DAEE",
        "#FFE600",
        "#307799",
        "#E6A210",
        "#D0A64E",
        "#FECB58"
    };
    [Range(0.01f, 0.5f)] public float colorMatchTolerance = 0.12f;
    public bool clearUnmatchedPixels = true;

    private RawImage rawImage;
    private float elapsed;
    private bool isPlaying;
    private int currentFrame = -1;
    private Texture2D recoloredSpriteSheet;
    private string lastRecolorKey;
    private static readonly Dictionary<string, Texture2D> RecoloredSpriteSheetCache = new Dictionary<string, Texture2D>();

    private void Awake()
    {
        rawImage = GetComponent<RawImage>();
    }

    private void OnEnable()
    {
        if (playOnEnable)
        {
            Play();
        }
        else
        {
            ConfigureRawImageReference();

            if (rawImage != null)
            {
                rawImage.enabled = false;
            }
        }
    }

    private void Update()
    {
        if (!isPlaying || GetActiveSpriteSheet() == null)
        {
            return;
        }

        float deltaTime = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        elapsed += deltaTime;

        int safeFrameCount = Mathf.Max(1, frameCount);
        int frame = Mathf.FloorToInt(elapsed * Mathf.Max(1f, framesPerSecond));

        if (loop)
        {
            frame %= safeFrameCount;
        }
        else if (frame >= safeFrameCount)
        {
            SetFrame(safeFrameCount - 1);
            isPlaying = false;

            if (hideWhenFinished)
            {
                gameObject.SetActive(false);
            }

            return;
        }

        SetFrame(frame);
    }

    public void Play()
    {
        if (!ConfigureRawImage())
        {
            return;
        }

        transform.SetAsLastSibling();
        elapsed = 0f;
        currentFrame = -1;
        isPlaying = true;
        SetFrame(0);
    }

    public void Stop()
    {
        isPlaying = false;
    }

    private bool ConfigureRawImage()
    {
        ConfigureRawImageReference();

        if (rawImage == null)
        {
            Debug.LogWarning("RawImageSpriteSheetConfetti needs a RawImage on this GameObject or one of its children.", this);
            return false;
        }

        Texture activeSpriteSheet = GetActiveSpriteSheet();
        if (activeSpriteSheet == null)
        {
            rawImage.enabled = false;
            return false;
        }

        rawImage.texture = activeSpriteSheet;
        rawImage.enabled = true;
        rawImage.raycastTarget = false;
        rawImage.color = Color.white;
        return true;
    }

    private void ConfigureRawImageReference()
    {
        if (rawImage == null)
        {
            rawImage = GetComponent<RawImage>();
        }

        if (rawImage == null)
        {
            rawImage = GetComponentInChildren<RawImage>(true);
        }
    }

    private void SetFrame(int frame)
    {
        Texture activeSpriteSheet = GetActiveSpriteSheet();

        if (rawImage == null || activeSpriteSheet == null || frame == currentFrame)
        {
            return;
        }

        int safeColumns = Mathf.Max(1, columns);
        int safeRows = Mathf.Max(1, rows);
        int safeFrameCount = Mathf.Max(1, frameCount);
        int safeFrame = Mathf.Clamp(frame, 0, safeFrameCount - 1);

        int column = safeFrame % safeColumns;
        int row = safeFrame / safeColumns;

        float width = 1f / safeColumns;
        float height = 1f / safeRows;
        float x = column * width;
        float y = 1f - ((row + 1) * height);

        rawImage.texture = activeSpriteSheet;
        rawImage.uvRect = new Rect(x, y, width, height);
        rawImage.SetAllDirty();
        currentFrame = safeFrame;
    }

    private Texture GetActiveSpriteSheet()
    {
        if (spriteSheet == null)
        {
            return null;
        }

        if (!useHexColorOverride)
        {
            return spriteSheet;
        }

        string recolorKey = BuildRecolorKey();
        if (RecoloredSpriteSheetCache.TryGetValue(recolorKey, out Texture2D cachedSpriteSheet))
        {
            recoloredSpriteSheet = cachedSpriteSheet;
            lastRecolorKey = recolorKey;
            return recoloredSpriteSheet;
        }

        if (recoloredSpriteSheet == null || recolorKey != lastRecolorKey)
        {
            BuildRecoloredSpriteSheet(recolorKey);
        }

        return recoloredSpriteSheet != null ? recoloredSpriteSheet : spriteSheet;
    }

    private void BuildRecoloredSpriteSheet(string recolorKey)
    {
        Texture2D readableSource = CreateReadableCopy(spriteSheet);
        if (readableSource == null)
        {
            return;
        }

        Color32[] pixels = readableSource.GetPixels32();
        Color[] sourceColors = ParseColorList(sourceHexColors);
        Color[] replacementColors = ParseColorList(replacementHexColors);
        float toleranceSquared = colorMatchTolerance * colorMatchTolerance;

        for (int i = 0; i < pixels.Length; i++)
        {
            Color pixel = pixels[i];
            if (pixel.a <= 0.01f)
            {
                pixels[i] = Color.clear;
                continue;
            }

            int sourceIndex = FindClosestSourceColor(pixel, sourceColors, toleranceSquared);
            if (sourceIndex < 0)
            {
                if (clearUnmatchedPixels)
                {
                    pixels[i] = Color.clear;
                }

                continue;
            }

            Color replacement = sourceIndex < replacementColors.Length ? replacementColors[sourceIndex] : sourceColors[sourceIndex];
            replacement.a = pixel.a;
            pixels[i] = replacement;
        }

        recoloredSpriteSheet = new Texture2D(readableSource.width, readableSource.height, TextureFormat.RGBA32, false)
        {
            name = spriteSheet.name + " Recolored"
        };
        recoloredSpriteSheet.SetPixels32(pixels);
        recoloredSpriteSheet.Apply(false, false);
        lastRecolorKey = recolorKey;
        RecoloredSpriteSheetCache[recolorKey] = recoloredSpriteSheet;

        Destroy(readableSource);
    }

    private Texture2D CreateReadableCopy(Texture2D source)
    {
        RenderTexture previous = RenderTexture.active;
        RenderTexture renderTexture = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGB32);

        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.clear);
        Graphics.Blit(source, renderTexture);

        Texture2D readable = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);
        readable.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0);
        readable.Apply(false, false);

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTexture);

        return readable;
    }

    private Color[] ParseColorList(string[] hexColors)
    {
        if (hexColors == null || hexColors.Length == 0)
        {
            return new[] { Color.white };
        }

        Color[] parsedColors = new Color[hexColors.Length];
        for (int i = 0; i < hexColors.Length; i++)
        {
            if (!ColorUtility.TryParseHtmlString(hexColors[i], out parsedColors[i]))
            {
                parsedColors[i] = Color.white;
            }
        }

        return parsedColors;
    }

    private int FindClosestSourceColor(Color pixel, Color[] sourceColors, float toleranceSquared)
    {
        int closestIndex = -1;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < sourceColors.Length; i++)
        {
            float distance =
                Mathf.Pow(pixel.r - sourceColors[i].r, 2f) +
                Mathf.Pow(pixel.g - sourceColors[i].g, 2f) +
                Mathf.Pow(pixel.b - sourceColors[i].b, 2f);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestDistance <= toleranceSquared ? closestIndex : -1;
    }

    private string BuildRecolorKey()
    {
        string sourceKey = sourceHexColors == null ? string.Empty : string.Join(",", sourceHexColors);
        string replacementKey = replacementHexColors == null ? string.Empty : string.Join(",", replacementHexColors);
        return spriteSheet.GetInstanceID() + "|" + useHexColorOverride + "|" + colorMatchTolerance + "|" + clearUnmatchedPixels + "|" + sourceKey + "|" + replacementKey;
    }

}
