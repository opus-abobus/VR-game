using System;
using System.Collections;
using System.Collections.Generic;
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

        StartCoroutine(InitProcess());
    }

    IEnumerator InitProcess() {
        while (true) {
            _gameSettingsManager.Initialize();
            while (!_gameSettingsManager.HasInitialized) {
                yield return null;
            }
            print("GameSettingsManager has been initialized!");

            _gameManager.Initialize();
            while (!_gameManager.HasInitialized) {
                yield return null;
            }
            print("GameManager has been initialized!");

            break;
        }

        print("Bootstrap finished! Starting spawning...");
        BootstrapFinished?.Invoke();
    }
}
