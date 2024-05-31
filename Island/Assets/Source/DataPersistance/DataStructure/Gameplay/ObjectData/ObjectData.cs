using System;

namespace DataPersistence.Gameplay
{
    [Serializable]
    public class ObjectData : Data
    {
        public string prefabAssetGUID;
        public ComponentData[] componentsData;

        public bool isActive;

        public ObjectData() { }

        public ObjectData(string prefabAssetGUID, ComponentData[] componentsData = null, bool isActive = true)
        {
            this.prefabAssetGUID = prefabAssetGUID;
            this.componentsData = componentsData;
            this.isActive = isActive;
        }
    }
}
