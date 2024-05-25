using System;
using UnityEngine;
using UnityEngine.UI;

public class HomeButtonsController : MonoBehaviour
{
    [SerializeField]
    private Button _loadLastSave;

    [SerializeField]
    private Button _selectSaveToLoad;

    [SerializeField]
    private Button _startNewGame;

    [SerializeField]
    private Button _settings;

    [SerializeField]
    private Button _quit;

    public event Action OnSettingsWindow, OnLoadLastSavePress, OnSaveSelectionWindow, OnStartNewGamePress, OnQuitPress;

    public void Init() {
        _loadLastSave.onClick.AddListener(OnLoadLastSaveButton_Clicked);

        _settings.onClick.AddListener(OnSettingsButton_Clicked);

        _quit.onClick.AddListener(OnQuitButton_Clicked);

        _startNewGame.onClick.AddListener(OnStartNewGameButton_Clicked);

        _selectSaveToLoad.onClick.AddListener(OnSelectSaveButton_Clicked);

        if (AppManager.Instance.DataManager.GetLastSave() == null)
        {
            _loadLastSave.gameObject.SetActive(false);
            _selectSaveToLoad.gameObject.SetActive(false);
        }
    }

    private void OnSettingsButton_Clicked() {
        OnSettingsWindow?.Invoke();
    }

    private void OnLoadLastSaveButton_Clicked() {
        OnLoadLastSavePress?.Invoke();
    }

    private void OnQuitButton_Clicked() {
        OnQuitPress?.Invoke();
    }

    private void OnStartNewGameButton_Clicked() {
        OnStartNewGamePress?.Invoke();
    }

    private void OnSelectSaveButton_Clicked() {
        OnSaveSelectionWindow?.Invoke();
    }

    private void OnDestroy() {
        _loadLastSave.onClick.RemoveAllListeners();

        _selectSaveToLoad.onClick.RemoveAllListeners();

        _startNewGame.onClick.RemoveAllListeners();

        _settings.onClick.RemoveAllListeners();

        _quit.onClick.RemoveAllListeners();
    }
}
