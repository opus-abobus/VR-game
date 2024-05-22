using UnityEngine;
using UnityEngine.UI;
using AppManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField]
    private Button _resumeGame;

    [SerializeField]
    private Button _settingsButton;

    [SerializeField]
    private Button _returnToMainMenu;

    [SerializeField]
    private Button _exitApp;

    [SerializeField]
    private GameObject _menuObject;

    private void Awake() {
        AppEventBus.Instance.OnGamePaused += OnGamePaused;
        AppEventBus.Instance.OnGameplay += OnGameplay;

        _resumeGame.onClick.AddListener(OnResumeGameClicked);
        _settingsButton.onClick.AddListener(OnSettingsClicked);
        _returnToMainMenu.onClick.AddListener(OnReturnToMainMenuClicked);
        _exitApp.onClick.AddListener(OnExitAppClicked);
    }

    private void OnReturnToMainMenuClicked() {
        //AppManager.Instance.LoadMainMenu();
    }

    private void OnExitAppClicked() {
        AppManager.Instance.ExitApp();
    }

    private void OnResumeGameClicked() {
        //_menuObject.SetActive(false);

        //AppManager.Instance.ResumeGame();
    }

    private void OnSettingsClicked() {

    }

    private void OnGamePaused() {
        //_menuObject.SetActive(true);
    }

    private void OnGameplay() {
        //_menuObject.SetActive(false);
    }

    private void OnDestroy() {
        AppEventBus.Instance.OnGamePaused -= OnGamePaused;
        AppEventBus.Instance.OnGameplay -= OnGameplay;

        _resumeGame.onClick.RemoveAllListeners();
        _settingsButton.onClick.RemoveAllListeners();
        _exitApp.onClick.RemoveAllListeners();
        _returnToMainMenu.onClick.RemoveAllListeners();
    }
}
