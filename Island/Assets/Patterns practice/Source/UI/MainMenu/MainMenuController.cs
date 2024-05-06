using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
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

    private void Awake() {
        _quit.onClick.AddListener(OnQuitClicked);
        _settings.onClick.AddListener(OnSettingsClicked);
        _loadLastSave.onClick.AddListener(OnLoadLastSaveClicked);
        _selectSaveToLoad.onClick.AddListener(OnSelectSaveClicked);
        _startNewGame.onClick.AddListener(OnNewGameClicked);
    }

    private void OnDestroy() {
        _quit.onClick.RemoveAllListeners();
        _settings.onClick.RemoveAllListeners();
        _loadLastSave.onClick.RemoveAllListeners();
        _selectSaveToLoad.onClick.RemoveAllListeners();
        _startNewGame.onClick.RemoveAllListeners();
    }

    private void OnNewGameClicked() {
        AppManager.Instance.LoadLevel();
    }

    private void OnQuitClicked() {
        AppManager.Instance.ExitApp();
    }

    private void OnSettingsClicked() {

    }

    private void OnLoadLastSaveClicked() {

    }

    private void OnSelectSaveClicked() {

    }
}
