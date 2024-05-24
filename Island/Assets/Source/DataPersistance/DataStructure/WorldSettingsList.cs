using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultiesMap", menuName = "ScriptableObject/DifficultiesMap")]
public class WorldSettingsList : ScriptableObject
{
    [Serializable]
    public class WorldSettingsEntry
    {
        public WorldSettings WorldSettings;
    }

    public WorldSettingsEntry[] entries;
}
