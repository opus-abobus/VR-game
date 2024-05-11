using UnityEngine;

namespace UI.WindowsManagement {
    public class Window {

        private GameObject _windowObject;

        private bool _isModal;
        public bool IsModal { get { return _isModal; } }

        private Window() { }

        public Window(GameObject windowObject, bool isModal = false) {
            _windowObject = windowObject;
            _isModal = isModal;
        }

        public void Show() {
            _windowObject.SetActive(true);
        }

        public void Hide() {
            _windowObject.SetActive(false);
        }
    }
}
