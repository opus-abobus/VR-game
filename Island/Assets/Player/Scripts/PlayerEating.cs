using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEating : MonoBehaviour
{
    List<string> tagsWithEatableObjects = new List<string> { "berry", "banana", "cocount", "coconut" };
    [SerializeField] HungerSystem hungerSystem;

    private void Awake() {
        if (hungerSystem == null) { Debug.LogError("hungerSystem instance null reference exception"); }
    }

    private void OnCollisionEnter(Collision collision) {
        var tag = collision.gameObject.tag;
        foreach (var item in tagsWithEatableObjects) {
            if (tag == item && collision.gameObject.GetComponent<SetEatability>().IsEatable) {
                hungerSystem.Satiety += collision.gameObject.GetComponent<SetEatability>().nutritionalValue;
                Destroy(collision.gameObject);
            }
        }
    }

}
