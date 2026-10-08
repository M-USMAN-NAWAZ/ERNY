using UnityEngine;
using UnityEngine.UI;

public class GalleryPreviewPanel : MonoBehaviour
{
    [SerializeField] private Image previewImage;

    private Sprite runtimeSprite;

    public void Show(Texture2D texture)
    {
        ClearSprite();

        if (texture == null)
            return;

        runtimeSprite = Sprite.Create(
            texture,
            new Rect(0f, 0f, texture.width, texture.height),
            new Vector2(0.5f, 0.5f),
            100f);

        previewImage.sprite = runtimeSprite;
        previewImage.preserveAspect = true;

        gameObject.SetActive(true);
    }

    public void ClosePreview()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        ClearSprite();
    }

    private void ClearSprite()
    {
        if (previewImage != null)
            previewImage.sprite = null;

        if (runtimeSprite != null)
        {
            Destroy(runtimeSprite);
            runtimeSprite = null;
        }
    }
}
