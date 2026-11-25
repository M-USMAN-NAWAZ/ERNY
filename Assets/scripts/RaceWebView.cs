using System;
using System.Runtime.InteropServices;
using UnityEngine;

internal sealed class RaceWebView : IDisposable
{
    private bool open;
    private Rect bounds, cutout;
    private float nextCookieProbe, cookieInset;
    private const string CookieProbe = "(() => { const e=document.getElementById('cookiePolicyPopup'); if(!e || e.hidden || e.getAttribute('aria-hidden')==='true') return 0; const s=getComputedStyle(e), r=e.getBoundingClientRect(), v=window.visualViewport, h=v?v.height:innerHeight, y=v?v.offsetTop:0; return h>0 && s.visibility==='visible' && +s.opacity>0 && r.width>0 && r.height>0 && r.bottom>=y+h-24 ? Math.min(1,Math.max(0,(y+h-r.top)/h)) : 0; })()";
#if UNITY_EDITOR_WIN
    private readonly RaceWebEditorPreview preview = new RaceWebEditorPreview();
    public string Status => preview.Status;
#else
    public string Status => "";
#endif
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")] private static extern void ErnyRaceWebOpen(string url);
    [DllImport("__Internal")] private static extern IntPtr ErnyRaceWebCopyAddress(int landing);
    [DllImport("__Internal")] private static extern void ErnyRaceWebFreeAddress(IntPtr url);
    [DllImport("__Internal")] private static extern void ErnyRaceWebLayout(float left, float top, float right, float bottom,
        float cutLeft, float cutTop, float cutRight, float cutBottom);
    [DllImport("__Internal")] private static extern void ErnyRaceWebVisible([MarshalAs(UnmanagedType.I1)] bool visible);
    [DllImport("__Internal")] private static extern void ErnyRaceWebClose();
    [DllImport("__Internal")] private static extern float ErnyRaceWebMeasureCookieBanner(string script);
#elif UNITY_ANDROID && !UNITY_EDITOR
    private static void Call(string method, params object[] args)
    {
        using var player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        using var activity = player.GetStatic<AndroidJavaObject>("currentActivity");
        using var bridge = new AndroidJavaClass("com.blingar.ErnyRaceWebView");
        var parameters = new object[args.Length + 1];
        parameters[0] = activity;
        Array.Copy(args, 0, parameters, 1, args.Length);
        bridge.CallStatic(method, parameters);
    }
#endif

    public bool IsOnSubpage(Uri main)
    {
        if (!open || main == null) return false;
        string current = null, landing = null;
#if UNITY_EDITOR_WIN
        current = preview.CurrentAddress;
        landing = preview.LandingAddress;
#elif UNITY_IOS && !UNITY_EDITOR
        current = ReadAddress(0);
        landing = ReadAddress(1);
#elif UNITY_ANDROID && !UNITY_EDITOR
        using var bridge = new AndroidJavaClass("com.blingar.ErnyRaceWebView");
        current = bridge.CallStatic<string>("address", false);
        landing = bridge.CallStatic<string>("address", true);
#endif
        return IsSubpage(main, current, landing);
    }

    private static bool IsSubpage(Uri main, string current, string landing)
    {
        return Uri.TryCreate(current, UriKind.Absolute, out var page) && !SamePage(page, main)
            && (!Uri.TryCreate(landing, UriKind.Absolute, out var redirectedMain) || !SamePage(page, redirectedMain));
    }

    private static bool SamePage(Uri first, Uri second)
    {
        static string Host(Uri address) => address.IdnHost.StartsWith("www.", StringComparison.OrdinalIgnoreCase)
            ? address.IdnHost.Substring(4) : address.IdnHost;
        return first.Scheme == second.Scheme && first.Port == second.Port
            && string.Equals(Host(first), Host(second), StringComparison.OrdinalIgnoreCase)
            && string.Equals(first.AbsolutePath.TrimEnd('/'), second.AbsolutePath.TrimEnd('/'), StringComparison.OrdinalIgnoreCase)
            && string.Equals(first.Query, second.Query, StringComparison.Ordinal);
    }

#if UNITY_IOS && !UNITY_EDITOR
    private static string ReadAddress(int landing)
    {
        IntPtr url = ErnyRaceWebCopyAddress(landing);
        if (url == IntPtr.Zero) return null;
        try { return Marshal.PtrToStringUTF8(url); }
        finally { ErnyRaceWebFreeAddress(url); }
    }
#endif

