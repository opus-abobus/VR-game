using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelWithSlotsManager : MonoBehaviour
{
    [HideInInspector] public bool readyToPlace = true;

    private void OnTriggerExit(Collider other) {
        InteractableManager manager = other.GetComponent<InteractableManager>();
        if (manager != null && manager.IsPickedUp) {
            StartCoroutine(waitForFixedUpdates(3));
            readyToPlace = true;
        }
    }
    IEnumerator waitForFixedUpdates(int frames) {
        for (int i = 0; i < frames; i++) {
            yield return new WaitForFixedUpdate();
        }
    }
}
