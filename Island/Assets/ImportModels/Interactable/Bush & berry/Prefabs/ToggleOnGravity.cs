using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOnGravity : MonoBehaviour
{
    Rigidbody rb;
    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision) {
        //if (collision.gameObject.name != "Bush")
            rb.useGravity = true;
    }
}
