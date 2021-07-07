using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AddScenesToBuild
{
    // Hotkey is CTRL+ALT+R
    [MenuItem("Tools/Add microgames to build %&r")]
    private static void AddMicrogamesToBuild() {
        List<EditorBuildSettingsScene> newSceneList = new List<EditorBuildSettingsScene>();

        // Just used to make sure we're not adding scenes we already have.
        List<string> sceneComparison = new List<string>();

        // And this is all the scenes currently in the build order.
        var buildScenes = EditorBuildSettings.scenes;

        foreach (var s in buildScenes) {
            newSceneList.Add(new EditorBuildSettingsScene(s.path, true));
            sceneComparison.Add(s.path);
            Debug.Log("Found " + s.path);
        }

        // Get all assets with type of scene.
        var allScenes = AssetDatabase.FindAssets("t:Scene");
        foreach (var s in allScenes)
        {
            var scenePath = AssetDatabase.GUIDToAssetPath(s);
            if (!sceneComparison.Contains(scenePath))
            {
                newSceneList.Add(new EditorBuildSettingsScene(scenePath, true));
                sceneComparison.Add(scenePath);
                Debug.Log(scenePath + " added to build order.");
            }
        }

        EditorBuildSettings.scenes = newSceneList.ToArray();
        Debug.Log("Scenes added.");
    }
}
