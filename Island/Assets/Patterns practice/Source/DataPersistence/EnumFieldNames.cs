using System;

namespace DataPersistence {

    using Category = SettingFieldCategoryAttribute.SettingsCategory;

    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class SettingFieldCategoryAttribute : Attribute {
        
        public enum SettingsCategory {
            None,
            Graphics,
            Sound,
            Input,
            Other
        }

        public SettingsCategory Category { get; }

        public SettingFieldCategoryAttribute(SettingsCategory category) {
            Category = category;
        }
    }

    public enum FieldName {

        [SettingFieldCategory(Category.None)] UNDEFINED,

        [SettingFieldCategory(Category.Graphics)]
        ScreenResolutions,

        [SettingFieldCategory(Category.Graphics)]
        ScreenResolution,

        //[SettingFieldCategory(Category.Graphics)] DisplayMode,

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
        QuickSave,
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
