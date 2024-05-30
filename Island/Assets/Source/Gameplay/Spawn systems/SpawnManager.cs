using DataPersistence.Gameplay;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour, GameplayBootstrap.IBootstrap {
    private static SpawnManager _instance;
    public static SpawnManager Instance { get { return _instance; } }

    private List<BananaTreeManager> _bananaSpawners;
    public List<BananaTreeManager> BananaSpawners { get { return _bananaSpawners; } set { _bananaSpawners = value; } }

    private List<BerrySpawnManager> _berrySpawners;
    public List<BerrySpawnManager> BerrySpawners { get { return _berrySpawners; } set { _berrySpawners = value; } }

    private List<CocountSpawner> _coconutSpawners;
    public List<CocountSpawner> CocountSpawners { get { return _coconutSpawners; } set { _coconutSpawners = value; } }

    public event Action OnInitialized;

    [SerializeField] private GameObjectsRegistries _registry;

    public interface ISpawner {
        void BeginSpawn();
    }

    void GameplayBootstrap.IBootstrap.Initialize() {
        if (_instance == null) {
            _instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }

        _bananaSpawners = new List<BananaTreeManager>();
        _berrySpawners = new List<BerrySpawnManager>();
        _coconutSpawners = new List<CocountSpawner>();

        StartCoroutine(InitProcess());
    }

    IEnumerator InitProcess() {
        while (!CocountSpawner.HasStarted) {
            yield return null;
        }
        _coconutSpawners = FindObjectsOfType<CocountSpawner>().ToList();
        foreach (var cocSpawner in _coconutSpawners)
        {
            _registry.Register(cocSpawner.PalmRootObject.gameObject, cocSpawner);
            cocSpawner.Init(_registry.GetData<CoconutSpawnerData>(cocSpawner.PalmRootObject.name));
        }

        while (!BerrySpawnManager.HasStarted) {
            yield return null;
        }
        _berrySpawners = FindObjectsOfType<BerrySpawnManager>().ToList();
        foreach (var berrySpawner in _berrySpawners)
        {
            _registry.Register(berrySpawner.gameObject, berrySpawner);
            berrySpawner.Init(_registry.GetData<BerryBushData>(berrySpawner.gameObject.name));
        }

        while (!BananaTreeManager.HasStarted) {
            yield return null;
        }
        _bananaSpawners = FindObjectsOfType<BananaTreeManager>().ToList();
        foreach (var ban in _bananaSpawners)
        {
            _registry.Register(ban.gameObject, ban);
            ban.Init(_registry.GetData<BananaTreeData>(ban.gameObject.name));
        }

        OnInitialized?.Invoke();

        StartAllSpawners();

        yield return null;
    }

    void StartAllSpawners() {
        StartSpawner(_bananaSpawners.Cast<ISpawner>().ToList());
        StartSpawner(_berrySpawners.Cast<ISpawner>().ToList());
        StartSpawner(_coconutSpawners.Cast<ISpawner>().ToList());
    }

    void StartSpawner(List<ISpawner> spawner) {
        foreach (var sp in spawner) {
            sp.BeginSpawn();
        }
    }
}
