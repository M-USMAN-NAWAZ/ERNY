using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

internal sealed class EventPacketPreview : MonoBehaviour
{
    private GalleryPreviewPanel preview;
    private Texture2D texture;
    private UnityWebRequest download;
    private string workFolder, sourcePath;
    private const long PageCacheLimit = 64L * 1024 * 1024;
    private Text message, pageNumber;
    private int pageCount;
    private RectTransform viewport, content;
    private float pageAspect, pageHeight, pageStride;
    private Vector2 viewportSize;
    private readonly Dictionary<int, RawImage> renderedPages = new Dictionary<int, RawImage>();
    private readonly HashSet<int> failedPages = new HashSet<int>();
    private readonly List<int> stalePages = new List<int>();
    private Task<int> render;
    private static EventPacketPreview current;

    internal static void Open(Uri address, Font font)
    {
        if (current != null) current.preview.ClosePreview();
        var gallery = FindObjectsByType<GalleryPreviewPanel>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .FirstOrDefault(panel => panel.GetComponent<EventPacketPreview>() == null);
        if (gallery == null) return;

        var panel = Instantiate(gallery, gallery.transform.parent, false);
        panel.name = "Packet Document Preview";
        panel.transform.SetAsLastSibling();
        var viewer = panel.gameObject.AddComponent<EventPacketPreview>();
        current = viewer;
        viewer.preview = panel;
        viewer.message = viewer.Label("Opening document...", font, .16f, .42f, .84f, .58f);
        viewer.pageNumber = viewer.Label("", font, .36f, .055f, .64f, .125f);
        viewer.pageNumber.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
        viewer.StartCoroutine(viewer.Load(address));
    }

