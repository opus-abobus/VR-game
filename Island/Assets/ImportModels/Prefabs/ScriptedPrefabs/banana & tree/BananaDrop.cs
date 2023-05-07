using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BananaDrop : MonoBehaviour
{
    public int maxBananasToSpawn = -1;
    public GameObject banana;
    public GameObject ground;

    [SerializeField] bool useGlobalSettings = true;
    [SerializeField] GameSettings globalSettings;
    int bananaAmount = 0;

    List <string> tags;

    Rigidbody rb;
    BoxCollider _collider;

    private void Awake() {
        tags = new List<string>();
        AddThrowingObjects();

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        _collider = GetComponent<BoxCollider>();
        _collider.isTrigger = true;

        if (useGlobalSettings && globalSettings != null) {
            bananaAmount = Random.Range(globalSettings.minBananasToDrop, globalSettings.maxBananasToDrop);
        }
        else {
            bananaAmount = Random.Range(0, maxBananasToSpawn);
        }
    }

    void AddThrowingObjects() {
        tags.Add("banana");
        tags.Add("berry");
        tags.Add("rock");
        tags.Add("cocount"); tags.Add("coconut");
    }

    private void Start() {
        if (ground == null) {
            ground = GameObject.FindGameObjectWithTag("ground");
            if (ground == null) {
                Debug.LogError("У бананов нет ссылки на землю");
            }
        }
        else if (SceneManager.GetActiveScene().name == "Mechanic test") {
            ground = GameObject.FindGameObjectWithTag("ground");
        }
    }

    private void OnTriggerEnter(Collider other) {
        foreach (var tag in tags) {
            if (other.tag == tag) {
                DropBanana();
                return;
            }
        }
    }
    private void DropBanana() {
        rb.useGravity = true;
        _collider.isTrigger = false;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject == ground) {
            Vector3 spawnPoint = collision.GetContact(0).point;
            SpawnBanana(spawnPoint);
            Destroy(this.gameObject);
        }
    }
    [SerializeField] public BananaRipening bananaRipening;
    private void OnDestroy() {
        //print(spawnedBanana.transform.position + "  origin: " + transform.position);
        bananaRipening.isBananasFallen = true;
    }
    void SpawnBanana(Vector3 spawnPoint) {
        /*int bananaAmount;
        if (maxBananasToSpawn != -1) bananaAmount = maxBananasToSpawn;
        else bananaAmount = Random.Range(1, 5);*/

        for (int i = 0; i < bananaAmount; i++) {
            spawnPoint.y += _collider.size.y / 2;
            GameObject spawnedBanana = Instantiate(banana, spawnPoint, Random.rotation);
            spawnedBanana.transform.parent = this.gameObject.transform.parent;
            print(spawnedBanana.transform.position + "  origin: " + transform.position);
            //spawnedBanana.transform.localScale = transform.lossyScale;
        }
    }
}
