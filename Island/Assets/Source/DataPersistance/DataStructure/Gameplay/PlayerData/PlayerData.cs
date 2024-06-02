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
            public float cameraPitch;

            public PlayerCameraTransform() { }
            public PlayerCameraTransform(float cameraPitch)
            {
                this.cameraPitch = cameraPitch;
            }
        }
    }
}
