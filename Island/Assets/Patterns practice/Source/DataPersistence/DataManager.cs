using DataPersistence;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace DataPersistence {
    public class DataManager : MonoBehaviour {
        private const string
                _gameSettingsFileName = "Game settings",
                _gameplayFilename = "Gameplay data";

        private string _gameDataPath;

        private string _formattedString;

        private SettingsData _settingsDataDefault;
        private SettingsData _settingsData;

        private ISaveSystem _saveSystem;

        public SettingsData SettingsData { get { return _settingsData; } }

        private static void SetDefaultSettingsData(ref SettingsData settingsData) {

            // Graphics
            settingsData.screenResolution.width = Screen.width;
            settingsData.screenResolution.height = Screen.height;
            settingsData.screenResolution.refreshRateRatio = Screen.currentResolution.refreshRateRatio;

            settingsData.screenResolutions = Screen.resolutions.ToList();

            settingsData.fullScreenMode = Screen.fullScreenMode;

            // Sound
            settingsData.totalVolume = 1;
            settingsData.musicVolume = 1;
            settingsData.playerStepsVolume = 1;

            //Input
            settingsData.mouseSensitivityX = 1;
            settingsData.mouseSensitivityY = 1;
            // Key binds
            settingsData.quickSave = KeyCode.F5;

            // Gameplay


            // Other
            settingsData.skipIntro = false;

            settingsData.fieldOfView = 60;

            settingsData.saveOnExit = true;

            settingsData.autosave = true;
            settingsData.autoSaveIntervalInMinutes = 7;
        }

        private void Awake() {
            DontDestroyOnLoad(this);

            _gameDataPath = Application.persistentDataPath;
            _formattedString = _gameDataPath + "/" + _gameSettingsFileName + ".xml";

            _saveSystem = new XMLSaveSystem();

            if (!File.Exists(_formattedString)) {
                File.Create(_formattedString).Close();

                SetDefaultSettingsData(ref _settingsDataDefault);

                _saveSystem.Save(_settingsDataDefault, _formattedString);

                _settingsData = _settingsDataDefault;
            }
            else {
                _settingsData = _saveSystem.Load<SettingsData>(_formattedString);
            }

        }

        public SettingsData SaveSettings(ref SettingsData settingsData) {
            _settingsData = settingsData;

            _saveSystem.Save(_settingsData, _formattedString);

            return _settingsData;
        }
    }
}
