using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace AppManagement.FSM.States {
    public class IntroState : IAppState {

        private readonly AppContext _context;

        private const string SCENE_NAME = "Intro";

        private VideoPlayer _videoPlayer;
        private bool _isIntroStarted = false;

        public IntroState(AppContext context) {
            _context = context;
        }

        void IAppState.Enter() {
            SceneManager.sceneLoaded += OnSceneLoaded;

            SceneManager.LoadSceneAsync(SCENE_NAME, LoadSceneMode.Single);
        }

        void IAppState.Update() {
            if (!_isIntroStarted)
                return;

            if ((Input.anyKey && _videoPlayer.isPlaying) || !_videoPlayer.isPlaying) {
                _context.RequestStateTransition<LoadingMainMenuState>();
            }
        }

        void IAppState.Exit() {
            _videoPlayer.Pause();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
            if (scene.name != SCENE_NAME)
                return;

            SceneManager.sceneLoaded -= OnSceneLoaded;

            _videoPlayer = UnityEngine.Object.FindObjectOfType<VideoPlayer>();

            if (_videoPlayer == null) {
                throw new NullReferenceException("VideoPlayer was not find in loaded scene.");
            }

            _videoPlayer.started += OnVideoClipStarted;

            if (!_videoPlayer.playOnAwake)
                _videoPlayer.Play();
        }

        private void OnVideoClipStarted(VideoPlayer videoPlayer) {
            _videoPlayer.started -= OnVideoClipStarted;

            _isIntroStarted = true;
        }
    }
}
