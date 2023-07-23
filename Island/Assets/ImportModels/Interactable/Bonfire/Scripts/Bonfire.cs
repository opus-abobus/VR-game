using UnityEngine;

public class Bonfire : MonoBehaviour
{
#pragma warning disable CS0108
    [SerializeField] public ParticleSystem _particleSystem;
#pragma warning restore CS0108

    [SerializeField] public AudioSource _audioSource;

    [HideInInspector] public bool _isFired = false;

    private void OnCollisionEnter(Collision collision) {
        if (!_isFired && collision.gameObject.tag == "lighter") {
            if (collision.gameObject.GetComponent<Lighter>()._isFired) {
                _isFired = true;
                _particleSystem.Play();
                _audioSource.Play();

                EvacuationSystem.Instance.AddEvacItem(EvacuationSystem.EvacItem.TypesOfItems.bonfire, this);
            }
        }
    }
}
