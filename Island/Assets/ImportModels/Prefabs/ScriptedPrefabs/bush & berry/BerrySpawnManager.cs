using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerrySpawnManager : MonoBehaviour
{
    public int minBerries = 1;
    public int maxBerries = -1;
    public Transform[] spawnPoints;
    public GameObject spawnPrefab;
    public Transform parent;

    public bool allowRespawnBerries = true;

    int spawnCount = 0;
    bool allowSpawnPerform;
    private void Awake() {
        if (spawnPoints != null) {
            allowSpawnPerform = true;
            SpawnBerries();
            timeToSpawn = Random.Range(minTimeToSpawn, maxTimeToSpawn);
        }
        else allowSpawnPerform = false;
    }
    private void Start() {
        if (allowRespawnBerries && allowSpawnPerform)
            StartCoroutine(RespawnBerries());
    }

    float _elapsedTime = 0;
    public float minTimeToSpawn = 1;
    public float maxTimeToSpawn = 4;
    float timeToSpawn;
    private void Update() {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > timeToSpawn) {
            timeToSpawn = Random.Range(minTimeToSpawn, maxTimeToSpawn);
            _elapsedTime = 0;
        }
    }
    IEnumerator RespawnBerries() {
        if (maxBerries == -1)
            spawnCount = Random.Range(minBerries, spawnPoints.Length + 1);
        else
            spawnCount = Random.Range(minBerries, maxBerries + 1);

        int spawnPoint, lastPoint = -1;
        while (spawnCount > 0) {
            spawnPoint = Random.Range(0, spawnPoints.Length);
            if (spawnPoint == lastPoint) continue;

            yield return new WaitForSeconds(timeToSpawn);
            Instantiate(spawnPrefab, spawnPoints[spawnPoint].position, Quaternion.identity, parent);

            lastPoint = spawnPoint;
            spawnCount--;
        }
    }
    void SpawnBerries() {
        if (allowSpawnPerform) {
            if (maxBerries == -1)
                spawnCount = Random.Range(minBerries, spawnPoints.Length + 1);
            else
                spawnCount = Random.Range(minBerries, maxBerries + 1);

            int spawnPoint, lastPoint = -1;
            while (spawnCount > 0) {
                spawnPoint = Random.Range(0, spawnPoints.Length);
                if (spawnPoint == lastPoint) continue;

                Instantiate(spawnPrefab, spawnPoints[spawnPoint].position, Quaternion.identity, parent);

                lastPoint = spawnPoint;
                spawnCount--;
            }
        }
    }
}
