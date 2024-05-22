namespace DataPersistence
{
    using Category = SettingFieldCategoryAttribute.SettingsCategory;

    public enum FieldName
    {
        [SettingFieldCategory(Category.None)]
        UNDEFINED = 0,

        [SettingFieldCategory(Category.Graphics)]
        ScreenResolution = 1,

        [SettingFieldCategory(Category.Graphics)]
        FullscreenMode = 2,

        [SettingFieldCategory(Category.Graphics)]
        GraphicsPreset = 3,

        // Sound
        [SettingFieldCategory(Category.Sound)]
        TotalVolume = 4,

        [SettingFieldCategory(Category.Sound)]
        MusicVolume = 5,

        [SettingFieldCategory(Category.Sound)]
        PlayerStepsVolume = 6,
        //

        // Input
        [SettingFieldCategory(Category.Input)]
        MouseSensX = 7,

        [SettingFieldCategory(Category.Input)]
        MouseSensY = 8,
        //
        // Input key binds
        [SettingFieldCategory(Category.Input)]
        QuickSaveKey = 9,
        //

        // Other
        [SettingFieldCategory(Category.Other)]
        SkipIntro = 10,

        [SettingFieldCategory(Category.Other)]
        FieldOfView = 11,

        [SettingFieldCategory(Category.Other)]
        AutoSave = 12,

        [SettingFieldCategory(Category.Other)]
        SaveOnExit = 13,

        [SettingFieldCategory(Category.Other)]
        AutoSaveIntervalInMinutes = 14,
        //
    }
}
