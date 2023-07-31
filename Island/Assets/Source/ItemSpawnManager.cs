using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    public static ItemSpawnManager Instance;

    public bool useGlobalSettings = true;

    //------------------------LIGHTERS------------------------
    [SerializeField] Transform[] lighterSpawnPoints;
    [SerializeField] GameObject lighterPrefab;
    public int lighterAmount = 1;
    //--------------------------------------------------------

    //----------------------SIGNAL GUNS-----------------------
    [SerializeField] Transform[] signalGunSpawnPoints;
    [SerializeField] GameObject signalGunPrefab;
    public int signalGunAmount = 1;
    //--------------------------------------------------------

    //----------------------BONFIRES--------------------------
    [SerializeField] Transform[] bonfireSpawnPoints;
    [SerializeField] GameObject bonfirePrefab;
    public int bonfireAmount = 3;
    //--------------------------------------------------------

    public enum ItemType {
        lighter, signalGun, bonfire
    }
    public class ItemSpawner {
        Transform[] spawnPoints;
        GameObject itemPrefab;
        int amount;
        Dictionary<int, bool> spawnPointsDictionary;
        public ItemType ItemType { get; private set; }

        public ItemSpawner (ItemType itemType) {
            ItemType = itemType;

            switch (itemType) {
                case ItemType.bonfire: {
                        this.itemPrefab = Instance.bonfirePrefab;
                        this.spawnPoints = Instance.bonfireSpawnPoints;
                        this.amount = Instance.bonfireAmount;
                        break;
                    }
                case ItemType.signalGun: {
                        this.itemPrefab = Instance.signalGunPrefab;
                        this.spawnPoints = Instance.signalGunSpawnPoints;
                        this.amount = Instance.signalGunAmount;
                        break;
                    }
                case ItemType.lighter: {
                        this.itemPrefab = Instance.lighterPrefab;
                        this.spawnPoints = Instance.lighterSpawnPoints;
                        this.amount = Instance.lighterAmount;
                        break;
                    }
            }

            InitSpawnPointsDictionary();
        }

        void InitSpawnPointsDictionary() {
            spawnPointsDictionary = new Dictionary<int, bool>();

            for (int i = 0; i < spawnPoints.Length; i++) {
                spawnPointsDictionary.Add(i, false);
            }
        }

        public void SpawnItems() {
            if (itemPrefab == null) return;
            if (amount <= 0) return;

            int totalPoints = spawnPointsDictionary.Count;
            if (amount > totalPoints) amount = totalPoints;

            int[] points = new int[totalPoints];
            for (int k = 0; k < totalPoints; k++) { points[k] = k; }

            int spawnPoint;

            int i = 0;
            while (amount > 0) {
                int index = UnityEngine.Random.Range(i, totalPoints);
                spawnPoint = points[index];

                if (spawnPointsDictionary[spawnPoint]) {

                    int a = points[i];
                    points[i] = spawnPoint;
                    points[index] = a;

                    i++;

                    spawnPoint = points[UnityEngine.Random.Range(i, totalPoints)];
                }

                Instantiate(itemPrefab, spawnPoints[spawnPoint].position, itemPrefab.transform.rotation);
                spawnPointsDictionary[spawnPoint] = true;

                amount--;
            }
        }
    }

    ItemSpawner[] itemSpawners;
    void InitSpawners() {
        itemSpawners = new ItemSpawner[Enum.GetValues(typeof(ItemType)).Length];

        int i = 0;
        foreach (var val in Enum.GetValues(typeof(ItemType)).Cast<ItemType>()) {
            itemSpawners[i] = new ItemSpawner(val);
            i++;
        }
    }

    void SpawnItems() {
        foreach (var spawner in itemSpawners) {
            spawner.SpawnItems();
        }
    }

    void OnGameSettingsAwakeEnded() {
        if (useGlobalSettings) {
            signalGunAmount = GameSettingsManager.Instance.EvacSettings.SignalGunsAmount;
            bonfireAmount = GameSettingsManager.Instance.EvacSettings.BonfiresAmount;
        }

        InitSpawners();
        SpawnItems();
    }

    private void Awake() {
        GameSettingsManager.Instance.EvacSettings.Awaked += OnGameSettingsAwakeEnded;

        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    public static int GetFreeSpawnPoint(Dictionary<int, bool> spawnPointsDictionary) {
        var points = new List<int>();

        for (int i = 0; i < spawnPointsDictionary.Count; i++) {
            if (!spawnPointsDictionary[i]) {
                points.Add(i);
            }
        }

        if (points.Count == 0) {
            return -1;
        }

        return points[UnityEngine.Random.Range(0, points.Count)];
    }
}
