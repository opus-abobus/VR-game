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

        [SerializeField] private SettingsView _view;

        [SerializeField] private GameObject _commandPanel;
        [SerializeField] private Button _discardButton, _applyButton;

        private List<BaseFieldView> _unsavedViews;

        private void Awake() {
            Init();
        }

        private void Init() {
            _model.Init(AppManager.Instance.DataManager.SettingsData);

            _unsavedViews = new List<BaseFieldView>();

            _view.FieldViewUpdated += OnViewUpdated;
            _view.AddListeners();

            _applyButton.onClick.AddListener(OnApplyButtonClicked);
            _discardButton.onClick.AddListener(OnDiscardButtonClicked);
        }

        private void OnViewUpdated<T>(T fieldView) where T : BaseFieldView {

            if (!_unsavedViews.Contains(fieldView)) {
                _unsavedViews.Add(fieldView);
            }

            _model.UpdateViewText(fieldView);

            _commandPanel.SetActive(true);
        }

        private void OnApplyButtonClicked() {
            _model.SaveChanges(_unsavedViews);

            _commandPanel.SetActive(false);
        }

        private void OnDiscardButtonClicked() {
            _model.DiscardChanges(_unsavedViews);

            _commandPanel.SetActive(false);
        }

        private void OnDestroy() {
            _view?.RemoveListeners();

            _applyButton?.onClick.RemoveAllListeners();
            _discardButton?.onClick.RemoveAllListeners();
        }
    }
}
