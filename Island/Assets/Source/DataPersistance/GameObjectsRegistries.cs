using DataPersistence.Gameplay;
using System;
using System.Collections.Generic;
using UnityEngine;
using static SpawnManager;

public class GameObjectsRegistries : MonoBehaviour
{
    private Dictionary<string, ISpawner> _spawners = new();

    [SerializeField] private LevelDataManager _levelDataManager;

    [SerializeField] private Transform _restoredObjectsRoot;
    private Dictionary<GameObject, Tuple<string, Component[]>> _dynamicObjects = new();

    public static GameObjectsRegistries Instance { get; private set; }

    private void OnGameSave(GameplayData gameplayData)
    {
        gameplayData.spawnersData = new SpawnerData[_spawners.Count];
        int i = 0;
        foreach (var spawner in _spawners.Values)
        {
            gameplayData.spawnersData[i++] = spawner.GetData();
        }

        SaveDynamicObjectsData(gameplayData);
    }

    private void SaveDynamicObjectsData(GameplayData gameplayData)
    {
        ObjectData[] data = new ObjectData[_dynamicObjects.Count];
        int i = 0;
        foreach (var objData in _dynamicObjects)
        {
            if (objData.Key == null) continue;

            var components = objData.Value.Item2;

            List<ComponentData> componentsData = new List<ComponentData>();

            if (components != null && components.Length > 0)
            {
                foreach (var c in components)
                {
                    if (ComponentData.GetDataFromComponent(c) != null)
                    {
                        componentsData.Add(ComponentData.GetDataFromComponent(c));
                    }
                }
            }

            data[i++] = new ObjectData(objData.Value.Item1, componentsData.ToArray());
        }

        gameplayData.gameObjectsData = data;
    }

    public string GetDynamicObjectAssetGUID(GameObject gameObject)
    {
        if (_dynamicObjects.ContainsKey(gameObject))
        {
            return _dynamicObjects[gameObject].Item1;
        }

        return null;
    }

    public void Init(GameplayData data)
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            return;
        }

        _levelDataManager.OnGameSave += OnGameSave;

        if (data != null)
        {
            RestoreDynamicObjects(data.gameObjectsData);
        }
    }

    private void RestoreDynamicObjects(ObjectData[] gameObjectsData)
    {
        foreach (var data in gameObjectsData)
        {
            var prefab = AddressableItems.Instance.GetPrefabByGUID(data.prefabAssetGUID);
            if (prefab != null)
            {
                GameObject @object = Instantiate(prefab, _restoredObjectsRoot);

                var componentsData = data.componentsData;
                foreach (var component in componentsData)
                {
                    component.SetDataToGameObject(@object);
                }

                RegisterObject(@object, data.prefabAssetGUID, @object.GetComponents<Component>());
            }
        }
    }

    public void RegisterObject(GameObject gameObject, string prefabPath, Component[] components = null)
    {
        if (!_dynamicObjects.ContainsKey(gameObject))
        {
            if (components == null)
            {
                components = new Component[] { gameObject.transform };
            }

            _dynamicObjects.Add(gameObject, new Tuple<string, Component[]>(prefabPath, components));
        }
    }

    public void RegisterSpawner<T>(string key, T spawner) where T : ISpawner
    {
        _spawners.Add(key, spawner);
    }

    public void UnregisterObject(GameObject gameObject)
    {
        if (_dynamicObjects.ContainsKey(gameObject))
            _dynamicObjects.Remove(gameObject);
    }

    private void OnDestroy()
    {
        _levelDataManager.OnGameSave -= OnGameSave;
    }
}
