using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    public AppManager Instance { get; private set; }

    //public event Action OnUpdate;

    private AppStateMachine _appStateMachine;

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this);

        _appStateMachine = new AppStateMachine();
        //_appStateMachine.Init();

        PlayIntro();
    }

    private void Update() {
        _appStateMachine.UpdateState();
    }

    private void PlayIntro() {
        _appStateMachine.EnterState<IntroState>();
    }

    private void LoadMainMenu() {
        _appStateMachine.EnterState<LoadingMainMenuState>();
    }
}
