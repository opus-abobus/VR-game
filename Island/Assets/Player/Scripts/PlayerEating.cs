using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEating : MonoBehaviour
{
    List<string> tagsWithEatableObjects = new List<string> { "berry", "banana", "cocount", "coconut" };

    private void OnCollisionEnter(Collision collision) {
        var tag = collision.gameObject.tag;
        foreach (var item in tagsWithEatableObjects) {
            if (tag == item && collision.gameObject.GetComponent<SetEatability>().IsEatable) {
                Destroy(collision.gameObject);
            }
        }
    }

}
