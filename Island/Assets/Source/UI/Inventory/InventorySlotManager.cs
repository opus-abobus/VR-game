using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class InventorySlotManager : MonoBehaviour {
    [SerializeField] 
    private PanelWithSlotsManager _panelWithSlotsManager;

    [SerializeField] 
    private Hand _hand;

    private UIElement _uIElement;

    private Image _image;
    private Sprite _emptySlotSprite;
    private Color _oldColor;

    private void Start() {
        _uIElement = GetComponent<UIElement>();
        _image = GetComponent<Image>();
        _emptySlotSprite = _image.sprite;
        _oldColor = _image.color;
    }

    bool isEmpty = true;
    void SetImageSource(Sprite sprite) {
        _image.sprite = sprite;

        _image.color += new Color(0f, 0f, 0f, 1f);

        isEmpty = false;
    }

    GameObject storedObject = null;
    
    private void OnTriggerEnter(Collider other) {
        if (isEmpty) {
            InventorySlotSprite interManager = other.GetComponent<InventorySlotSprite>();
            if (interManager != null && _hand.currentAttachedObject == other.gameObject) {
                if ((wasPickedUpFromSlot && _panelWithSlotsManager.readyToPlace) || (wasPickedUpFromSlot == false)) {
                    storedObject = other.gameObject;

                    SetImageSource(interManager.SriteInInventory);

                    if (_hand.currentAttachedObject != null)
                        _hand.DetachObject(storedObject);

                    wasPickedUpFromSlot = false;

                    other.gameObject.SetActive(false);
                }
            }
        }
    }

    bool wasPickedUpFromSlot = false;

    public void OnHandPressedSlot() {
        if (!isEmpty) {
            _image.sprite = _emptySlotSprite;
            _image.color = _oldColor;

            storedObject.SetActive(true);
            storedObject.transform.position = _hand.transform.position;

            wasPickedUpFromSlot = true;
            _panelWithSlotsManager.readyToPlace = false;
            isEmpty = true;

            _hand.AttachObject(storedObject, GrabTypes.Grip, Hand.AttachmentFlags.VelocityMovement);

            _panelWithSlotsManager.readyToPlace = false;

            storedObject = null;
        }
    }

    public void InvokeSlotButton() {
        if (!isEmpty)
            _uIElement.onHandClick.Invoke(_hand);
    }
}
