using System.Collections.Generic;
using UnityEngine;

namespace UI.WindowsManagement
{
    public class WindowsManager : MonoBehaviour, IColleague
    {
        [SerializeField] private WindowView _view;
        [SerializeField] private WindowModel _model;

        private Window _activeWindow = null;

        public Window ActiveWindow { get { return _activeWindow; } }

        private Stack<Window> _openedWindows;

        public enum OpenWindowMode
        {
            Single, Additive
        }

        private void Awake()
        {
            _model.Init();

            _model.home.Show();
            _activeWindow = _model.home;

            _openedWindows = new Stack<Window>();
            _openedWindows.Push(_model.home);

            _view.modalUnsavedChanges.SaveButtonClicked += NotyfyMediator_OnSaveSettingsInModal;
            _view.modalUnsavedChanges.DiscardButtonClicked += NotyfyMediator_OnDiscardSettingsInModal;
        }

        public void OpenWindow(Window window, OpenWindowMode openWindowMode = OpenWindowMode.Single)
        {
            if (_activeWindow == window || _activeWindow.IsModal)
            {
                return;
            }

            switch (openWindowMode)
            {
                case OpenWindowMode.Single:
                    {
                        GoHome();
                        _model.home.Hide();

                        break;
                    }
                case OpenWindowMode.Additive:
                    {


                        break;
                    }
            }

            window.Show();
            _activeWindow = window;
            _openedWindows.Push(window);
        }

        public void OpenSettingsWindow()
        {
            OpenWindow(_model.settingsMain, OpenWindowMode.Single);
        }

        public void OpenSaveSelectionWindow()
        {
            OpenWindow(_model.saveSelection, OpenWindowMode.Single);
        }

        public void OpenUnsavedChangesInSettings()
        {
            OpenWindow(_view.modalUnsavedChanges.Window, OpenWindowMode.Additive);
        }
        
        public void OpenPrevWindow()
        {
            if (_activeWindow == _model.settingsMain)
            {
                _mediator.Notyfy(this, "CloseSettingsAttempt");
            }

            if (_activeWindow == _model.home || _activeWindow.IsModal)
            {
                return;
            }

            _openedWindows.Pop().Hide();

            if (_openedWindows.Count == 0)
            {
                _openedWindows.Push(_model.home);
            }
            _openedWindows.Peek().Show();

            _activeWindow = _openedWindows.Peek();
        }

        public void GoHome()
        {
            while (_openedWindows.Peek() != _model.home)
            {
                _openedWindows.Pop().Hide();
            }

            (_activeWindow = _openedWindows.Peek()).Show();
        }

        private IMediator _mediator;

        public void SetMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        private void NotyfyMediator_OnSaveSettingsInModal()
        {
            _mediator.Notyfy(this, "SaveSettingsDataInModal");
        }

        private void NotyfyMediator_OnDiscardSettingsInModal()
        {
            _mediator.Notyfy(this, "DiscardSettingsDataInModal");
        }

        private void OnDestroy()
        {
            _view.modalUnsavedChanges.SaveButtonClicked -= NotyfyMediator_OnSaveSettingsInModal;
            _view.modalUnsavedChanges.DiscardButtonClicked -= NotyfyMediator_OnDiscardSettingsInModal;
        }
    }
}
