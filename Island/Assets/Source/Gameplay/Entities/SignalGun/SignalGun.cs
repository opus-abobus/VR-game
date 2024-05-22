using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SignalGun : MonoBehaviour
{
    [SerializeField] Interactable interactable;
    [SerializeField] AudioClip audioClip;
    [SerializeField] AudioSource audioSource;
    [SerializeField] ParticleSystem rocketParticleSystem;
    public int rocketFireEffectDuration = 20;

    public float explosionForce = 20000;
    public SteamVR_Action_Boolean fireAction;
    public Transform rocket, bottomPart;
    Rigidbody rb_rocket;
    RocketShot rocketShot;

    bool isLoaded = true;

    private void Start() {
        rb_rocket = rocket.GetComponent<Rigidbody>();
        rocketShot = rocket.GetComponent<RocketShot>();
    }
    
    private void Update() {
        if (interactable.attachedToHand != null) {
            SteamVR_Input_Sources hand = interactable.attachedToHand.handType;

            if (fireAction[hand].stateDown || Input.GetKeyDown(KeyCode.Space)) {
                Fire();
            }
        }
    }

    Vector3 explosionPos;
    void Fire() {
        /*RaycastHit hit;
        if (Physics.Raycast(rocket.position, rocket.forward, out hit, 300)) {
            print(hit.collider.name);
        }*/

        if (isLoaded) {
            audioSource.PlayOneShot(audioClip);

            isLoaded = false;

            interactable.attachedToHand.DetachObject(rocket.gameObject, false);

            rocket.parent = null;
            rb_rocket.isKinematic = false;
            explosionPos = bottomPart.position;
            rb_rocket.AddExplosionForce(rb_rocket.mass * explosionForce, explosionPos, 10);

            rocketShot.isFired = true;
            StartCoroutine(RocketFire());
        }
    }

    IEnumerator RocketFire() {
        rocketParticleSystem.Play();
        yield return new WaitForSeconds(rocketFireEffectDuration);
        rocketParticleSystem.Stop();
        yield return null;
    }


    //---------------
    [HideInInspector] public static float dayTimeChance = 0.3f;
    [HideInInspector] public static float nightTimeChance = 0.35f;
}
