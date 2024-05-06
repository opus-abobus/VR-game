using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameScreen : MonoBehaviour, Bootstrap.IBootstrap
{
    [SerializeField] 
    private GameObject _screen_survived;

    private float _startTimeScale;

    private void Awake() {
        _screen_survived.SetActive(false);
        _startTimeScale = Time.timeScale;
    }

    void Bootstrap.IBootstrap.Initialize() {
        StartCoroutine(UpdateProcess());
    }

    IEnumerator UpdateProcess() {
        while (true) {
            if (EvacuationSystem.Instance._isEvacuated && !_isActiveScreen) {
                DisplayEndGameScreen();
            }

            if (_isActiveScreen) {
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    _screen_survived.SetActive(false);
                    _isActiveScreen = false;
                    LoadMenu();
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public void LoadMenu() {
        Time.timeScale = _startTimeScale;
        SceneManager.LoadScene("Menu");
    }

    private bool _isActiveScreen = false;
    public void DisplayEndGameScreen() {
        if (EvacuationSystem.Instance._isEvacuated) {
            _screen_survived.SetActive(true);
            _isActiveScreen = true;
        }
    }
}