using System;
using UnityEngine;

namespace DataPersistence.Gameplay
{
    [Serializable]
    public class DynamicObjectsData
    {
        public ObjectData[] objectDatas;

        public DynamicObjectsData() { }
        public DynamicObjectsData(ObjectData[] objectDatas)
        {
            this.objectDatas = objectDatas;
        }

        [Serializable]
        public class ObjectData
        {
            public Vector3 postiton, scale;
            public Quaternion rotation;
            public Vector3 velocity, angularVelocity;
            public bool useGravityRB, isKinematicRB, isTriggerCol, isEnabledCol;
            public string prefabAssetGUID;

            public ObjectData() { }
            public ObjectData(Vector3 postiton, Vector3 scale, Quaternion rotation, Vector3 velocity, 
                Vector3 angularVelocity, bool useGravityRB, bool isKinematicRB, 
                bool isTriggerCol, bool isEnabledCol, string prefabAssetGUID)
            {
                this.postiton = postiton;
                this.scale = scale;
                this.rotation = rotation;
                this.velocity = velocity;
                this.angularVelocity = angularVelocity;
                this.useGravityRB = useGravityRB;
                this.isKinematicRB = isKinematicRB;
                this.isTriggerCol = isTriggerCol;
                this.isEnabledCol = isEnabledCol;
                this.prefabAssetGUID = prefabAssetGUID;
            }
        }
    }
}