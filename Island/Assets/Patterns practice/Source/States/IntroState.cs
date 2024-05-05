using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroState : IUpdateableState {

    private readonly AppContext _context;
    private VideoPlayer _videoPlayer;
    private bool _isVideoEnded;

    public IntroState(AppContext context) {
        _context = context;
    }

    void IAppState.Enter() {
        SceneManager.LoadScene("Intro", LoadSceneMode.Single);
        Debug.Log("Scene loaded");

        _videoPlayer = UnityEngine.Object.FindObjectOfType<VideoPlayer>();

        _videoPlayer.loopPointReached += OnVideoClipEnded;
    }

    private void OnVideoClipEnded(VideoPlayer videoPlayer) {
        _isVideoEnded = true;
    }

    void IAppState.Exit() {
        _videoPlayer.loopPointReached -= OnVideoClipEnded;
    }

    void IUpdateableState.Update() {
        if (Input.anyKey || _isVideoEnded) {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }
}
