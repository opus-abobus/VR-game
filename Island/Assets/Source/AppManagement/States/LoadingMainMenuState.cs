using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace AppManagement.FSM.States
{
    public class LoadingMainMenuState : IAppState
    {
        private readonly AppContext _context;

        private AsyncOperationHandle<SceneInstance> _loadScene;

        public LoadingMainMenuState(AppContext context)
        {
            _context = context;
        }

        void IAppState.Enter()
        {
            _loadScene = Addressables.LoadSceneAsync(ScenesDatabase.Instance.MainMenu, LoadSceneMode.Single, true);
            _loadScene.Completed += OnSceneLoaded;
        }

        void IAppState.Update()
        {

        }

        void IAppState.Exit()
        {

        }

        private void OnSceneLoaded(AsyncOperationHandle<SceneInstance> asyncOperationHandle)
        {
            _loadScene.Completed -= OnSceneLoaded;
            _loadScene = default;

            _context.RequestStateTransition<MainMenuState>();
        }
    }
}
