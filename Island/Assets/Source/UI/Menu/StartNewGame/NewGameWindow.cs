using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.WindowsManagement
{
    public class NewGameWindow : MonoBehaviour
    {
        [SerializeField] private Button _start;
        [SerializeField] private Button _back;

        [SerializeField] private TMP_Dropdown _difficultyDropdown;

        [SerializeField] private GameObject _windowObject;

        //[SerializeField] private Sett

        private Window _window;
        public Window Window { get { return _window; } }

        public event Action StartNewGameButtonClicked, BackButtonClicked;

        public void Init()
        {
            _window = new Window(_windowObject, false);

            _start.onClick.AddListener(OnNewGameButtonClicked);
            _back.onClick.AddListener(OnBackButtonClicked);

            _difficultyDropdown.ClearOptions();
        }

        private void OnNewGameButtonClicked()
        {
            StartNewGameButtonClicked?.Invoke();
        }

        private void OnBackButtonClicked()
        {
            BackButtonClicked?.Invoke();
        }
    }
}
