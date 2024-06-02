using DataPersistence.Gameplay;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameplayBootstrap : MonoBehaviour
{
    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private GameSettingsManager _gameSettingsManager;

    [SerializeField]
    private SpawnManager _spawnManager;

    [SerializeField]
    private EvacuationSystem _evacuationSystem;

    [SerializeField]
    private HybridPlayerController _hybridPlayerController;

    [SerializeField]
    private PlayerEating _playerEating;

    [SerializeField]
    private BananaPool _bananaPool;

    [SerializeField]
    private HungerSystem _hungerSystem;

    [SerializeField]
    private GameTime _gameTime;

    [SerializeField]
    private InventoryPanelController _inventoryPanelController;

    [SerializeField]
    private SOS_Manager _sosManager;

    [SerializeField]
    private GameObjectsRegistries _registries;

    [SerializeField]
    private AddressableItems _addressableItems;

    private static GameplayBootstrap _instance;
    public static GameplayBootstrap Instance { get { return _instance; } }

    public event Action BootstrapFinished;

    public interface IBootstrap
    {
        void Initialize();
    }

    public void Init(GameplayData gameplayData, bool isNewGame)
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _addressableItems.LoadAssets();

        _bananaPool.Init();
        _gameSettingsManager.Initialize(gameplayData.difficultyID);

        _gameManager.Initialize();

        if (isNewGame)
        {
            _registries.Init(null);
            _hybridPlayerController.Initialize(null);
            _hungerSystem.Initialize(null);
            _gameTime.Init(null);
            _inventoryPanelController.Init(null);
            _sosManager.Init(null);
        }
        else
        {
            _registries.Init(gameplayData);
            _hybridPlayerController.Initialize(gameplayData.playerData);
            _hungerSystem.Initialize(gameplayData.playerData.hungerSystemData);
            _gameTime.Init(gameplayData.gameTimeData);
            _inventoryPanelController.Init(gameplayData.playerData.inventoryData);
            _sosManager.Init(gameplayData.SOS_ManagerData);
        }

        _spawnManager.Initialize(gameplayData.spawnersData);


        Queue<IBootstrap> bootQueue = new Queue<IBootstrap>();
        bootQueue.Enqueue(_evacuationSystem);
        bootQueue.Enqueue(_playerEating);

        while (bootQueue.Count > 0)
        {
            bootQueue.Dequeue().Initialize();
        }

        AudioListener.volume = AppManager.Instance.DataManager.SettingsData.TotalVolume;

        BootstrapFinished?.Invoke();
    }
}