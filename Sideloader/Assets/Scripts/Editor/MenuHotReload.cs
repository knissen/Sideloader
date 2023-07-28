using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuHotReload : EditorWindow
{
    [MenuItem("VividHelix/Config HotSwap")]
    public static void ConfigureHotSwapProject()
    {
        Debug.Log("Config!");

        HotReloadSettings settings = GetOrCreateSettings();

        if(settings != null)
        {
            Debug.Log("Settings found");
        }

        // Find Visual Studio project
        if(TryGetPathForProjectFile(out string projPath))
        {
            Debug.Log("Path to proj: " + projPath);


        }
        else
        {
            Debug.Log("Config Cancelled");
            return;
        }

        // Prompt for path to Unity install

        // 
    }

    private static HotReloadSettings GetOrCreateSettings()
    {
        string[] path = AssetDatabase.FindAssets("t:HotReloadSettigns");

        if(path.Length > 1)
        {
            Debug.LogError("Too many HotReloadSettings found. There shoudl be only one. Using first found by default");
        }

        if (path.Length == 0)
        {
            HotReloadSettings settings = ScriptableObject.CreateInstance(typeof(HotReloadSettings)) as HotReloadSettings;

            AssetDatabase.CreateAsset(settings, "Assets/HotReloadSettings.asset");

            return settings;
        }

        return AssetDatabase.LoadAssetAtPath(path[0], typeof(HotReloadSettings)) as HotReloadSettings;

    }

    private static bool TryGetPathForProjectFile(out string projPath)
    {
        projPath = EditorUtility.OpenFilePanel("Find GameCore Project File", "Assets", "csproj");

        if (string.IsNullOrEmpty(projPath))
            return false;

        return true;
    }
}
