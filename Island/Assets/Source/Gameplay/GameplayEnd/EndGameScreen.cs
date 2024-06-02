using SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameScreen : MonoBehaviour
{
    [SerializeField] 
    private GameObject _screen_survived;

    [SerializeField] private float _timeToExit = 1.0f;

    private float _startTimeScale;

    private void Awake() {
        _screen_survived.SetActive(false);
        _startTimeScale = Time.timeScale;
    }

    public void Initialize() {
        StartCoroutine(UpdateProcess());
    }

    IEnumerator UpdateProcess() {
        while (true) {
            if (EvacuationSystem.Instance._isEvacuated && !_isActiveScreen) {
                DisplayEndGameScreen();
                yield return new WaitForSeconds(_timeToExit);
                LoadMenu();
            }

/*            if (_isActiveScreen) {
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    //_screen_survived.SetActive(false);
                    //_isActiveScreen = false;

                    yield return new WaitForSeconds(_timeToExit);

                    LoadMenu();
                }
            }*/

            yield return null;
        }
    }

    public async void LoadMenu() {
        Time.timeScale = _startTimeScale;
        //SceneManager.LoadScene("Menu");

        SceneLoader2 sceneLoader = new SceneLoader2();
        await sceneLoader.LoadSceneAsync(ScenesDatabase.Instance.MainMenu, LoadSceneMode.Single, true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private bool _isActiveScreen = false;
    public void DisplayEndGameScreen() {
        if (EvacuationSystem.Instance._isEvacuated) {
            _screen_survived.SetActive(true);
            _isActiveScreen = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
