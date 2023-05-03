using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CocountSpawner : MonoBehaviour
{
    [SerializeField] GameObject cocount;
    [SerializeField] Transform parent;
    [SerializeField] int timeToSpawnInSeconds = 4;
    [SerializeField] bool useRandomTimeSpawn = true;
    [SerializeField] int minTimeToSpawn = 5;
    [SerializeField] int maxTimeToSpawnInSeconds = 10;

    MeshRenderer _meshRenderer;
    Vector3 boundsSize;
    IEnumerator _CocountSpawning;
    private void Awake() {
        GetComponent<MeshRenderer>().enabled = false;
        if (cocount == null) { Debug.LogError("” спавнера кокосов отсутствует ссылка на кокос"); this.enabled = false; }
        if (parent == null) {
            GameObject parentObj = new GameObject("SpawnedCocounts");
            parent = parentObj.transform;
        }
        _meshRenderer = GetComponent<MeshRenderer>();
        boundsSize = _meshRenderer.bounds.size / 2;

        _CocountSpawning = CocountSpawning();
        StartCoroutine(_CocountSpawning);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.G)) { StopCoroutine(_CocountSpawning); }
    }

    IEnumerator CocountSpawning() {
        if (useRandomTimeSpawn) timeToSpawnInSeconds = Random.Range(minTimeToSpawn, maxTimeToSpawnInSeconds + 1);
        yield return new WaitForSeconds(timeToSpawnInSeconds);

        while (true) {
            if (useRandomTimeSpawn) timeToSpawnInSeconds = Random.Range(minTimeToSpawn, maxTimeToSpawnInSeconds + 1);

            Vector3 randPoint = Random.insideUnitSphere;
            Vector3 spawnPos = transform.position + MultiplyVectors(randPoint, boundsSize);
            GameObject _cocountInstance = Instantiate(cocount, spawnPos, cocount.transform.rotation);
            _cocountInstance.transform.parent = parent;
            yield return new WaitForSeconds(timeToSpawnInSeconds);
        }
    }

    Vector3 MultiplyVectors(Vector3 v1, Vector3 v2) {
        return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
    }
}
