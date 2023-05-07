using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Placeholding : MonoBehaviour
{
    [SerializeField] MeshFilter _meshFilter;
    [SerializeField] MeshRenderer _meshRenderer;
    [SerializeField] MeshCollider _collider;

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<InteractableManager>().IsPickedUp) {
            string tag = other.tag;
            if (tag == "rock" || tag == "cocount" || tag == "coconut") {
                Destroy(other.GetComponent<VelocityEstimator>());
                Destroy(other.GetComponent<Throwable>());
                Destroy(other.GetComponent<Interactable>());
                Destroy(other.GetComponent<InteractableManager>());

                other.transform.parent = this.transform;
                other.transform.localPosition = Vector3.zero;
                other.transform.localRotation = Quaternion.identity;
                other.attachedRigidbody.isKinematic = true;

                Destroy(_meshRenderer);
                Destroy(_meshFilter);
                Destroy(_collider);
                Destroy(this);
            }
        }
    }
}
