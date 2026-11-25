using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public sealed class EventPacketPicker : MonoBehaviour
{
    private const string Receiver = "Erny Packet Picker";
    private static EventPacketPicker instance;
    private Action<string, string> complete;

#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")] private static extern void ErnyPickPacketFile();
#endif

    internal static void Pick(Action<string, string> callback)
    {
        if (instance == null)
        {
            instance = new GameObject(Receiver).AddComponent<EventPacketPicker>();
            DontDestroyOnLoad(instance.gameObject);
        }
        if (instance.complete != null) { callback(null, "A file picker is already open."); return; }
        instance.complete = callback;
        try
        {
#if UNITY_EDITOR
            string path = UnityEditor.EditorUtility.OpenFilePanelWithFilters("Choose a race document", "",
                new[] { "PDF and images", "pdf,jpg,jpeg,png" });
            instance.Picked(path);
#elif UNITY_ANDROID
            using var player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            using var activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            using var picker = new AndroidJavaClass("com.blingar.ErnyPacketPicker");
            picker.CallStatic("open", activity);
#elif UNITY_IOS
            ErnyPickPacketFile();
#else
            instance.PickFailed("File picking is not available on this platform.");
#endif
        }
        catch (Exception) { instance.PickFailed("Couldn't open the file picker."); }
    }

    [UnityEngine.Scripting.Preserve]
    public void Picked(string path)
    {
        if (string.IsNullOrEmpty(path)) { Finish(null, null); return; }
        var file = new FileInfo(path);
        if (!file.Exists || file.Length == 0 || file.Length > EventPacketApi.MaxBytes || EventPacketApi.Mime(path) == null)
        {
            Release(path);
            Finish(null, "Choose a PDF, JPG or PNG up to 25 MB.");
            return;
        }
        Finish(path, null);
    }

    [UnityEngine.Scripting.Preserve]
    public void PickFailed(string message) => Finish(null, message);

    private void Finish(string path, string message)
    {
        var callback = complete;
        complete = null;
        callback?.Invoke(path, message);
    }

    internal static void Release(string path)
    {
#if !UNITY_EDITOR
        if (string.IsNullOrEmpty(path)) return;
        try
        {
            var folder = Path.GetDirectoryName(path);
            if (folder == null || !Path.GetFileName(folder).StartsWith("erny-packet-", StringComparison.Ordinal)) return;
            File.Delete(path);
            Directory.Delete(folder);
        }
        catch (IOException) { }
        catch (UnauthorizedAccessException) { }
#endif
    }
}
