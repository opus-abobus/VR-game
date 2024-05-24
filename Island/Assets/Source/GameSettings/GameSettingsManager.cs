using System;
using UnityEngine;

public class GameSettingsManager : MonoBehaviour
{
    public static GameSettingsManager Instance { get { return _instance; } }

    [SerializeField]
    private WorldSettingsList _worldSettings;

    public WorldSettings ActiveWorldSettings
    {
        get
        {
            return _worldSettings.entries[_difficultyIndex].WorldSettings;
        }
    }

    private int _difficultyIndex;

    private static GameSettingsManager _instance;

    public event Action OnInitialized, OnActiveWorldSettingsChanged;

    public void Initialize(int difficultyID)
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _difficultyIndex = difficultyID;

        OnInitialized?.Invoke();
    }
}


