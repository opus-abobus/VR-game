using System;
using System.Collections.Generic;

public class AppStateMachine
{
    private Dictionary<Type, IAppState> _states;
    public AppContext AppContext { get; private set; }

    public AppStateMachine() {
        _states = new Dictionary<Type, IAppState>() {
            [typeof(IntroState)] = new IntroState(AppContext),
            [typeof(LoadingMainMenuState)] = new LoadingMainMenuState(AppContext),
            [typeof(MainMenuState)] = new MainMenuState(AppContext)
        };

        AppContext = new AppContext(_states[typeof(IntroState)]);
    }

/*    public void Init() {
        AppContext = new AppContext(_states[typeof(IntroState)]);
    }*/

/*    public void Init<T>() where T : IAppState {
        if (!_states.ContainsKey(typeof(T))) {
            throw new System.InvalidOperationException("Error on initialize. The machine does not contain input type.");
        }

        AppContext = new AppContext(_states[typeof(T)]);
    }*/

    public void EnterState<T>() where T : IAppState {
        if (!_states.ContainsKey(typeof(T))) {
            throw new System.NotImplementedException("The state in the argument was not defined");
        }

        AppContext.SetState(_states[typeof(T)]);
    }

/*    public void EnterState<T>(T appState) where T : IAppState {
        if (!_states.ContainsKey(typeof(T))) {
            throw new System.NotImplementedException("The state in the argument was not defined");
        }

        AppContext.SetState(appState);
    }*/

/*    public void UpdateState() {
        if (AppContext.CurrentState is IUpdateableState) {
            (AppContext.CurrentState as IUpdateableState).Update();
        }
    }*/

    public void AddState<T>(T state) where T : IAppState {
        if (_states.ContainsKey(typeof(T))) {
            return;
        }

        _states.Add(typeof(T), state);
    }

    public void RemoveState<T>() where T : IAppState {
        if (!_states.ContainsKey(typeof(T))) {
            return;
        }

        _states.Remove(typeof(T));
    }
}
