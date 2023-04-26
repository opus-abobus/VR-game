using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BananaDrop : MonoBehaviour
{
    //[Tooltip("Объекты, которые можно кидать в бананы, чтобы они падали")]
    //public GameObject[] throwingObjects;
    public int maxBananasToSpawn = -1;
    public GameObject bananaPart;
    public GameObject banana;
    public GameObject ground;

    List <string> tags;

    Rigidbody rb;
    BoxCollider collider;

    private void Awake() {
        tags = new List<string>();
        /*if (throwingObjects != null) {
            foreach (var obj in throwingObjects) {
                tags.Add(obj.tag);
            }
        }*/
        AddThrowingObjects();

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        collider = GetComponent<BoxCollider>();
        collider.isTrigger = true;
    }

    void AddThrowingObjects() {
        tags.Add("banana");
        tags.Add("berry");
        tags.Add("rock");
        tags.Add("cocount");
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
        collider.isTrigger = false;
    }

    private void OnCollisionEnter(Collision collision) {
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject == ground) {
            SpawnBanana();
            Destroy(bananaPart);
        }
    }
    [SerializeField] BananaRipening bananaRipening;
    private void OnDestroy() {
        bananaRipening.isBananasFallen = true;
    }

    void SpawnBanana() {
        Vector3 spawnPoint = transform.position;

        float l_Bound = collider.bounds.min.x - collider.bounds.extents.x;
        float r_Bound = collider.bounds.min.x + collider.bounds.extents.x;

        int bananaAmount;
        if (maxBananasToSpawn != -1) bananaAmount = maxBananasToSpawn;
        else bananaAmount = Random.Range(1, 5);

        for (int i = 0; i < bananaAmount; i++) {
            spawnPoint = new Vector3(Random.Range(l_Bound, r_Bound), spawnPoint.y, spawnPoint.z);
            GameObject spawnedBanana = Instantiate(banana, spawnPoint, Quaternion.identity);
            spawnedBanana.transform.localScale = transform.lossyScale;

            Rigidbody bananaRb = spawnedBanana.GetComponent<Rigidbody>();
            if (bananaRb != null) {
                bananaRb.AddForceAtPosition(-Vector3.right, spawnPoint);
            }
        }
    }
}
