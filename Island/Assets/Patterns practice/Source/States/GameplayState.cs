using UnityEngine;

public class GameplayState : IAppState {

    private AppContext _context;

    public GameplayState(AppContext context) {
        _context = context;
    }

    void IAppState.Enter() {
        if (_context.GetCachedState<GameplayState>() == null) {
            _context.AddCachedState(this);
        }

        if (_context.GetCachedState<GamePauseState>() == null) {
            _context.AddCachedState(new GamePauseState(_context));
        }
    }

    void IAppState.Update() {
        if (Input.GetKey(KeyCode.Escape)) {
            _context.SetState<GamePauseState>();
        }
    }

    void IAppState.Exit() {
        
    }
}
