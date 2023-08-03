using System;
using UnityEngine;

public class GameManager : MonoBehaviour, Bootstrap.IBootstrap
{
    //<summary> Данный класс отвечает за управление менеджерами, например, GameSettingsManager
    //</summary>

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [SerializeField]
    private SpawnManager _spawnManager;

    [SerializeField]
    private EvacuationSystem _evacuationSystem;

    [SerializeField]
    private PlayerEating _playerEating;

    public Action OnInitialized;

    public void Initialize() {
        if (_instance == null) {
            _instance = this;
        }
        else {
            Destroy(gameObject);
        }

        _spawnManager.OnInitialized += OnSpawnManagerInitialized;
        _spawnManager.Initialize();

        _evacuationSystem.OnInitialized += OnEvacSystemInitialized;
        _playerEating.OnInitialized += OnPlayerEatingInitialized;
    }

    void OnSpawnManagerInitialized() {
        //print("spawn manager initialized.");
        _spawnManager.OnInitialized -= OnSpawnManagerInitialized;

        _evacuationSystem.Initialize();
    }

    void OnEvacSystemInitialized() {
        _evacuationSystem.OnInitialized -= OnEvacSystemInitialized;
        //print("evac system initialized.\nGame manager initialized.");

        _playerEating.Initialize();
    }

    void OnPlayerEatingInitialized() {
        _playerEating.OnInitialized -= OnPlayerEatingInitialized;
        //print("player eating initialized.");

        OnInitialized?.Invoke();
    }
}
