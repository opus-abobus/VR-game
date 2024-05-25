using DataPersistence;
using DataPersistence.Gameplay;
using System;
using UnityEngine;

public class LevelDataManager : MonoBehaviour
{
    [SerializeField] private GameplayBootstrap _boostrap;

    //[SerializeField] private GameObject _textMessageOnHUD;
    public event Action<GameplayData> OnGameSave;

    private void Awake()
    {
        if (CurrentSessionDataManager.Instance.IsNewGame)
        {
            CurrentSessionDataManager.Instance.CurrentData.playerData = new PlayerData();
        }

        _boostrap.Init(CurrentSessionDataManager.Instance.CurrentData, CurrentSessionDataManager.Instance.IsNewGame);
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameStates.ACTIVE)
        {
            if (Input.GetKeyDown(AppManager.Instance.DataManager.SettingsData.QuickSaveKey))
            {
                SaveGame("quicksave");
            }
        }
    }

    private void SaveGame(string saveDescription)
    {
        OnGameSave?.Invoke(CurrentSessionDataManager.Instance.CurrentData);

        CurrentSessionDataManager.Instance.CurrentData.displayName = Environment.UserName + " " + DateTime.Now + " " + saveDescription;
        CurrentSessionDataManager.Instance.SaveCurrentDataOnDisk();
    }
}
