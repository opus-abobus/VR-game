using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class BananaDrop : MonoBehaviour
{
    private BananaPool _bananaPool;

    public event Action FallenFruit;

    private int _bananaAmount = 0;

    private List <string> _tags;

    private Vector3 _colliderSize;

    [SerializeField] private AssetReferenceGameObject _bananaPlotPrefabRef;

    private GameObjectsRegistries _registry;

    public void Init() {
        _registry = GameObjectsRegistries.Instance;

        _tags = new List<string>();
        AddThrowingObjects();

        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<BoxCollider>().isTrigger = true;

        WorldSettings.IBananaTreeSettings bananasSettings = GameSettingsManager.Instance.ActiveWorldSettings;
        _bananaAmount = UnityEngine.Random.Range(bananasSettings.MinBananasToDrop, bananasSettings.MaxBananasToDrop);

        _bananaPool = BananaPool.Instance;
    }

    void AddThrowingObjects() {
        _tags.Add("banana");
        _tags.Add("berry");
        _tags.Add("rock");
        _tags.Add("cocount"); _tags.Add("coconut"); _tags.Add("coconutUnbroken");
    }

    private void OnTriggerEnter(Collider other) {
        if (_tags == null) return;

        foreach (var tag in _tags) {
            if (other.tag == tag) {
                DropBanana();
                return;
            }
        }
    }
    private void DropBanana() {
        FallenFruit?.Invoke();

        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<BoxCollider>().isTrigger = false;

        _registry.Register(gameObject, _bananaPlotPrefabRef.AssetGUID);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("ground")) {
            Vector3 spawnPoint = collision.GetContact(0).point;
            SpawnBanana(spawnPoint);

            _registry.Unregister(gameObject);

            Destroy(gameObject);
        }
    }

    void SpawnBanana(Vector3 spawnPoint) {
        _colliderSize = GetComponent<BoxCollider>().size;

        for (int i = 0; i < _bananaAmount; i++) {
            spawnPoint.y += _colliderSize.y / 2;

            GameObject spawnedBanana = _bananaPool.Get();
            spawnedBanana.transform.parent = gameObject.transform.parent;
            spawnedBanana.transform.position = spawnPoint;
            spawnedBanana.transform.rotation = UnityEngine.Random.rotation;
            spawnedBanana.SetActive(true);

            _registry.Register(spawnedBanana, _bananaPool.GetAssetGUID());
        }
    }
}