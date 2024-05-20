using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader2
{
    private TaskCompletionSource<bool> _activationTaskSource;

    public event Action LoadedAndNotActivated;
    public event Action LoadedAndActivated;
    public event Action<float> ProgressUpdated;

    public async Task<AsyncOperationHandle<SceneInstance>> LoadSceneAsync(AssetReference sceneAssetRef, LoadSceneMode loadSceneMode, bool activateOnLoad)
    {
        AsyncOperationHandle<SceneInstance> loadScene = Addressables.LoadSceneAsync(sceneAssetRef, loadSceneMode, activateOnLoad);

        TrackProgress(loadScene);

        while (loadScene.PercentComplete <= 0.89f)
        {
            await Task.Yield();
        }

        if (activateOnLoad)
        {
            await loadScene.Task;

            LoadedAndActivated?.Invoke();
            return loadScene;
        }
        else
        {
            _activationTaskSource = new TaskCompletionSource<bool>();
            await _activationTaskSource.Task;

            AsyncOperation activateOperaton = loadScene.Result.ActivateAsync();

            while (!activateOperaton.isDone)
            {
                await Task.Yield();
            }

            return loadScene;
        }
    }

    public void CompleteSceneActivation()
    {
        _activationTaskSource?.TrySetResult(true);
    }

    private async void TrackProgress(AsyncOperationHandle<SceneInstance> loadScene)
    {
        while (!loadScene.IsDone)
        {
            ProgressUpdated?.Invoke(loadScene.PercentComplete);
            await Task.Yield();
        }

        ProgressUpdated?.Invoke(1);
    }

    private float NormilizeProgress(float operationHandleProgress)
    {
        return (operationHandleProgress - 0.9f) / (1.0f - 0.9f);
    }
}
