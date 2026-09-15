using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UIConfetti : MonoBehaviour
{
    [Header("Playback")]
    public bool playOnEnable = true;
    public bool clearOnDisable = true;
    public bool useUnscaledTime = false;
    public float duration = 1.4f;
    public int pieceCount = 90;

    [Header("Area")]
    public float topOffset = 20f;
    public float horizontalPadding = 10f;

    [Header("Motion")]
    public Vector2 fallSpeedRange = new Vector2(280f, 520f);
    public Vector2 driftSpeedRange = new Vector2(-120f, 120f);
    public float gravity = 320f;
    public Vector2 spinSpeedRange = new Vector2(220f, 760f);

    [Header("Shape")]
    public Vector2 widthRange = new Vector2(5f, 12f);
    public Vector2 heightRange = new Vector2(8f, 18f);
    [Range(0f, 1f)] public float fadeStart = 0.65f;

    [Header("Colors")]
    public Color[] colors =
    {
        new Color(1f, 0.82f, 0.12f, 1f),
        new Color(0.16f, 0.67f, 0.92f, 1f),
        new Color(0.95f, 0.28f, 0.28f, 1f),
        new Color(0.53f, 0.72f, 0.36f, 1f),
        new Color(1f, 1f, 1f, 1f)
    };

    private readonly List<Piece> pieces = new List<Piece>();
    private RectTransform rectTransform;
    private Sprite whiteSprite;
    private bool playing;
    private float elapsed;

    private class Piece
    {
        public RectTransform Rect;
        public Image Image;
        public Vector2 Velocity;
        public float Spin;
        public float LifeOffset;
        public Color BaseColor;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        whiteSprite = CreateWhiteSprite();
    }

    private void OnEnable()
    {
        if (playOnEnable)
        {
            Play();
        }
    }

    private void OnDisable()
    {
        if (clearOnDisable)
        {
            Clear();
        }
    }

    private void Update()
    {
        if (!playing)
        {
            return;
        }

        float deltaTime = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        elapsed += deltaTime;

        for (int i = pieces.Count - 1; i >= 0; i--)
        {
            Piece piece = pieces[i];
            float age = elapsed - piece.LifeOffset;

            if (age < 0f)
            {
                continue;
            }

            piece.Velocity += Vector2.down * gravity * deltaTime;
            piece.Rect.anchoredPosition += piece.Velocity * deltaTime;
            piece.Rect.Rotate(0f, 0f, piece.Spin * deltaTime);

            float fadeProgress = Mathf.InverseLerp(duration * fadeStart, duration, age);
            Color color = piece.BaseColor;
            color.a = Mathf.Lerp(piece.BaseColor.a, 0f, fadeProgress);
            piece.Image.color = color;

            if (age >= duration)
            {
                Destroy(piece.Rect.gameObject);
                pieces.RemoveAt(i);
            }
        }

        if (elapsed >= duration && pieces.Count == 0)
        {
            playing = false;
        }
    }

    public void Play()
    {
        Clear();
        elapsed = 0f;
        playing = true;

        for (int i = 0; i < pieceCount; i++)
        {
            pieces.Add(CreatePiece(i));
        }
    }

    public void Stop()
    {
        playing = false;
        Clear();
    }

    private Piece CreatePiece(int index)
    {
        GameObject pieceObject = new GameObject("Confetti Piece", typeof(RectTransform), typeof(Image));
        pieceObject.transform.SetParent(transform, false);

        RectTransform pieceRect = pieceObject.GetComponent<RectTransform>();
        Image pieceImage = pieceObject.GetComponent<Image>();
        pieceImage.sprite = whiteSprite;
        pieceImage.raycastTarget = false;

        Rect area = rectTransform.rect;
        float x = Random.Range(area.xMin + horizontalPadding, area.xMax - horizontalPadding);
        float y = area.yMax + topOffset + Random.Range(0f, area.height * 0.18f);

        pieceRect.anchorMin = new Vector2(0.5f, 0.5f);
        pieceRect.anchorMax = new Vector2(0.5f, 0.5f);
        pieceRect.pivot = new Vector2(0.5f, 0.5f);
        pieceRect.anchoredPosition = new Vector2(x, y);
        pieceRect.sizeDelta = new Vector2(Random.Range(widthRange.x, widthRange.y), Random.Range(heightRange.x, heightRange.y));
        pieceRect.localRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

        Color color = colors != null && colors.Length > 0 ? colors[Random.Range(0, colors.Length)] : Color.white;
        pieceImage.color = color;

        return new Piece
        {
            Rect = pieceRect,
            Image = pieceImage,
            Velocity = new Vector2(Random.Range(driftSpeedRange.x, driftSpeedRange.y), -Random.Range(fallSpeedRange.x, fallSpeedRange.y)),
            Spin = Random.Range(spinSpeedRange.x, spinSpeedRange.y) * (Random.value > 0.5f ? 1f : -1f),
            LifeOffset = Random.Range(0f, duration * 0.25f),
            BaseColor = color
        };
    }

    private void Clear()
    {
        for (int i = pieces.Count - 1; i >= 0; i--)
        {
            if (pieces[i].Rect != null)
            {
                Destroy(pieces[i].Rect.gameObject);
            }
        }

        pieces.Clear();
    }

    private static Sprite CreateWhiteSprite()
    {
        Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        texture.name = "UIConfetti White Pixel";
        texture.hideFlags = HideFlags.HideAndDontSave;
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
        sprite.name = "UIConfetti White Sprite";
        sprite.hideFlags = HideFlags.HideAndDontSave;
        return sprite;
    }
}
