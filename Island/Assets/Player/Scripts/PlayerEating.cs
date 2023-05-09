using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class PlayerEating : MonoBehaviour
{
    [SerializeField] Hand hand;
    [SerializeField] Transform playerCamera;

    List<string> tagsWithEatableObjects = new List<string> { "berry", "banana", "cocount", "coconut" };
    [SerializeField] HungerSystem hungerSystem;

    private void Awake() {
        if (playerCamera == null) { Debug.LogAssertion("player camera null reference exception"); }
        else { this.gameObject.transform.position = playerCamera.position; }
        if (hungerSystem == null) { Debug.LogError("hungerSystem instance null reference exception"); }
    }

    private void OnCollisionEnter(Collision collision) {
        var tag = collision.gameObject.tag;
        print(hand.currentAttachedObject);
        foreach (var item in tagsWithEatableObjects) {
            
            if (tag == item && collision.gameObject.GetComponent<SetEatability>().IsEatable) {
                hungerSystem.Satiety += collision.gameObject.GetComponent<SetEatability>().nutritionalValue;
                Destroy(collision.gameObject);
            }
        }
    }

}
