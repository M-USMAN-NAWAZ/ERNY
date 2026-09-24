using System.Collections;
using System.Collections.Generic;
using System;
using ImageCropperNamespace;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class EventGalleryPicker : MonoBehaviour
{
    public static EventGalleryPicker Instance { get; private set; }

    [SerializeField] private GameObject galleryPanel;

    private void Awake()
    {
        Instance = this;
    }

    [Header("Popup")]
    [SerializeField] private GameObject sourcePopup;

    [Header("Image Row")]
    [SerializeField] private RectTransform imageRow;
    [SerializeField] private Button addImageButton;
    [SerializeField] private GalleryThumbnailItem thumbnailPrefab;

    [Header("Settings")]
    [SerializeField] private int maximumImages = 20;
    [SerializeField] private int maximumImageSize = 4096;
    [SerializeField] private bool ovalCrop;

    private readonly List<GalleryThumbnailItem> thumbnails =
        new List<GalleryThumbnailItem>();

    private readonly List<string> deletedUploadedUrls =
        new List<string>();

    private List<string> preparedUploadedUrls;
    private bool loadPreparedImages;
    private Text emptyGalleryMessage;
    private const int NormalTopPadding = 20;
    private const int EmptyTopPadding = 200;

    // New images that still need uploading.
    public List<Texture2D> SelectedImages
    {
        get
        {
            List<Texture2D> images = new List<Texture2D>();

            foreach (GalleryThumbnailItem thumbnail in thumbnails)
            {
                if (string.IsNullOrEmpty(thumbnail.UploadedUrl))
                    images.Add(thumbnail.Texture);
            }

            return images;
        }
    }

    // Previously uploaded images that the user kept.
    public List<string> ExistingUploadedUrls
    {
        get
        {
            List<string> urls = new List<string>();

            foreach (GalleryThumbnailItem thumbnail in thumbnails)
            {
                if (!string.IsNullOrEmpty(thumbnail.UploadedUrl))
                    urls.Add(thumbnail.UploadedUrl);
            }

            return urls;
        }
    }

    // Previously uploaded images that were deleted or replaced.
    public List<string> DeletedUploadedUrls =>
        new List<string>(deletedUploadedUrls);

    public void OpenPopup()
    {
        BindImageRow(ExpandOnClick.LastExpandedWindow);

        if (thumbnails.Count < maximumImages)
            sourcePopup.SetActive(true);
    }

    public void ClosePopup()
    {
        //imageBeingUpdated = null;
        sourcePopup.SetActive(false);
    }

    public void PickFromGallery()
    {
        BindImageRow(ExpandOnClick.LastExpandedWindow);
        sourcePopup.SetActive(false);

        if (NativeGallery.IsMediaPickerBusy())
            return;

        NativeGallery.GetImageFromGallery(path =>
        {
            if (string.IsNullOrEmpty(path))
                return;

            Texture2D image = NativeGallery.LoadImageAtPath(
                path,
                maximumImageSize,
                false);

            if (image == null)
            {
                Debug.LogError("Could not load gallery image: " + path);
                return;
            }

            OpenCropper(image);

        }, "Select an image", "image/*");
    }

    public void CaptureFromCamera()
    {
        BindImageRow(ExpandOnClick.LastExpandedWindow);
        sourcePopup.SetActive(false);

        if (NativeCamera.IsCameraBusy())
            return;

        NativeCamera.TakePicture(path =>
        {
            if (string.IsNullOrEmpty(path))
                return;

            Texture2D image = NativeCamera.LoadImageAtPath(
                path,
                maximumImageSize,
                false);

            if (image == null)
            {
                Debug.LogError("Could not load captured image: " + path);
                return;
            }

            OpenCropper(image);

        }, maximumImageSize, true, NativeCamera.PreferredCamera.Default);
    }

    private void OpenCropper(Texture2D sourceImage)
    {
        ImageCropper.Instance.Show(
            sourceImage,
            (result, originalImage, croppedImage) =>
            {
                if (result && croppedImage != null)
                    AddImage(croppedImage);

                if (originalImage != null && originalImage != croppedImage)
                    Destroy(originalImage);
            },
            new ImageCropper.Settings
            {
                markTextureNonReadable = false,
                ovalSelection = ovalCrop,
                autoZoomEnabled = true,
                imageBackground = Color.clear,
                selectionMinAspectRatio = 1f,
                selectionMaxAspectRatio = 1f
            },
            LimitCroppedImageSize);
    }

    private static void LimitCroppedImageSize(ref int width, ref int height)
    {
        const int maxDimension = 2048;
        int largest = Mathf.Max(width, height);
        if (largest <= maxDimension)
            return;

        float scale = maxDimension / (float)largest;
        width = Mathf.Max(1, Mathf.RoundToInt(width * scale));
        height = Mathf.Max(1, Mathf.RoundToInt(height * scale));
    }

    private void AddImage(Texture2D image)
    {
        BindImageRow(ExpandOnClick.LastExpandedWindow);

        if (imageRow == null || addImageButton == null)
        {
            Debug.LogError("No editable GalleryOpen image row is active.");
            Destroy(image);
            return;
        }

        GalleryThumbnailItem thumbnail =
            Instantiate(thumbnailPrefab, imageRow);

        thumbnail.Initialize(
            image,
            null,
            DeleteImage);

        KeepAddButtonAtTopLeft();

        thumbnails.Add(thumbnail);
        UpdateEmptyGalleryState();
        ExpandOnClick.RefreshGalleryHeight(imageRow);

        addImageButton.interactable =
            thumbnails.Count < maximumImages;
    }

    private void DeleteImage(GalleryThumbnailItem thumbnail)
    {
        RememberDeletedUrl(thumbnail.UploadedUrl);
        thumbnails.Remove(thumbnail);

        if (thumbnail.Texture != null)
            Destroy(thumbnail.Texture);

        thumbnail.transform.SetParent(null, false);
        Destroy(thumbnail.gameObject);
        KeepAddButtonAtTopLeft();
        UpdateEmptyGalleryState();
        ExpandOnClick.RefreshGalleryHeight(imageRow);

        addImageButton.interactable =
            thumbnails.Count < maximumImages;
    }

    private void RememberDeletedUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
            return;

        if (!deletedUploadedUrls.Contains(url))
            deletedUploadedUrls.Add(url);
    }

    public void LoadUploadedImages(List<string> urls)
    {
        ResetImages();

        if (urls == null)
            return;

        foreach (string url in urls)
        {
            if (string.IsNullOrWhiteSpace(url))
                continue;

            GalleryThumbnailItem thumbnail =
                Instantiate(thumbnailPrefab, imageRow);
            thumbnail.Initialize(null, url, DeleteImage);
            thumbnails.Add(thumbnail);
        }

        KeepAddButtonAtTopLeft();
        UpdateEmptyGalleryState();
        ExpandOnClick.RefreshGalleryHeight(imageRow);
        addImageButton.interactable = thumbnails.Count < maximumImages;

        foreach (GalleryThumbnailItem thumbnail in thumbnails)
            StartCoroutine(DownloadUploadedImage(thumbnail));
    }

    public void PrepareForUpdate(List<string> urls)
    {
        ResetImages();
        preparedUploadedUrls = urls != null
            ? new List<string>(urls)
            : new List<string>();
        loadPreparedImages = true;
    }

    public void BindImageRow(RectTransform row)
    {
        if (row == null || row.name != "GalleryOpen")
            return;

        Button rowAddButton = FindAddImageButton(row);

        // The read-only event gallery has no visible add button.
        if (rowAddButton == null || !rowAddButton.gameObject.activeSelf)
            return;

        if (imageRow != row)
        {
            ResetImages();
            imageRow = row;
            addImageButton = rowAddButton;
            emptyGalleryMessage = null;
        }
        else
        {
            addImageButton = rowAddButton;
        }

        ConfigureImageRow();

        if (!loadPreparedImages)
            return;

        List<string> urls = preparedUploadedUrls;
        preparedUploadedUrls = null;
        loadPreparedImages = false;
        LoadUploadedImages(urls);
    }

    private Button FindAddImageButton(RectTransform row)
    {
        foreach (Transform child in row)
        {
            Button button = child.GetComponent<Button>();

            if (button != null &&
                child.GetComponent<GalleryThumbnailItem>() == null)
                return button;
        }

        return null;
    }

    private void ConfigureImageRow()
    {
        GridLayoutGroup grid = imageRow.GetComponent<GridLayoutGroup>();

        if (grid != null)
        {
            grid.startAxis = GridLayoutGroup.Axis.Horizontal;
            grid.childAlignment = TextAnchor.UpperLeft;
        }

        KeepAddButtonAtTopLeft();
        EnsureEmptyGalleryMessage();
        UpdateEmptyGalleryState();
    }

    private void KeepAddButtonAtTopLeft()
    {
        if (imageRow == null || addImageButton == null)
            return;

        addImageButton.transform.SetAsFirstSibling();
    }

    private void EnsureEmptyGalleryMessage()
    {
        Transform existing = imageRow.Find("EmptyGalleryMessage");

        if (existing != null)
        {
            emptyGalleryMessage = existing.GetComponent<Text>();
            return;
        }

        GameObject messageObject = new GameObject(
            "EmptyGalleryMessage",
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(Text),
            typeof(LayoutElement));

        messageObject.transform.SetParent(imageRow, false);

        RectTransform rect = messageObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(0.5f, 1f);
        rect.anchoredPosition = new Vector2(0f, -35f);
        rect.sizeDelta = new Vector2(-140f, 120f);

        LayoutElement layout = messageObject.GetComponent<LayoutElement>();
        layout.ignoreLayout = true;

        emptyGalleryMessage = messageObject.GetComponent<Text>();
        emptyGalleryMessage.text = "Add your favorite photos.";
        emptyGalleryMessage.font =
            Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        emptyGalleryMessage.fontSize = 64;
        emptyGalleryMessage.color = new Color32(41, 46, 67, 255);
        emptyGalleryMessage.alignment = TextAnchor.MiddleCenter;
        emptyGalleryMessage.raycastTarget = false;
    }

    private void UpdateEmptyGalleryState()
    {
        SetEmptyGalleryVisible(thumbnails.Count == 0);
    }

    private void SetEmptyGalleryVisible(bool visible)
    {
        if (imageRow == null)
            return;

        EnsureEmptyGalleryMessage();
        emptyGalleryMessage.gameObject.SetActive(visible);

        GridLayoutGroup grid = imageRow.GetComponent<GridLayoutGroup>();
        if (grid != null)
            grid.padding.top = visible ? EmptyTopPadding : NormalTopPadding;
    }

    private IEnumerator DownloadUploadedImage(GalleryThumbnailItem thumbnail)
    {
        string url = thumbnail.UploadedUrl;
        string requestUrl;

        try
        {
            requestUrl = new Uri(url).AbsoluteUri;
        }
        catch (UriFormatException exception)
        {
            Debug.LogError("Invalid gallery image URL: " + url +
                "\n" + exception.Message);
            yield break;
        }

        using (UnityWebRequest request =
               UnityWebRequestTexture.GetTexture(requestUrl, false))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Could not load gallery image: " + url);
                yield break;
            }

            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            if (thumbnail == null)
            {
                Destroy(texture);
                yield break;
            }

            thumbnail.SetImage(texture, url);
        }
    }

    // Call after upload when the API returns URLs in the same order.
    public void ApplyUploadedUrls(List<string> uploadedUrls)
    {
        List<GalleryThumbnailItem> pending =
            new List<GalleryThumbnailItem>();

        foreach (GalleryThumbnailItem thumbnail in thumbnails)
        {
            if (string.IsNullOrEmpty(thumbnail.UploadedUrl))
                pending.Add(thumbnail);
        }

        if (uploadedUrls == null ||
            uploadedUrls.Count != pending.Count)
        {
            Debug.LogError(
                "Uploaded URL count does not match pending images.");
            return;
        }

        for (int i = 0; i < pending.Count; i++)
        {
            pending[i].SetImage(
                pending[i].Texture,
                uploadedUrls[i]);
        }

        deletedUploadedUrls.Clear();
    }

    public void ResetImages()
    {
        StopAllCoroutines();
        sourcePopup.SetActive(false);

        foreach (GalleryThumbnailItem thumbnail in thumbnails)
        {
            if (thumbnail.Texture != null)
                Destroy(thumbnail.Texture);

            thumbnail.transform.SetParent(null, false);
            Destroy(thumbnail.gameObject);
        }

        thumbnails.Clear();
        deletedUploadedUrls.Clear();
        UpdateEmptyGalleryState();
        ExpandOnClick.RefreshGalleryHeight(imageRow);
        if (addImageButton != null)
            addImageButton.interactable = true;
    }



    public void ShowUploadedImages(List<string> urls)
    {
        galleryPanel.SetActive(true);
        LoadUploadedImages(urls);
    }

    public void HideGallery()
    {
        ResetImages();
        galleryPanel.SetActive(false);
    }


}
