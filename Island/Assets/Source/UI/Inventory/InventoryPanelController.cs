using UnityEngine;
using Valve.VR.InteractionSystem;

public class InventoryPanelController : MonoBehaviour
{
    [SerializeField]
    private Hand _hand;

    public static bool ReadyToPlace { get; set; } = true;
    private void OnTriggerExit(Collider other) {
        if (other.gameObject == _hand.currentAttachedObject) {
            ReadyToPlace = true;
        }
    }
}
