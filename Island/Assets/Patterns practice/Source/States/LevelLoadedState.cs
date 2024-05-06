using UnityEngine;

namespace AppManagement.FSM.States {
    public class LevelLoadedState : IAppState {

        private AppContext _context;

        public LevelLoadedState(AppContext context) {
            _context = context;
        }

        void IAppState.Enter() {

        }

        void IAppState.Update() {
            if (Input.anyKeyDown) {
                UnityEngine.Object.FindObjectOfType<LevelSceneLoader>().ActivateScene();

                _context.RequestStateTransition<GameplayState>();
            }
        }

        void IAppState.Exit() {

        }
    }
}
