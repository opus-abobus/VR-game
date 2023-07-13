using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemSpawnManager))]
public class MyCustomEditor : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        //DrawAddItemSpawnerButton();
    }

    void DrawAddItemSpawnerButton() {
        ItemSpawnManager spawnManager = (ItemSpawnManager)target;

        if (GUILayout.Button("Add item")) {
            
        }

        if (GUILayout.Button("Delete item")) {

        }
    }
}
