using System.Collections.Generic;
using UnityEngine;

namespace UI.WindowsManagement {
    public class WindowsManager : MonoBehaviour {

        [SerializeField] private WindowView _view;
        [SerializeField] private WindowModel _model;

        private Window _activeWindow;

        public Window ActiveWindow { get { return _activeWindow; } }

        private Stack<Window> _openedWindows;

        public enum OpenWindowMode {
            Single, Additive
        }

        private void Awake() {
            _model.Init();

            _model.home.Show();
            _activeWindow = _model.home;

            _openedWindows = new Stack<Window>();
            _openedWindows.Push(_model.home);
        }

        private void OpenWindow(Window window, OpenWindowMode openWindowMode = OpenWindowMode.Single) {
            if (_activeWindow == window || _activeWindow.IsModal) {
                return;
            }

            switch (openWindowMode) {
                case OpenWindowMode.Single: {

                        _openedWindows.Peek().Hide();

                        break;
                    }
                case OpenWindowMode.Additive: {

                        

                        break;
                    }
            }

            window.Show();

            _activeWindow = window;

            _openedWindows.Push(window);
        }

        public void OpenSettingsWindow() {
            OpenWindow(_model.settingsMain);
        }

        public void OpenSaveSelectionWindow() {
            OpenWindow(_model.saveSelection);
        }

        public void OpenPrevWindow() {
            if (_openedWindows.Peek() == _model.home || _openedWindows.Count < 2) {
                return;
            }

            _openedWindows.Pop().Hide();
            
            _openedWindows.Peek().Show();

            _activeWindow = _openedWindows.Peek();
        }
    }
}
