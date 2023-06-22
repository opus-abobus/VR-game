using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Lighter : MonoBehaviour
{
    [SerializeField] Interactable interactable;
    [SerializeField] ParticleSystem particleSystem;

    public SteamVR_Action_Boolean fireLighterAction;

    private void Update() {
        if (interactable.attachedToHand != null) {
            SteamVR_Input_Sources hand = interactable.attachedToHand.handType;

            if (fireLighterAction[hand].stateDown || Input.GetKeyDown(KeyCode.F)) {
                if (isFired) {
                    isFired = false;
                    particleSystem.Stop();
                }
                else {
                    isFired = true;
                    particleSystem.Play();
                }
            }
        }
        else {
            isFired = false;
            particleSystem.Stop();
        }
    }

   [HideInInspector] public bool isFired = false;
}
