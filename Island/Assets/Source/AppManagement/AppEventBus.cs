using System;

namespace AppManagement
{
    public class AppEventBus
    {
        private AppEventBus() { }

        private static AppEventBus _instance;
        public static AppEventBus Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AppEventBus();
                }

                return _instance;
            }
        }

        public event Action OnGameLoading, OnGameLoaded, OnGamePaused, OnGameplay;

        public void TriggerGameLoading()
        {
            OnGameLoading?.Invoke();
        }

        public void TriggerGameLoaded()
        {
            OnGameLoaded?.Invoke();
        }

        public void TriggerGamePause()
        {
            OnGamePaused?.Invoke();
        }

        public void TriggerGameplay()
        {
            OnGameplay?.Invoke();
        }
    }
}
