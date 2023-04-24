using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public int maxSpawnedBerries = -1;
    public Transform[] spawnPoints;
    public GameObject spawnPrefab;
    public Transform parent;

    int spawnCount = 0;
    private void Awake() {
        if (spawnPoints != null) {
            if (maxSpawnedBerries == -1)
                spawnCount = Random.Range(0, spawnPoints.Length + 1);
            else 
                spawnCount = Random.Range(0, maxSpawnedBerries + 1);
            //Debug.Log(spawnCount + " berries spawned");

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
