using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace AppManagement.FSM.States
{
    public class IntroState : IAppState
    {
        private readonly AppContext _context;

        private VideoPlayer _videoPlayer;

        private bool _isIntroStarted = false;

        private AsyncOperationHandle<SceneInstance> _loadScene;

        public IntroState(AppContext context)
        {
            _context = context;
        }

        void IAppState.Enter()
        {
            _loadScene = Addressables.LoadSceneAsync(ScenesDatabase.Instance.Intro, LoadSceneMode.Single, true);

            _loadScene.Completed += OnSceneLoaded;
        }

        void IAppState.Update()
        {
            if (!_isIntroStarted)
                return;

            if ((Input.anyKey && _videoPlayer.isPlaying) || !_videoPlayer.isPlaying)
            {
                _context.RequestStateTransition<LoadingMainMenuState>();
            }
        }

        void IAppState.Exit()
        {
            _videoPlayer.Pause();
        }

        private void OnSceneLoaded(AsyncOperationHandle<SceneInstance> asyncOperationHandle)
        {
            _loadScene.Completed -= OnSceneLoaded;
            _loadScene = default;

            _videoPlayer = UnityEngine.Object.FindObjectOfType<VideoPlayer>();

            if (_videoPlayer == null)
            {
                throw new NullReferenceException("VideoPlayer was not find in loaded scene.");
            }

            _videoPlayer.started += OnVideoClipStarted;

            if (!_videoPlayer.playOnAwake)
                _videoPlayer.Play();
        }

        private void OnVideoClipStarted(VideoPlayer videoPlayer)
        {
            _videoPlayer.started -= OnVideoClipStarted;

            _isIntroStarted = true;
        }
    }
}
