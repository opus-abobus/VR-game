using DataPersistence.Gameplay;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectsRegistries : MonoBehaviour
{
    private Dictionary<string, InventorySlotController> _inventoryRegistry;
    private Dictionary<string, BananaTreeManager> _bananaTrees;
    private Dictionary<string, BananaTreeData> _treeData;

    [SerializeField] private SpawnManager _spawnManager;

    [SerializeField] private LevelDataManager _levelDataManager;

    [SerializeField] private Transform _restoredObjectsRoot;
    private Dictionary<GameObject, string> _dynamicObjects;

    private void OnGameSave(GameplayData gameplayData)
    {
        SaveBananaTreesData(gameplayData);

        SaveDynamicObjectsData(gameplayData);
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

    public void Init(GameplayData data)
    {
        _levelDataManager.OnGameSave += OnGameSave;

        _spawnManager.OnInitialized += OnSpawnManagerInitialized;

        _inventoryRegistry = new Dictionary<string, InventorySlotController>();
        _dynamicObjects = new Dictionary<GameObject, string>();
        _bananaTrees = new Dictionary<string, BananaTreeManager>();
        _treeData = new Dictionary<string, BananaTreeData>();

        if (data != null)
        {
            RestoreBananaTreesData(data.bananaTreesData);

            RestoreDynamicObjects(data.dynamicObjectsData);
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

    private void OnSpawnManagerInitialized()
    {

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
        if (typeof(T) == typeof(InventorySlotController))
        {

        }
        else if (typeof(T) == typeof(BananaTreeManager))
        {
            if (_bananaTrees.ContainsKey(gameObject.name))
                _bananaTrees.Remove(gameObject.name);
        }
    }

    // not keys but components data
    public void SetData<T>(T data)
    {
        if (typeof(T) == typeof(BananaTreeManager))
        {
            foreach (var tree in _bananaTrees)
            {
                //_bananaTrees.Add(key, );
            }
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

        return default;
    }

    private void InitInventoryRegistry(PlayerData data)
    {
        
    }

    private void OnDestroy()
    {
        _spawnManager.OnInitialized -= OnSpawnManagerInitialized;
        _levelDataManager.OnGameSave -= OnGameSave;
    }
}
