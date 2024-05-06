using System;

namespace AppManagement {
    public class AppEventBus {

        private static AppEventBus _instance;
        public static AppEventBus Instance {
            get {
                if (_instance == null) {
                    _instance = new AppEventBus();
                }

                return _instance;
            }
        }

        public event Action OnGameLoaded, OnGamePaused, OnGameplay;

        public void TriggerGameLoaded() {
            OnGameLoaded?.Invoke();
        }

        public void TriggerGamePause() {
            OnGamePaused?.Invoke();
        }

        public void TriggerGameplay() {
            OnGameplay?.Invoke();
        }
    }
}
