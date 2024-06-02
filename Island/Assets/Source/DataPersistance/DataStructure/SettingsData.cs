using UnityEngine;

namespace DataPersistence
{
    [System.Serializable]
    public struct SettingsData
    {
        [HideInInspector, SaveField(FieldName.ScreenResolution)]
        public Resolution ScreenResolution;

        [HideInInspector, SaveField(FieldName.FullscreenMode)]
        public FullScreenMode fullScreenMode;

        //public List<GraphicsPreset> graphicsPresets;
        //public GraphicsPreset graphicsPreset;

        // Sound
        [SaveField(FieldName.TotalVolume), ConstraintField(0.0f, 1.0f)]
        public float TotalVolume;

        [SaveField(FieldName.MusicVolume), ConstraintField(0.0f, 1.0f)]
        public float MusicVolume;

        [SaveField(FieldName.PlayerStepsVolume), ConstraintField(0.0f, 1.0f)]
        public float PlayerStepsVolume;

        // Input
        [SaveField(FieldName.MouseSensX), ConstraintField(0.0f, 1.0f)]
        public float MouseSensitivityX;

        [SaveField(FieldName.MouseSensY), ConstraintField(0.0f, 1.0f)]
        public float MouseSensitivityY;

        // Key binds
        [SaveField(FieldName.QuickSaveKey)]
        public KeyCode QuickSaveKey;

        // Gameplay


        // Other
        [SaveField(FieldName.SkipIntro)]
        public bool SkipIntro;

        [SaveField(FieldName.FieldOfView), ConstraintField(50, 100)]
        public int FieldOfView;

        //[SerializeField] private List<string> languages;

        [SaveField(FieldName.AutoSave)]
        public bool Autosave;

        [SaveField(FieldName.SaveOnExit)]
        public bool SaveOnExit;

        [SaveField(FieldName.AutoSaveIntervalInMinutes), ConstraintField(1, 60)]
        public int AutoSaveIntervalInMinutes;
    }
}
