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

    private void OnGameSave(GameplayData gameplayData)
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

    public void Init(GameplayData data)
    {
        _levelDataManager.OnGameSave += OnGameSave;

        _spawnManager.OnInitialized += OnSpawnManagerInitialized;

        //_isNewGame = CurrentSessionDataManager.Instance.IsNewGame;

        _inventoryRegistry = new Dictionary<string, InventorySlotController>();

        _bananaTrees = new Dictionary<string, BananaTreeManager>();
        _treeData = new Dictionary<string, BananaTreeData>();

        if (data != null)
        {
            var treeData = data.bananaTreesData.data;
            foreach (var d in treeData)
            {
                _treeData.Add(d.objectName, d);
            }
        }
    }

    private void OnSpawnManagerInitialized()
    {

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
