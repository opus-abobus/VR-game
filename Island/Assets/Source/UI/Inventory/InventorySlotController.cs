using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class InventorySlotController : MonoBehaviour {
    [SerializeField]
    private Hand _hand;

    private UIElement _uIElement;

    private Image _image;
    private Sprite _emptySlotSprite;
    private Color _oldColor;

    private bool _isEmpty = true;
    public bool IsEmpty { get { return _isEmpty; } }

    private GameObject _storedObject = null;

    private bool _readyToPlace = true;

    private event Action onItemPlaced, onItemPickedUp;

    private void Start() {
        _uIElement = GetComponent<UIElement>();
        _image = GetComponent<Image>();
        _emptySlotSprite = _image.sprite;
        _oldColor = _image.color;
    }

    private void SetImageForSlot(Sprite sprite) {
        _image.sprite = sprite;
        _image.color += new Color(0f, 0f, 0f, 1f);
    }

    private void OnTriggerEnter(Collider other) {
        if (_isEmpty) {

            var inventorySlotSprite = other.GetComponent<InventorySlotSprite>();
            if (_hand.currentAttachedObject == other.gameObject && inventorySlotSprite != null) {

                if (_readyToPlace) {
                    PlaceItem(other.gameObject, inventorySlotSprite.SriteInInventory);
                }
            }
        }
    }

/*    private void OnTriggerExit(Collider other) {
        if (_storedObject == _hand.currentAttachedObject) {
            _readyToPlace = true;
            _storedObject = null;
        }
    }*/

    private void OnItemPlaced() {

    }

    private void OnItemPickedUp() {
        StartCoroutine(SlotCooldwon());
    }

    private void SubscribeAll() {
        onItemPlaced += OnItemPlaced;
        onItemPickedUp += OnItemPickedUp;
    }
    private void UnsubscribeAll() {
        onItemPickedUp -= OnItemPickedUp;
        onItemPlaced -= OnItemPlaced;
    }

    private void OnEnable() {
        SubscribeAll();
    }

    private void OnDisable() {
        UnsubscribeAll();
    }

    private void OnDestroy() {
        UnsubscribeAll();
    }

    private IEnumerator SlotCooldwon() {
        for (int i = 0, frames = 10; i < frames; i++) {
            yield return new WaitForFixedUpdate();
        }

        _readyToPlace = true;
    }

    void PlaceItem(GameObject item, Sprite sprite) {
        _storedObject = item;
        _storedObject.SetActive(false);

        _isEmpty = false;

        SetImageForSlot(sprite);

        if (_hand.currentAttachedObject != null)
            _hand.DetachObject(_storedObject);

        onItemPlaced?.Invoke();
    }



    public void OnHandPressedSlot() {
        if (!_isEmpty) {
            _image.sprite = _emptySlotSprite;
            _image.color = _oldColor;

            _storedObject.SetActive(true);
            _storedObject.transform.position = _hand.transform.position;

            _isEmpty = true;

            _readyToPlace = false;

            _hand.AttachObject(_storedObject, GrabTypes.Grip, Hand.AttachmentFlags.VelocityMovement);

            _storedObject = null;

            onItemPickedUp?.Invoke();
        }
    }

    public void InvokeSlotButton() {
        if (!_isEmpty)
            _uIElement.onHandClick.Invoke(_hand);
    }
}
