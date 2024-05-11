using System;
using UnityEngine.SceneManagement;
using static LevelSceneLoader;

namespace AppManagement.FSM.States {
    public class LoadingLevelState : IAppState {

        private AppContext _context;

        private const string LOADING_LEVEL_SCENE_NAME = "LevelLoading";
        private const string LEVEL_SCENE_NAME = "MainLevel";
        private bool _isLoadSceneLoaded = false;

        private LevelSceneLoader _levelSceneLoader;

        public LoadingLevelState(AppContext context) {
            _context = context;
        }

        void IAppState.Enter() {
            SceneManager.sceneLoaded += LoadingLevel_OnSceneLoaded;

            SceneManager.LoadSceneAsync(LOADING_LEVEL_SCENE_NAME, LoadSceneMode.Single);
        }

        void IAppState.Update() {
            if (!_isLoadSceneLoaded)
                return;

            if (_levelSceneLoader.State == LoadState.loadedAndNotActivated) {
                _context.RequestStateTransition<LevelLoadedState>();
            }
        }

        void IAppState.Exit() {

        }

        private void LoadingLevel_OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
            if (scene.name != LOADING_LEVEL_SCENE_NAME)
                return;

            SceneManager.sceneLoaded -= LoadingLevel_OnSceneLoaded;

            _levelSceneLoader = UnityEngine.Object.FindObjectOfType<LevelSceneLoader>();

            if (_levelSceneLoader == null) {
                throw new NullReferenceException("LevelSceneLoader component was not find in loaded scene.");
            }

            _isLoadSceneLoaded = true;

            _levelSceneLoader.StateChanged += OnLevelLoaderStateChanged;
            _levelSceneLoader.LoadLevelSceneViaCoroutine(LEVEL_SCENE_NAME, LoadSceneMode.Additive);
        }

        private void OnLevelLoaderStateChanged(LoadState loadState) {
            if (loadState == LoadState.loadedAndActivated) {
                _levelSceneLoader.StateChanged -= OnLevelLoaderStateChanged;

                SceneManager.sceneUnloaded += OnLoadingSceneUnloaded;
                SceneManager.UnloadSceneAsync(LOADING_LEVEL_SCENE_NAME);
            }
        }

        private void OnLoadingSceneUnloaded(Scene scene) {
            if (scene.name != LOADING_LEVEL_SCENE_NAME)
                return;

            SceneManager.sceneUnloaded -= OnLoadingSceneUnloaded;
        }
    }
}
