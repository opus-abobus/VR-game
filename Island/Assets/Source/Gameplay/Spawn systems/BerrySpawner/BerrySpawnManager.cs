using DataPersistence.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static DataPersistence.Gameplay.BerrySpawnManagerData;

public class BerrySpawnManager : MonoBehaviour, SpawnManager.ISpawner
{
    private bool _wasStartSpawn;

    private BerrySpawnZone[] _spawnZoneControllers;

    [SerializeField] private Transform _spawnPointsRoot;

    [SerializeField] private AssetReferenceGameObject _berryPrefabRef;
    private GameObject _berryPrefab;

    [SerializeField] private Transform _parent;

    private WorldSettings.IBerriesSettings _berriesSettings;

    //respawn properties
    [SerializeField] private bool _allowRespawnBerries = true;
    [SerializeField] private bool _useRandomAmount = false;

    //private int _timeToRespawnInSeconds = -1;

    //коллекция для хранения информации о точках, в которых был осуществлен спавн:
    //ключ - индекс позиции, значение - зона спавна
    private Dictionary<int, BerrySpawnZone> _spawnZones = new();

    public SpawnerData GetData()
    {
        var spawnsZoneData = new BerrySpawnManagerData.BerrySpawnZoneData[_spawnZones.Count];
        int i = 0;
        bool hasBerry;
        foreach (var data in _spawnZones)
        {
            hasBerry = data.Value.Berry == null ? false : true;
            spawnsZoneData[i++] = new BerrySpawnManagerData.BerrySpawnZoneData(data.Key, hasBerry, data.Value.CooldownTimeLeft);
        }

        return new BerrySpawnManagerData(gameObject.name, _wasStartSpawn, spawnsZoneData);
    }

    public void SetData<TSpawnerData>(TSpawnerData tSpawnerData) where TSpawnerData : SpawnerData
    {
        var spawnerData = tSpawnerData as BerrySpawnManagerData;

        if (spawnerData != null)
        {
            _wasStartSpawn = spawnerData.wasStartSpawn;

            foreach (var zoneData in spawnerData.berrySpawnZonesData)
            {
                if (_spawnZones.ContainsKey(zoneData.index))
                {
                    if (zoneData.hasBerry)
                    {
                        _spawnZones[zoneData.index].SetBerry(
                            Instantiate(_berryPrefab, _spawnZones[zoneData.index].transform.position, Quaternion.identity, _parent));
                    }
                    _spawnZones[zoneData.index].CooldownTimeLeft = zoneData.cooldownTimeLeft;
                    if (_spawnZones[zoneData.index].CooldownTimeLeft > 0.001f)
                    {
                        StartCoroutine(RespawnCooldown(zoneData.index, _spawnZones[zoneData.index].CooldownTimeLeft));
                    }
                }
            }
        }
        else
        {
            Debug.LogAssertion("Trying to set null data");
        }

    }

    public void Init()
    {
        _registry = GameObjectsRegistries.Instance;

        _berryPrefab = AddressableItems.Instance.GetPrefabByGUID(_berryPrefabRef.AssetGUID);

        _berriesSettings = GameSettingsManager.Instance.ActiveWorldSettings;

        if (_spawnPointsRoot != null)
        {
            _spawnZoneControllers = _spawnPointsRoot.GetComponentsInChildren<BerrySpawnZone>();
            if (_spawnZoneControllers != null && _spawnZoneControllers.Length > 0)
            {
                int i = 0;
                foreach (var zone in _spawnZoneControllers)
                {
                    zone.BerryFell += OnBerryFell;
                    zone.SetIndex(i);
                    _spawnZones.Add(i++, zone);
                }
            }
            else
            {
                Debug.LogAssertion("У куста отсутствуют точки спавна ягод. Скрипт будет уничтожен");
                Destroy(this);
            }
        }
        else
        {
            Debug.LogAssertion("У куста отсутствуют точки спавна ягод. Скрипт будет уничтожен");
            Destroy(this);
        }
    }

    private void OnBerryFell(int index, GameObject fallenBerry)
    {
        _registry.RegisterObject(fallenBerry, _berryPrefabRef.AssetGUID, new Component[]
        {
            fallenBerry.transform, fallenBerry.GetComponent<Collider>(), fallenBerry.GetComponent<Rigidbody>()
        });
        StartCoroutine(RespawnCooldown(index, _berriesSettings.TimeoutBerryRespawnInSeconds));
    }

