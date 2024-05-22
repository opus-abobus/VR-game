using UI.Navigation.Tabs;
using UI.SettingsManagement;
using UI.WindowsManagement;
using UnityEngine;

namespace UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private HomeButtonsController _homeButtonsController;

        [SerializeField] private TabController _settingsNavigationController;

        [SerializeField] private WindowsManager _windowsManager;

        [SerializeField] private SettingsController _settingsController;

        private SettingsWindowMediator _settingsWindowMediator;

        private void Awake()
        {
            _settingsWindowMediator = new SettingsWindowMediator(_settingsController, _windowsManager);

            InitHomeButtons();

            _settingsNavigationController.Init();

            _windowsManager.enabled = true;
            _windowsManager.GoHome();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _windowsManager.OpenPrevWindow();
            }
        }

        private void InitHomeButtons()
        {
            _homeButtonsController.OnQuitPress += OnQuitClicked;

            _homeButtonsController.OnSettingsWindow += OnSettingsClicked;

            _homeButtonsController.OnLoadLastSavePress += OnLoadLastSaveClicked;

            _homeButtonsController.OnSaveSelectionWindow += OnSelectSaveClicked;

            _homeButtonsController.OnStartNewGamePress += OnNewGameClicked;

            _homeButtonsController.Init();
        }

        private void OnDestroy()
        {
            _homeButtonsController.OnQuitPress -= OnQuitClicked;

            _homeButtonsController.OnSettingsWindow -= OnSettingsClicked;

            _homeButtonsController.OnLoadLastSavePress -= OnLoadLastSaveClicked;

            _homeButtonsController.OnSaveSelectionWindow -= OnSelectSaveClicked;

            _homeButtonsController.OnStartNewGamePress -= OnNewGameClicked;
        }

        private void OnNewGameClicked()
        {
            //AppManager.Instance.LoadLevel();
            _windowsManager.OpenNewGameWindow();
        }

        private void OnQuitClicked()
        {
            AppManager.Instance.ExitApp();
        }

        private void OnSettingsClicked()
        {
            _windowsManager.OpenSettingsWindow();
        }

        private void OnLoadLastSaveClicked()
        {
            
        }

        private void OnSelectSaveClicked()
        {
            _windowsManager.OpenSaveSelectionWindow();
        }
    }
}
