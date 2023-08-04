using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    private void Awake() {
        _startTimeScale = Time.timeScale;
        AudioListener.pause = false;
        OnResumeButtonClicked();
    }

    [HideInInspector] 
    public bool _pauseGame;

    [SerializeField]
    private GameObject _pauseGameMenu;

    [SerializeField] 
    private GameObject _overlay;

    public event Action onPauseButtonClicked, onResumeButtonClicked, onMenuButtonClicked;

    public bool PauseGame
    {
        get { return _pauseGame; }
        set { _pauseGame = value; }
    }

    private void Start() {
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    void OnGameStateChanged() {
        switch (GameManager.Instance.State) {
            case GameManager.GameStates.ACTIVE: {
                    OnResumeButtonClicked();
                    break;
                }
            case GameManager.GameStates.PAUSE: {
                    OnPauseButtonClicked(); 
                    break;
                }
        }
    }

    private void OnDisable() {
        GameManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private float _startTimeScale;

    public void OnResumeButtonClicked()
    {
        _pauseGameMenu.SetActive(false);
        Time.timeScale = _startTimeScale;
        _pauseGame = false;

        AudioListener.pause = false;
        _overlay.SetActive(true);

        onResumeButtonClicked?.Invoke();
    }
    
    public void OnPauseButtonClicked()
    {
        _pauseGameMenu.SetActive(true);
        Time.timeScale = 0;
        _pauseGame = true;

        AudioListener.pause = true;
        _overlay.SetActive(false);

        onPauseButtonClicked?.Invoke();
    }

    public void OnLoadMenuButtonClicked()
    {
        Time.timeScale = _startTimeScale;
        SceneManager.LoadScene("Menu");
    }
}
