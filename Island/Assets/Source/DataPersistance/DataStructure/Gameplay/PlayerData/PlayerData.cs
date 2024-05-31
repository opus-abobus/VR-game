using System;
using UnityEngine;

namespace DataPersistence.Gameplay
{
    [Serializable]
    public class PlayerData : Data
    {
        public Vector3 playerPosition;
        public Quaternion playerRotation;

        public PlayerCameraTransform playerCameraTransform;

        public HungerSystemData hungerSystemData;

        public InventoryData inventoryData;

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
    }
}
