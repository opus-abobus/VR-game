using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataPersistence;
using System.Linq;
using TMPro;
using System;

namespace UI.SettingsManagement {
    public class SettingsDataModel : MonoBehaviour {

        [SerializeField] private GraphicsView _graphicsView;
        [SerializeField] private SoundView _soundView;
        [SerializeField] private InputView _inputView;
        [SerializeField] private GameplayView _gameplayView;
        [SerializeField] private OtherView _otherView;

        private SettingsData _settingsData;

        public event Action ViewUpdated;

        public void Init(SettingsData settingsData) {
            _settingsData = settingsData;

            UpdateView();
        }

        public void OnDataChanged<T>(T value) {
            ViewUpdated?.Invoke();
        }

        public void SaveChanges() {
            SaveGraphics();
            SaveSound();
            SaveInput();
            SaveGameplay();
            SaveOther();

            WriteDataOnDisk();
            UpdateView();
        }

        public void DiscardChanges() {
            UpdateView();
        }

        private void WriteDataOnDisk() {
            _settingsData = AppManager.Instance.DataManager.SaveSettings(ref _settingsData);
        }

        public void SubscribeOnView() {
            _graphicsView.displayMode.dropdown.onValueChanged.AddListener(OnDataChanged);

            _soundView.totalVolume.slider.onValueChanged.AddListener(OnDataChanged);
            _soundView.musicVolume.slider.onValueChanged.AddListener(OnDataChanged);
            _soundView.playersStepsVolume.slider.onValueChanged.AddListener(OnDataChanged);

            _inputView.mouseSensitivityX.slider.onValueChanged.AddListener(OnDataChanged);
            _inputView.mouseSensitivityY.slider.onValueChanged.AddListener(OnDataChanged);
            _inputView.saveGame.inputField.onValueChanged.AddListener(OnDataChanged);

            _otherView.skipIntroVideo.toggle.onValueChanged.AddListener(OnDataChanged);
            _otherView.autosave.toggle.onValueChanged.AddListener(OnDataChanged);
            _otherView.saveOnExit.toggle.onValueChanged.AddListener(OnDataChanged);
            _otherView.autosaveInterval.slider.onValueChanged.AddListener(OnDataChanged);
            _otherView.fieldOfView.slider.onValueChanged.AddListener(OnDataChanged);
        }

        public void UnsubscribeOnView() {
            _graphicsView.displayMode.dropdown.onValueChanged.RemoveAllListeners();

            _soundView.totalVolume.slider.onValueChanged.RemoveAllListeners();
            _soundView.musicVolume.slider.onValueChanged.RemoveAllListeners();
            _soundView.playersStepsVolume.slider.onValueChanged.RemoveAllListeners();

            _inputView.mouseSensitivityX.slider.onValueChanged.RemoveAllListeners();
            _inputView.mouseSensitivityY.slider.onValueChanged.RemoveAllListeners();
            _inputView.saveGame.inputField.onValueChanged.RemoveAllListeners();

            _otherView.skipIntroVideo.toggle.onValueChanged.RemoveAllListeners();
            _otherView.autosave.toggle.onValueChanged.RemoveAllListeners();
            _otherView.saveOnExit.toggle.onValueChanged.RemoveAllListeners();
            _otherView.autosaveInterval.slider.onValueChanged.RemoveAllListeners();
            _otherView.fieldOfView.slider.onValueChanged.RemoveAllListeners();
        }

        private void UpdateView() {
            SetGraphics();
            SetSound();
            SetInput();
            SetGameplay();
            SetOther();
        }

        private void SaveGraphics() {

        }

        private void SetGraphics() {
            _graphicsView.displayMode.dropdown.options.Clear();

            //_graphicsView.displayMode.dropdown.options = _settingsData.screenResolutions;

            //_graphicsView.graphicsPreset.dropdown.options.Clear();

            //_graphicsView.screenRatio.dropdown.options.Clear();

            //_graphicsView.screenRes.dropdown.options.Clear();

            
        }

