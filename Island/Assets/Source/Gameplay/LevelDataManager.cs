using DataPersistence;
using DataPersistence.Gameplay;
using System;
using System.Collections;
using UnityEngine;

public class LevelDataManager : MonoBehaviour
{
    [SerializeField] private GameplayBootstrap _boostrap;

    [SerializeField] private GameObject _textMessagePanel;
    [SerializeField] private float _saveCooldown;
    private bool _allowSave = true;

    public event Action<GameplayData> OnGameSave;

    private bool _autosaveAllowed, _saveOnExit;
    private int _autosaveIntervalInMin;

    [SerializeField] private PauseMenuController _pauseMenuController;
    [SerializeField] private GameObject _pauseCanvas;

    private void Awake()
    {
        if (CurrentSessionDataManager.Instance.IsNewGame)
        {
            //CurrentSessionDataManager.Instance.CurrentData.playerData = new PlayerData();
        }

        _boostrap.Init(CurrentSessionDataManager.Instance.CurrentData, CurrentSessionDataManager.Instance.IsNewGame);

        _autosaveAllowed = AppManager.Instance.DataManager.SettingsData.Autosave;
        _saveOnExit = AppManager.Instance.DataManager.SettingsData.SaveOnExit;
        _autosaveIntervalInMin = AppManager.Instance.DataManager.SettingsData.AutoSaveIntervalInMinutes;

        _pauseMenuController.GameplayExit += OnGameplayExit;
    }

    private void Start()
    {
        if (_autosaveAllowed)
            StartCoroutine(AutosaveProcess());
    }

    private void OnGameplayExit()
    {
        if (_saveOnExit)
        {
            _pauseCanvas.SetActive(false);

            SaveGame(": " + GameSettingsManager.Instance.ActiveWorldSettings.Difficulty + " - exitSave");
        }
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameStates.ACTIVE)
        {
            if (Input.GetKeyDown(AppManager.Instance.DataManager.SettingsData.QuickSaveKey))
            {
                if (_allowSave)
                    SaveGame(": " + GameSettingsManager.Instance.ActiveWorldSettings.Difficulty + " - quicksave");
            }
        }
    }

    private void SaveGame(string saveDescription)
    {
        OnGameSave?.Invoke(CurrentSessionDataManager.Instance.CurrentData);

        CurrentSessionDataManager.Instance.CurrentData.displayName = Environment.UserName + " " + DateTime.Now + " " + saveDescription;
        CurrentSessionDataManager.Instance.SaveCurrentDataOnDisk();

        _allowSave = false;
        StartCoroutine(SaveCooldown());
    }

    private IEnumerator SaveCooldown()
    {
        float timeLeft = _saveCooldown;

        yield return new WaitForEndOfFrame();

        _textMessagePanel.SetActive(true);

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;

            if (timeLeft < 0.15f * _saveCooldown)
            {
                _textMessagePanel.SetActive(false);
            }

            yield return null;
        }

        _allowSave = true;
    }

    private IEnumerator AutosaveProcess()
    {
        var wait = new WaitForSeconds(_autosaveIntervalInMin * 60);

        while (true)
        {
            yield return wait;

            if (_allowSave)
                SaveGame(": " + GameSettingsManager.Instance.ActiveWorldSettings.Difficulty + " - autosave");
        }
    }

    private void OnDestroy()
    {
        _pauseMenuController.GameplayExit -= OnGameplayExit;
    }
}
