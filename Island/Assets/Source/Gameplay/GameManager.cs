using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour, GameplayBootstrap.IBootstrap
{
    //<summary> Данный класс отвечает за управление состояниями игрового процесса
    //</summary>

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public event Action OnInitialized, OnGameStateChanged;

    [SerializeField]
    private PauseMenu _pauseMenu;

    public enum GameStates
    {
        PAUSE,
        ACTIVE,
        DEAD,
        EVACUATED
    }

    public GameStates CurrentState { get; private set; }

    void GameplayBootstrap.IBootstrap.Initialize()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        GameplayBootstrap.Instance.BootstrapFinished += OnBootstrapFinished;

        CurrentState = GameStates.ACTIVE;

        OnInitialized?.Invoke();

        StartCoroutine(UpdateProcess());
    }

    IEnumerator UpdateProcess()
    {
        GameStates prevState = CurrentState;
        while (true)
        {
            switch (CurrentState)
            {
                case GameStates.ACTIVE:
                    {
                        if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            CurrentState = GameStates.PAUSE;
                        }
                        break;
                    }
                case GameStates.PAUSE:
                    {
                        if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            CurrentState = GameStates.ACTIVE;
                        }
                        break;
                    }
                case GameStates.DEAD:
                    {

                        break;
                    }
                case GameStates.EVACUATED:
                    {

                        break;
                    }
            }

            if (prevState != CurrentState)
            {
                print("Game state changed to " + CurrentState);
                OnGameStateChanged?.Invoke();
                prevState = CurrentState;
            }

            yield return null;
        }
    }

    void OnBootstrapFinished()
    {
        _pauseMenu.onPauseButtonClicked += OnPauseButtonClicked;
        _pauseMenu.onResumeButtonClicked += OnResumeButtonClicked;

        GameplayBootstrap.Instance.BootstrapFinished -= OnBootstrapFinished;
    }

    void OnPauseButtonClicked()
    {
        CurrentState = GameStates.PAUSE;
    }

    void OnResumeButtonClicked()
    {
        CurrentState = GameStates.ACTIVE;
    }

    void OnMenuButtonClicked()
    {
        
    }
}
