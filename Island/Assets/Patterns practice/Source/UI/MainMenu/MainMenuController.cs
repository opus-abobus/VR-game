using UI.WindowsManagement;
using UI.Navigation.Tabs;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private HomeButtonsController _homeButtonsController;

    [SerializeField] private TabController _settingsNavigationController;

    [SerializeField] private WindowsManager _windowsManager;

    private void Awake() {
        InitHomeButtons();

        _settingsNavigationController.Init();

        _windowsManager.enabled = true;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            _windowsManager.OpenPrevWindow();
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
        _windowsManager.OpenSettingsWindow();
    }

    private void OnLoadLastSaveClicked() {
        _windowsManager.OpenSaveSelectionWindow();
    }

    private void OnSelectSaveClicked() {
        _windowsManager.OpenSaveSelectionWindow();
    }
}
