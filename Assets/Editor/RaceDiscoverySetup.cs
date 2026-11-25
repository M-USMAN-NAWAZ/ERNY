using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[InitializeOnLoad]
public static class RaceDiscoverySetup
{
    private const string Request = "Library/RaceDiscoverySetup.request";
    private const string RacePageRequest = "Library/RaceWebPageRefresh.request";
    private const string MainScene = "Assets/Scene/4. MainScreens.unity";
    static RaceDiscoverySetup() { EditorApplication.update += CheckRequest; }

    private static void CheckRequest()
    {
        if (EditorApplication.isCompiling || EditorApplication.isUpdating || EditorApplication.isPlayingOrWillChangePlaymode)
            return;
        if (File.Exists(Request))
        {
            File.Delete(Request);
            try { Install(); }
            catch (Exception e) { File.WriteAllText("Library/RaceDiscoverySetup.result", e.ToString()); Debug.LogException(e); }
        }

        if (!File.Exists(RacePageRequest)) return;
        var scene = SceneManager.GetSceneByPath(MainScene);
        if (!scene.isLoaded) return;
        File.Delete(RacePageRequest);
        try { File.WriteAllText("Library/RaceWebPageSetup.result", InstallRacePage(scene)); }
        catch (Exception e) { File.WriteAllText("Library/RaceWebPageSetup.result", e.ToString()); Debug.LogException(e); }
    }

    [MenuItem("Tools/Erny/Add Race Discovery UI")]
    public static void Install()
    {
        var scene = EditorSceneManager.GetActiveScene();
        if (scene.path != MainScene)
            throw new InvalidOperationException("Open 4. MainScreens before installing discovery UI.");
        var owner = scene.GetRootGameObjects().SelectMany(go => go.GetComponentsInChildren<eventgetter>(true)).First();
        if (owner.panels[0].GetComponentInChildren<RaceDiscoveryController>(true) != null)
        { File.WriteAllText("Library/RaceDiscoverySetup.result", "Already installed"); return; }
        bool wasDirty = scene.isDirty;
        var body = AssetDatabase.LoadAssetAtPath<Font>("Assets/Roboto_Condensed/static/RobotoCondensed-Regular.ttf") ?? owner.name.font;
        var title = owner.panels[0].GetComponentsInChildren<Text>(true).First(t => t.text.Trim() == "Add a race");
        var border = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/NewErny_UI/EventCreate/Asset 468@4x.png");
        var miles = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/NewErny_UI/EventCreate/Asset 501@4x.png");
        var kilometers = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/NewErny_UI/EventCreate/Asset 502@4x.png");
        var controller = RaceDiscoveryController.CreateInterface(owner, body, title.font, border, miles, kilometers);
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs/RaceDiscovery"))
            AssetDatabase.CreateFolder("Assets/Prefabs", "RaceDiscovery");
        var serialized = new SerializedObject(controller);
        var row = (GameObject)serialized.FindProperty("rowTemplate").objectReferenceValue;
        PrefabUtility.SaveAsPrefabAssetAndConnect(row, "Assets/Prefabs/RaceDiscovery/Race Result.prefab", InteractionMode.AutomatedAction);
        EditorSceneManager.MarkSceneDirty(scene);
        if (!wasDirty) EditorSceneManager.SaveScene(scene);
        File.WriteAllText("Library/RaceDiscoverySetup.result", "Installed. Scene saved: " + !wasDirty);
        Selection.activeGameObject = controller.gameObject;
        Debug.Log("Race discovery hierarchy installed. Existing race buttons and their click listeners retained.");
    }

    [MenuItem("Tools/Erny/Add or Select Race Web View UI")]
    public static void InstallRacePage()
    {
        var scene = SceneManager.GetSceneByPath(MainScene);
        if (EditorApplication.isPlayingOrWillChangePlaymode || !scene.isLoaded)
        {
            Debug.LogWarning("Open 4. MainScreens and exit Play Mode first.");
            return;
        }
        Debug.Log(InstallRacePage(scene));
    }

    private static string InstallRacePage(Scene scene)
    {
        var controller = scene.GetRootGameObjects()
            .SelectMany(go => go.GetComponentsInChildren<RaceDiscoveryController>(true)).FirstOrDefault();
        if (controller == null)
            throw new InvalidOperationException("Install Race Discovery UI first.");

        bool wasDirty = scene.isDirty;
        bool installed = new SerializedObject(controller).FindProperty("racePanel").objectReferenceValue != null;
        if (!installed) Undo.RecordObject(controller, "Add race web view UI");
        var panel = controller.CreateEditableRacePage();
        var serialized = new SerializedObject(controller);
        var addedIcon = serialized.FindProperty("addedRaceIcon");
        bool assignedIcon = false;
        if (addedIcon.objectReferenceValue == null)
        {
            addedIcon.objectReferenceValue = AssetDatabase.LoadAssetAtPath<Sprite>(
                "Assets/UI/NewErny_UI/Discover RunSignup Ui/Asset 8@4x.png");
            assignedIcon = serialized.ApplyModifiedProperties();
        }
        string result = "Race web view UI already exists in the scene.";
        if (!installed) Undo.RegisterCreatedObjectUndo(panel, "Add race web view UI");
        if (!installed || assignedIcon)
        {
            EditorSceneManager.MarkSceneDirty(scene);
            result = !wasDirty && EditorSceneManager.SaveScene(scene)
                ? "Race web view UI saved."
                : "Race web view UI updated. Save the scene to keep it and any other unsaved edits.";
        }
        Selection.activeGameObject = panel;
        EditorGUIUtility.PingObject(panel);
        return result;
    }
}
