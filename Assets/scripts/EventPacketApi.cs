using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

internal static class EventPacketApi
{
    internal const int MaxBytes = 25 * 1024 * 1024;
    private const string Stage = "https://api-stage.blingar.cloud";
    private const string Endpoint = Stage + "/v1/event/documents";
    internal static bool Enabled => string.Equals(baselink.Url?.TrimEnd('/'), Stage, StringComparison.OrdinalIgnoreCase);

    internal sealed class Document
    {
        public string field, url, name, mimeType;
    }

    internal static string Mime(string path)
    {
        switch (Path.GetExtension(path).ToLowerInvariant())
        {
            case ".pdf": return "application/pdf";
            case ".jpg": case ".jpeg": return "image/jpeg";
            case ".png": return "image/png";
            default: return null;
        }
    }

    internal static IEnumerator Request(string method, string eventId, string field, string path, string name,
        Action<List<Document>, string> complete)
    {
        if (!Enabled || string.IsNullOrWhiteSpace(eventId) || string.IsNullOrEmpty(apigetter.jwt))
        {
            complete(null, "Sign in on staging to manage race documents.");
            yield break;
        }

        UnityWebRequest request;
        try
        {
            if (method == "GET")
            {
                request = UnityWebRequest.Get(Endpoint + "?eventId=" + Uri.EscapeDataString(eventId));
            }
            else if (method == "DELETE")
            {
                request = new UnityWebRequest(Endpoint, method);
                request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { eventId, field })));
                request.SetRequestHeader("Content-Type", "application/json");
            }
            else
            {
                var form = new List<IMultipartFormSection> {
                    new MultipartFormDataSection("eventId", eventId),
                    new MultipartFormDataSection("field", field)
                };
                if (name != null) form.Add(new MultipartFormDataSection("name", name));
                if (path != null)
                {
                    string mime = Mime(path);
                    var file = new FileInfo(path);
                    if (mime == null || file.Length <= 0 || file.Length > MaxBytes)
                        throw new IOException("Choose a PDF, JPG or PNG up to 25 MB.");
                    form.Add(new MultipartFormFileSection("document", File.ReadAllBytes(path), file.Name, mime));
                }
                request = UnityWebRequest.Post(Endpoint, form);
                request.method = method;
            }
        }
        catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
        {
            complete(null, "Couldn't read the document. Choose a PDF, JPG or PNG up to 25 MB.");
            yield break;
        }

        using (request)
        {
            if (request.downloadHandler == null) request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Authorization", "Bearer " + apigetter.jwt);
            request.timeout = 90;
            request.redirectLimit = 0;
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                string message = request.responseCode == 401 ? "Sign in again to access this packet."
                    : request.responseCode == 403 ? "Only the event owner can access this packet."
                    : request.responseCode == 400 ? "Staging rejected the document. Check the file type and size."
                    : "Couldn't access the packet. Try again.";
                complete(null, message);
                yield break;
            }
            if (TryRead(request.downloadHandler.text, out var documents)) complete(documents, null);
            else complete(null, "Staging returned an unexpected packet response.");
        }
    }

    internal static bool TryRead(string json, out List<Document> documents)
    {
        documents = null;
        try
        {
            var root = JObject.Parse(json);
            if ((bool?)root["success"] != true) return false;
            var payload = root["response"] ?? root["data"];
            var items = payload is JObject obj ? obj["documents"] as JArray : payload as JArray;
            if (items == null) return false;
            documents = items.ToObject<List<Document>>();
            return documents != null;
        }
        catch (JsonException) { return false; }
    }
}
