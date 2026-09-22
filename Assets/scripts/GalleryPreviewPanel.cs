using UnityEngine;
using UnityEngine.UI;

public class GalleryPreviewPanel : MonoBehaviour
{
    [SerializeField] private RawImage previewImage;

    public void Show(Texture texture)
    {
        if (previewImage == null)
        {
            Debug.LogError("Assign the preview RawImage on GalleryPreviewPanel.");
            return;
        }

        previewImage.texture = texture;
        if (texture != null)
            Debug.Log($"Gallery preview resolution: {texture.width}x{texture.height}");
        gameObject.SetActive(true);
    }

    public void ClosePreview()
    {
        gameObject.SetActive(false);
    }
}
