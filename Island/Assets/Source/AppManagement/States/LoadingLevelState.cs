using SceneManagement;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using LoadSceneMode = UnityEngine.SceneManagement.LoadSceneMode;

namespace AppManagement.FSM.States
{
    public class LoadingLevelState : IAppState
    {
        private AppContext _context;

        private SceneLoader _levelSceneLoader;

        private AsyncOperationHandle<SceneInstance> _loadScene;

        public LoadingLevelState(AppContext context)
        {
            _context = context;
        }

        void IAppState.Enter()
        {
            _loadScene = Addressables.LoadSceneAsync(ScenesDatabase.Instance.LevelLoading, LoadSceneMode.Single, true);

            _loadScene.Completed += LoadingLevel_OnSceneLoaded;
        }

        void IAppState.Update()
        {

        }

        void IAppState.Exit()
        {

        }

        private void LoadingLevel_OnSceneLoaded(AsyncOperationHandle<SceneInstance> asyncOperationHandle)
        {
            _loadScene.Completed -= LoadingLevel_OnSceneLoaded;
            _loadScene = default;

            _levelSceneLoader = UnityEngine.Object.FindObjectOfType<SceneLoader>();

            if (_levelSceneLoader == null)
            {
                throw new NullReferenceException("LevelSceneLoader component was not find in loaded scene.");
            }

            _levelSceneLoader.LoadedAndNotActivated += OnLevelLoadedAndNotActivated;
            _levelSceneLoader.LoadSceneAsync(ScenesDatabase.Instance.MainLevel, LoadSceneMode.Single, false);
        }

        private void OnLevelLoadedAndNotActivated()
        {
            _levelSceneLoader.LoadedAndNotActivated -= OnLevelLoadedAndNotActivated;

            _context.RequestStateTransition<LevelLoadedState>();
        }
    }
}
