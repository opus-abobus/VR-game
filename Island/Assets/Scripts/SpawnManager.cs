using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour, Bootstrap.IBootstrap {
    private static SpawnManager _instance;
    public static SpawnManager Instance { get { return _instance; } }

    private List<BananaTreeManager> _bananaSpawners;
    public List<BananaTreeManager> BananaSpawners { get { return _bananaSpawners; } set { _bananaSpawners = value; } }

    private List<BerrySpawnManager> _berrySpawners;
    public List<BerrySpawnManager> BerrySpawners { get { return _berrySpawners; } set { _berrySpawners = value; } }

    private List<CocountSpawner> _coconutSpawners;
    public List<CocountSpawner> CocountSpawners { get { return _coconutSpawners; } set { _coconutSpawners = value; } }

    public bool HasInitialized { get; private set; } = false;

    public event Action Initialized;

    public interface ISpawner {
        bool HasInitialized { get; }
        void Init();
        void BeginSpawn();
    }

    public void Initialize() {
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
            print("waiting for coco spawner");
            yield return null;
        }
        print("coconut spawner script has been started (should be initialized).");
        _coconutSpawners = FindObjectsOfType<CocountSpawner>().ToList();

        while (!BerrySpawnManager.HasStarted) {
            print("waiting for berry spawner");
            yield return null;
        }
        print("berry spawner script has been started (should be initialized).");
        _berrySpawners = FindObjectsOfType<BerrySpawnManager>().ToList();

        while (!BananaTreeManager.HasStarted) {
            print("waiting for banana spawner");
            yield return null;
        }
        print("banana spawner script has been started (should be initialized).");
        _bananaSpawners = FindObjectsOfType<BananaTreeManager>().ToList();

        Bootstrap.Instance.BootstrapFinished += OnBootstrapFinished;

        Initialized?.Invoke();
        HasInitialized = true;

        print("Banana spawners count: " + _bananaSpawners.Count);
        print("Coconut spawners count: " + _coconutSpawners.Count);
        print("Berry spawners count: " + _berrySpawners.Count);

        yield return null;
    }

    private void OnBootstrapFinished() {
        StartAllSpawners();
    }

    void StartAllSpawners() {
        StartSpawner(_bananaSpawners.Cast<ISpawner>().ToList());
        StartSpawner(_berrySpawners.Cast<ISpawner>().ToList());
        StartSpawner(_coconutSpawners.Cast<ISpawner>().ToList());
    }

    void StartSpawner(List<ISpawner> spawner) {
        foreach (var sp in spawner) {
            sp.Init();
            sp.BeginSpawn();
        }
    }
}
