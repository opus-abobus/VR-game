using System;
using AppManagement.FSM.States;

namespace AppManagement.FSM {
    public class AppContext {
        public IAppState CurrentState { get; private set; } = null;

        public event Action stateEntered, stateExit;
        public event Action<Type> stateTransitionRequested;
        public void SetState<T>(T state) where T : IAppState {
            if (CurrentState != null) {

                CurrentState.Exit();

                stateExit?.Invoke();
            }
  
            state.Enter();
            CurrentState = state;

            stateEntered?.Invoke();
        }
        public void RequestStateTransition<T>() where T : IAppState {
            stateTransitionRequested?.Invoke(typeof(T));
        }
    }
}
