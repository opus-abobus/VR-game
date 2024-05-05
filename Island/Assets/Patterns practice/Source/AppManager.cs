using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance { get; private set; }

    private AppContext _appContext;
    private IAppState _initialState;

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this);

        _appContext = new AppContext();

        _initialState = new IntroState(_appContext);
    }

    private void Start() {
        PlayIntro();
    }

    private void Update() {
        _appContext.CurrentState.Update();
    }

    private void PlayIntro() {
        _appContext.SetState(_initialState);
    }

    private void LoadMainMenu() {
        _appContext.SetState(new LoadingMainMenuState(_appContext));
    }

    public void LoadLevel() {
        _appContext.SetState(new LoadingLevelState(_appContext));
    }
}
