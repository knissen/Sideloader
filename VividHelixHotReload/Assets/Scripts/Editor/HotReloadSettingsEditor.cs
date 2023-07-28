using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HotReloadSettings))]
public class HotReloadSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Configure HotReload"))
        {
            MenuHotReload.ConfigureHotSwapProject();
        }
    }
}