using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class InventorySlotManager : MonoBehaviour {
    //[SerializeField] InventoryManager inventoryManager;

    [SerializeField] Hand hand;

    UIElement uIElement;

    Button button;
    Image _image;
    Sprite _oldSprite;
    Color _oldColor;

    private void Start() {
        button = GetComponent<Button>();
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
    bool isReadyToPlaceAgaing = false;
    
    private void OnTriggerEnter(Collider other) {
        if (isEmpty && storedObject == null) {
            InteractableManager manager = other.GetComponent<InteractableManager>();
            if (manager != null && manager.IsPickedUp) {
                string tag = other.tag;
                print(tag);
                if (tag == "berry" || tag == "cocount" || tag == "coconut" || tag == "rock"
                    || tag == "banana" || tag == "signalGun") {
                    storedObject = other.gameObject;
                    SetImageSource(manager.spriteInInvenory);
                    other.gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        InteractableManager manager = other.GetComponent<InteractableManager>();
        if (manager != null && manager.IsPickedUp) {
            isReadyToPlaceAgaing = true;
        }
    }

    public void OnHandPressedSlot() {
        if (!isEmpty && storedObject != null) {
            _image.sprite = _oldSprite;
            _image.color = _oldColor;
            storedObject.SetActive(true);
            hand.AttachObject(storedObject, GrabTypes.Grip);
            storedObject = null;
        }
    }

    public void InvokeSlotButton() {
        //button.onClick.Invoke();
        uIElement.onHandClick.Invoke(hand);
    }
}
