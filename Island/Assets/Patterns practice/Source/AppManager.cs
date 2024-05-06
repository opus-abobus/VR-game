using System;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance { get; private set; }

    private AppContext _appContext;

    public event Action GamePaused;

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this);

        _appContext = new AppContext();
    }

    private void Start() {
        PlayIntro();
    }

    private void Update() {
        _appContext.CurrentState.Update();

        print(_appContext.CurrentState);

        if (_appContext.CurrentState == _appContext.GetCachedState<GamePauseState>()) {
            GamePaused?.Invoke();
        }
    }

    private void PlayIntro() {
        _appContext.SetState(new IntroState(_appContext));
    }

    public void LoadMainMenu() {
        _appContext.SetState(new LoadingMainMenuState(_appContext));
    }

    public void LoadLevel() {
        _appContext.SetState(new LoadingLevelState(_appContext));
    }

    public void ResumeGame() {
        _appContext.SetState<GameplayState>();
    }

    public void ExitApp() {
        Application.Quit();
    }
}
