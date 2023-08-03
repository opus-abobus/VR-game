using System;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private GameSettingsManager _gameSettingsManager;

    private static Bootstrap _instance;
    public static Bootstrap Instance { get { return _instance; } }

    public event Action BootstrapFinished;

    public interface IBootstrap {
        void Initialize();
    }

    private void Awake() {
        if (_instance == null) { 
            _instance = this; 
        }
        else {
            Destroy(gameObject); 
            return;
        }

        _gameSettingsManager.OnInitialized += OnGameSettingsManagerInitialized;
        _gameManager.OnInitialized += OnGameManagerInitialized;

        _gameSettingsManager.Initialize();
    }

    void OnGameSettingsManagerInitialized() {
        print("GameSettingsManager has been initialized!");
        _gameSettingsManager.OnInitialized -= OnGameSettingsManagerInitialized;

        _gameManager.Initialize();
    }

    void OnGameManagerInitialized() {
        print("GameManager has been initialized!");
        _gameManager.OnInitialized -= OnGameManagerInitialized;

        print("Bootstrap finished! Starting spawning...");
        BootstrapFinished?.Invoke();
    }
}
