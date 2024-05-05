public class LoadingMainMenuState : IAppState {

    private readonly AppContext _context;

    public LoadingMainMenuState(AppContext context) {
        _context = context;
    }

    void IAppState.Enter() {
        throw new System.NotImplementedException();
    }

    void IAppState.Exit() {
        throw new System.NotImplementedException();
    }
}
