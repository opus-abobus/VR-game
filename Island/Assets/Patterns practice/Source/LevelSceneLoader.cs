using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSceneLoader : MonoBehaviour
{
    public enum LoadState { none, loading, loadedAndNotActivated, loadedAndActivated }
    public LoadState State { get; private set; } = LoadState.none;

    public event Action<LoadState> StateChanged;

    [SerializeField]
    private Slider _slider;

    [SerializeField]
    private TextMeshProUGUI _pressKeyText;
    
    private float _progress = 0;

    private bool _activateSceneWhenLoaded = false;

    public void LoadLevelSceneViaCoroutine(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single) {
        StartCoroutine(LoadScene(sceneName, loadSceneMode));
    }

    private IEnumerator LoadScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        operation.allowSceneActivation = false;

        State = LoadState.loading;
        StateChanged?.Invoke(State);

        while (!operation.isDone) {
            _progress = operation.progress * 1.1f;

            SetProgress(_slider);

            if (operation.progress >= 0.9f) {
                _pressKeyText.gameObject.SetActive(true);
                _slider.gameObject.SetActive(false);

                State = LoadState.loadedAndNotActivated;
                StateChanged?.Invoke(State);

                while (!_activateSceneWhenLoaded) {
                    yield return null;
                }

                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        State = LoadState.loadedAndActivated;
        StateChanged?.Invoke(State);

        yield return null;
    }

    public void ActivateScene() {
        _activateSceneWhenLoaded = true;
    }

    private void SetProgress(Slider slider) {
        slider.value = _progress;
    }
}
