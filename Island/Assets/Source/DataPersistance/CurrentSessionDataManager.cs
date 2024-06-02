using DataPersistence.Gameplay;
using System;
using UI.WindowsManagement;
using UnityEngine;

namespace DataPersistence
{
    public class CurrentSessionDataManager : MonoBehaviour
    {
        [SerializeField] private NewGameWindow _newGameWindow;
        public bool IsNewGame { get; private set; } = true;

        private GameplayData _gameplayData;

        public GameplayData CurrentData { get { return _gameplayData; } }

        public void SetData(GameplayData gameplayData)
        {
            _gameplayData = gameplayData;
            IsNewGame = false;
        }

        public void SaveCurrentDataOnDisk()
        {
            AppManager.Instance.DataManager.WriteSave(_gameplayData);
        }

        private static CurrentSessionDataManager _instance;
        public static CurrentSessionDataManager Instance { get { return _instance; } }

        private void Awake()
        {
            if (Instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(this);
                return;
            }

            DontDestroyOnLoad(this);

            _newGameWindow.StartNewGameButtonClicked += OnNewGame_StartButtonClicked;
        }

        private void OnNewGame_StartButtonClicked(int difficultyID)
        {
            _gameplayData = new GameplayData(difficultyID);
        }

        private void OnDestroy()
        {
            if (_newGameWindow != null)
            {
                _newGameWindow.StartNewGameButtonClicked -= OnNewGame_StartButtonClicked;
            }
        }
    }
}
