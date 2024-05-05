public class MainMenuState : IAppState {

    private readonly AppContext _context;
    public MainMenuState(AppContext context) {
        _context = context;
    }
    void IAppState.Enter() {
        throw new System.NotImplementedException();
    }

    void IAppState.Exit() {
        throw new System.NotImplementedException();
    }
}
