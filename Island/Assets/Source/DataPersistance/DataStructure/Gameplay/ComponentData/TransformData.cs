using System;
using UnityEngine;

namespace DataPersistence.Gameplay
{
    [Serializable]
    public class TransformData : ComponentData
    {
        public Vector3 position, scale;
        public Quaternion rotation;

        public override void SetDataToGameObject(GameObject gameObject)
        {
            gameObject.transform.position = position;
            gameObject.transform.localScale = scale;
            gameObject.transform.rotation = rotation;
        }

        public override void SetDataFromComponent<T>(T component)
        {
            if (component is Transform c)
            {
                this.position = c.position;
                this.scale = c.lossyScale;
                this.rotation = c.rotation;
            }
        }

        public TransformData() { }

        public TransformData(Vector3 position, Vector3 scale, Quaternion rotation)
        {
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;
        }
    }
}