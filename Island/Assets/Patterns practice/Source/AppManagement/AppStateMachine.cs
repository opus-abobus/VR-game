using System;
using System.Collections.Generic;
using UnityEngine;
using AppManagement.FSM.States;

namespace AppManagement.FSM {

    public class AppStateMachine : MonoBehaviour {
        public AppContext AppContext { get; private set; }

        private Dictionary<Type, IAppState> _states;

        private bool _allowUpdateState;

        private AppEventBus _appEventBus;

        public AppStateMachine() {
            AppContext = new AppContext();

            _appEventBus = AppEventBus.Instance;

            _states = new Dictionary<Type, IAppState>() {
                [typeof(IntroState)] = new IntroState(AppContext),
                [typeof(LoadingMainMenuState)] = new LoadingMainMenuState(AppContext),
                [typeof(MainMenuState)] = new MainMenuState(AppContext),
                [typeof(LoadingLevelState)] = new LoadingLevelState(AppContext),
                [typeof(LevelLoadedState)] = new LevelLoadedState(AppContext),
                [typeof(GameplayState)] = new GameplayState(AppContext),
                [typeof(GamePauseState)] = new GamePauseState(AppContext)
            };
        }

        public void Init() {
            AppContext.stateEntered += OnEnteredState;
            AppContext.stateExit += OnExitState;
            AppContext.stateTransitionRequested += OnStateTransitionRequested;
        }

        private void OnStateTransitionRequested(Type nextStateType) {
            if (nextStateType != null) {
                EnterState(nextStateType);
            }
            else {
                throw new InvalidOperationException("Can not make transition on request. The type was null.");
            }
        }

        private void OnEnteredState() {
            TriggerAppEvent(AppContext.CurrentState);

            _allowUpdateState = true;
        }

        private void TriggerAppEvent(IAppState state) {
            if (state == _states[typeof(LevelLoadedState)]) {
                _appEventBus.TriggerGameLoaded();
            }
            else if (state == _states[typeof(GameplayState)]) {
                _appEventBus.TriggerGameplay();
            }
            else if (state == _states[typeof(GamePauseState)]) {
                _appEventBus.TriggerGamePause();
            }
        }

        private void OnExitState() {
            _allowUpdateState = false;
        }

        public void EnterState<T>() where T : IAppState {
            if (!_states.ContainsKey(typeof(T))) {
                throw new System.NotImplementedException("State in the argument was not defined");
            }

            AppContext.SetState(_states[typeof(T)]);
        }

        private void EnterState(Type state) {
            if (!_states.ContainsKey(state)) {
                throw new System.NotImplementedException("State in the argument was not defined");
            }

            AppContext.SetState(_states[state]);
        }

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

        private void Update() {
            if (_allowUpdateState) {
                AppContext.CurrentState.Update();
            }
        }

        private void OnDestroy() {
            AppContext.stateEntered -= OnEnteredState;
            AppContext.stateExit -= OnExitState;
            AppContext.stateTransitionRequested -= OnStateTransitionRequested;
        }
    }
}
