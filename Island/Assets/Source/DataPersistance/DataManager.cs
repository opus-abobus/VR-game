using System.IO;
using UnityEngine;

namespace DataPersistence
{
    public class DataManager : MonoBehaviour
    {
        private const string
                _gameSettingsFileName = "Game settings",
                _gameplayFilename = "Gameplay data";

        private string _gameDataPath;

        private string _formattedString;

        private SettingsData _settingsDataDefault;
        private SettingsData _settingsData;

        private ISaveSystem _saveSystem;

        public SettingsData SettingsData { get { return _settingsData; } }

        private static void SetDefaultSettingsData(ref SettingsData settingsData)
        {
            // Graphics
            settingsData.ScreenResolution = Screen.currentResolution;

            settingsData.fullScreenMode = FullScreenMode.FullScreenWindow;

            // Sound
            settingsData.TotalVolume = 1;
            settingsData.MusicVolume = 1;
            settingsData.PlayerStepsVolume = 1;

            //Input
            settingsData.MouseSensitivityX = 1;
            settingsData.MouseSensitivityY = 1;
            // Key binds
            settingsData.QuickSaveKey = KeyCode.F5;

            // Gameplay


            // Other
            settingsData.SkipIntro = false;

            settingsData.FieldOfView = 60;

            settingsData.SaveOnExit = true;

            settingsData.Autosave = true;
            settingsData.AutoSaveIntervalInMinutes = 5;
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);

            _gameDataPath = Application.persistentDataPath;
            _formattedString = _gameDataPath + "/" + _gameSettingsFileName + ".xml";

            _saveSystem = new XMLSaveSystem();

            if (!File.Exists(_formattedString))
            {
                File.Create(_formattedString).Close();

                SetDefaultSettingsData(ref _settingsDataDefault);
                Screen.fullScreenMode = _settingsDataDefault.fullScreenMode;

                _saveSystem.Save(_settingsDataDefault, _formattedString);

                _settingsData = _settingsDataDefault;
            }
            else
            {
                _settingsData = _saveSystem.Load<SettingsData>(_formattedString);
            }
        }

        public SettingsData SaveSettings(ref SettingsData settingsData)
        {
            _settingsData = settingsData;

            _saveSystem.Save(_settingsData, _formattedString);

            return _settingsData;
        }
    }
}
