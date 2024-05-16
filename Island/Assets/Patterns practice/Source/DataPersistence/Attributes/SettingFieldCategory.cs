using System;

namespace DataPersistence
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class SettingFieldCategoryAttribute : Attribute
    {
        public enum SettingsCategory
        {
            None,
            Graphics,
            Sound,
            Input,
            Other
        }

        public SettingsCategory Category { get; }

        public SettingFieldCategoryAttribute(SettingsCategory category)
        {
            Category = category;
        }
    }
}