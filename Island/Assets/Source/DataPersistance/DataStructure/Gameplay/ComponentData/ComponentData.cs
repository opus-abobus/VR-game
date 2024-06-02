using System;
using System.Xml.Serialization;
using UnityEngine;

namespace DataPersistence.Gameplay
{
    [Serializable]
    [XmlInclude(typeof(TransformData))]
    [XmlInclude(typeof(RigidbodyData))]
    [XmlInclude(typeof(ColliderData))]
    public abstract class ComponentData : Data
    {
        public virtual void SetDataToGameObject(GameObject gameObject) { }
        public virtual void SetDataFromComponent<T>(T component) where T : UnityEngine.Component { }
        public static ComponentData GetDataFromComponent<T>(T component) where T : UnityEngine.Component
        {
            if (component == null) return null;

            if (component is Transform transform)
            {
                return new TransformData(transform.position, transform.lossyScale, transform.rotation);
            }

            if (component is Collider collider)
            {
                return new ColliderData(collider.enabled, collider.isTrigger);
            }

            if (component is Rigidbody rigidbody)
            {
                return new RigidbodyData(rigidbody.useGravity, rigidbody.isKinematic,
                    rigidbody.velocity, rigidbody.angularVelocity);
            }

            if (component is SignalGun signalGun)
            {
                return new SignalGunData(signalGun.IsLoaded, signalGun.rocketFireEffectTimeLeft);
            }

            if (component is Bonfire bonfire)
            {
                return new BonfireData(bonfire._isFired);
            }

            return null;
        }
    }
}