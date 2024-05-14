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

        [HideInInspector, SaveField(FieldName.FullscreenMode)] 
        public FullScreenMode fullScreenMode;

        //public List<GraphicsPreset> graphicsPresets;
        //public GraphicsPreset graphicsPreset;

        // Sound
        [SaveField(FieldName.TotalVolume), ConstraintField(0.0f, 1.0f)]
        public float totalVolume;

        [SaveField(FieldName.MusicVolume), ConstraintField(0.0f, 1.0f)]
        public float musicVolume;

        [SaveField(FieldName.PlayerStepsVolume), ConstraintField(0.0f, 1.0f)]
        public float playerStepsVolume;

        // Input
        [SaveField(FieldName.MouseSensX), ConstraintField(0.0f, 1.0f)]
        public float mouseSensitivityX;

        [SaveField(FieldName.MouseSensY), ConstraintField(0.0f, 1.0f)]
        public float mouseSensitivityY;

        // Key binds
        [SaveField(FieldName.QuickSave)]
        public KeyCode quickSave;

        // Gameplay


        // Other
        [SaveField(FieldName.SkipIntro)]
        public bool skipIntro;

        [SaveField(FieldName.FieldOfView), ConstraintField(50, 300)]
        public int fieldOfView;

        //[SerializeField] private List<string> languages;

        [SaveField(FieldName.AutoSave)]
        public bool autosave;
        
        [SaveField(FieldName.SaveOnExit)]
        public bool saveOnExit;

        [SaveField(FieldName.AutoSaveIntervalInMinutes), ConstraintField(1, 60)]
        public int autoSaveIntervalInMinutes;
    }
}
