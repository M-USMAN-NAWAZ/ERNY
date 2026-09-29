using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GalleryThumbnailItem : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerClickHandler,
    IPointerExitHandler
{
    [Header("Thumbnail")]
    [SerializeField] private Image preview;
    //[SerializeField] private AspectRatioFitter previewAspect;

    [Header("Delete")]
    [SerializeField] private Button deleteButton;

    public Texture2D Texture { get; private set; }
    public string UploadedUrl { get; private set; }

    private Sprite runtimeSprite;
    private Coroutine holdCoroutine;
    private Coroutine hideCoroutine;
    private bool revealedByThisPress;

    public void Initialize(
        Texture2D texture,
        string uploadedUrl,
        Action<GalleryThumbnailItem> onDelete)
    {
        SetImage(texture, uploadedUrl);

        deleteButton.gameObject.SetActive(false);
        deleteButton.onClick.AddListener(() => onDelete(this));
    }

    public void SetImage(Texture2D texture, string uploadedUrl)
    {
        Texture = texture;
        UploadedUrl = uploadedUrl;

        if (runtimeSprite != null)
        {
            Destroy(runtimeSprite);
            runtimeSprite = null;
        }

        // Keep the prefab placeholder while an uploaded image downloads.
        if (texture == null)
            return;

        runtimeSprite = Sprite.Create(
            texture,
            new Rect(0f, 0f, texture.width, texture.height),
            new Vector2(0.5f, 0.5f),
            100f);

        preview.sprite = runtimeSprite;
        preview.preserveAspect = true;
    }
    public void OpenPreview()
    {
        if (revealedByThisPress || Texture == null)
            return;

        GalleryPreviewPanel panel =
            FindFirstObjectByType<GalleryPreviewPanel>(
                FindObjectsInactive.Include);

        if (panel != null)
        {
            panel.Show(Texture);
        }
        else
        {
            Debug.LogError(
                "No GalleryPreviewPanel was found in the scene.");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        revealedByThisPress = false;

        if (!deleteButton.gameObject.activeSelf)
        {
            holdCoroutine =
                StartCoroutine(RevealDeleteAfterHold());
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CancelHold();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CancelHold();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!revealedByThisPress)
            HideDeleteButton();
    }

    private IEnumerator RevealDeleteAfterHold()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        holdCoroutine = null;
        revealedByThisPress = true;
        deleteButton.gameObject.SetActive(true);

        hideCoroutine =
            StartCoroutine(HideDeleteAfterDelay());
    }

    private IEnumerator HideDeleteAfterDelay()
    {
        yield return new WaitForSecondsRealtime(2f);

        hideCoroutine = null;
        HideDeleteButton();
    }

    private void HideDeleteButton()
    {
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        revealedByThisPress = false;
        deleteButton.gameObject.SetActive(false);
    }

    private void CancelHold()
    {
        if (holdCoroutine == null)
            return;

        StopCoroutine(holdCoroutine);
        holdCoroutine = null;
    }

    private void OnDisable()
    {
        CancelHold();

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        revealedByThisPress = false;

        if (deleteButton != null)
            deleteButton.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (runtimeSprite != null)
            Destroy(runtimeSprite);
    }
}