    void SpawnManager.ISpawner.BeginSpawn()
    {
        SpawnBerries();
        StartRespawn();
    }

    public void StartRespawn()
    {
        StartCoroutine(RespawnBerries());
    }

    IEnumerator RespawnBerries()
    {
        yield return new WaitForEndOfFrame();

        int minTimeToRespawn = _berriesSettings.MinTimeToRespawnBerryInSeconds;
        int maxTimeToRespawn = _berriesSettings.MaxTimeToRespawnBerryInSeconds;

        int timeToRespawn;
        while (true)
        {
            if (!_allowRespawnBerries) yield return null;

            timeToRespawn = UnityEngine.Random.Range(minTimeToRespawn, maxTimeToRespawn);
            yield return new WaitForSeconds(timeToRespawn);
            SpawnBerries();
        }
    }

    void GetValidAmountOfBerries(out int minBerriesOnStart, out int maxBerriesOnStart)
    {
        minBerriesOnStart = _berriesSettings.MinBerriesOnStart;
        maxBerriesOnStart = _berriesSettings.MaxBerriesOnStart;

        if (minBerriesOnStart < 0) minBerriesOnStart = 0;
        if (maxBerriesOnStart < 0) maxBerriesOnStart = 0;

        if (minBerriesOnStart == maxBerriesOnStart)
        {
            if (minBerriesOnStart < 0)
                minBerriesOnStart = maxBerriesOnStart = 0;
            if (minBerriesOnStart > _spawnZones.Count)
                minBerriesOnStart = maxBerriesOnStart = _spawnZones.Count;
            return;
        }

        if (minBerriesOnStart > maxBerriesOnStart)
        {
            if (minBerriesOnStart > _spawnZones.Count)
            {
                minBerriesOnStart = maxBerriesOnStart = _spawnZones.Count;
            }
            else
            {
                minBerriesOnStart = maxBerriesOnStart;
            }
            return;
        }

        if (maxBerriesOnStart > _spawnZones.Count) maxBerriesOnStart = _spawnZones.Count;
    }

    void SpawnBerries()
    {
        int spawnCount;

        if (!_wasStartSpawn)
        {
            if (_useRandomAmount)
                spawnCount = UnityEngine.Random.Range(0, _spawnZones.Count + 1);
            else
            {
                int minBerriesOnStart, maxBerriesOnStart;
                GetValidAmountOfBerries(out minBerriesOnStart, out maxBerriesOnStart);

                spawnCount = UnityEngine.Random.Range(minBerriesOnStart, maxBerriesOnStart);
            }

            _wasStartSpawn = true;
        }
        else
        {
            spawnCount = 1;
        }

        int spawnPoint;
        GameObject obj;

        while (spawnCount > 0)
        {
            spawnPoint = GetRandomFreeSpawnIndex();
            if (spawnPoint == -1)
            {
                return;
            }

            obj = Instantiate(_berryPrefab, _spawnZones[spawnPoint].transform.position, Quaternion.identity, _parent);

            _spawnZones[spawnPoint].SetBerry(obj);

            spawnCount--;
        }
    }
    IEnumerator RespawnCooldown(int index, float cooldownTimeInSeconds)
    {
        _spawnZones[index].CooldownTimeLeft = cooldownTimeInSeconds;

        while (true)
        {
            yield return null;
            _spawnZones[index].CooldownTimeLeft -= Time.deltaTime;
            if (_spawnZones[index].CooldownTimeLeft <= 0.001f)
            {
                break;
            }
        }
    }

    private int GetRandomFreeSpawnIndex()
    {
        List<int> freeIndexes = new();

        foreach (var spawnZone in _spawnZones.Values)
        {
            if (spawnZone.Berry == null && spawnZone.CooldownTimeLeft <= 0.001f)
            {
                freeIndexes.Add(spawnZone.Index);
            }
        }

        if (freeIndexes.Count > 0)
            return freeIndexes[UnityEngine.Random.Range(0, freeIndexes.Count)];

        return -1;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private GameObjectsRegistries _registry;

    private void OnDestroy()
    {
        if (_spawnZoneControllers != null && _spawnZoneControllers.Length > 0)
        {
            foreach (var zone in _spawnZoneControllers)
            {
                zone.BerryFell -= OnBerryFell;
            }
        }
    }
}
