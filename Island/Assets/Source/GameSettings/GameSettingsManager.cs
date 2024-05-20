using System;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsManager : MonoBehaviour, GameplayBootstrap.IBootstrap {
    public static GameSettingsManager Instance { get { return _instance; } }

    [SerializeField]
    private List<WorldSettings> _worldSettings;

    public WorldSettings ActiveWorldSettings { 
        get { 
            return _worldSettings[_currentDifficultyPresetId]; 
        }
    }

    private int _currentDifficultyPresetId = -1;

    private static GameSettingsManager _instance;

    public event Action OnInitialized, OnActiveWorldSettingsChanged;

    void GameplayBootstrap.IBootstrap.Initialize() {
        if (_instance == null) {
            _instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }

        if (_worldSettings == null || _worldSettings.Count == 0) {
            throw new NullReferenceException("World settings was not set to an instance or empty.");
        }

        for (int i = 0; i < _worldSettings.Count; i++) {
            if (i != (int)_worldSettings[i].Difficulty) {
                Debug.LogAssertion("Порядок сложностей в списке не соблюден или имеются настройки с одинаковыми сложностяим.");
            }
        }

        //!!!
        _currentDifficultyPresetId = 0;

        OnInitialized?.Invoke();
    }

    void OnWorldSettingsChanged(int settingsIndex) {
        _currentDifficultyPresetId = settingsIndex;
    }
}

    
