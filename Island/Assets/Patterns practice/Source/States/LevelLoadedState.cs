using SceneManagement;
using UnityEngine;

namespace AppManagement.FSM.States
{
    public class LevelLoadedState : IAppState
    {
        private AppContext _context;

        private LevelSceneLoader _sceneLoader;

        public LevelLoadedState(AppContext context)
        {
            _context = context;
        }

        void IAppState.Enter()
        {
            _sceneLoader = UnityEngine.Object.FindObjectOfType<LevelSceneLoader>();
            _sceneLoader.LoadedAndActivated += OnSceneActivated;
        }

        void IAppState.Update()
        {
            if (Input.anyKeyDown)
            {
                _sceneLoader.CompleteSceneActivation();
            }
        }

        void IAppState.Exit()
        {

        }

        private void OnSceneActivated()
        {
            _context.RequestStateTransition<GameplayState>();
        }
    }
}
