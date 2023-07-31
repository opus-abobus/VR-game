using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaDrop : MonoBehaviour
{
    public int _maxBananasToSpawn = -1;

    public GameObject _banana;

    public event Action FallenFruit;

/*    [SerializeField]
    private BananaRipening _bananaRipening;*/

    private int _bananaAmount = 0;

    private List <string> _tags;

    private Vector3 _colliderSize;

    private void Awake() {
        Init();
    }

    public void Init() {
        _tags = new List<string>();
        AddThrowingObjects();

        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<BoxCollider>().isTrigger = true;

        var bananasSettings = GameSettingsManager.Instance.BananasSettings;
        _bananaAmount = UnityEngine.Random.Range(bananasSettings.MinBananasToDrop, bananasSettings.MaxBananasToDrop);
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
            GameObject spawnedBanana = Instantiate(_banana, spawnPoint, UnityEngine.Random.rotation);
            spawnedBanana.transform.parent = gameObject.transform.parent;
        }
    }
}
