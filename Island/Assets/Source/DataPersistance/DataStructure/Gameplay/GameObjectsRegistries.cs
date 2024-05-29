using DataPersistence.Gameplay;
using System.Collections.Generic;
using UnityEngine;
using static DataPersistence.Gameplay.PlayerData.InventorySlotsData;

public class GameObjectsRegistries : MonoBehaviour
{
    private Dictionary<string, InventorySlotData> _inventorySlotsData;
    private Dictionary<string, BananaTreeManager> _bananaTrees;
    private Dictionary<string, BananaTreeData> _treeData;

    [SerializeField] private LevelDataManager _levelDataManager;

    [SerializeField] private InventoryPanelController _inventoryPanelController;

    [SerializeField] private Transform _restoredObjectsRoot;
    private Dictionary<GameObject, string> _dynamicObjects;

    private void OnGameSave(GameplayData gameplayData)
    {
        SaveBananaTreesData(gameplayData);

        SaveDynamicObjectsData(gameplayData);

        SaveInventoryData(gameplayData);
    }

    private void SaveBananaTreesData(GameplayData gameplayData)
    {
        BananaTreeData[] treeData = new BananaTreeData[_bananaTrees.Count];
        int i = 0;
        foreach (var tree in _bananaTrees.Values)
        {
            var data = tree.GetData();
            treeData[i++] = new BananaTreeData(data.objectName, data.ripeningData, data.growthData);
        }

        gameplayData.bananaTreesData = new BananaTreesData(treeData);
    }

    private void SaveDynamicObjectsData(GameplayData gameplayData, bool saveVelocity = true)
    {
        DynamicObjectsData.ObjectData[] data = new DynamicObjectsData.ObjectData[_dynamicObjects.Count];
        int i = 0;
        foreach (KeyValuePair<GameObject, string> objData in _dynamicObjects)
        {
            Vector3 velocity = Vector3.zero, angularVelocity = Vector3.zero;
            bool useGravityRB = false, isKinematicRB = false; 
            if (objData.Key.TryGetComponent<Rigidbody>(out var rB))
            {
                if (saveVelocity)
                {
                    velocity = rB.velocity;
                    angularVelocity = rB.angularVelocity;
                }

                useGravityRB = rB.useGravity;
                isKinematicRB = rB.isKinematic;
            }

            bool isTriggerCol = false, isEnabledCol = false;
            if (objData.Key.TryGetComponent<Collider>(out var rC))
            {
                isTriggerCol = rC.isTrigger;
                isEnabledCol = rC.enabled;
            }

            data[i++] = new DynamicObjectsData.ObjectData(objData.Key.transform.position,
                objData.Key.transform.lossyScale, objData.Key.transform.rotation, 
                velocity, angularVelocity, useGravityRB, isKinematicRB, isTriggerCol,
                isEnabledCol, objData.Value);
        }
        DynamicObjectsData dynamicObjectsData = new DynamicObjectsData(data);

        gameplayData.dynamicObjectsData = dynamicObjectsData;
    }

    private void SaveInventoryData(GameplayData data)
    {
        data.playerData.inventorySlotsData = _inventoryPanelController.GetData();
    }

    public string GetDynamicObjectAssetGUID(GameObject gameObject)
    {
        if (_dynamicObjects.ContainsKey(gameObject))
        {
            return _dynamicObjects[gameObject];
        }

        return null;
    }

    public void Init(GameplayData data)
    {
        _levelDataManager.OnGameSave += OnGameSave;

        _dynamicObjects = new Dictionary<GameObject, string>();
        _bananaTrees = new Dictionary<string, BananaTreeManager>();
        _treeData = new Dictionary<string, BananaTreeData>();
        _inventorySlotsData = new Dictionary<string, InventorySlotData>();

        if (data != null)
        {
            RestoreBananaTreesData(data.bananaTreesData);

            RestoreDynamicObjects(data.dynamicObjectsData);

            RestoreInventoryData(data.playerData.inventorySlotsData);
        }
    }

    private void RestoreBananaTreesData(BananaTreesData treesData)
    {
        var arrayData = treesData.data;
        foreach (var data in arrayData)
        {
            _treeData.Add(data.objectName, data);
        }
    }

    private void RestoreDynamicObjects(DynamicObjectsData dynamicObjectsData, bool restoreVelocity = true)
    {
        foreach (var data in dynamicObjectsData.objectDatas)
        {
            var prefab = AddressableItems.Instance.GetPrefabByGUID(data.prefabAssetGUID);
            if (prefab != null)
            {
                GameObject @object = Instantiate(prefab, data.postiton, data.rotation);
                @object.transform.localScale = data.scale;
                @object.transform.parent = _restoredObjectsRoot;

                if (@object.TryGetComponent<Rigidbody>(out var rB))
                {
                    rB.useGravity = data.useGravityRB;
                    rB.isKinematic = data.isKinematicRB;

                    if (restoreVelocity)
                    {
                        rB.velocity = data.velocity;
                        rB.angularVelocity = data.angularVelocity;
                    }
                }

                if (@object.TryGetComponent<Collider>(out var rC))
                {
                    rC.isTrigger = data.isTriggerCol;
                    rC.enabled = data.isEnabledCol;
                }

                if (@object.TryGetComponent<BananaDrop>(out var fallingBananaPlot))
                {
                    fallingBananaPlot.Init();
                    fallingBananaPlot.SetRegistries(this);
                }

                Register(@object, data.prefabAssetGUID);
            }
        }
    }

    private void RestoreInventoryData(PlayerData.InventorySlotsData data)
    {
        foreach (var slotData in data.slotsData)
        {
            if (slotData != null && !_inventorySlotsData.ContainsKey(slotData.slotObjectName))
            {
                _inventorySlotsData.Add(slotData.slotObjectName, slotData);
            }
        }

        _inventoryPanelController.SetData();
    }

    public void Register(GameObject gameObject, string prefabPath)
    {
        if (!_dynamicObjects.ContainsKey(gameObject))
        {
            _dynamicObjects.Add(gameObject, prefabPath);
        }
    }

    public void Register<T>(GameObject gameObject, T component) where T : UnityEngine.Object
    {
        if (typeof(T) == typeof(InventorySlotController))
        {
            
        }
        else if (typeof(T) == typeof(BananaTreeManager))
        {
            _bananaTrees.Add(gameObject.name, component as BananaTreeManager);
        }
    }

    public void Unregister(GameObject gameObject)
    {
        if (_dynamicObjects.ContainsKey(gameObject))
            _dynamicObjects.Remove(gameObject);
    }

    public void Unregister<T>(GameObject gameObject) where T : UnityEngine.Object
    {
        if (typeof(T) == typeof(BananaTreeManager))
        {
            if (_bananaTrees.ContainsKey(gameObject.name))
                _bananaTrees.Remove(gameObject.name);
        }
    }

    public T GetData<T>(string key) where T : class
    {
        if (typeof(T) == typeof(BananaTreeData))
        {
            if (_treeData.Count == 0) return default;
            if (_treeData.TryGetValue(key, out var treeData))
            {
                return treeData as T;
            }
        }
        else if (typeof(T) == typeof(InventorySlotData))
        {
            if (_inventorySlotsData.Count == 0) return default;
            if (_inventorySlotsData.TryGetValue(key, out var slotData))
            {
                return slotData as T;
            }
        }

        return default;
    }

    private void OnDestroy()
    {
        _levelDataManager.OnGameSave -= OnGameSave;
    }
}
