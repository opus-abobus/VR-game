using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CocountSpawner : MonoBehaviour
{
    [SerializeField] GameObject cocount;
    [SerializeField] Transform parent;

    public bool useGlobalSettings = true;
    [SerializeField] GameSettings globalSettings;

    [SerializeField] int minTimeToSpawn = 5;
    [SerializeField] int maxTimeToSpawn = 10;

    [SerializeField] MeshRenderer _meshRenderer;
    [SerializeField] MeshFilter _meshFilter;

    int cocountsOnStart = 0;
    Vector3 boundsSize;
    IEnumerator _CocountSpawning;
    private void Awake() {
        _meshRenderer.enabled = false;
        if (cocount == null) { Debug.LogError("” спавнера кокосов отсутствует ссылка на кокос"); this.enabled = false; }
        if (parent == null) { GameObject parentObj = new GameObject("SpawnedCocounts"); parent = parentObj.transform; }

        boundsSize = _meshRenderer.bounds.size / 2;
        Destroy(_meshRenderer);
        Destroy(_meshFilter);

        if (useGlobalSettings && globalSettings != null) {
            minTimeToSpawn = globalSettings.minTimeToRespawnCoconutInSeconds;
            maxTimeToSpawn = globalSettings.maxTimeToRespawnCoconutInSeconds;
            
            if (Random.Range(0f, 1) <= globalSettings.chanceToSpawnOnStart)
                cocountsOnStart = Random.Range(globalSettings.minCoconutsOnStart, globalSettings.maxCoconutsOnStart);
            else
                cocountsOnStart = 0;
        }

        _CocountSpawning = CocountSpawning();
        StartCoroutine(_CocountSpawning);
    }

    private void Update() {
        //if (Input.GetKeyDown(KeyCode.G)) { StopCoroutine(_CocountSpawning); }
    }

    IEnumerator CocountSpawning() {
        SpawnCocount(cocountsOnStart);

        int timeToSpawn = Random.Range(minTimeToSpawn, maxTimeToSpawn + 1);
        yield return new WaitForSeconds(timeToSpawn);

        while (true) {
            timeToSpawn = Random.Range(minTimeToSpawn, maxTimeToSpawn + 1);
            SpawnCocount();
            yield return new WaitForSeconds(timeToSpawn);
        }
    }

    void SpawnCocount(int amount = 1) {
        while (amount > 0) {
            Vector3 randPoint = Random.insideUnitSphere;
            Vector3 spawnPos = transform.position + MultiplyVectors(randPoint, boundsSize);
            GameObject _cocountInstance = Instantiate(cocount, spawnPos, cocount.transform.rotation);
            _cocountInstance.transform.parent = parent;
            amount--;
        }
    }

    Vector3 MultiplyVectors(Vector3 v1, Vector3 v2) {
        return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
    }
}
