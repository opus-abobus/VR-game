using System;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
#pragma warning disable CS0108
    [SerializeField] 
    private ParticleSystem _particleSystem;
    public ParticleSystem ParticleSystem { get { return _particleSystem; } }
#pragma warning restore CS0108

    [SerializeField]
    private AudioSource _audioSource;
    public AudioSource AudioSource { get { return _audioSource; } }

    [HideInInspector]
    public bool _isFired = false;

    private void Start()
    {
        if (_isFired)
        {
            _particleSystem.Play();
            _audioSource.Play();

            EvacuationSystem.Instance.AddEvacItem(EvacuationSystem.EvacItem.TypesOfItems.bonfire, this);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (!_isFired && collision.gameObject.tag == "lighter") {
            if (collision.gameObject.GetComponent<Lighter>().Fired) {

                _isFired = true;
                _particleSystem.Play();
                _audioSource.Play();

                EvacuationSystem.Instance.AddEvacItem(EvacuationSystem.EvacItem.TypesOfItems.bonfire, this);
            }
        }
    }
}

namespace DataPersistence.Gameplay
{
    [Serializable]
    public class BonfireData : MonoBehaviourData
    {
        public bool isFired;

        public override void SetDataFromComponent<T>(T component)
        {
            if (component is Bonfire bonfire)
            {
                this.isFired = bonfire._isFired;
            }
        }

        public override void SetDataToGameObject(GameObject gameObject)
        {
            if (gameObject.TryGetComponent<Bonfire>(out var bonfire))
            {
                bonfire._isFired = this.isFired;
            }
        }

        public BonfireData() { }

        public BonfireData(bool isFired)
        {
            this.isFired = isFired;
        }
    }
}