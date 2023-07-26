using DefaultNamespace;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class HotReload : MonoBehaviour
{
    public bool AutoReload;
    public bool LogToConsole;
    public KeyCode ForceReloadKey = KeyCode.Alpha0;

    private static string Namespace = "GameCore";
    private static string ClassName = "GameMain";
    private static string DllName = "GameCore.dll";

    private IGameCore gameCore;

#if UNITY_EDITOR
    private static string PdbName = "GameCore.pdb";
    private static string DllDirectory = "Assets//GameCore//Resources//";
#else
    private static string DllDirectory = "Assets\\GameCore\\Resources\\";
#endif    
    private static string DllPath = DllDirectory + DllName + ".bytes";
    private static string MdbPath = DllDirectory + PdbName + ".bytes";
    private DateTime lastModifiedTime;

    #region UnityLifecycle Events

    public void Start()
    {
        if (!AutoReload)
            ReloadDLLFile(new Stopwatch(), null, false);
    }

    public void Update()
    {
        try
        {
#if (UNITY_STANDALONE_WIN||UNITY_STANDALONE_OSX) && UNITY_EDITOR
            ReloadDllIfNeeded();
#endif
            gameCore?.Update();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    #endregion

    #region Reload Methods

    private void ReloadDllIfNeeded()
    {
        if ((AutoReload && (File.GetLastWriteTime(DllPath) != lastModifiedTime) || Input.GetKeyDown(ForceReloadKey)))
        {
            if (LogToConsole)
                Debug.LogError($"Reloading: {lastModifiedTime} // {File.GetLastWriteTime(DllPath)}");

            var stopwatch = new Stopwatch();
            var saved = gameCore?.Save(true);

            stopwatch.Start();
            var shouldLogReload = lastModifiedTime.Ticks != 0;

            ReloadDLLFile(stopwatch, saved, shouldLogReload);
        }
    }

    private void ReloadDLLFile(Stopwatch stopwatch, object saved, bool shouldLogReload)
    {
        lastModifiedTime = File.GetLastWriteTime(DllPath);
        var dllBytes = File.ReadAllBytes(DllPath);
        var mdbBytes = File.ReadAllBytes(MdbPath);

        if (shouldLogReload)
            Debug.LogError($"Reloaded dll took {stopwatch.ElapsedMilliseconds / 1000f:0.000}");

        LoadDll(dllBytes, mdbBytes, saved);
    }

    private void LoadDll(byte[] dllBytes, byte[] mdbBytes = null, object saved = null)
    {
        var assembly = (mdbBytes != null) ? Assembly.Load(dllBytes, mdbBytes) : Assembly.Load(dllBytes);
        var gameCoreType = assembly.ExportedTypes.FirstOrDefault(x => x.Name.Equals(ClassName) && x.Namespace.Equals(Namespace));
        gameCore = Activator.CreateInstance(gameCoreType) as IGameCore;
        gameCore.Load(saved);
    }

    #endregion
}
