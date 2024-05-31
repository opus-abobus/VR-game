using System;
using UnityEngine;

namespace DataPersistence.Gameplay
{
    [Serializable]
    public class ColliderData : ComponentData
    {
        public bool enabled;
        public bool isTrigger;

        public override void SetDataToGameObject(GameObject gameObject)
        {
            if (gameObject.TryGetComponent<Collider>(out var collider))
            {
                collider.enabled = enabled;
                collider.isTrigger = isTrigger;
            }
        }

        public override void SetDataFromComponent<T>(T component)
        {
            if (component is Collider c)
            {
                this.enabled = c.enabled;
                this.isTrigger = c.isTrigger;
            }
        }

        public ColliderData() { }

        public ColliderData(bool enabled, bool isTrigger)
        {
            this.enabled = enabled;
            this.isTrigger = isTrigger;
        }
    }
}