        private void SaveSound() {
            _settingsData.totalVolume = _soundView.totalVolume.slider.value;

            _settingsData.musicVolume = _soundView.musicVolume.slider.value;
            
            _settingsData.playerStepsVolume = _soundView.playersStepsVolume.slider.value;
        }

        private void SetSound() {
            _soundView.totalVolume.slider.value = _settingsData.totalVolume;
            _soundView.totalVolume.sliderValueText.text = string.Empty;

            _soundView.musicVolume.slider.value = _settingsData.musicVolume;
            _soundView.musicVolume.sliderValueText.text = string.Empty;

            _soundView.playersStepsVolume.slider.value = _settingsData.playerStepsVolume;
            _soundView.playersStepsVolume.sliderValueText.text = string.Empty;
        }

        private void SaveInput() {
            _settingsData.mouseSensitivityX = _inputView.mouseSensitivityX.slider.value;

            _settingsData.mouseSensitivityY = _inputView.mouseSensitivityY.slider.value;

            _settingsData.quickSave = (KeyCode) Enum.Parse(typeof(KeyCode), _inputView.saveGame.inputField.text);
        }

        private void SetInput() {
            _inputView.mouseSensitivityX.slider.value = _settingsData.mouseSensitivityX;
            _inputView.mouseSensitivityX.sliderValueText.text = _settingsData.mouseSensitivityX.ToString();

            _inputView.mouseSensitivityY.slider.value = _settingsData.mouseSensitivityY;
            _inputView.mouseSensitivityY.sliderValueText.text = _settingsData.mouseSensitivityY.ToString();

            _inputView.saveGame.inputField.text = _settingsData.quickSave.ToString();
        }

        private void SaveGameplay() {

        }

        private void SetGameplay() {
            //_gameplayView.difficultyPreset.dropdown.options.Clear();
        }

        private void SaveOther() {
            _settingsData.autosave = _otherView.autosave.toggle.isOn;
            _settingsData.saveOnExit = _otherView.saveOnExit.toggle.isOn;

            _settingsData.skipIntro = _otherView.skipIntroVideo.toggle.isOn;

            _settingsData.autoSaveIntervalInMinutes = (int) _otherView.autosaveInterval.slider.value;

            _settingsData.fieldOfView = (int) _otherView.fieldOfView.slider.value;
        }

        private void SetOther() {
            _otherView.autosave.toggle.isOn = _settingsData.autosave;
            _otherView.autosave.toggleText.text = string.Empty;

            _otherView.saveOnExit.toggle.isOn = _settingsData.saveOnExit;
            _otherView.saveOnExit.toggleText.text = string.Empty;

            _otherView.skipIntroVideo.toggle.isOn = _settingsData.skipIntro;
            _otherView.skipIntroVideo.toggleText.text = string.Empty;

            //_otherView.UILanguage

            _otherView.autosaveInterval.slider.maxValue = _settingsData.autoSaveIntervalInMinutes_MAX;
            _otherView.autosaveInterval.slider.minValue = _settingsData.autoSaveIntervalInMinutes_MIN;
            //_otherView.autosaveInterval.slider.wholeNumbers = true;
            _otherView.autosaveInterval.slider.value = _settingsData.autoSaveIntervalInMinutes;
            _otherView.autosaveInterval.sliderValueText.text = _settingsData.autoSaveIntervalInMinutes.ToString();

            _otherView.fieldOfView.slider.minValue = _settingsData.fieldOfView_MIN;
            _otherView.fieldOfView.slider.maxValue = _settingsData.fieldOfView_MAX;
            _otherView.fieldOfView.slider.value = _settingsData.fieldOfView;
            _otherView.fieldOfView.sliderValueText.text = _settingsData.fieldOfView.ToString();
        }

        private void OnDestroy() {
            //UnsubscribeOnView();
        }
    }
}
