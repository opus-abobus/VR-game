using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    [SerializeField] public ParticleSystem particleSystem;
    [SerializeField] public AudioSource audioSource;

    [HideInInspector] public bool isFired = false;

    private void OnCollisionEnter(Collision collision) {
        if (!isFired && collision.gameObject.tag == "lighter") {
            if (collision.gameObject.GetComponent<Lighter>().isFired) {
                isFired = true;
                particleSystem.Play();
                audioSource.Play();

                EvacuationSystem.instance.AddEvacItem(EvacuationSystem.EvacItem.TypesOfItems.bonfire, this);
            }
        }
    }
}
