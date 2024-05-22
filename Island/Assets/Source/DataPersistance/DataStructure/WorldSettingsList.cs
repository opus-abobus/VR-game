using UnityEngine;

[CreateAssetMenu(fileName = "DifficultiesMap", menuName = "ScriptableObject/DifficultiesMap")]
public class WorldSettingsList : ScriptableObject
{
    [System.Serializable]
    public class WorldSettingsEntry
    {
        public WorldSettings WorldSettings;
    }

    public WorldSettingsEntry[] entries;

    // make id's for each entry if file on disk wasn't find
}
