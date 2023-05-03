using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SetKinematicAfterSleep : MonoBehaviour
{
    [SerializeField, Range(1, 100)] int setKinematicAfterSeconds = 3;
    
    Rigidbody rb;
    private void Awake() {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    public bool IsKinematic {
        get { return rb.isKinematic; }
        set { rb.isKinematic = value; }
    }
    public bool IsStatic {
        get { return this.gameObject.isStatic; }
        set { this.gameObject.isStatic = value; }
    }

    bool isGrounded;
    
    void SetKinematicThis(bool value) {
        if (value) {
            IsKinematic = true;
        }
        else {
            IsKinematic = false;
        }
    }

    private void FixedUpdate() {
        if (isGrounded) {
            if (!CR_running) { 
                RestartCoroutine(DetermineIsSleeping());
            }
        }
        else {
            if (CR_running) {
                StopCoroutine(DetermineIsSleeping());
                CR_running = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "ground") {
            isGrounded = true;
        }
    }
    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.tag == "ground") {
            isGrounded = false;
        }
    }

    bool CR_running = false;
    IEnumerator DetermineIsSleeping() {
        CR_running = true;
        float _elapsedTime = 0;
        while (true) {
            if (rb.IsSleeping()) {
                _elapsedTime += Time.deltaTime;
                if (_elapsedTime >= setKinematicAfterSeconds) { SetKinematicThis(true); break; }
            }
            else {
                _elapsedTime = 0;
            }
            yield return null;
        }
        yield return null;
    }

    public void RestartCoroutine(IEnumerator coroutine) {
        StopCoroutine(coroutine);
        StartCoroutine(coroutine);
    }
}