    public bool Open(string url)
    {
        bounds = default;
        cookieInset = nextCookieProbe = 0;
#if UNITY_EDITOR_WIN
        preview.Open(url);
#elif UNITY_IOS && !UNITY_EDITOR
        ErnyRaceWebOpen(url);
#elif UNITY_ANDROID && !UNITY_EDITOR
        Call("open", url);
#else
        return false;
#endif
#if UNITY_EDITOR_WIN || (!UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS))
        open = true;
        return true;
#endif
    }

    public void Layout(RectTransform area, RectTransform actions, Vector2 restingPosition, Vector3[] corners)
    {
        if (!open || Screen.width == 0 || Screen.height == 0) return;
        var canvas = area.GetComponentInParent<Canvas>().rootCanvas;
        var camera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
        var viewport = NormalizedRect(area, camera, corners);
        var safe = UnityEngine.Device.Screen.safeArea;
        int safeWidth = Mathf.Max(1, UnityEngine.Device.Screen.width);
        int safeHeight = Mathf.Max(1, UnityEngine.Device.Screen.height);
        var next = Rect.MinMaxRect(Mathf.Max(viewport.xMin, safe.xMin / safeWidth),
            Mathf.Max(viewport.yMin, 1 - safe.yMax / safeHeight),
            Mathf.Min(viewport.xMax, safe.xMax / safeWidth),
            Mathf.Min(viewport.yMax, 1 - safe.yMin / safeHeight));
        if (next.width <= 0 || next.height <= 0) return;
        if (Time.unscaledTime >= nextCookieProbe)
        {
            nextCookieProbe = Time.unscaledTime + .25f;
            cookieInset = MeasureCookieBanner();
        }
        var parent = (RectTransform)actions.parent;
        var lift = Mathf.Clamp(cookieInset, 0, .8f) * next.height * Screen.height;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, Vector2.zero, camera, out var bottom)
            && RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, Vector2.up * lift, camera, out var top))
        {
            var position = restingPosition + Vector2.up * (top.y - bottom.y);
            if (actions.anchoredPosition != position) actions.anchoredPosition = position;
        }
        var hole = NormalizedRect(actions, camera, corners);
        if (next == bounds && hole == cutout) return;
        bounds = next;
        cutout = hole;
#if UNITY_EDITOR_WIN
        preview.Layout(bounds, cutout);
#elif UNITY_IOS && !UNITY_EDITOR
        ErnyRaceWebLayout(bounds.xMin, bounds.yMin, bounds.xMax, bounds.yMax, cutout.xMin, cutout.yMin, cutout.xMax, cutout.yMax);
#elif UNITY_ANDROID && !UNITY_EDITOR
        Call("layout", bounds.xMin, bounds.yMin, bounds.xMax, bounds.yMax, cutout.xMin, cutout.yMin, cutout.xMax, cutout.yMax);
#endif
    }

    private float MeasureCookieBanner()
    {
#if UNITY_EDITOR_WIN
        return preview.MeasureCookieBanner(CookieProbe);
#elif UNITY_IOS && !UNITY_EDITOR
        return ErnyRaceWebMeasureCookieBanner(CookieProbe);
#elif UNITY_ANDROID && !UNITY_EDITOR
        Call("measureCookieBanner", CookieProbe);
        using var bridge = new AndroidJavaClass("com.blingar.ErnyRaceWebView");
        return bridge.CallStatic<float>("cookieBannerInset");
#else
        return 0;
#endif
    }

    private static Rect NormalizedRect(RectTransform rect, Camera camera, Vector3[] corners)
    {
        rect.GetWorldCorners(corners);
        Vector2 min = RectTransformUtility.WorldToScreenPoint(camera, corners[0]);
        Vector2 max = RectTransformUtility.WorldToScreenPoint(camera, corners[2]);
        return Rect.MinMaxRect(Mathf.Clamp01(min.x / Screen.width), Mathf.Clamp01(1 - max.y / Screen.height),
            Mathf.Clamp01(max.x / Screen.width), Mathf.Clamp01(1 - min.y / Screen.height));
    }

    public void SetVisible(bool visible)
    {
        if (!open) return;
        bounds = default;
#if UNITY_EDITOR_WIN
        preview.SetVisible(visible);
#elif UNITY_IOS && !UNITY_EDITOR
        ErnyRaceWebVisible(visible);
#elif UNITY_ANDROID && !UNITY_EDITOR
        Call("visible", visible);
#endif
    }

    public void Dispose()
    {
        if (!open) return;
        open = false;
#if UNITY_EDITOR_WIN
        preview.Dispose();
#elif UNITY_IOS && !UNITY_EDITOR
        ErnyRaceWebClose();
#elif UNITY_ANDROID && !UNITY_EDITOR
        Call("close");
#endif
    }
}
