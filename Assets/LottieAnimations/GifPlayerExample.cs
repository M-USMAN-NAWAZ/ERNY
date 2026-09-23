using UnityEngine;
using UnityEngine.UI;
using UnityGif;

public class GifPlayerExample : MonoBehaviour
{
    public GifData gifData;
    public RawImage rawImage;

    void Start()
    {
        GifImage.Instance.Play(gifData, rawImage);
    }
}