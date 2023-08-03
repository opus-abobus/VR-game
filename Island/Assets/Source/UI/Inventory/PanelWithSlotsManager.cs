using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class PanelWithSlotsManager : MonoBehaviour
{
    [HideInInspector]
    public bool readyToPlace = true;

    [SerializeField]
    private Hand _hand;
    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<InventorySlotSprite>() != null && _hand.currentAttachedObject != null) {
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
