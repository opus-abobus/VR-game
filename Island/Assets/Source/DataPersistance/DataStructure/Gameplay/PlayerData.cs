using System;
using UnityEngine;

namespace DataPersistence.Gameplay
{
    [Serializable]
    public class PlayerData
    {
        public Vector3 playerPosition;
        public Quaternion playerRotation;

        public PlayerCameraTransform playerCameraTransform;

        public HungerSystemData hungerSystemData;

        public InventorySlotsData inventorySlotsData;

        [Serializable]
        public class PlayerCameraTransform
        {
            public Vector3 localEulerAngles;

            public PlayerCameraTransform() { }
            public PlayerCameraTransform(Vector3 localEulerAngles)
            {
                this.localEulerAngles = localEulerAngles;
            }
        }

        [Serializable]
        public class HungerSystemData
        {
            public float satiety;
            public float health;

            public HungerSystemData() { }
            public HungerSystemData(float health, float satiety)
            {
                this.health = health;
                this.satiety = satiety;
            }
        }

        [Serializable]
        public class InventorySlotsData
        {
            public InventorySlotData[] slotsData;

            public InventorySlotsData() { }
            public InventorySlotsData(InventorySlotData[] data)
            {
                slotsData = data;
            }

            [Serializable]
            public class InventorySlotData
            {
                public string slotObjectName;
                public DynamicObjectsData.ObjectData storedObjectData;
                //public bool isEmpty;
                
                public InventorySlotData() { }
                public InventorySlotData(string slotObjectName, DynamicObjectsData.ObjectData objectData)
                {
                    this.slotObjectName = slotObjectName;
                    this.storedObjectData = objectData;
                }
            }
        }
    }
}
