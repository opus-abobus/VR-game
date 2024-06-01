using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Lighter : MonoBehaviour
{
    //[SerializeField] private SteamVR_Action_Boolean _fireLighterAction;

    [SerializeField] private ParticleSystem _particleSystem;

    private Interactable _interactable;
    
    private void Awake()
    {
        _interactable = GetComponent<Interactable>();

        if (_interactable != null)
        {
            _interactable.onAttachedToHand += OnAttached;
            _interactable.onDetachedFromHand += OnDetached;
        }
    }

    private bool _isUseable = false;
    private bool _isFired = false;

    public bool Fired { get { return _isFired; } }

    private void OnAttached(Hand hand)
    {
        _isUseable = true;
    }

    private void OnDetached(Hand hand)
    {
        _isUseable = false;
    }

    private void Update()
    {
        if (_isUseable)
        {
            //SteamVR_Input_Sources hand = _interactable.attachedToHand.handType;
            //if (_fireLighterAction[hand].stateDown || Input.GetKeyDown(KeyCode.F))
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (_isFired)
                {
                    _isFired = false;
                    _particleSystem.Stop();
                }
                else
                {
                    _isFired = true;
                    _particleSystem.Play();
                }
            }
        }
        else
        {
            _isFired = false;
            _particleSystem.Stop();
        }
    }

    private void OnDestroy()
    {
        if (_interactable != null)
        {
            _interactable.onAttachedToHand -= OnAttached;
            _interactable.onDetachedFromHand -= OnDetached;
        }
    }
}
