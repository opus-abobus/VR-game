using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetacheInteractable : MonoBehaviour
{
    Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    void TurnOnGravity() {
        rb.useGravity = true;
    }
}
