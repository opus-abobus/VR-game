using DataPersistence.Gameplay;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using static DataPersistence.Gameplay.InventoryData;

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
            int i = 0;
            List<ComponentData> componentsList = new()
            {
                new TransformData()
            };
            componentsList[i++].SetDataFromComponent(_storedObject.transform);

            if (_storedObject.TryGetComponent<Rigidbody>(out var rB))
            {
                componentsList.Add(new RigidbodyData());
                componentsList[i++].SetDataFromComponent(rB);
            }

            if (_storedObject.TryGetComponent<Collider>(out var Col))
            {
                componentsList.Add(new ColliderData());
                componentsList[i++].SetDataFromComponent(Col);
            }

            var objectData = new ObjectData(_objectPrefabGUID, componentsList.ToArray());

            return new InventorySlotData(gameObject.name, objectData);
        }
        else
        {
            //return null;
            return new InventorySlotData(null, null);
        }
    }

    public void SetData(InventorySlotData data)
    {
        if (data != null)
        {
            _objectPrefabGUID = data.objectData.prefabAssetGUID;
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

    public void Init(InventorySlotData data)
    {
        _uIElement = GetComponent<UIElement>();
        _image = GetComponent<Image>();
        _emptySlotSprite = _image.sprite;
        _oldColor = _image.color;

        _registry = GameObjectsRegistries.Instance;

        SetData(data);
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
        _registry.UnregisterObject(_storedObject);
    }

    private void OnItemPickedUp() {
        _registry.RegisterObject(_storedObject, _objectPrefabGUID, new Component[]
        {
            _storedObject.transform, _storedObject.GetComponent<Collider>(), _storedObject.GetComponent<Rigidbody>()
        });
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
