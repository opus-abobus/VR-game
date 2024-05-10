using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject _settingsWindow, _saveSelectionWindow;

    [SerializeField, Header("Set this as start window")]
    private GameObject _homeWindow;

    private GameObject _activeWindow;

    [SerializeField]
    private HomeButtonsController _homeButtonsController;

    [SerializeField]
    private TabController _settingsController;

    private void Awake() {
        _homeWindow.SetActive(true);
        _activeWindow = _homeWindow;

        InitHomeButtons();

        _settingsController.Init();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (_activeWindow == _settingsWindow) {
                _settingsWindow.SetActive(false); // not really correct
                _homeWindow.SetActive(true);
                _activeWindow = _homeWindow;
            }
            else if (_activeWindow == _saveSelectionWindow) {
                _saveSelectionWindow.SetActive(false);
                _homeWindow.SetActive(true);
                _activeWindow = _homeWindow;
            }
        }
    }

    private void InitHomeButtons() {
        _homeButtonsController.OnQuitPress += OnQuitClicked;

        _homeButtonsController.OnSettingsWindow += OnSettingsClicked;

        _homeButtonsController.OnLoadLastSavePress += OnLoadLastSaveClicked;

        _homeButtonsController.OnSaveSelectionWindow += OnSelectSaveClicked;

        _homeButtonsController.OnStartNewGamePress += OnNewGameClicked;

        _homeButtonsController.Init();
    }

    private void OnDestroy() {
        _homeButtonsController.OnQuitPress -= OnQuitClicked;

        _homeButtonsController.OnSettingsWindow -= OnSettingsClicked;

        _homeButtonsController.OnLoadLastSavePress -= OnLoadLastSaveClicked;

        _homeButtonsController.OnSaveSelectionWindow -= OnSelectSaveClicked;

        _homeButtonsController.OnStartNewGamePress -= OnNewGameClicked;
    }

    private void OnNewGameClicked() {
        AppManager.Instance.LoadLevel();
    }

    private void OnQuitClicked() {
        AppManager.Instance.ExitApp();
    }

    private void OnSettingsClicked() {
        _settingsController.Init();

        _activeWindow.SetActive(false);

        _settingsWindow.SetActive(true);

        _activeWindow = _settingsWindow;
    }

    private void OnLoadLastSaveClicked() {

    }

    private void OnSelectSaveClicked() {
        _activeWindow.SetActive(false);
        _saveSelectionWindow.SetActive(true);
        _activeWindow = _saveSelectionWindow;
    }
}
