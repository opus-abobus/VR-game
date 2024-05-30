using UnityEngine;
using Valve.VR.InteractionSystem;
using static DataPersistence.Gameplay.PlayerData;
using static DataPersistence.Gameplay.PlayerData.InventorySlotsData;

public class InventoryPanelController : MonoBehaviour
{
    [SerializeField] private Hand _hand;

    [SerializeField] private GameObjectsRegistries _registry;

    private InventorySlotController[] _slotControllers;

    [field: SerializeField] public Sprite FallbackItemSprite { get; private set; }

    public static InventoryPanelController Instance { get; private set; }

    public void Init()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            return;
        }

        _slotControllers = gameObject.GetComponentsInChildren<InventorySlotController>(true);
        foreach (var slot in _slotControllers)
        {
            slot.Init(_registry);
        }
    }

    public void SetData()
    {
        foreach (var slot in _slotControllers)
        {
            slot.SetData(_registry.GetData<InventorySlotData>(slot.gameObject.name));
        }
    }

    public InventorySlotsData GetData()
    {
        InventorySlotData[] slotsData = new InventorySlotData[_slotControllers.Length];
        int i = 0;
        foreach (var slot in _slotControllers)
        {
            slotsData[i++] = slot.GetData();
        }

        return new InventorySlotsData(slotsData);
    }

    public bool ReadyToPlace { get; set; } = true;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _hand.currentAttachedObject)
        {
            ReadyToPlace = true;
        }
    }
}
