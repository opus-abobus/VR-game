namespace DataPersistence
{
    using Category = SettingFieldCategoryAttribute.SettingsCategory;

    public enum FieldName
    {
        [SettingFieldCategory(Category.None)]
        UNDEFINED,

/*        [SettingFieldCategory(Category.Graphics)]
        SupportedScreenResolutions,*/

        [SettingFieldCategory(Category.Graphics)]
        ScreenResolution,

        [SettingFieldCategory(Category.Graphics)]
        FullscreenMode,

        [SettingFieldCategory(Category.Graphics)]
        GraphicsPreset,

        // Sound
        [SettingFieldCategory(Category.Sound)]
        TotalVolume,

        [SettingFieldCategory(Category.Sound)]
        MusicVolume,

        [SettingFieldCategory(Category.Sound)]
        PlayerStepsVolume,
        //

        // Input
        [SettingFieldCategory(Category.Input)]
        MouseSensX,

        [SettingFieldCategory(Category.Input)]
        MouseSensY,
        //
        // Input key binds
        [SettingFieldCategory(Category.Input)]
        QuickSaveKey,
        //

        // Other
        [SettingFieldCategory(Category.Other)]
        SkipIntro,

        [SettingFieldCategory(Category.Other)]
        FieldOfView,

        [SettingFieldCategory(Category.Other)]
        AutoSave,

        [SettingFieldCategory(Category.Other)]
        SaveOnExit,

        [SettingFieldCategory(Category.Other)]
        AutoSaveIntervalInMinutes,
        //
    }
}
