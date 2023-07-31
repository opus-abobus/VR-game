using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class InventorySlotManager : MonoBehaviour {
    [SerializeField] PanelWithSlotsManager panelWithSlotsManager;
    [SerializeField] Hand hand;

    UIElement uIElement;

    Image _image;
    Sprite _oldSprite;
    Color _oldColor;

    private void Start() {
        uIElement = GetComponent<UIElement>();
        _image = GetComponent<Image>();
        _oldSprite = _image.sprite;
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
            InteractableManager manager = other.GetComponent<InteractableManager>();
            if (manager != null && hand.currentAttachedObject == other.gameObject) {
                if ((wasPickedUpFromSlot && panelWithSlotsManager.readyToPlace) || (wasPickedUpFromSlot == false)) {
                    storedObject = other.gameObject;
                    SetImageSource(manager.spriteInInvenory);
                    hand.DetachObject(storedObject);
                    wasPickedUpFromSlot = false;
                    other.gameObject.SetActive(false);
                }
            }
        }
    }

    bool wasPickedUpFromSlot = false;
    public void OnHandPressedSlot() {
        if (!isEmpty) {
            _image.sprite = _oldSprite;
            _image.color = _oldColor;
            storedObject.SetActive(true);
            storedObject.transform.position = hand.transform.position;
            wasPickedUpFromSlot = true;
            panelWithSlotsManager.readyToPlace = false;
            isEmpty = true;
            hand.AttachObject(storedObject, GrabTypes.Grip, Hand.AttachmentFlags.VelocityMovement);
            panelWithSlotsManager.readyToPlace = false;

            SetEatability _component1 = storedObject.GetComponent<SetEatability>();
            if (_component1 != null) { _component1.IsEatable = true; }
            InteractableManager _component2 = storedObject.GetComponent<InteractableManager>();
            if (_component2 != null) { _component2.IsPickedUp = true; }

            //StartCoroutine(resetVelocity(3));
            storedObject = null;
        }
    }

    public void InvokeSlotButton() {
        if (!isEmpty)
            uIElement.onHandClick.Invoke(hand);
    }

    IEnumerator resetVelocity(int frames) {
        for (int i = 0; i < frames; i++) {
            storedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            yield return new WaitForFixedUpdate();
        }
    }
}
