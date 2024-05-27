using System;
using System.Collections.Generic;
using UnityEngine;

public class BananaDrop : MonoBehaviour
{
    public GameObject _banana;

    private BananaPool _bananaPool;

    public event Action FallenFruit;

    private int _bananaAmount = 0;

    private List <string> _tags;

    private Vector3 _colliderSize;

    public void Init() {
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
        _tags.Add("cocount"); _tags.Add("coconut");
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
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<BoxCollider>().isTrigger = false;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("ground")) {
            Vector3 spawnPoint = collision.GetContact(0).point;
            SpawnBanana(spawnPoint);

            FallenFruit?.Invoke();

            Destroy(gameObject);
        }
    }

    void SpawnBanana(Vector3 spawnPoint) {
        _colliderSize = GetComponent<BoxCollider>().size;

        for (int i = 0; i < _bananaAmount; i++) {
            spawnPoint.y += _colliderSize.y / 2;
            //GameObject spawnedBanana = Instantiate(_banana, spawnPoint, UnityEngine.Random.rotation);
            GameObject spawnedBanana = _bananaPool.Get();
            spawnedBanana.transform.parent = gameObject.transform.parent;
            spawnedBanana.transform.position = spawnPoint;
            spawnedBanana.transform.rotation = UnityEngine.Random.rotation;
            spawnedBanana.SetActive(true);

            
        }
    }
}
