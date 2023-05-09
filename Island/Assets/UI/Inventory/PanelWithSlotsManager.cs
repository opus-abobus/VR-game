using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelWithSlotsManager : MonoBehaviour
{
    [HideInInspector] public bool readyToPlace;

    private void OnTriggerEnter(Collider other) {
        InteractableManager manager = other.GetComponent<InteractableManager>();
        if (manager != null && manager.IsPickedUp) {
            print("pickedUp");
            readyToPlace = false;
        }
            
    }

    private void OnTriggerExit(Collider other) {
        InteractableManager manager = other.GetComponent<InteractableManager>();
        if (manager != null && manager.IsPickedUp) {
            print("pickedUp");
            readyToPlace = true;
        }
            
    }
}
