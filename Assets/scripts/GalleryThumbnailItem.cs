using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GalleryThumbnailItem : MonoBehaviour, IPointerDownHandler,
    IPointerUpHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private RawImage preview;
    [SerializeField] private Button deleteButton;

    public Texture2D Texture { get; private set; }
    public string UploadedUrl { get; private set; }
    private Coroutine holdCoroutine;
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

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        revealedByThisPress = false;
        if (!deleteButton.gameObject.activeSelf)
            holdCoroutine = StartCoroutine(RevealDeleteAfterHold());
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
            deleteButton.gameObject.SetActive(false);
    }

    public void OpenPreview()
    {
        if (revealedByThisPress || preview.texture == null)
            return;

        GalleryPreviewPanel panel = FindFirstObjectByType<GalleryPreviewPanel>(
            FindObjectsInactive.Include);

        if (panel != null)
            panel.Show(preview.texture);
        else
            Debug.LogError("No GalleryPreviewPanel was found in the scene.");
    }

    private IEnumerator RevealDeleteAfterHold()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        holdCoroutine = null;
        revealedByThisPress = true;
        deleteButton.gameObject.SetActive(true);
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
        if (deleteButton != null)
            deleteButton.gameObject.SetActive(false);
    }

    public void SetImage(Texture2D texture, string uploadedUrl)
    {
        Texture = texture;
        UploadedUrl = uploadedUrl;
        if (texture != null)
            preview.texture = texture;
    }
}
