using System;
using UnityEngine;

namespace DataPersistence.Gameplay
{
    [Serializable]
    public class RigidbodyData : ComponentData
    {
        public bool useGravity;
        public bool isKinematic;

        public Vector3 velocity, angularVelocity;

        public override void SetDataToGameObject(GameObject gameObject)
        {
            if (gameObject.TryGetComponent<Rigidbody>(out var rigidbody))
            {
                rigidbody.isKinematic = isKinematic;
                rigidbody.useGravity = useGravity;
                rigidbody.velocity = velocity;
                rigidbody.angularVelocity = angularVelocity;
            }
        }

        public override void SetDataFromComponent<T>(T component)
        {
            if (component is Rigidbody c)
            {
                this.useGravity = c.useGravity;
                this.isKinematic = c.isKinematic;
                this.velocity = c.velocity;
                this.angularVelocity = c.angularVelocity;
            }
        }

        public RigidbodyData() { }

        public RigidbodyData(bool useGravity, bool isKinematic, Vector3 velocity, Vector3 angularVelocity)
        {
            this.useGravity = useGravity;
            this.isKinematic = isKinematic;
            this.velocity = velocity;
            this.angularVelocity = angularVelocity;
        }
    }
}