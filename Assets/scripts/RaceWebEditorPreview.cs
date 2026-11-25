#if UNITY_EDITOR_WIN
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

internal sealed class RaceWebEditorPreview : IDisposable
{
    private const string BuildFolder = "Library/RaceWebPreview";
    private const string Executable = BuildFolder + "/bin/RaceWebPreview/release/RaceWebPreview.exe";
    private const string ToolFolder = "Tools/RaceWebPreview";
    private const BindingFlags InstanceFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
    private static readonly int EditorPid = Process.GetCurrentProcess().Id;
    private static readonly FieldInfo Parent = typeof(EditorWindow).GetField("m_Parent", InstanceFlags);
    private Process build, host;
    private string url, lastPlacement;
    private Rect bounds, cutout;
    private bool visible, watching;
    private EditorWindow simulator;
    private IMGUIContainer anchor;
    private Vector2 panelScreenOrigin, anchorScreenPoint;
    private float dpi;
    private double nextUpdate;
    public string Status { get; private set; } = "";
    public string CurrentAddress { get; private set; }
    public string LandingAddress { get; private set; }
    private volatile float cookieInset;

    public void Open(string address)
    {
        url = address;
        CurrentAddress = address;
        LandingAddress = null;
        cookieInset = 0;
        visible = true;
        bounds = default;
        lastPlacement = null;
        Status = "Loading RunSignup...";
        if (!watching)
        {
            watching = true;
            EditorApplication.update += Update;
            AssemblyReloadEvents.beforeAssemblyReload += Dispose;
        }
        if (host != null && !host.HasExited) Send("open " + url);
        else if (build == null) StartHostOrBuild();
    }

    public void Layout(Rect area, Rect overlay)
    {
        bounds = area;
        cutout = overlay;
    }

    public float MeasureCookieBanner(string script)
    {
        Send("cookie " + script);
        return cookieInset;
    }

    public void SetVisible(bool show)
    {
        visible = show;
        lastPlacement = null;
        if (!show) Send("hide");
    }

    private void StartHostOrBuild()
    {
        try
        {
            var compiled = File.GetLastWriteTimeUtc(Path.ChangeExtension(Executable, ".dll"));
            if (!File.Exists(Executable) || compiled < File.GetLastWriteTimeUtc(ToolFolder + "/Program.cs") ||
                compiled < File.GetLastWriteTimeUtc(ToolFolder + "/RaceWebPreview.csproj"))
            {
                Status = "Preparing the Windows browser preview...";
                build = Process.Start(new ProcessStartInfo("dotnet",
                    "build " + ToolFolder + "/RaceWebPreview.csproj --artifacts-path " + BuildFolder + " -c Release --nologo")
                    { UseShellExecute = false, CreateNoWindow = true });
                return;
            }

            Directory.CreateDirectory(BuildFolder + "/Profile");
            host?.Dispose();
            host = Process.Start(new ProcessStartInfo(Path.GetFullPath(Executable),
                "\"" + Path.GetFullPath(BuildFolder + "/Profile") + "\"")
                { UseShellExecute = false, CreateNoWindow = true, RedirectStandardInput = true, RedirectStandardOutput = true,
                    RedirectStandardError = true });
            host.OutputDataReceived += (_, data) =>
            {
                if (data.Data == "loaded") Status = "";
                else if (data.Data != null && data.Data.StartsWith("cookie ", StringComparison.Ordinal))
                    cookieInset = float.TryParse(data.Data.Substring(7), NumberStyles.Float, CultureInfo.InvariantCulture, out var value)
                        && !float.IsNaN(value) ? value : 0;
                else if (data.Data != null && data.Data.StartsWith("location ", StringComparison.Ordinal))
                    CurrentAddress = data.Data.Substring(9);
                else if (data.Data != null && data.Data.StartsWith("landing ", StringComparison.Ordinal))
                    LandingAddress = data.Data.Substring(8);
            };
            host.ErrorDataReceived += (_, data) =>
            {
                if (data.Data != null) Status = "WebView2 could not start: " + data.Data;
            };
            host.BeginOutputReadLine();
            host.BeginErrorReadLine();
            Send("open " + url);
        }
        catch (Exception e)
        {
            Status = "Could not start the Windows browser preview: " + e.Message;
        }
    }

    private void Update()
    {
        if (EditorApplication.timeSinceStartup < nextUpdate) return;
        nextUpdate = EditorApplication.timeSinceStartup + .05;
        if (build != null && build.HasExited)
        {
            bool success = build.ExitCode == 0 && File.Exists(Executable);
            build.Dispose();
            build = null;
            if (success) StartHostOrBuild();
            else Status = "The Windows browser preview could not build. Check the .NET SDK and NuGet connection.";
        }
        if (host == null) return;
        if (host.HasExited)
        {
            if (string.IsNullOrEmpty(Status) || Status == "Loading RunSignup...")
                Status = "The Windows browser preview closed. Reopen the race to retry.";
            return;
        }

        if (visible && bounds.width > 0 && TryPlace(out string placement))
        {
            if (lastPlacement != placement) Send(placement);
            lastPlacement = placement;
        }
        else if (lastPlacement != null)
        {
            Send("hide");
            lastPlacement = null;
        }
    }

