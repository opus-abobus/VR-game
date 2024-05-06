using UnityEngine;

public class GamePauseState : IAppState {

    private AppContext _context;

    public GamePauseState(AppContext context) {
        _context = context;
    }

    void IAppState.Enter() {
        
    }

    void IAppState.Update() {
        if (Input.GetKey(KeyCode.Escape)) {
            _context.SetState<GameplayState>();
        }
    }

    void IAppState.Exit() {
        
    }
}
