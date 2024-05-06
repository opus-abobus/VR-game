using UnityEngine;
using UnityEngine.SceneManagement;

namespace AppManagement.FSM.States {
    public class LoadingMainMenuState : IAppState {

        private const string SCENE_NAME = "MainMenu";

        private readonly AppContext _context;
        private AsyncOperation _loadScene;

        public LoadingMainMenuState(AppContext context) {
            _context = context;
        }

        void IAppState.Enter() {
            _loadScene = SceneManager.LoadSceneAsync(SCENE_NAME, LoadSceneMode.Single);
        }

        void IAppState.Update() {
            if (_loadScene.isDone) {
                _context.RequestStateTransition<MainMenuState>();
            }
        }

        void IAppState.Exit() {

        }
    }
}
