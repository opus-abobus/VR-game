using DataPersistence.Gameplay;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CocountSpawner : MonoBehaviour, SpawnManager.ISpawner {

    [SerializeField] private AssetReferenceGameObject _coconutPrefabRef;
    private GameObject _coconut;

    private static Transform _parent;

    [field: SerializeField] public Transform PalmRootObject { get; private set; }

    [SerializeField]
    private int _minTimeToSpawn = 5;
    [SerializeField]
    private int _maxTimeToSpawn = 10;

    [SerializeField]
    private MeshRenderer _meshRenderer;
    [SerializeField]
    private MeshFilter _meshFilter;

    private int _cocountsOnStart = 0;

    private Vector3 _boundsSize;

    private IEnumerator _coconutSpawning;

    private SpawnManager.ISpawner _spawner;

    private static bool _hasStarted = false;
    public static bool HasStarted { get { return _hasStarted; } }

    private bool _wasStartSpawn;

    public CoconutSpawnerData GetData()
    {
        return new CoconutSpawnerData(PalmRootObject.name, _wasStartSpawn);
    }

    private void Awake() {
        _spawner = this;

        if (_parent == null) {
            _parent = GameObject.Find("SpawnedCoconuts").transform;
            if (_parent == null) {
                _parent = new GameObject("SpawnedCoconuts").transform;
            }
        }
    }

    private void Start() {
        _hasStarted = true;
    }

    public void Init(CoconutSpawnerData data) {

        if (data != null)
        {
            _wasStartSpawn = data.wasStartSpawn;
        }

        _registry = GameObjectsRegistries.Instance;

        _coconut = AddressableItems.Instance.GetPrefabByGUID(_coconutPrefabRef.AssetGUID);

        _meshRenderer.enabled = false;

        _boundsSize = _meshRenderer.bounds.size / 2;
        Destroy(_meshRenderer);
        Destroy(_meshFilter);

        var settings = GameSettingsManager.Instance.ActiveWorldSettings as WorldSettings.ICoconutPalmSettings;
        _minTimeToSpawn = settings.MinTimeToRespawnInSeconds;
        _maxTimeToSpawn = settings.MaxTimeToRespawnInSeconds;

        if (Random.Range(0f, 1) <= settings.ChanceToSpawnOnStart) {
            _cocountsOnStart = Random.Range(settings.MinCoconutsOnStart, settings.MaxCoconutsOnStart);
        }
        else {
            _cocountsOnStart = 0;
        }
    }

    void SpawnManager.ISpawner.BeginSpawn() {
        _coconutSpawning = CocountSpawning();
        StartCoroutine(_coconutSpawning);
    }

    IEnumerator CocountSpawning() {

        if (!_wasStartSpawn)
        {
            SpawnCocount(_cocountsOnStart);
            _wasStartSpawn = true;
        }
        else
        {
            int timeToSpawn = Random.Range(_minTimeToSpawn, _maxTimeToSpawn + 1);
            yield return new WaitForSeconds(timeToSpawn);
        }

        while (true)
        {
            SpawnCocount();
            int timeToSpawn = Random.Range(_minTimeToSpawn, _maxTimeToSpawn + 1);
            yield return new WaitForSeconds(timeToSpawn);
        }
    }

    void SpawnCocount(int amount = 1) {
        while (amount > 0) {
            Vector3 randPoint = Random.insideUnitSphere;
            Vector3 spawnPos = transform.position + MultiplyVectors(randPoint, _boundsSize);
            GameObject _cocountInstance = Instantiate(_coconut, spawnPos, _coconut.transform.rotation);
            _cocountInstance.transform.parent = _parent;
            amount--;

            _registry.Register(_cocountInstance, _coconutPrefabRef.AssetGUID);
        }
    }

    Vector3 MultiplyVectors(Vector3 v1, Vector3 v2) {
        return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
    }

    private GameObjectsRegistries _registry;
}
