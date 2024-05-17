using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour, Bootstrap.IBootstrap
{
    //<summary> Данный класс отвечает за управление состояниями игрового процесса
    //</summary>

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public event Action OnInitialized, OnGameStateChanged;

    [SerializeField]
    private PauseMenu _pauseMenu;

    public enum GameStates {
        MENU,
        PAUSE,
        ACTIVE,
        DEAD
    }
    public GameStates State { get; private set; }

    void Bootstrap.IBootstrap.Initialize() {
        if (_instance == null) {
            _instance = this;
        }
        else {
            Destroy(gameObject);
        }

        Bootstrap.Instance.BootstrapFinished += OnBootstrapFinished;

        State = GameStates.ACTIVE;

        OnInitialized?.Invoke();

        StartCoroutine(UpdateProcess());
    }

    IEnumerator UpdateProcess() {
        GameStates lastState = State;
        while (true) {
            switch (State) {
                case GameStates.MENU: {
                        
                        break;
                    }
                case GameStates.ACTIVE: {
                        if (Input.GetKeyDown(KeyCode.Escape)) {
                            State = GameStates.PAUSE;
                        }
                        break;
                    }
                case GameStates.PAUSE: {
                        if (Input.GetKeyDown(KeyCode.Escape)) {
                            State = GameStates.ACTIVE;
                        }
                        break;
                    }
                case GameStates.DEAD: {

                        break;
                    }
            }

            if (lastState != State) {
                print("Game state changed to " + State);
                OnGameStateChanged?.Invoke();
                lastState = State;
            }

            yield return null;
        }
    }

    void OnBootstrapFinished() {
        //_pauseMenu.onPauseButtonClicked += OnPauseButtonClicked;
        //_pauseMenu.onResumeButtonClicked += OnResumeButtonClicked;
    }

    void OnPauseButtonClicked() {
        State = GameStates.PAUSE;
    }

    void OnResumeButtonClicked() {
        State = GameStates.ACTIVE;
    }

    void OnMenuButtonClicked() {
        State = GameStates.MENU;
    }

    private void OnDisable() {
        Bootstrap.Instance.BootstrapFinished -= OnBootstrapFinished;
    }
}
