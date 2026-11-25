using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class EventPacketSetup
{
    private const string MainScene = "Assets/Scene/4. MainScreens.unity";
    private const string Request = "Library/EventPacketSprites.request";
    private const string Result = "Library/EventPacketSprites.result";
    private const string Background = "Assets/Prefabs/Packet Icon Background.asset";

    static EventPacketSetup() => EditorApplication.update += HandleRequest;

    private static void HandleRequest()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling
            || EditorApplication.isUpdating || !File.Exists(Request)) return;
        File.Delete(Request);
        try { File.WriteAllText(Result, Install(false)); }
        catch (Exception error) { File.WriteAllText(Result, error.ToString()); Debug.LogException(error); }
    }

    [MenuItem("Tools/Erny/Add or Select Packet UI")]
    private static void SelectPacket()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            Debug.LogWarning("Exit Play Mode before editing Packet UI.");
            return;
        }
        Debug.Log(Install(true));
    }

    private static string Install(bool select)
    {
        Scene scene = SceneManager.GetSceneByPath(MainScene);
        bool openedForInstall = !scene.isLoaded;
        if (openedForInstall) scene = EditorSceneManager.OpenScene(MainScene, OpenSceneMode.Additive);
        try
        {
            var roots = scene.GetRootGameObjects();
            var create = roots.SelectMany(root => root.GetComponentsInChildren<eventgetter>(true))
                .FirstOrDefault(screen => EventPacketPanel.HasLayout(screen.parentscroll));
            var card = roots.SelectMany(root => root.GetComponentsInChildren<recievedata>(true))
                .FirstOrDefault(screen => EventPacketPanel.HasLayout(screen.parentscroll) && screen.update != null
                    && EventPacketPanel.HasLayout(screen.update.parentscroll));
            if (create == null || card == null || create.parentscroll == card.parentscroll ||
                create.parentscroll == card.update.parentscroll || card.parentscroll == card.update.parentscroll)
                throw new InvalidOperationException("Couldn't find three distinct Create, Update, and Event Card layouts.");

            var update = card.update;
            var screens = new[] { create.parentscroll, update.parentscroll, card.parentscroll };
            bool changed = screens.Any(scroll => EventPacketPanel.FindPanel(scroll)?.HasLinksLayout != true ||
                EventPacketPanel.FindPanel(scroll)?.HasStateSprites != true);
            bool wasDirty = scene.isDirty;
            if (changed)
            {
                var icon = AssetDatabase.LoadAllAssetsAtPath(Background).OfType<Sprite>().FirstOrDefault();
                if (icon == null)
                {
                    icon = EventPacketPanel.CreateIconBackground();
                    AssetDatabase.CreateAsset(icon.texture, Background);
                    AssetDatabase.AddObjectToAsset(icon, Background);
                    AssetDatabase.SaveAssets();
                }
                Add(create.parentscroll, EventPacketPanel.Mode.Create, create.links, icon);
                Add(update.parentscroll, EventPacketPanel.Mode.Edit, update.links, icon);
                Add(card.parentscroll, EventPacketPanel.Mode.Card, card.links, icon);
                EditorSceneManager.MarkSceneDirty(scene);
            }

            bool saved = !changed || (!wasDirty && EditorSceneManager.SaveScene(scene));
            if (select || !openedForInstall)
            {
                Selection.activeGameObject = EventPacketPanel.FindPanel(create.parentscroll).gameObject;
                EditorGUIUtility.PingObject(Selection.activeGameObject);
            }
            return saved ? "Packet state sprites are saved on all three screens in 4. MainScreens."
                : "Packet state sprites are assigned on all three screens. Save 4. MainScreens to keep them and your other unsaved edits.";
        }
        finally
        {
            if (openedForInstall && !select && !scene.isDirty) EditorSceneManager.CloseScene(scene, true);
        }
    }

    private static void Add(UnityEngine.UI.ScrollRect scroll, EventPacketPanel.Mode mode,
        UnityEngine.UI.Selectable[] sourceIcons, Sprite icon)
    {
        var existing = EventPacketPanel.FindPanel(scroll);
        if (existing != null)
        {
            if (!existing.HasLinksLayout)
            {
                Undo.RegisterFullObjectHierarchyUndo(existing.gameObject, "Match Packet to Links");
                if (!existing.MatchLinksLayout(scroll, sourceIcons, true))
                    throw new InvalidOperationException("Couldn't update " + existing.name + ".");
            }
        }
        else
        {
            existing = EventPacketPanel.CreateEditable(scroll, mode, sourceIcons, icon);
            Undo.RegisterCreatedObjectUndo(existing.gameObject, "Add editable Packet UI");
        }
        if (existing.HasStateSprites) return;

        var empty = LoadState(0, 40);
        var filled = LoadState(5, 20);
        var selected = LoadState(10, 0);
        Undo.RegisterFullObjectHierarchyUndo(existing.gameObject, "Assign Packet state sprites");
        existing.SetSlotSprites(empty, filled, selected);
    }

    private static Sprite[] LoadState(int packetOffset, int linksOffset)
    {
        int[] packet = { 0, 0, 0, 4, 5, 0, 6, 7, 8, 8, 8, 8 };
        int[] links = { 11, 12, 13, 0, 0, 15, 0, 0, 0, 0, 0, 0 };
        var sprites = new Sprite[packet.Length];
        for (int i = 0; i < sprites.Length; i++)
        {
            bool isPacket = packet[i] != 0;
            int asset = isPacket ? packet[i] + packetOffset : links[i] + linksOffset;
            string folder = isPacket ? "Packet" : "TonnyGiven/NewLinks";
            string path = $"Assets/UI/NewErny_UI/{folder}/Asset {asset}@4x.png";
            sprites[i] = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprites[i] == null) throw new InvalidOperationException("Packet sprite not imported: " + path);
        }
        return sprites;
    }
}
