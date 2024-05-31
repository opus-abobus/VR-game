using DataPersistence.Gameplay;
using UnityEngine;
using Valve.VR.InteractionSystem;
using static DataPersistence.Gameplay.InventoryData;

public class InventoryPanelController : MonoBehaviour
{
    [SerializeField] private Hand _hand;

    private InventorySlotController[] _slotControllers;

    [field: SerializeField] public Sprite FallbackItemSprite { get; private set; }

    [SerializeField] private LevelDataManager _levelDataManager;

    public static InventoryPanelController Instance { get; private set; }

    public void Init(InventoryData data)
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            return;
        }

        _levelDataManager.OnGameSave += OnGameSave;

        _slotControllers = gameObject.GetComponentsInChildren<InventorySlotController>(true);
        foreach (var slot in _slotControllers)
        {
            if (data != null)
                slot.Init(data.GetSlotData(slot.gameObject.name));
            else
                slot.Init(null);
        }
    }

    private void OnGameSave(GameplayData gameplayData)
    {
        gameplayData.playerData.inventoryData = GetData();
    }

    public InventoryData GetData()
    {
        InventorySlotData[] slotsData = new InventorySlotData[_slotControllers.Length];
        int i = 0;
        foreach (var slot in _slotControllers)
        {
            slotsData[i++] = slot.GetData();
        }

        return new InventoryData(slotsData);
    }

    public bool ReadyToPlace { get; set; } = true;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _hand.currentAttachedObject)
        {
            ReadyToPlace = true;
        }
    }

    private void OnDestroy()
    {
        _levelDataManager.OnGameSave -= OnGameSave;
    }
}
