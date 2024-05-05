public class AppContext
{
    public IAppState CurrentState { get; private set; }

    public void SetState<T>(T state) where T : IAppState {
        CurrentState?.Exit();
        CurrentState = state;
        CurrentState.Enter();
    }

    public AppContext() {
        CurrentState = null;
    }

    public AppContext(IAppState appState) {
        CurrentState = appState;
    }
}
