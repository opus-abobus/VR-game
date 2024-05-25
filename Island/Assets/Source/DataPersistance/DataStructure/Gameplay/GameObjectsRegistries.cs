using DataPersistence;
using DataPersistence.Gameplay;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectsRegistries : MonoBehaviour
{
    private Dictionary<string, InventorySlotController> _inventoryRegistry;

    public void Init(GameplayData data)
    {
        if (CurrentSessionDataManager.Instance.IsNewGame)
        {
            _inventoryRegistry = new Dictionary<string, InventorySlotController>();
        }
        else
        {
            InitInventoryRegistry(data.playerData);
        }
    }

    public void Register<T>()
    {
        if (typeof(T) == typeof(InventorySlotController))
        {
            
        }
    }

    public void Unregister<T>()
    {
        if (typeof(T) == typeof(InventorySlotController))
        {
            
        }
    }

    private void InitInventoryRegistry(PlayerData data)
    {
        
    }
}
