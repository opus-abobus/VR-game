using UnityEngine;

namespace AppManagement.FSM.States {
    public class GamePauseState : IAppState {

        private readonly AppContext _context;

        public GamePauseState(AppContext context) {
            _context = context;
        }

        void IAppState.Enter() {

        }

        void IAppState.Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                _context.RequestStateTransition<GameplayState>();
            }
        }

        void IAppState.Exit() {

        }
    }
}
