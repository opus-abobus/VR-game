using System;
using System.Collections.Generic;

public class AppContext
{
    public IAppState CurrentState { get; private set; }

    private Dictionary<Type, IAppState> _cachedStates = new Dictionary<Type, IAppState>();

    public void SetState<T>(T state) where T : IAppState {
        CurrentState?.Exit();
        CurrentState = state;
        CurrentState.Enter();
    }

    public void AddCachedState<T>(T state) where T : IAppState {
        if (_cachedStates.Count == 0) {
            _cachedStates = new Dictionary<Type, IAppState>();
        }

        if (!_cachedStates.ContainsKey(typeof(T))) {
            _cachedStates.Add(typeof(T), state);
        }
    }

    public void SetState<T>() where T : IAppState {
        if (!_cachedStates.ContainsKey(typeof(T))) {
            throw new InvalidOperationException("Trying to set unknown state");
        }

        CurrentState?.Exit();
        CurrentState = _cachedStates[typeof(T)];
        CurrentState.Enter();
    }

    public IAppState GetCachedState<T>() where T : IAppState {
        if (!_cachedStates.ContainsKey(typeof(T))) {
            return null;
        }

        return _cachedStates[typeof(T)];
    }

    public AppContext() {
        CurrentState = null;
    }

    public AppContext(IAppState appState) {
        CurrentState = appState;
    }
}
