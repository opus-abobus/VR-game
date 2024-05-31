using System;
using System.Linq;

namespace DataPersistence.Gameplay
{
    [Serializable]
    public class InventoryData : MonoBehaviourData
    {
        public InventorySlotData[] slotsData;

        public InventorySlotData GetSlotData(string key)
        {
            return slotsData.FirstOrDefault(x => x.key == key);
        }

        public InventoryData() { }
        public InventoryData(InventorySlotData[] slotsData)
        {
            this.slotsData = slotsData;
        }

        [Serializable]
        public class InventorySlotData : MonoBehaviourData
        {
            public string key;
            public ObjectData objectData;

            public InventorySlotData() { }

            public InventorySlotData(string key, ObjectData objectData)
            {
                this.key = key;
                this.objectData = objectData;
            }
        }
    }
}