using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, Bootstrap.IBootstrap
{
    //<summary> Данный класс отвечает за управление менеджерами, например, GameSettingsManager
    //</summary>

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [SerializeField]
    private SpawnManager _spawnManager;

    public bool HasInitialized { get; private set; } = false;

    public void Initialize() {
        if (_instance == null) {
            _instance = this;
        }
        else {
            Destroy(gameObject);
        }

        StartCoroutine(InitProcess());
    }

    IEnumerator InitProcess() {
        _spawnManager.Initialize();
        while (!_spawnManager.HasInitialized) {
            yield return null;
        }
        print("spawn manager initialized.");

        //Bootstrap.Instance.BootstrapFinished += OnBootstrapFinished;

        HasInitialized = true;
    }
}
