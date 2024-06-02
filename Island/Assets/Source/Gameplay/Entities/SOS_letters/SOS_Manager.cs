using DataPersistence.Gameplay;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SOS_Manager : MonoBehaviour
{

    /*    [HideInInspector]
        public static float dayTimeChance = 0.1f;
        [HideInInspector]
        public static float nightTimeChance = 0.01f;*/

    [HideInInspector]
    public bool isSOSLayedOut = false;

    private int _occupiedPlaceholderCount;

    private Dictionary<Placeholding, int> _placeholdings = new();

    [SerializeField] private LevelDataManager _levelDataManager;

    private void OnGameSave(GameplayData gameplayData)
    {
        var pData = new SOS_ManagerData.PlaceholdingData[_placeholdings.Count];

        int i = 0;
        foreach (var p in _placeholdings)
        {
            pData[i++] = p.Key.GetData();
        }

        gameplayData.SOS_ManagerData = new SOS_ManagerData(pData);
    }

    public void Init(SOS_ManagerData data)
    {
        _levelDataManager.OnGameSave += OnGameSave;

        _occupiedPlaceholderCount = 0;

        var placeholdings = GetComponentsInChildren<Placeholding>();

        int i = 0;
        foreach (var p in placeholdings)
        {
            p.index = i;
            _placeholdings.Add(p, i);
            i++;
        }

        if (data != null)
        {
            foreach (Placeholding p in placeholdings)
            {
                var pData = data.GetData(p.index);
                if (pData != null)
                {
                    p.SetData(pData);

                    if (!pData.isOccupied)
                    {
                        placeholdings[pData.index].ItemPlaced += OnItemPlaced;
                    }
                    else
                    {
                        _occupiedPlaceholderCount++;
                    }
                }
            }
        }
    }

    private void Start()
    {
        if (_occupiedPlaceholderCount == transform.childCount)
        {
            isSOSLayedOut = true;

            EvacuationSystem.Instance.AddEvacItem(EvacuationSystem.EvacItem.TypesOfItems.sosRocks);
        }
    }

    private void OnItemPlaced(Placeholding placeholding)
    {
        placeholding.ItemPlaced -= OnItemPlaced;

        _occupiedPlaceholderCount++;

        if (transform.childCount == _occupiedPlaceholderCount)
        {
            isSOSLayedOut = true;

            EvacuationSystem.Instance.AddEvacItem(EvacuationSystem.EvacItem.TypesOfItems.sosRocks);
        }
    }

    private void OnDestroy()
    {
        _levelDataManager.OnGameSave -= OnGameSave;

        foreach (var p in _placeholdings)
        {
            if (!p.Key.itemPlaced)
            {
                p.Key.ItemPlaced -= OnItemPlaced;
            }
        }
    }
}

namespace DataPersistence.Gameplay
{
    [Serializable]
    public class SOS_ManagerData : MonoBehaviourData
    {
        public PlaceholdingData[] placeholdingData;

        public PlaceholdingData GetData(int index)
        {
            foreach (var p in placeholdingData)
            {
                if (p.index == index)
                {
                    return p;
                }
            }

            return null;
        }

        public SOS_ManagerData() { }

        public SOS_ManagerData(PlaceholdingData[] placeholdingData)
        {
            this.placeholdingData = placeholdingData;
        }

        [Serializable]
        public class PlaceholdingData : MonoBehaviourData
        {
            public int index;
            public bool isOccupied;

            public PlaceholdingData() { }

            public PlaceholdingData(int index, bool isOccupied)
            {
                this.index = index;
                this.isOccupied = isOccupied;
            }
        }
    }
}
