using UnityEngine;
using UnityEditor;

public class BuildLevel : EditorWindow
{
    string LevelName = "WRITE LEVEL NAME HERE";

    [MenuItem("Window/Build Level")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<BuildLevel>("Build Level");
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Data", EditorStyles.boldLabel);

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        LevelName = EditorGUILayout.TextField("Level Name", LevelName);

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        if (GUILayout.Button("Build Level"))
        {
            Debug.Log("Pressed");
        }

    }
}
