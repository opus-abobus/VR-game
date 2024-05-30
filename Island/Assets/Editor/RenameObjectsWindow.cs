using UnityEditor;
using UnityEngine;

public class RenameObjectsWindow : EditorWindow
{
    [MenuItem("Tools/Rename objects to ID")]
    public static void ShowWindow()
    {
        GetWindow<RenameObjectsWindow>("Rename objects to ID");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Find all objects with ObjectIDMaker"))
        {
            FindAllIDMakers<ObjectIDMaker>();
        }

        if (_idMakers != null && _idMakers.Length > 0)
        {
            EditorGUILayout.LabelField("Found Objects", EditorStyles.boldLabel);
            EditorGUILayout.TextArea(_idMakers[0].GetType().Name + " count: " + _idMakers.Length);

            if (GUILayout.Button("Apply ID to names"))
            {
                ApplyIDToNames();
            }
        }
    }

    private ObjectIDMaker[] _idMakers;
    private void FindAllIDMakers<T>(bool includeInactive = true) where T : ObjectIDMaker
    {
        _idMakers = FindObjectsOfType<T>(includeInactive);
    }

    private void ApplyIDToNames()
    {
        foreach (var obj in _idMakers)
        {
            obj.gameObject.name = obj.HeadName + " [" + GUID.Generate().ToString() + "]";

            EditorUtility.SetDirty(obj.gameObject);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(obj.gameObject.scene);
        }
    }
}
