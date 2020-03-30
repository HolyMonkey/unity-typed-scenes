using IJunior.TypedScenes;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Test))]
public class TestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Анализировать"))
        {
            SceneAnalyzer.GetLoadingParameters("5b8faaa294206a3449e17b31fc2cfc4c");
        }
    }
}
