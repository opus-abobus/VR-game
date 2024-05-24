using DataPersistence;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataManager : MonoBehaviour
{
    [SerializeField] private GameplayBootstrap _boostrap;

    //[SerializeField] private GameObject _textMessageOnHUD;

    private void Awake()
    {
        _boostrap.Init(CurrentSessionDataManager.Instance.CurrentData);
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameStates.ACTIVE)
        {
            if (Input.GetKeyDown(AppManager.Instance.DataManager.SettingsData.QuickSaveKey))
            {
                SaveGame();
            }
        }
    }

    private void SaveGame()
    {
        CurrentSessionDataManager.Instance.SaveCurrentDataOnDisk();
    }
}
