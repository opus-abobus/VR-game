using System;
using UnityEngine;
using AppManagement.FSM;
using AppManagement.FSM.States;
using DataPersistence;

[RequireComponent(typeof(AppStateMachine))]
public class AppManager : MonoBehaviour
{
    public static AppManager Instance { get; private set; }

    private AppStateMachine _appStateMachine;

    [SerializeField] private DataManager _dataManager;
    public DataManager DataManager { get { return _dataManager; } }

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        _appStateMachine = GetComponent<AppStateMachine>();
        _appStateMachine.Init();
    }

    private void Start() {
        PlayIntro();
    }

    private void Update() {
        //Debug.Log(_appStateMachine.AppContext.CurrentState);
    }

    private void PlayIntro() {
        _appStateMachine.EnterState<IntroState>();
    }

    public void LoadMainMenu() {
        _appStateMachine.EnterState<LoadingMainMenuState>();
    }

    public void LoadLevel() {
        _appStateMachine.EnterState<LoadingLevelState>();
    }

    public void PauseGame() {
        _appStateMachine.EnterState<GamePauseState>();
    }

    public void ResumeGame() {
        _appStateMachine.EnterState<GameplayState>();
    }

    public void ExitApp() {
        Application.Quit();
    }
}
