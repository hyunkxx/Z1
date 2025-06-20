using UnityEngine;
using UnityEditor;

using System.Diagnostics;
using System.IO;


public class DatabaseTool : EditorWindow
{
    [MenuItem("Tools/Database/Open(Persistent)")]
    public static void OpenPersistent()
    {
        const string DatabasePath = "Database/";
        string path = Path.Combine(Application.persistentDataPath, DatabasePath);
        OpenDirectory(path);
    }

    [MenuItem("Tools/Database/Open(StreamingAssets)")]
    public static void OpenStreamingAssets()
    {
        const string DatabasePath = "Database/";
        string path = Path.Combine(Application.streamingAssetsPath, DatabasePath);
        OpenDirectory(path);
    }

    [MenuItem("Tools/Database/RemoveDB(Persistent)")]
    public static void RemoveDatabase()
    {
        const string DatabasePath = "Database/GameDatabase.db";
        string persistentDBPath = Path.Combine(Application.persistentDataPath, DatabasePath);

        if(File.Exists(persistentDBPath))
        {
            File.Delete(persistentDBPath);
            UnityEngine.Debug.Log($"database file has been successfully delete.");
        }
        else
        {
            UnityEngine.Debug.Log($"database file does not exist at the specified path.");
        }
    }

    private static void OpenDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            UnityEngine.Debug.Log("Folder does not exist: " + path);
            return;
        }

        Process.Start("explorer.exe", path.Replace("/", "\\"));
    }
}