    private bool TryPlace(out string placement)
    {
        placement = null;
        if (!ActiveTab(simulator))
        {
            anchor?.RemoveFromHierarchy();
            anchor = null;
            simulator = null;
            foreach (var window in Resources.FindObjectsOfTypeAll<EditorWindow>())
            {
                if (window.GetType().FullName != "UnityEditor.DeviceSimulation.SimulatorWindow" || !ActiveTab(window)) continue;
                simulator = window;
                dpi = 0;
                anchor = new IMGUIContainer(() =>
                {
                    if (Event.current.type != EventType.Repaint) return;
                    anchorScreenPoint = GUIUtility.GUIToScreenPoint(Vector2.zero);
                    // Simulator padding is panel-relative, not relative to this IMGUI element.
                    panelScreenOrigin = GUIUtility.GUIToScreenPoint(anchor.WorldToLocal(Vector2.zero));
                    dpi = EditorGUIUtility.pixelsPerPoint;
                }) { pickingMode = PickingMode.Ignore };
                anchor.style.position = Position.Absolute;
                anchor.style.left = anchor.style.top = 0;
                anchor.style.width = anchor.style.height = 1;
                simulator.rootVisualElement.Add(anchor);
                simulator.Repaint();
                break;
            }
        }
        if (simulator == null)
        {
            Status = "Open the Device Simulator tab to view the live race page.";
            return false;
        }

        // Use the same screen origin and zoom that the Simulator uses for pointer input.
        var type = simulator.GetType();
        if (dpi <= 0 || !(type.GetProperty("simulatorViewPadding", InstanceFlags)?.GetValue(simulator) is Vector2 padding)
            || !(type.GetProperty("simulatorViewMouseScale", InstanceFlags)?.GetValue(simulator) is float mouseScale)
            || mouseScale <= 0) return false;
        var origin = (panelScreenOrigin + padding) * dpi;
        var owner = GetAncestor(WindowFromPoint(new Point((int)((anchorScreenPoint.x + 5) * dpi), (int)((anchorScreenPoint.y + 5) * dpi))), 2);
        if (owner == IntPtr.Zero || WindowProcess(owner) != EditorPid) return false;
        var foreground = GetForegroundWindow();
        if (foreground != owner && WindowProcess(foreground) != host.Id) return false;
        float scale = dpi / mouseScale;
        float width = scale * UnityEngine.Device.Screen.width;
        float height = scale * UnityEngine.Device.Screen.height;
        int x = Mathf.RoundToInt(origin.x + bounds.xMin * width);
        int y = Mathf.RoundToInt(origin.y + bounds.yMin * height);
        int w = Mathf.RoundToInt(origin.x + bounds.xMax * width) - x;
        int h = Mathf.RoundToInt(origin.y + bounds.yMax * height) - y;
        int left = Mathf.FloorToInt(origin.x + cutout.xMin * width) - x;
        int top = Mathf.FloorToInt(origin.y + cutout.yMin * height) - y;
        int right = Mathf.CeilToInt(origin.x + cutout.xMax * width) - x;
        int bottom = Mathf.CeilToInt(origin.y + cutout.yMax * height) - y;
        if (w <= 0 || h <= 0) return false;
        if (Status == "Open the Device Simulator tab to view the live race page.") Status = "";
        placement = FormattableString.Invariant($"place {owner.ToInt64()} {x} {y} {w} {h} {left} {top} {right} {bottom}");
        return true;
    }

    private static bool ActiveTab(EditorWindow window)
    {
        if (window == null || window.rootVisualElement.panel == null) return false;
        object parent = Parent?.GetValue(window);
        return ReferenceEquals(parent?.GetType().GetProperty("actualView", InstanceFlags)?.GetValue(parent), window);
    }

    private void Send(string line)
    {
        try { if (host != null && !host.HasExited) host.StandardInput.WriteLine(line); }
        catch (IOException) { }
        catch (InvalidOperationException) { }
    }

    public void Dispose()
    {
        if (!watching) return;
        watching = false;
        EditorApplication.update -= Update;
        AssemblyReloadEvents.beforeAssemblyReload -= Dispose;
        anchor?.RemoveFromHierarchy();
        anchor = null;
        simulator = null;
        if (host != null)
        {
            host.StandardInput.Close();
            host.Dispose();
            host = null;
        }
        build?.Dispose();
        build = null;
    }

    [StructLayout(LayoutKind.Sequential)]
    private readonly struct Point
    {
        internal readonly int X, Y;
        internal Point(int x, int y) { X = x; Y = y; }
    }

    private static uint WindowProcess(IntPtr window)
    {
        GetWindowThreadProcessId(window, out uint process);
        return process;
    }

    [DllImport("user32.dll")] private static extern IntPtr WindowFromPoint(Point point);
    [DllImport("user32.dll")] private static extern IntPtr GetAncestor(IntPtr window, uint flags);
    [DllImport("user32.dll")] private static extern IntPtr GetForegroundWindow();
    [DllImport("user32.dll")] private static extern uint GetWindowThreadProcessId(IntPtr window, out uint process);
}
#endif
