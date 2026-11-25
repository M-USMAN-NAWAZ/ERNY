using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System.Drawing.Drawing2D;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        if (args.Length != 1) return;
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
        Application.EnableVisualStyles();
        using var view = new RacePreview(args[0]);
        _ = view.Handle;
        _ = Task.Run(() =>
        {
            while (Console.ReadLine() is { } command)
                view.BeginInvoke(() => view.Apply(command));
            view.BeginInvoke(view.Close);
        });
        using var context = new ApplicationContext();
        view.FormClosed += (_, _) => context.ExitThread();
        Application.Run(context);
    }
}

internal sealed class RacePreview : Form
{
    private readonly WebView2 browser = new() { Dock = DockStyle.Fill, AllowExternalDrop = false };
    private Uri? next;
    private bool initializing;
    private bool awaitingLanding;
    private bool measuringCookie;
    private IntPtr owner;
    protected override bool ShowWithoutActivation => true;

    internal RacePreview(string profile)
    {
        FormBorderStyle = FormBorderStyle.None;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.Manual;
        Size = new Size(1, 1);
        browser.CreationProperties = new CoreWebView2CreationProperties { UserDataFolder = profile };
        Controls.Add(browser);
    }

    internal void Apply(string command)
    {
        if (command == "hide") { Hide(); return; }
        if (command.StartsWith("cookie ", StringComparison.Ordinal))
        {
            if (browser.CoreWebView2 != null && !measuringCookie) MeasureCookie(command[7..]);
            return;
        }
        if (command.StartsWith("open ") && Https(command[5..], out var uri))
        {
            next = uri;
            awaitingLanding = true;
            if (browser.CoreWebView2 != null) browser.Source = next;
            return;
        }

        string[] parts = command.Split(' ');
        if (parts.Length != 10 || parts[0] != "place" || !long.TryParse(parts[1], out long handle)
            || !int.TryParse(parts[2], out int x) || !int.TryParse(parts[3], out int y)
            || !int.TryParse(parts[4], out int width) || !int.TryParse(parts[5], out int height)
            || !int.TryParse(parts[6], out int left) || !int.TryParse(parts[7], out int top)
            || !int.TryParse(parts[8], out int right) || !int.TryParse(parts[9], out int bottom)
            || width <= 0 || height <= 0 || handle == 0) return;
        var newOwner = new IntPtr(handle);
        if (owner != newOwner) Hide();
        Bounds = new Rectangle(x, y, width, height);
        var region = new Region(new Rectangle(0, 0, width, height));
        if (right > left && bottom > top)
        {
            using var capsule = new GraphicsPath();
            int diameter = Math.Min(right - left, bottom - top);
            capsule.AddArc(left, top, diameter, diameter, 180, 90);
            capsule.AddArc(right - diameter, top, diameter, diameter, 270, 90);
            capsule.AddArc(right - diameter, bottom - diameter, diameter, diameter, 0, 90);
            capsule.AddArc(left, bottom - diameter, diameter, diameter, 90, 90);
            capsule.CloseFigure();
            region.Exclude(capsule);
        }
        Region = region;
        if (!Visible) Show(new WindowHandle(newOwner));
        owner = newOwner;
        if (!initializing) Initialize();
    }

    private async void MeasureCookie(string script)
    {
        measuringCookie = true;
        try { Console.WriteLine("cookie " + await browser.CoreWebView2.ExecuteScriptAsync(script)); }
        catch (Exception) { Console.WriteLine("cookie 0"); }
        finally { measuringCookie = false; }
    }

    private async void Initialize()
    {
        initializing = true;
        try
        {
            await browser.EnsureCoreWebView2Async();
            var core = browser.CoreWebView2;
            core.Settings.IsWebMessageEnabled = false;
            core.Settings.AreHostObjectsAllowed = false;
            core.NavigationStarting += (sender, e) => e.Cancel = !Https(e.Uri, out _);
            core.NewWindowRequested += (_, e) =>
            {
                e.Handled = true;
                if (Https(e.Uri, out var uri)) browser.Source = uri;
            };
            core.SourceChanged += (_, _) => Console.WriteLine("location " + core.Source);
            core.NavigationCompleted += (_, e) =>
            {
                if (awaitingLanding && e.IsSuccess)
                {
                    awaitingLanding = false;
                    Console.WriteLine("landing " + core.Source);
                }
                Console.WriteLine(e.IsSuccess ? "loaded" : "navigation failed: " + e.WebErrorStatus);
            };
            if (next != null) browser.Source = next;
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
            Close();
        }
    }

    private static bool Https(string address, out Uri uri) =>
        Uri.TryCreate(address, UriKind.Absolute, out uri!) && uri.Scheme == Uri.UriSchemeHttps;

    private sealed record WindowHandle(IntPtr Handle) : IWin32Window;
}
