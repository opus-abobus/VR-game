using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketShot : MonoBehaviour
{
    [HideInInspector] public bool isFired = false;

    private void OnTriggerEnter(Collider other) {
        if (isFired) {
            if (other.name == "RocketZone") {
                print("detected");
                EvacuationSystem.Instance.AddEvacItem(EvacuationSystem.EvacItem.TypesOfItems.rocket);
                isFired = false;
            }
        }
    }
}
