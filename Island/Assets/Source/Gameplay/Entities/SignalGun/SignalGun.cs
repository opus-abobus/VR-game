using System;
using System.Collections;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SignalGun : MonoBehaviour
{
    [SerializeField] Interactable interactable;
    [SerializeField] AudioClip audioClip;
    [SerializeField] AudioSource audioSource;
    [SerializeField] ParticleSystem rocketParticleSystem;
    public float rocketFireEffectDuration = 20;

    public float rocketFireEffectTimeLeft = 0;

    public float explosionForce = 20000;
    public SteamVR_Action_Boolean fireAction;
    public Transform rocket, bottomPart;
    Rigidbody rb_rocket;
    RocketShot rocketShot;

    public bool IsLoaded = true;

    //private SteamVR_Input_Sources handType;

    private void Start() {
        rb_rocket = rocket.GetComponent<Rigidbody>();
        rocketShot = rocket.GetComponent<RocketShot>();

        //handType = interactable.attachedToHand.handType;
    }
    
    private void Update() {
        if (interactable.attachedToHand != null) {

            //if (fireAction[handType].stateDown || Input.GetKeyDown(KeyCode.Space)) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (IsLoaded)
                {
                    Fire();
                }
            }
        }
    }

    Vector3 explosionPos;
    void Fire() {
        /*RaycastHit hit;
        if (Physics.Raycast(rocket.position, rocket.forward, out hit, 300)) {
            print(hit.collider.name);
        }*/

        if (IsLoaded) {
            audioSource.PlayOneShot(audioClip);

            IsLoaded = false;

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
        rocketFireEffectTimeLeft = rocketFireEffectDuration;

        while (true)
        {
            if (rocketFireEffectTimeLeft <= 0)
                break;

            rocketFireEffectTimeLeft -= Time.deltaTime;
            yield return null;
        }

        rocketParticleSystem.Stop();
    }


    //---------------
    [HideInInspector] public static float dayTimeChance = 0.3f;
    [HideInInspector] public static float nightTimeChance = 0.35f;
}

namespace DataPersistence.Gameplay
{
    [Serializable]
    public class SignalGunData : MonoBehaviourData
    {
        public bool isLoaded;
        public float rocketFireEffectTimeLeft;

        public override void SetDataFromComponent<T>(T component)
        {
            if (component is SignalGun signalGun)
            {
                isLoaded = signalGun.IsLoaded;
                //rocketFireEffectTimeLeft = signalGun.rocketFireEffectTimeLeft;
            }
        }

        public override void SetDataToGameObject(GameObject gameObject)
        {
            if (gameObject.TryGetComponent<SignalGun>(out var signalGun))
            {
                signalGun.IsLoaded = isLoaded;
                //signalGun.rocketFireEffectTimeLeft = rocketFireEffectTimeLeft;
            }
        }

        public SignalGunData() { }
        public SignalGunData(bool isLoaded, float rocketFireEffectTimeLeft)
        {
            this.isLoaded = isLoaded;
            //this.rocketFireEffectTimeLeft = rocketFireEffectTimeLeft;
        }
    }
}
