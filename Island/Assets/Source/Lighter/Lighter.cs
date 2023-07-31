using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Lighter : MonoBehaviour
{
    [SerializeField] Interactable _interactable;
    [SerializeField] ParticleSystem _particleSystem;

    public SteamVR_Action_Boolean _fireLighterAction;

    private void Update() {
        if (_interactable.attachedToHand != null) {
            SteamVR_Input_Sources hand = _interactable.attachedToHand.handType;

            if (_fireLighterAction[hand].stateDown || Input.GetKeyDown(KeyCode.F)) {
                if (_isFired) {
                    _isFired = false;
                    _particleSystem.Stop();
                }
                else {
                    _isFired = true;
                    _particleSystem.Play();
                }
            }
        }
        else {
            _isFired = false;
            _particleSystem.Stop();
        }
    }

   [HideInInspector] public bool _isFired = false;
}
