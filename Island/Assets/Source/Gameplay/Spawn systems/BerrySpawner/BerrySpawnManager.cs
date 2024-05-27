using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerrySpawnManager : MonoBehaviour, SpawnManager.ISpawner
{
    private bool _wasStartSpawn;

    private bool _hasInitialized = false;

    private Berry[] _spawnedBerries;

    [SerializeField]
    private Transform _spawnPointsRoot;
    private Vector3[] _spawnPoints;

    [SerializeField]
    private GameObject _spawnPrefab;

    [SerializeField]
    private Transform _parent;

    private WorldSettings.IBerriesSettings _berriesSettings;

    //respawn properties
    [SerializeField]
    private bool _allowRespawnBerries = true;
    [SerializeField]
    private bool _useRandomAmount = false;

    //коллекция для хранения информации о точках, в которых был осуществлен спавн:
    //false - точка свободна, true - точка занята
    private Dictionary<int, bool> spawnPointsDictionary;
    public void Init() {
        _berriesSettings = GameSettingsManager.Instance.ActiveWorldSettings;

        _wasStartSpawn = false;

        if (_spawnPointsRoot != null) {

            InitSpawnPoints();
            InitSpawnPointsDictionary();

            _spawnedBerries = new Berry[_spawnPoints.Length];

            _hasInitialized = true;
            
            SpawnBerries();
        }
        else {
            Debug.LogAssertion("У куста отсутствуют точки спавна ягод. Скрипт будет уничтожен");
            Destroy(this);
        }
    }

    void SpawnManager.ISpawner.BeginSpawn() {
/*        if (!_hasInitialized) {
            SpawnManager.ISpawner _spawner = this;
            _spawner.Init();
        }*/

        SpawnBerries();
        StartRespawn();
    }

    public void StartRespawn() {
        StartCoroutine(RespawnBerries());
    }

    void InitSpawnPoints() {
        int length = _spawnPointsRoot.childCount;

        if (length == 0) {
            Debug.LogAssertion("У куста отсутствуют точки спавна ягод. Скрипт будет уничтожен");
            Destroy(this);
            return;
        }

        _spawnPoints = new Vector3[length];

        for (int i = 0; i < length; i++) {
            _spawnPoints[i] = _spawnPointsRoot.GetChild(i).position;
        }
    }

    void InitSpawnPointsDictionary() {
        spawnPointsDictionary = new Dictionary<int, bool>();
        for (int i = 0; i < _spawnPoints.Length; i++) {
            spawnPointsDictionary.Add(i, false);
        }
    }

    IEnumerator RespawnBerries() {
        yield return new WaitForEndOfFrame();

        int minTimeToRespawn = _berriesSettings.MinTimeToRespawnBerryInSeconds;
        int maxTimeToRespawn = _berriesSettings.MaxTimeToRespawnBerryInSeconds;

        int timeToRespawn;
        while (true) {
            if (!_allowRespawnBerries) yield return null;

            timeToRespawn = UnityEngine.Random.Range(minTimeToRespawn, maxTimeToRespawn);
            yield return new WaitForSeconds(timeToRespawn);
            SpawnBerries();
        }
    }

    void GetValidAmountOfBerries(out int minBerriesOnStart, out int maxBerriesOnStart) {
        minBerriesOnStart = _berriesSettings.MinBerriesOnStart;
        maxBerriesOnStart = _berriesSettings.MaxBerriesOnStart;

        if (minBerriesOnStart < 0) minBerriesOnStart = 0;
        if (maxBerriesOnStart < 0) maxBerriesOnStart = 0;

        if (minBerriesOnStart == maxBerriesOnStart) {
            if (minBerriesOnStart < 0) 
                minBerriesOnStart = maxBerriesOnStart = 0;
            if (minBerriesOnStart > _spawnPoints.Length) 
                minBerriesOnStart = maxBerriesOnStart = _spawnPoints.Length;
            return;
        }

        if (minBerriesOnStart > maxBerriesOnStart) {
            if (minBerriesOnStart > _spawnPoints.Length) {
                minBerriesOnStart = maxBerriesOnStart = _spawnPoints.Length;
            }
            else {
                minBerriesOnStart = maxBerriesOnStart; 
            }
            return;
        }

        if (maxBerriesOnStart > _spawnPoints.Length) maxBerriesOnStart = _spawnPoints.Length;
    }

    void SpawnBerries() {
        int spawnCount;

        if (!_wasStartSpawn) {
            if (_useRandomAmount) 
                spawnCount = UnityEngine.Random.Range(0, _spawnPoints.Length + 1);
            else {
                int minBerriesOnStart, maxBerriesOnStart;
                GetValidAmountOfBerries(out minBerriesOnStart, out maxBerriesOnStart);

                spawnCount = UnityEngine.Random.Range(minBerriesOnStart, maxBerriesOnStart);
            }

            _wasStartSpawn = true;
        }
        else {
            spawnCount = 1;
        }

        int spawnPoint;
        GameObject obj;

        while (spawnCount > 0) {
            spawnPoint = ItemSpawnManager.GetFreeSpawnPoint(spawnPointsDictionary);
            if (spawnPoint == -1) {
                break;
            }

            obj = Instantiate(_spawnPrefab, _spawnPoints[spawnPoint], Quaternion.identity, _parent);

            var berry = obj.GetComponent<Berry>();
            if (berry != null) {
                _spawnedBerries.SetValue(berry, spawnPoint);
            }

            // сохранение значения флага спавна в точке
            spawnPointsDictionary[spawnPoint] = true;

            if (_spawnedBerries[spawnPoint].IsFallen)
                StartCoroutine(RespawnCooldown(spawnPoint));

            spawnCount--;
        }
    }
    IEnumerator RespawnCooldown(int index) {
        yield return new WaitForSeconds(_berriesSettings.TimeoutBerryRespawnInSeconds);
        spawnPointsDictionary[index] = false;
    }

    private static bool _hasStarted = false;
    public static bool HasStarted { get { return _hasStarted; } }
    private void Start() {
        _hasStarted = true;
    }

    private void OnDisable() {
        StopAllCoroutines();
    }
}
