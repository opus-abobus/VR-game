using DataPersistence.Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static SpawnManager;

public class ItemSpawnManager : MonoBehaviour, ISpawner
{
    //------------------------LIGHTERS------------------------
    [SerializeField] private Transform lighterSpawnPointsRoot;

    [SerializeField] private AssetReferenceGameObject _lighterPrefabRef;
    private GameObject lighterPrefab;

    private int lighterAmount = 1;
    //--------------------------------------------------------

    //----------------------SIGNAL GUNS-----------------------
    [SerializeField] private Transform signalGunSpawnPointsRoot;

    [SerializeField] private AssetReferenceGameObject _signalGunPrefabRef;
    private GameObject signalGunPrefab;

    private int signalGunAmount = 1;
    //--------------------------------------------------------

    //----------------------BONFIRES--------------------------
    [SerializeField] private Transform bonfireSpawnPointsRoot;

    [SerializeField] private AssetReferenceGameObject _bonfirePrefabRef;
    private GameObject bonfirePrefab;

    private int bonfireAmount = 3;
    //--------------------------------------------------------

    [SerializeField] private Transform _spawnedOrigin;

    public enum ItemType {
        lighter, signalGun, bonfire
    }

    private bool _wasSpawn = false;

    public SpawnerData GetData()
    {
        return new ItemSpawnerManagerData(gameObject.name, _wasSpawn);
    }

    public void SetData<TSpawnerData>(TSpawnerData tSpawnerData) where TSpawnerData : SpawnerData
    {
        var data = tSpawnerData as ItemSpawnerManagerData;

        if (data != null)
        {
            _wasSpawn = data.wasSpawn;
        }
    }

    public void Init()
    {
        var settings = GameSettingsManager.Instance.ActiveWorldSettings as WorldSettings.IEvacSettings;
        signalGunAmount = settings.SignalGunsAmount;
        bonfireAmount = settings.BonfiresAmount;
        lighterAmount = settings.LightersAmount;

        lighterPrefab = AddressableItems.Instance.GetPrefabByGUID(_lighterPrefabRef.AssetGUID);
        bonfirePrefab = AddressableItems.Instance.GetPrefabByGUID(_bonfirePrefabRef.AssetGUID);
        signalGunPrefab = AddressableItems.Instance.GetPrefabByGUID(_signalGunPrefabRef.AssetGUID);

        InitSpawners(this);
    }

    void ISpawner.BeginSpawn()
    {
        if (_wasSpawn)
            return;

        SpawnItems(_spawnedOrigin);
        _wasSpawn = true;
    }

    public class ItemSpawner {
        Transform[] spawnPoints;
        GameObject itemPrefab;
        int amount;
        string prefabGuid;

        Dictionary<int, bool> freeSpawnPoints;
        public ItemType ItemType { get; private set; }

        public ItemSpawner (ItemSpawnManager spawnManager, ItemType itemType) {
            ItemType = itemType;

            switch (itemType) {
                case ItemType.bonfire: {
                        this.itemPrefab = spawnManager.bonfirePrefab;
                        this.spawnPoints = spawnManager.bonfireSpawnPointsRoot.GetComponentsInChildren<Transform>();
                        this.amount = spawnManager.bonfireAmount;
                        this.prefabGuid = spawnManager._bonfirePrefabRef.AssetGUID;
                        break;
                    }
                case ItemType.signalGun: {
                        this.itemPrefab = spawnManager.signalGunPrefab;
                        this.spawnPoints = spawnManager.signalGunSpawnPointsRoot.GetComponentsInChildren<Transform>();
                        this.amount = spawnManager.signalGunAmount;
                        this.prefabGuid = spawnManager._signalGunPrefabRef.AssetGUID;
                        break;
                    }
                case ItemType.lighter: {
                        this.itemPrefab = spawnManager.lighterPrefab;
                        this.spawnPoints = spawnManager.lighterSpawnPointsRoot.GetComponentsInChildren<Transform>();
                        this.amount = spawnManager.lighterAmount;
                        this.prefabGuid = spawnManager._lighterPrefabRef.AssetGUID;
                        break;
                    }
            }

            InitSpawnPointsDictionary();
        }

        void InitSpawnPointsDictionary() {
            freeSpawnPoints = new Dictionary<int, bool>();

            for (int i = 0; i < spawnPoints.Length; i++) {
                freeSpawnPoints.Add(i, true);
            }
        }

        public void SpawnItems(Transform parent) {
            if (itemPrefab == null) return;
            if (amount <= 0) return;

            if (amount > freeSpawnPoints.Count) amount = freeSpawnPoints.Count;

            while (amount > 0) {

                int freeIndexPoint = GetFreeSpawnPoint(freeSpawnPoints);
                if (freeIndexPoint != -1)
                {
                    var spawnedItem = Instantiate(itemPrefab, spawnPoints[freeIndexPoint].position, itemPrefab.transform.rotation, parent);
                    freeSpawnPoints[freeIndexPoint] = false;

                    GameObjectsRegistries.Instance.RegisterObject(spawnedItem, prefabGuid, spawnedItem.GetComponents<Component>());
                }
                else
                {
                    amount = 0;
                    break;
                }

                amount--;
            }
        }
    }

    ItemSpawner[] itemSpawners;
    void InitSpawners(ItemSpawnManager spawnManager) {
        itemSpawners = new ItemSpawner[Enum.GetValues(typeof(ItemType)).Length];

        int i = 0;
        foreach (var val in Enum.GetValues(typeof(ItemType)).Cast<ItemType>()) {
            itemSpawners[i] = new ItemSpawner(spawnManager, val);
            i++;
        }
    }

    void SpawnItems(Transform parent) {
        foreach (var spawner in itemSpawners) {
            spawner.SpawnItems(parent);
        }
    }

    public static int GetFreeSpawnPoint(Dictionary<int, bool> spawnPoints) {
        var freePoints = new List<int>();

        for (int i = 0; i < spawnPoints.Count; i++) {
            if (spawnPoints[i]) {
                freePoints.Add(i);
            }
        }

        if (freePoints.Count == 0) {
            return -1;
        }

        return freePoints[UnityEngine.Random.Range(0, freePoints.Count)];
    }
}
