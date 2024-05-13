using DataPersistence;
using System;
using System.Collections;
using System.Collections.Generic;
using UI.WindowsManagement;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SettingsManagement {
    public class SettingsController : MonoBehaviour {
        [SerializeField] private SettingsDataModel _model;

        [SerializeField] private GraphicsView _graphicsView;
        [SerializeField] private SoundView _soundView;
        [SerializeField] private InputView _inputView;
        [SerializeField] private GameplayView _gameplayView;
        [SerializeField] private OtherView _otherView;

        [SerializeField] private GameObject _commandPanel;
        [SerializeField] private Button _discardButton, _applyButton;

        //public event Action SettingsUpdate;

        private void Awake() {
            Init();
        }

        private void Init() {
            _model.ViewUpdated += OnViewDataChanged;
            _model.Init(AppManager.Instance.DataManager.SettingsData);

            _model.SubscribeOnView();

            _applyButton.onClick.AddListener(OnApplyButtonClicked);
            _discardButton.onClick.AddListener(OnDiscardButtonClicked);
        }

        private void OnViewDataChanged() {
            _commandPanel.SetActive(true);
        }

        private void OnApplyButtonClicked() {
            _model.SaveChanges();

            _commandPanel.SetActive(false);
        }

        private void OnDiscardButtonClicked() {
            _model.DiscardChanges();

            _commandPanel.SetActive(false);
        }

        private void OnDestroy() {
            _model.UnsubscribeOnView();

            _applyButton?.onClick?.RemoveAllListeners();
            _discardButton?.onClick?.RemoveAllListeners();
        }
    }
}
