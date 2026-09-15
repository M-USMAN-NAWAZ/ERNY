using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine.UI;

public class BadgeImageManager : MonoBehaviour
{
    public static BadgeImageManager instance;

    public ImageDatabase database;

    private string cacheFolder;
    private Dictionary<string, Sprite> loadedSprites = new Dictionary<string, Sprite>();
    private Dictionary<string, string> loadedtext = new Dictionary<string, string>();

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        cacheFolder = Path.Combine(Application.persistentDataPath, "BadgeImages");
        if (!Directory.Exists(cacheFolder))
            Directory.CreateDirectory(cacheFolder);
    }

    public void ApplyBadgeImage(string badgeID, string badgeURL, Image targetImage, string heading, Sprite spr)
    {
        Debug.Log("Inside ApplyBadgeImage: " + badgeID);

        // Find badge by ID
        BadgeData badge = database.badges.Find(b => b.id == badgeID);

        // If not found, create it
        if (badge == null)
        {
            Debug.Log("New badge added: " + badgeID);

            badge = new BadgeData
            {
                id = badgeID,
                URL = badgeURL,
                badgeHeading = heading
            };

            database.badges.Add(badge);
        }
        else
        {
            // Update URL if changed
            if (badge.URL != badgeURL)
            {
                Debug.Log("Badge URL updated: " + badgeID);
                badge.URL = badgeURL;
            }
            else if(badge.badgeHeading != heading)
            {
                badge.badgeHeading = heading;
            }
        }

        // Safety check
        if (!string.IsNullOrEmpty(badge.URL))
        {
            StartCoroutine(LoadOrGetSprite(badge, targetImage, spr));
        }
        else
        {
            Debug.LogError("Badge URL is empty: " + badgeID);
        }

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(database);
#endif
    }

    private IEnumerator LoadOrGetSprite(BadgeData badge, Image targetImage, Sprite spr)
    {
        string imageKey = baselink.Url + "/" + badge.URL;
        string filePath = GetImagePath(imageKey);

        // 1️⃣ Already loaded in memory
        if (loadedSprites.TryGetValue(imageKey, out Sprite cachedSprite))
        {
            targetImage.sprite = cachedSprite;
            yield break;
        }

        // 2️⃣ Already downloaded on disk
        if (File.Exists(filePath))
        {
            byte[] bytes = File.ReadAllBytes(filePath);
            Sprite sprite = CreateSpriteFromBytes(bytes);
            loadedSprites[imageKey] = sprite;
            targetImage.sprite = sprite;
            yield break;
        }

        // 3️⃣ Download
        using UnityWebRequest req = UnityWebRequestTexture.GetTexture(imageKey);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to download badge image: " + imageKey);
            yield break;
        }

        Texture2D tex = DownloadHandlerTexture.GetContent(req);
        byte[] png = tex.EncodeToPNG();
        File.WriteAllBytes(filePath, png);

        Sprite downloadedSprite = CreateSpriteFromBytes(png);
        loadedSprites[imageKey] = downloadedSprite;
        targetImage.sprite = downloadedSprite;
        spr = downloadedSprite;
    }

    private string GetImagePath(string url)
    {
        return Path.Combine(cacheFolder, Hash(url) + ".png");
    }

    private string Hash(string input)
    {
        using SHA256 sha = SHA256.Create();
        byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
        return System.BitConverter.ToString(bytes).Replace("-", "");
    }

    private Sprite CreateSpriteFromBytes(byte[] bytes)
    {
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(bytes);
        return Sprite.Create(
            tex,
            new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f)
        );
    }
}