    private IEnumerator Load(Uri address)
    {
        try
        {
            workFolder = Path.Combine(Application.temporaryCachePath, "erny-preview-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(workFolder);
            sourcePath = Path.Combine(workFolder, "document");
        }
        catch (IOException) { message.text = "Couldn't prepare the document preview."; yield break; }

        using (var request = new UnityWebRequest(address.AbsoluteUri, UnityWebRequest.kHttpVerbGET))
        {
            download = request;
            request.downloadHandler = new DownloadHandlerFile(sourcePath) { removeFileOnAbort = true };
            request.timeout = 60;
            request.redirectLimit = 0;
            var operation = request.SendWebRequest();
            while (!operation.isDone)
            {
                if (request.downloadedBytes > EventPacketApi.MaxBytes)
                {
                    request.Abort();
                    break;
                }
                yield return null;
            }
            download = null;
            if (request.result != UnityWebRequest.Result.Success || request.downloadedBytes > EventPacketApi.MaxBytes)
            {
                message.text = "Couldn't load this document.";
                yield break;
            }
        }

        bool pdf;
        try
        {
            using var stream = File.OpenRead(sourcePath);
            var header = new byte[5];
            pdf = stream.Read(header, 0, header.Length) == header.Length &&
                System.Text.Encoding.ASCII.GetString(header) == "%PDF-";
        }
        catch (IOException) { message.text = "Couldn't read this document."; yield break; }

        if (pdf) yield return ScrollPdf();
        else if (!ShowImage(sourcePath)) message.text = "Couldn't preview this image.";
    }

    private IEnumerator RenderPage(int page, Action<Texture2D> loaded)
    {
        string input = sourcePath, output = Path.Combine(workFolder, $"page-{page}.png");
        var cached = ReadCachedPage(output);
        if (cached != null)
        {
            loaded(cached);
            yield break;
        }
        string tools = Path.Combine(Application.dataPath, "../Tools", "RenderPacketPdf.ps1");
        render = Task.Run(() => RenderPdf(input, output, page, tools));
        while (!render.IsCompleted) yield return null;
        int pages = render.IsFaulted || render.IsCanceled ? 0 : render.Result;
        render = null;
        if (pages > 0)
        {
            pageCount = pages;
            TrimPageCache(workFolder, output);
        }
        loaded(pages > 0 ? ReadImage(output) : null);
    }

    private IEnumerator ScrollPdf()
    {
        message.text = "Opening PDF...";
        Texture2D firstPage = null;
        yield return RenderPage(0, image => firstPage = image);
        if (firstPage == null)
        {
            message.text = "Couldn't preview this PDF.";
            yield break;
        }
        pageAspect = firstPage.height / (float)firstPage.width;
        BuildScroller();
        LayoutPages();
        AddPage(0, firstPage);
        pageNumber.gameObject.SetActive(pageCount > 1);

        while (true)
        {
            LayoutPages();
            var (first, last) = VisiblePages(content.anchoredPosition.y, viewport.rect.height, pageStride, pageCount);
            int nearFirst = Mathf.Max(0, first - 1), nearLast = Mathf.Min(pageCount - 1, last + 2);
            stalePages.Clear();
            foreach (var entry in renderedPages)
                if (entry.Key < nearFirst || entry.Key > nearLast) stalePages.Add(entry.Key);
            foreach (int page in stalePages)
            {
                Destroy(renderedPages[page].texture);
                Destroy(renderedPages[page].gameObject);
                renderedPages.Remove(page);
            }

            pageNumber.text = $"{first + 1} / {pageCount}";
            bool failed = failedPages.Contains(first);
            message.gameObject.SetActive(failed);
            if (failed) message.text = "Couldn't preview this page.";
            int nextPage = MissingPage(first, last);
            if (nextPage < 0) nextPage = MissingPage(nearFirst, nearLast);
            if (nextPage < 0) { yield return null; continue; }

            Texture2D image = null;
            yield return RenderPage(nextPage, page => image = page);
            if (image != null) AddPage(nextPage, image);
            else failedPages.Add(nextPage);
        }
    }

    private static Texture2D ReadCachedPage(string path)
    {
        if (!File.Exists(path)) return null;
        var image = ReadImage(path);
        if (image == null) return null;
        try { File.SetLastWriteTimeUtc(path, DateTime.UtcNow); }
        catch (IOException) { }
        catch (UnauthorizedAccessException) { }
        return image;
    }

    private static void TrimPageCache(string folder, string newest)
    {
        try
        {
            var pages = Directory.EnumerateFiles(folder, "page-*.png").Select(path => new FileInfo(path))
                .OrderBy(file => file.LastWriteTimeUtc).ToArray();
            long bytes = pages.Sum(file => file.Length);
            foreach (var page in pages)
            {
                if (bytes <= PageCacheLimit) break;
                if (page.FullName == newest) continue;
                long length = page.Length;
                page.Delete();
                bytes -= length;
            }
        }
        catch (IOException) { }
        catch (UnauthorizedAccessException) { }
    }

    internal static (int first, int last) VisiblePages(float scrollY, float height, float stride, int count)
    {
        float top = Mathf.Max(0, scrollY);
        int first = Mathf.Clamp(Mathf.FloorToInt(top / stride), 0, count - 1);
        int last = Mathf.Clamp(Mathf.CeilToInt((top + height) / stride) - 1, first, count - 1);
        return (first, last);
    }

    private int MissingPage(int first, int last)
    {
        for (int i = first; i <= last; i++)
            if (!renderedPages.ContainsKey(i) && !failedPages.Contains(i)) return i;
        return -1;
    }

    private void BuildScroller()
    {
        var image = preview.PreviewImage;
        image.enabled = false;
        var area = new GameObject("PDF Scroll Area", typeof(RectTransform), typeof(Image), typeof(RectMask2D),
            typeof(ScrollRect), typeof(Button));
        area.layer = gameObject.layer;
        viewport = (RectTransform)area.transform;
        viewport.SetParent(image.rectTransform, false);
        viewport.anchorMin = Vector2.zero;
        viewport.anchorMax = Vector2.one;
        viewport.offsetMin = viewport.offsetMax = Vector2.zero;
        area.GetComponent<Image>().color = Color.clear;
        // Let the scroller consume clicks without invoking the Gallery backdrop's Close action.
        area.GetComponent<Button>().transition = Selectable.Transition.None;
        content = (RectTransform)new GameObject("PDF Pages", typeof(RectTransform)).transform;
        content.SetParent(viewport, false);
        content.anchorMin = Vector2.up;
        content.anchorMax = Vector2.one;
        content.pivot = new Vector2(.5f, 1);
        content.anchoredPosition = Vector2.zero;
        var scroll = area.GetComponent<ScrollRect>();
        scroll.viewport = viewport;
        scroll.content = content;
        scroll.horizontal = false;
        scroll.movementType = ScrollRect.MovementType.Clamped;
        scroll.scrollSensitivity = 100;
        Canvas.ForceUpdateCanvases();
    }

    private void LayoutPages()
    {
        var size = viewport.rect.size;
        if (size == viewportSize && pageStride > 0) return;
        float pageOffset = pageStride > 0 ? content.anchoredPosition.y / pageStride : 0;
        viewportSize = size;
        pageHeight = Mathf.Max(1, size.x) * Mathf.Clamp(pageAspect, .25f, 3f);
        pageStride = pageHeight + 24;
        content.sizeDelta = new Vector2(0, pageStride * pageCount - 24);
        content.anchoredPosition = Vector2.up * (pageOffset * pageStride);
        foreach (var entry in renderedPages) PlacePage(entry.Key, entry.Value);
    }

    private void AddPage(int index, Texture2D image)
    {
        var page = new GameObject($"Page {index + 1}", typeof(RectTransform), typeof(RawImage));
        page.layer = gameObject.layer;
        page.transform.SetParent(content, false);
        var raw = page.GetComponent<RawImage>();
        raw.texture = image;
        raw.raycastTarget = false;
        renderedPages.Add(index, raw);
        PlacePage(index, raw);
    }

    private void PlacePage(int index, RawImage image)
    {
        var rect = image.rectTransform;
        rect.anchorMin = rect.anchorMax = new Vector2(.5f, 1);
        rect.anchoredPosition = Vector2.down * (index * pageStride + pageHeight * .5f);
        float scale = Mathf.Min(viewportSize.x / image.texture.width, pageHeight / image.texture.height);
        rect.sizeDelta = new Vector2(image.texture.width * scale, image.texture.height * scale);
    }

    private static Texture2D ReadImage(string path)
    {
        var image = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        try
        {
            if (image.LoadImage(File.ReadAllBytes(path), true)) return image;
        }
        catch (IOException) { }
        Destroy(image);
        return null;
    }

    private bool ShowImage(string path)
    {
        var image = ReadImage(path);
        if (image == null) return false;
        preview.Show(image);
        if (texture != null) Destroy(texture);
        texture = image;
        message.gameObject.SetActive(false);
        return true;
    }

    private Text Label(string caption, Font font, float left, float bottom, float right, float top)
    {
        var label = new GameObject("Packet viewer " + caption, typeof(RectTransform), typeof(Text));
        label.layer = gameObject.layer;
        var rect = (RectTransform)label.transform;
        rect.SetParent(transform, false);
        rect.anchorMin = new Vector2(left, bottom);
        rect.anchorMax = new Vector2(right, top);
        rect.offsetMin = rect.offsetMax = Vector2.zero;
        var text = label.GetComponent<Text>();
        text.font = font != null ? font : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.resizeTextForBestFit = true;
        text.resizeTextMinSize = 16;
        text.resizeTextMaxSize = 72;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        text.raycastTarget = false;
        text.text = caption;
        return text;
    }

    private void OnDisable()
    {
        if (current == this) current = null;
        var request = download;
        download = null;
        if (request != null) request.Abort();
        StopAllCoroutines();
        request?.Dispose();
        if (texture != null) Destroy(texture);
        foreach (var page in renderedPages.Values) Destroy(page.texture);
        renderedPages.Clear();
        string folder = workFolder;
        if (render != null && !render.IsCompleted)
            _ = render.ContinueWith(_ => RemoveTemporaryFiles(folder));
        else RemoveTemporaryFiles(folder);
        Destroy(gameObject);
    }

    private static void RemoveTemporaryFiles(string folder)
    {
        if (folder == null) return;
        try
        {
            File.Delete(Path.Combine(folder, "document"));
            foreach (var page in Directory.EnumerateFiles(folder, "page-*.png"))
                File.Delete(page);
            Directory.Delete(folder);
        }
        catch (IOException) { }
        catch (UnauthorizedAccessException) { }
    }

#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")] private static extern int ErnyRenderPacketPdf(string path, string output, int page);
#endif

    private static int RenderPdf(string source, string output, int page, string tool)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJNI.AttachCurrentThread();
        try
        {
            using var renderer = new AndroidJavaClass("com.blingar.ErnyPacketPdf");
            return renderer.CallStatic<int>("render", source, output, page);
        }
        finally { AndroidJNI.DetachCurrentThread(); }
#elif UNITY_IOS && !UNITY_EDITOR
        return ErnyRenderPacketPdf(source, output, page);
#elif UNITY_EDITOR_WIN
        static string Quote(string value) => "'" + value.Replace("'", "''") + "'";
        string script = $"$pdf={Quote(source)}; $png={Quote(output)}; $index={page};\n" + File.ReadAllText(tool);
        string encoded = Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(script));
        using var worker = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("powershell.exe",
            "-NoProfile -NonInteractive -EncodedCommand " + encoded)
            { UseShellExecute = false, CreateNoWindow = true, RedirectStandardOutput = true, RedirectStandardError = true });
        if (worker == null) return 0;
        var stdout = worker.StandardOutput.ReadToEndAsync();
        var stderr = worker.StandardError.ReadToEndAsync();
        if (!worker.WaitForExit(30000)) { worker.Kill(); worker.WaitForExit(); return 0; }
        return worker.ExitCode == 0 && int.TryParse(stdout.Result.Trim(), out int count) ? count : 0;
#else
        return 0;
#endif
    }
}
