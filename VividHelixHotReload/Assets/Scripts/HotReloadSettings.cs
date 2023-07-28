using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="HotReloadSettings", menuName = "VividHelix/HotReloadSettings")]
public class HotReloadSettings : ScriptableObject
{
    public string pathToGameCoreProjectFile;
    public string unityInstallPath;
    public string gameCoreOutputFolder;
}
