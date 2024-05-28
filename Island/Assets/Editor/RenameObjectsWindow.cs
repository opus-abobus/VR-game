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
        if (GUILayout.Button("Find all banana trees with ObjectIDMaker"))
        {
            FindAllIDMakers<BananaTreeIDMaker>();
        }
        else if (GUILayout.Button("Find all inventory slots with ObjectIDMaker"))
        {
            FindAllIDMakers<InventorySlotIDMaker>();
        }

        if (_idMakers != null && _idMakers.Length > 0)
        {
            EditorGUILayout.LabelField("Found Objects", EditorStyles.boldLabel);
            EditorGUILayout.TextArea(_idMakers[0].GetType().Name + " count: " + _idMakers.Length);

            if (GUILayout.Button("Apply New Names"))
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
            obj.gameObject.name = obj.GetHeadName() + " [" + GUID.Generate().ToString() + "]";

            EditorUtility.SetDirty(obj.gameObject);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(obj.gameObject.scene);
        }
    }
}
