using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ItemSpawnManager : MonoBehaviour
{
    public bool useGlobalSettings = true;

    [SerializeField] Transform[] lighterSpawnPoints;
    [SerializeField] GameObject lighterPrefab;

    [SerializeField] Transform[] signalGunSpawnPoints;
    [SerializeField] GameObject signalGunPrefab;
    public int signalGunAmount = 1;

    [SerializeField] Transform[] bonfireSpawnPoints;
    [SerializeField] GameObject bonfirePrefab;
    public int bonfireAmount = 3;

    Dictionary<int, bool> lighterSpawnPointsDictionary;
    Dictionary<int, bool> signalGunSpawnPointsDictionary;
    Dictionary<int, bool> bonfireSpawnPointsDictionary;

    void InitLighterSpawnPointsDictionary() {
        lighterSpawnPointsDictionary = new Dictionary<int, bool>();
        for (int i = 0; i < lighterSpawnPoints.Length; i++) {
            lighterSpawnPointsDictionary.Add(i, false);
        }
    }

    void InitSignalGunSpawnPoints() {
        signalGunSpawnPointsDictionary = new Dictionary<int, bool>();
        for (int i = 0; i < signalGunSpawnPoints.Length; i++) {
            signalGunSpawnPointsDictionary.Add(i, false);
        }
    }

    void InitBonfireSpawnPoints() {
        bonfireSpawnPointsDictionary = new Dictionary<int, bool>();
        for (int i = 0; i < bonfireSpawnPoints.Length; i++) {
            bonfireSpawnPointsDictionary.Add(i, false);
        }
    }

    void SpawnLighter() {
        if (lighterSpawnPoints != null) {
            int spawnPoint = Random.Range(0, lighterSpawnPoints.Length);
            Instantiate(lighterPrefab, lighterSpawnPoints[spawnPoint].position, Quaternion.identity);
        }
    }

    void SpawnSignalGun() {
        if (useGlobalSettings) {
            signalGunAmount = GameSettings.instance.signalGunsAmount;
        }

        int spawnPoint, count = signalGunAmount;

        while (count > 0) {
            spawnPoint = Random.Range(0, signalGunSpawnPoints.Length);

            if (signalGunSpawnPointsDictionary[spawnPoint]) {
                continue;
            }

            Instantiate(signalGunPrefab, signalGunSpawnPoints[spawnPoint].position, Quaternion.identity);
            signalGunSpawnPointsDictionary[spawnPoint] = true;

            count--;
        }
    }

    void SpawnBonfire() {
        if (useGlobalSettings) {
            bonfireAmount = GameSettings.instance.bonfiresAmount;
        }

        int spawnPoint, count = bonfireAmount;

        while (count > 0) {
            spawnPoint = Random.Range(0, bonfireSpawnPoints.Length);

            if (bonfireSpawnPointsDictionary[spawnPoint]) {
                continue;
            }

            Instantiate(bonfirePrefab, bonfireSpawnPoints[spawnPoint].position, Quaternion.identity);
            bonfireSpawnPointsDictionary[spawnPoint] = true;

            count--;
        }
    }

    private void Start() {
        InitLighterSpawnPointsDictionary();
        SpawnLighter();

        InitSignalGunSpawnPoints();
        SpawnSignalGun();

        InitBonfireSpawnPoints();
        SpawnBonfire();
    }
}
