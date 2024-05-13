using System.Collections.Generic;
using UnityEngine;

namespace DataPersistence {
    [System.Serializable]
    public struct SettingsData {
        // Graphics
        [HideInInspector] public List<Resolution> screenResolutions;
        [HideInInspector] public Resolution screenResolution;

        //[SerializeField] private List<object> screenRatios;
        //public List<DisplayMode> displayModes;
        //public DisplayMode displayMode;
        [HideInInspector] public FullScreenMode fullScreenMode;

        //public List<GraphicsPreset> graphicsPresets;
        //public GraphicsPreset graphicsPreset;

        // Sound
        [Range(0, 1)] public float totalVolume;
        [Range(0, 1)] public float musicVolume;
        [Range(0, 1)] public float playerStepsVolume;

        // Input
        [Range(0, 1)] public float mouseSensitivityX;
        [Range(0, 1)] public float mouseSensitivityY;
        // Key binds
        public KeyCode quickSave;

        // Gameplay
        //public GraphicsPresetData graphicsPresetData;

        // Other
        public bool skipIntro;

        public int fieldOfView;
        public int fieldOfView_MIN, fieldOfView_MAX;

        //[SerializeField] private List<string> languages;
        public bool autosave;
        public bool saveOnExit;

        public int autoSaveIntervalInMinutes;
        public int autoSaveIntervalInMinutes_MIN, autoSaveIntervalInMinutes_MAX;
    }
}
