using UnityEngine;
using UnityEditor;


public class CSVConverter : EditorWindow
{
    [SerializeField] private string path;

    [MenuItem("Tools/������")]
    public static void ShowWindow()
    {
        GetWindow(typeof(CSVConverter));
    }

    void OnGUI()
    {
        GUILayout.Label("CSV to SQLite", EditorStyles.boldLabel);
        path = EditorGUILayout.TextField("CSV File Path", path);

        GUILayout.Button("Generate Database");
    }
}
