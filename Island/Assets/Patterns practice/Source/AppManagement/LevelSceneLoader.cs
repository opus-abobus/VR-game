using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSceneLoader : MonoBehaviour
{
    public enum LoadState { none, loading, loadedAndNotActivated, loadedAndActivated }
    public LoadState State { get; private set; } = LoadState.none;

    public event Action<LoadState> StateChanged;

    public event Action LoadedAndNotActivated;
    public event Action LoadedAndActivated;

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

    public void LoadSceneAsync(AssetReference sceneAssetRef, LoadSceneMode loadSceneMode, bool activateSceneOnLoad)
    {
        StartCoroutine(LoadScene(sceneAssetRef, loadSceneMode, activateSceneOnLoad));
    }

    private IEnumerator LoadScene(AssetReference sceneAssetRef, LoadSceneMode loadSceneMode, bool activateSceneOnLoad)
    {
        AsyncOperationHandle<SceneInstance> handle = sceneAssetRef.LoadSceneAsync(loadSceneMode, activateSceneOnLoad);
        while (handle.Status == AsyncOperationStatus.None)
        {
            _progress = NormalizeProgress(handle.PercentComplete);
            SetProgress(_slider);
            yield return null;
        }

        LoadedAndNotActivated?.Invoke();
        _progress = handle.PercentComplete;
        SetProgress(_slider);

        _pressKeyText.gameObject.SetActive(true);
        _slider.gameObject.SetActive(false);

        if (!activateSceneOnLoad)
        {
            while (!_activateSceneWhenLoaded)
            {
                yield return null;
            }

            _activateSceneWhenLoaded = false;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                LoadedAndActivated?.Invoke();
                yield return handle.Result.ActivateAsync();
            }
        }
        else
        {
            LoadedAndActivated?.Invoke();
        }

        yield return null;
    }

    private void SetProgress(Slider slider) {
        slider.value = _progress;
    }

    public void CompleteSceneActivation()
    {
        _activateSceneWhenLoaded = true;
    }

    private float NormalizeProgress(float operationHandleProgress)
    {
        return (operationHandleProgress - 0.9f) / (1.0f - 0.9f);
    }
}
