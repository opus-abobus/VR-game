using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SettingsManagement
{
    public class SettingsController : MonoBehaviour, IColleague
    {
        [SerializeField] private SettingsDataModel _model;

        [SerializeField] private SettingsView _view;

        [SerializeField] private GameObject _commandPanel;
        [SerializeField] private Button _discardButton, _applyButton;

        private List<BaseFieldView> _unsavedFields;
        public bool HasUnsavedData { get; private set; } = false;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            _model.Init(AppManager.Instance.DataManager.SettingsData);

            _unsavedFields = new List<BaseFieldView>();

            _view.FieldViewUpdated += OnViewUpdated;
            _view.AddListeners();

            _applyButton.onClick.AddListener(OnApplyButtonClicked);
            _discardButton.onClick.AddListener(OnDiscardButtonClicked);
        }

        public void ApplySettings()
        {
            for (int i = 0; i < _unsavedFields.Count; i++)
                ShowUnsavedBorder(_unsavedFields[i], false);

            _model.SaveChanges(_unsavedFields);

            HasUnsavedData = false;
        }

        public void DiscardSettings()
        {
            for (int i = 0; i < _unsavedFields.Count; i++)
                ShowUnsavedBorder(_unsavedFields[i], false);

            _model.DiscardChanges(_unsavedFields);

            HasUnsavedData = false;
        }

        public void ShowCommandPanel(bool show)
        {
            if (show)
                _commandPanel.SetActive(true);
            else
                _commandPanel.SetActive(false);
        }

        private void OnViewUpdated<T>(T fieldView) where T : BaseFieldView
        {
            if (!_unsavedFields.Contains(fieldView) && _model.IsValueChanged(fieldView))
            {
                _unsavedFields.Add(fieldView);
                ShowUnsavedBorder(fieldView, true);
            }
            else if (_unsavedFields.Contains(fieldView) && !_model.IsValueChanged(fieldView))
            {
                _unsavedFields.Remove(fieldView);
                ShowUnsavedBorder(fieldView, false);
            }

            _model.UpdateViewText(fieldView);

            if (_unsavedFields.Count > 0)
            {
                _commandPanel.SetActive(true);
                HasUnsavedData = true;
            }
            else
            {
                _commandPanel.SetActive(false);
                HasUnsavedData = false;
            }
        }

        private void ShowUnsavedBorder(BaseFieldView fieldView, bool show)
        {
            if (fieldView is DropdownFieldView dropdownField)
                dropdownField.unsavedBorder.SetActive(show);
            else if (fieldView is SliderFieldView sliderField)
                sliderField.unsavedBorder.SetActive(show);
            else if (fieldView is KeyBindFieldView keyBindField)
                keyBindField.unsavedBorder.SetActive(show);
            else if (fieldView is ToggleFieldView toggleView)
                toggleView.unsavedBorder.SetActive(show);
        }

        private void OnApplyButtonClicked()
        {
            _commandPanel.SetActive(false);

            ApplySettings();
        }

        private void OnDiscardButtonClicked()
        {
            _commandPanel.SetActive(false);

            DiscardSettings();
        }

        private void OnDestroy()
        {
            _view?.RemoveListeners();

            _applyButton?.onClick.RemoveAllListeners();
            _discardButton?.onClick.RemoveAllListeners();
        }

        private IMediator _mediator;

        public void SetMediator(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}