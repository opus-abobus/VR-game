using DataPersistence.Gameplay;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static SpawnManager;

public class RockSpawner : MonoBehaviour, ISpawner
{
    [SerializeField] private AssetReferenceGameObject[] _rockPrefabRefs;
    private Tuple<GameObject, string>[] _rockPrefabs;

    private bool _wasSpawn = false;

    [SerializeField] private Transform _spawnPontsRoot;

    public SpawnerData GetData()
    {
        return new RockSpawnerData(gameObject.name, _wasSpawn);
    }

    public void SetData<TSpawnerData>(TSpawnerData tSpawnerData) where TSpawnerData : SpawnerData
    {
        var data = tSpawnerData as RockSpawnerData;

        if (data != null)
        {
            _wasSpawn = data.wasSpawn;
        }
    }

    public void Init()
    {
        _rockPrefabs = new Tuple<GameObject, string>[_rockPrefabRefs.Length];

        int i = 0;
        foreach (var @ref in _rockPrefabRefs)
        {
            _rockPrefabs[i++] = new Tuple<GameObject, string>(AddressableItems.Instance.GetPrefabByGUID(@ref.AssetGUID), @ref.AssetGUID);
        }
    }

    void ISpawner.BeginSpawn()
    {
        if (_wasSpawn || _rockPrefabs.Length == 0)
            return;

        var spawnPoints = _spawnPontsRoot.GetComponentsInChildren<Transform>();

        if (spawnPoints != null)
        {
            foreach (var point in spawnPoints)
            {
                int iRand = UnityEngine.Random.Range(0, _rockPrefabs.Length);

                var spawnedRock = Instantiate(_rockPrefabs[iRand].Item1, point);

                GameObjectsRegistries.Instance.RegisterObject(spawnedRock, _rockPrefabs[iRand].Item2, 
                    spawnedRock.GetComponents<Component>());
            }
        }

        _wasSpawn = true;
    }
}
