using System;
using UnityEngine;
using Valve.VR.InteractionSystem;
using static SOS_Manager;

public class Placeholding : MonoBehaviour {
    public event Action OnPlaceholding;

    private ISosLetters _sosLetters;

    private void Awake() {
        _sosLetters = GetComponentInParent<SOS_Manager>();
        OnPlaceholding += _sosLetters.UpdatePlaceholderCount;
    }

    private void OnTriggerEnter(Collider other) {
        string tag = other.tag;
        var interactable = other.GetComponent<Interactable>();
        if ((tag == "rock" || tag == "cocount" || tag == "coconut") && 
            interactable != null && interactable.attachedToHand != null &&
            interactable.attachedToHand.currentAttachedObject != null &&
            tag == other.GetComponent<Interactable>().attachedToHand.currentAttachedObject.tag) {

            OnPlaceholding.Invoke();
            OnPlaceholding -= _sosLetters.UpdatePlaceholderCount;

            other.GetComponent<Interactable>().attachedToHand.DetachObject(other.GetComponent<Interactable>().attachedToHand.currentAttachedObject);

            Destroy(other.GetComponent<VelocityEstimator>());
            Destroy(other.GetComponent<Throwable>());
            Destroy(other.GetComponent<Interactable>());

            other.transform.parent = this.transform;
            other.transform.localPosition = Vector3.zero;
            other.transform.localRotation = Quaternion.identity;

            Destroy(other.attachedRigidbody);
            Destroy(GetComponent<MeshRenderer>());
            Destroy(GetComponent<MeshFilter>());
            Destroy(GetComponent<Collider>());
            Destroy(this);
        }
    }
}
