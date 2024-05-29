using DataPersistence.Gameplay;
using System;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using static DataPersistence.Gameplay.PlayerData.InventorySlotsData;

public class InventorySlotController : MonoBehaviour {

    [SerializeField] private Hand _hand;

    private UIElement _uIElement;

    private Image _image;
    private Sprite _emptySlotSprite;
    private Color _oldColor;

    private bool _isEmpty = true;
    public bool IsEmpty { get { return _isEmpty; } }

    private GameObject _storedObject = null;
    private string _objectPrefabGUID = null;

    private event Action ItemPlaced, ItemPickedUp;

    private GameObjectsRegistries _registry;

    public InventorySlotData GetData()
    {
        if (_objectPrefabGUID != null && _storedObject != null)
        {
            Vector3 velocty = Vector3.zero, angularVelocity = Vector3.zero;
            bool useGravityRb = false, isKinematicRb = false;
            if (_storedObject.TryGetComponent<Rigidbody>(out var rB))
            {
                velocty = rB.velocity;
                angularVelocity = rB.angularVelocity;
                useGravityRb = rB.useGravity;
                isKinematicRb = rB.isKinematic;
            }

            bool isTriggerCol = false, isEnabledCol = false;
            if (_storedObject.TryGetComponent<Collider>(out var Col))
            {
                isTriggerCol = Col.isTrigger;
                isEnabledCol = Col.enabled;
            }

            var objectData = new DynamicObjectsData.ObjectData(_storedObject.transform.position, _storedObject.transform.lossyScale, 
                _storedObject.transform.rotation, velocty, angularVelocity, useGravityRb, isKinematicRb, isTriggerCol, isEnabledCol,
                _objectPrefabGUID);

            return new InventorySlotData(gameObject.name, objectData);
        }
        else
        {
            return null;
            //return new InventorySlotData();
        }
    }

    public void SetData(InventorySlotData data)
    {
        if (data != null)
        {
            _objectPrefabGUID = data.storedObjectData.prefabAssetGUID;
            if (!string.IsNullOrEmpty(_objectPrefabGUID))
            {
                _storedObject = Instantiate(AddressableItems.Instance.GetPrefabByGUID(_objectPrefabGUID));
                _storedObject.SetActive(false);

                if (_storedObject.TryGetComponent<InventorySlotSprite>(out var spriteHolder))
                {
                    SetImageForSlot(spriteHolder.SriteInInventory);
                }
                else
                {
                    SetImageForSlot(InventoryPanelController.Instance.FallbackItemSprite);
                }

                _isEmpty = false;
            }
            else
            {
                _objectPrefabGUID = null;
            }
        }
    }

    public void Init(GameObjectsRegistries registry)
    {
        _uIElement = GetComponent<UIElement>();
        _image = GetComponent<Image>();
        _emptySlotSprite = _image.sprite;
        _oldColor = _image.color;

        _registry = registry;
    }

    private void SetImageForSlot(Sprite sprite) {
        _image.sprite = sprite;
        _image.color += new Color(0f, 0f, 0f, 1f);
    }

    private void OnTriggerEnter(Collider other) {
        if (_isEmpty) {

            var inventorySlotSprite = other.GetComponent<InventorySlotSprite>();
            if (_hand.currentAttachedObject == other.gameObject && inventorySlotSprite != null) {

                if (InventoryPanelController.Instance.ReadyToPlace) {
                    PlaceItem(other.gameObject, inventorySlotSprite.SriteInInventory);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (_storedObject == _hand.currentAttachedObject) {
            _storedObject = null;
        }
    }

    private void OnItemPlaced() {
        _objectPrefabGUID = _registry.GetDynamicObjectAssetGUID(_storedObject);
        _registry.Unregister(_storedObject);
    }

    private void OnItemPickedUp() {
        _registry.Register(_storedObject, _objectPrefabGUID);
        _objectPrefabGUID = null;
    }

    private void SubscribeAll() {
        ItemPlaced += OnItemPlaced;
        ItemPickedUp += OnItemPickedUp;
    }
    private void UnsubscribeAll() {
        ItemPickedUp -= OnItemPickedUp;
        ItemPlaced -= OnItemPlaced;
    }

    private void OnEnable() {
        SubscribeAll();
    }

    private void OnDisable() {
        UnsubscribeAll();
    }

    void PlaceItem(GameObject item, Sprite sprite) {
        _storedObject = item;
        
        _isEmpty = false;

        SetImageForSlot(sprite);

        if (_hand.currentAttachedObject != null)
            _hand.DetachObject(_storedObject);

        _storedObject.SetActive(false);

        ItemPlaced?.Invoke();
    }

    public void OnHandPressedSlot() {
        if (!_isEmpty) {
            _image.sprite = _emptySlotSprite;
            _image.color = _oldColor;

            _storedObject.SetActive(true);
            _storedObject.transform.position = _hand.transform.position;

            _isEmpty = true;

            InventoryPanelController.Instance.ReadyToPlace = false;

            _hand.AttachObject(_storedObject, GrabTypes.Grip, Hand.AttachmentFlags.VelocityMovement);

            ItemPickedUp?.Invoke();
        }
    }

    public void InvokeSlotButton() {
        if (!_isEmpty)
            _uIElement.onHandClick.Invoke(_hand);
    }
}
