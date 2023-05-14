using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOnGravity : MonoBehaviour
{
    Rigidbody rb;
    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    bool _useGravity = false;
    private void OnCollisionEnter(Collision collision) {
        if (!_useGravity) {
            rb.useGravity = true;
            _useGravity = true;
        }
    }
}
