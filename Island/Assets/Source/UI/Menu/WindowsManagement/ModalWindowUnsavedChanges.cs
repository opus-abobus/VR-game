using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.WindowsManagement
{
    public class ModalWindowUnsavedChanges : MonoBehaviour
    {
        [SerializeField] public Button _saveBtn;
        [SerializeField] public Button _discardBtn;

        [SerializeField] private GameObject _windowObject;

        public event Action SaveButtonClicked, DiscardButtonClicked;

        private Window _window;
        public Window Window { get { return _window; } }

        public void Init()
        {
            _window = new Window(_windowObject, true);

            _saveBtn.onClick.AddListener(OnSaveButtonClicked);
            _discardBtn.onClick.AddListener(OnDiscardButtonClicked);
        }

        private void OnSaveButtonClicked()
        {
            SaveButtonClicked?.Invoke();
        }

        private void OnDiscardButtonClicked()
        {
            DiscardButtonClicked?.Invoke();
        }

        private void OnDestroy()
        {
            _saveBtn.onClick.RemoveAllListeners();
            _discardBtn.onClick.RemoveAllListeners();
        }
    }
}
