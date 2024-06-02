using SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadScreenController : MonoBehaviour
{
    [SerializeField] HungerSystem hungerSystem;
    [SerializeField] GameObject _screen;

    [SerializeField] private float _timeToExit = 2.0f;

    private bool _isActive = false;

    private float _startTimeScale;

    private void Awake()
    {
        _startTimeScale = Time.timeScale;
    }

    private float _elapsedTime = 0;
    private float _startTime;
    private void Update()
    {
        if (hungerSystem.IsGameOver && !_isActive)
        {
            ShowScreen();
            _isActive = true;
            _startTime = Time.unscaledTime;
        }
        else if (hungerSystem.IsGameOver)
        {
            _elapsedTime = Time.unscaledTime - _startTime;
            if (_elapsedTime > _timeToExit)
            {
                LoadMenu();
            }
        }
    }

    private void ShowScreen()
    {
        _screen.SetActive(true);
    }

    public async void LoadMenu()
    {
        Time.timeScale = _startTimeScale;

        SceneLoader2 sceneLoader = new SceneLoader2();
        await sceneLoader.LoadSceneAsync(ScenesDatabase.Instance.MainMenu, LoadSceneMode.Single, true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
