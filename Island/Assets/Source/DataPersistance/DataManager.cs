using DataPersistence.Gameplay;
using System.IO;
using UnityEngine;

namespace DataPersistence
{
    public class DataManager : MonoBehaviour
    {
        private const string
            GameSettingsFileName = "Game settings",
            GameplayFilesDirName = "/Saves/";

        private string _gameDataPath;

        private string _gameSettingsPath;

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

            _gameSettingsPath = Application.persistentDataPath + "/" + GameSettingsFileName + ".xml";
            _gameDataPath = Application.persistentDataPath + GameplayFilesDirName;

            _saveSystem = new XMLSaveSystem();

            InitGameSettings();
            InitGameplaySavesData();
        }

        public SettingsData SaveSettings(ref SettingsData settingsData)
        {
            _settingsData = settingsData;

            _saveSystem.Save(_settingsData, _gameSettingsPath);

            return _settingsData;
        }

        private void InitGameSettings()
        {
            if (!File.Exists(_gameSettingsPath))
            {
                File.Create(_gameSettingsPath).Close();

                SetDefaultSettingsData(ref _settingsDataDefault);
                Screen.fullScreenMode = _settingsDataDefault.fullScreenMode;

                _saveSystem.Save(_settingsDataDefault, _gameSettingsPath);

                _settingsData = _settingsDataDefault;
            }
            else
            {
                _settingsData = _saveSystem.Load<SettingsData>(_gameSettingsPath);
            }
        }

        private void InitGameplaySavesData()
        {
            if (!Directory.Exists(_gameDataPath))
            {
                Directory.CreateDirectory(_gameDataPath);
            }
        }

        public GameplayData[] GetSaves()
        {
            GameplayData[] result = new GameplayData[Directory.GetFiles(_gameDataPath, "*.xml").Length];
            if (result.Length == 0)
            {
                return null;
            }

            int i = 0;
            foreach (string filePath in Directory.GetFiles(_gameDataPath, "*.xml"))
            {
                result[i] = _saveSystem.Load<GameplayData>(filePath);
                i++;
            }

            return result;
        }

        public GameplayData GetSave(string saveFileName)
        {
            saveFileName = _gameDataPath + saveFileName + ".xml";

            foreach (string filePath in Directory.GetFiles(_gameDataPath, "*.xml"))
            {
                if (saveFileName.Equals(filePath))
                {
                    return _saveSystem.Load<GameplayData>(filePath);
                }
            }

            return null;
        }

        public GameplayData GetLastSave()
        {
            string[] files = Directory.GetFiles(_gameDataPath, "*.xml");
            if (files.Length == 0)
            {
                return null;
            }

            string lastSavePath = files[0];
            for (int i = 1; i < files.Length; i++)
            {
                if (File.GetCreationTime(files[i]) > File.GetCreationTime(lastSavePath))
                {
                    lastSavePath = files[i];
                }
            }

            return _saveSystem.Load<GameplayData>(lastSavePath);
        }

        public void WriteSave(GameplayData gameplayData, string saveName)
        {
            _saveSystem.Save(gameplayData, _gameDataPath + saveName + ".xml");
        }
    }
}
