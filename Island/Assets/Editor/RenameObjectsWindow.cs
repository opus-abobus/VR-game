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
        if (GUILayout.Button("Find All Objects with BananaTreeIDMaker"))
        {
            FindAllBananaTreeIDMakers();
        }

        if (bananaTreeIDMakers != null && bananaTreeIDMakers.Length > 0)
        {
            EditorGUILayout.LabelField("Found Objects", EditorStyles.boldLabel);
            EditorGUILayout.TextArea("Banana tree count: " + bananaTreeIDMakers.Length);

            if (GUILayout.Button("Apply New Names"))
            {
                ApplyNewNames();
            }
        }
    }

    private BananaTreeIDMaker[] bananaTreeIDMakers;

    private void FindAllBananaTreeIDMakers()
    {
        bananaTreeIDMakers = FindObjectsOfType<BananaTreeIDMaker>();
    }

    private void ApplyNewNames()
    {
        foreach (var obj in bananaTreeIDMakers)
        {
            obj.gameObject.name = obj.GetHeadName() + " [" + GUID.Generate().ToString() + "]";

            EditorUtility.SetDirty(obj.gameObject);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(obj.gameObject.scene);
        }
    }
}
