using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class EventGalleryViewer : MonoBehaviour
{
    public static EventGalleryViewer Instance { get; private set; }

    [SerializeField] private GameObject uploadedGalleryPanel;
    [SerializeField] private RectTransform imageContent;
    [SerializeField] private GalleryThumbnailItem thumbnailPrefab;

    private readonly List<GalleryThumbnailItem> spawnedImages =
        new List<GalleryThumbnailItem>();

    private readonly List<Texture2D> loadedTextures =
        new List<Texture2D>();

    private readonly List<string> currentUrls = new List<string>();
    private RectTransform activeImageContent;
    private string currentEventId;
    private Action<List<string>> onImagesChanged;
    private bool deleteInProgress;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowUploadedImages(List<string> urls)
    {
        ShowUploadedImages(urls, null, null);
    }

    public void ShowUploadedImages(
        List<string> urls,
        string eventId,
        Action<List<string>> imagesChanged)
    {
        ShowUploadedImages(urls, eventId, imagesChanged, null);
    }

    public void ShowUploadedImages(
        List<string> urls,
        string eventId,
        Action<List<string>> imagesChanged,
        RectTransform targetContent)
    {
        StopAllCoroutines();
        ClearImages();
        deleteInProgress = false;

        activeImageContent = targetContent != null
            ? targetContent
            : FindActiveGalleryContent();

        if (activeImageContent == null)
        {
            Debug.LogError("No active GalleryOpen container was found.");
            return;
        }

        if (IsEditableGallery(activeImageContent))
        {
            Debug.Log(
                "Skipped read-only gallery loading for the editable panel.");
            return;
        }

        Debug.Log(
            "Gallery images target: " +
            activeImageContent.transform.parent.name + "/" +
            activeImageContent.name);

        currentEventId = eventId;
        onImagesChanged = imagesChanged;
        currentUrls.Clear();

        if (urls != null)
            currentUrls.AddRange(urls);

        int imageCount = currentUrls.Count;
        Debug.Log("Uploaded gallery URL count: " + imageCount);

        if (imageCount == 0)
        {
            Debug.LogWarning(
                "This event does not contain any gallery image URLs.");
            return;
        }

        foreach (string url in currentUrls)
        {
            if (string.IsNullOrWhiteSpace(url))
                continue;

            GalleryThumbnailItem image =
                Instantiate(thumbnailPrefab, activeImageContent);
            image.Initialize(null, url, DeleteImage);
            spawnedImages.Add(image);
        }

        foreach (GalleryThumbnailItem image in spawnedImages)
            StartCoroutine(DownloadImage(image));
    }

    private RectTransform FindActiveGalleryContent()
    {
        RectTransform[] activeRects =
            FindObjectsByType<RectTransform>(
                FindObjectsInactive.Exclude,
                FindObjectsSortMode.None);

        foreach (RectTransform rect in activeRects)
        {
            if (rect.name == "GalleryOpen")
                return rect;
        }

        return imageContent;
    }

    private bool IsEditableGallery(RectTransform gallery)
    {
        foreach (Transform child in gallery)
        {
            Button button = child.GetComponent<Button>();

            if (button != null && button.gameObject.activeSelf)
                return true;
        }

        return false;
    }

    private IEnumerator DownloadImage(GalleryThumbnailItem image)
    {
        string url = image.UploadedUrl;
        string requestUrl;

        try
        {
            requestUrl = new Uri(url).AbsoluteUri;
        }
        catch (UriFormatException exception)
        {
            Debug.LogError(
                "Invalid gallery image URL: " + url +
                "\n" + exception.Message);
            yield break;
        }

        Debug.Log("Downloading gallery image: " + requestUrl);

        using (UnityWebRequest request =
               UnityWebRequestTexture.GetTexture(requestUrl, false))
        {
            request.timeout = 30;
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(
                    "Gallery image download failed: " +
                    request.error +
                    "\nURL: " + requestUrl);
                yield break;
            }

            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            if (image == null)
            {
                Destroy(texture);
                yield break;
            }

            image.SetImage(texture, url);
            loadedTextures.Add(texture);
            Debug.Log("Displayed gallery image: " + requestUrl);
        }
    }

    private void DeleteImage(GalleryThumbnailItem image)
    {
        if (!deleteInProgress)
            StartCoroutine(DeleteImageFromEvent(image));
    }

    private IEnumerator DeleteImageFromEvent(GalleryThumbnailItem image)
    {
        if (image == null || string.IsNullOrEmpty(currentEventId))
        {
            Debug.LogError("Cannot delete a gallery image without an event ID.");
            yield break;
        }

        List<string> remainingUrls = new List<string>(currentUrls);

        if (!remainingUrls.Remove(image.UploadedUrl))
            yield break;

        string baseUrl = eventgetter.instance != null
            ? eventgetter.instance.baseurl
            : baselink.Url;

        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            Debug.LogError("Cannot delete the gallery image: API URL is missing.");
            yield break;
        }

        deleteInProgress = true;

        string body = Newtonsoft.Json.JsonConvert.SerializeObject(
            new { id = currentEventId, images = remainingUrls });

        using (UnityWebRequest request = new UnityWebRequest(
                   baseUrl.TrimEnd('/') + "/v1/event",
                   "PATCH"))
        {
            request.SetRequestHeader(
                "Authorization",
                "Bearer " + apigetter.jwt);
            request.SetRequestHeader("Content-Type", "application/json");
            request.uploadHandler = new UploadHandlerRaw(
                System.Text.Encoding.UTF8.GetBytes(body));
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(
                    "Gallery image delete failed (" +
                    request.responseCode + "): " +
                    request.downloadHandler.text);
                deleteInProgress = false;
                yield break;
            }
        }

        currentUrls.Clear();
        currentUrls.AddRange(remainingUrls);
        spawnedImages.Remove(image);
        loadedTextures.Remove(image.Texture);

        if (image.Texture != null)
            Destroy(image.Texture);

        Destroy(image.gameObject);
        onImagesChanged?.Invoke(new List<string>(currentUrls));
        Debug.Log("Deleted gallery image URL: " + image.UploadedUrl);
        deleteInProgress = false;
    }

    public void HideUploadedImages()
    {
        StopAllCoroutines();
        ClearImages();
        if (activeImageContent != null)
            activeImageContent.gameObject.SetActive(false);
        else if (uploadedGalleryPanel != null)
            uploadedGalleryPanel.SetActive(false);
    }

    private void ClearImages()
    {
        foreach (GalleryThumbnailItem image in spawnedImages)
        {
            if (image != null)
            {
                image.transform.SetParent(null, false);
                Destroy(image.gameObject);
            }
        }

        foreach (Texture2D texture in loadedTextures)
        {
            if (texture != null)
                Destroy(texture);
        }

        spawnedImages.Clear();
        loadedTextures.Clear();
    }
}
