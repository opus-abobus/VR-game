using UnityEngine;

namespace AppManagement.FSM.States {
    public class GameplayState : IAppState {

        private readonly AppContext _context;

        public GameplayState(AppContext context) {
            _context = context;
        }

        void IAppState.Enter() {

        }

        void IAppState.Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                _context.RequestStateTransition<GamePauseState>();
            }
        }

        void IAppState.Exit() {

        }
    }
}
