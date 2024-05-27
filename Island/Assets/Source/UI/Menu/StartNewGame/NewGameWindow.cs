using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static WorldSettingsList;

namespace UI.WindowsManagement
{
    public class NewGameWindow : MonoBehaviour
    {
        [SerializeField] private Button _start;
        [SerializeField] private Button _back;

        [SerializeField] private TMP_Dropdown _difficultyDropdown;

        [SerializeField] private GameObject _windowObject;

        [SerializeField] private WorldSettingsList _difficultiesMap;

        private Window _window;
        public Window Window { get { return _window; } }

        public event Action<int> StartNewGameButtonClicked;
        public event Action BackButtonClicked;

        public void Init()
        {
            _window = new Window(_windowObject, false);

            _start.onClick.AddListener(OnNewGameButtonClicked);
            _back.onClick.AddListener(OnBackButtonClicked);
            
            InitDifficultiesDropdown();
        }

        private void OnNewGameButtonClicked()
        {
            StartNewGameButtonClicked?.Invoke(_difficultyDropdown.value);

            AppManager.Instance.LoadLevel();
        }

        private void OnBackButtonClicked()
        {
            BackButtonClicked?.Invoke();

            _difficultyDropdown.value = 0;
        }

        private void InitDifficultiesDropdown()
        {
            _difficultyDropdown.ClearOptions();

            foreach (WorldSettingsEntry entry in _difficultiesMap.entries)
            {
                _difficultyDropdown.options.Add(new TMP_Dropdown.OptionData(entry.WorldSettings.Difficulty.ToString(), null));
            }
        }
    }
}
