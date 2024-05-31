using DataPersistence.Gameplay;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager _instance;
    public static SpawnManager Instance { get { return _instance; } }

    private BananaTreeManager[] _bananaSpawners;
    private BerrySpawnManager[] _berrySpawners;
    private CocountSpawner[] _coconutSpawners;

    public event Action OnInitialized;

    private GameObjectsRegistries _registry;

    private SpawnerData[] _spawnerData;

    public interface ISpawner
    {
        void BeginSpawn();
        void SetData<TSpawnerData>(TSpawnerData data) where TSpawnerData : SpawnerData;
        void Init();
        SpawnerData GetData();
    }

    public void Initialize(SpawnerData[] spawnersData)
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _registry = GameObjectsRegistries.Instance;

        _spawnerData = spawnersData;

        _coconutSpawners = FindObjectsOfType<CocountSpawner>();
        _berrySpawners = FindObjectsOfType<BerrySpawnManager>();
        _bananaSpawners = FindObjectsOfType<BananaTreeManager>();

        StartCoroutine(InitProcess());
    }

    private IEnumerator InitProcess()
    {
        Dictionary<string, ISpawner> spawners = new();

        foreach (var cocSpawner in _coconutSpawners)
            spawners.Add(cocSpawner.PalmRootObject.gameObject.name, cocSpawner);
        foreach (var berrySpawner in _berrySpawners)
            spawners.Add(berrySpawner.gameObject.name, berrySpawner);
        foreach (var ban in _bananaSpawners)
            spawners.Add(ban.gameObject.name, ban);

        if (_spawnerData != null && _spawnerData.Length > 0)
        {
            foreach (var data in _spawnerData)
            {
                if (spawners.ContainsKey(data.key))
                {
                    _registry.RegisterSpawner(data.key, spawners[data.key]);
                    spawners[data.key].Init();
                    spawners[data.key].SetData(data);
                }
            }
        }
        else
        {
            foreach (var spawner in spawners)
            {
                _registry.RegisterSpawner(spawner.Key, spawner.Value);
                spawner.Value.Init();
            }
        }
        
        OnInitialized?.Invoke();

        foreach (var s in spawners.Values)
        {
            s.BeginSpawn();
        }

        yield return null;
    }
}
