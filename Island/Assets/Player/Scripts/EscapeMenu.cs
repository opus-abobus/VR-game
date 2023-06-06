using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour {
    private void Awake() {
        startTimeScale = Time.timeScale;
        AudioListener.pause = false;
    }

    [HideInInspector] public bool pauseGame;
    [SerializeField] GameObject pauseGameMenu;
    [SerializeField] GameObject overlay;
    [SerializeField] HungerSystem hungerSystem;

    public bool PauseGame
    {
        get { return pauseGame; }
        set { pauseGame = value; }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!hungerSystem.IsGameOver && !EvacuationSystem.instance.isEvacuated)
            {
                if (pauseGame)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
    }

    public void Resume()
    {
        pauseGameMenu.SetActive(false);
        Time.timeScale = startTimeScale;
        pauseGame = false;

        AudioListener.pause = false;
        overlay.SetActive(true);
    }

    float startTimeScale;
    public void Pause()
    {
        pauseGameMenu.SetActive(true);
        Time.timeScale = 0;
        pauseGame = true;

        AudioListener.pause = true;
        overlay.SetActive(false);
    }

    public void LoadMenu()
    {
        Time.timeScale = startTimeScale;
        SceneManager.LoadScene("Menu");
    }
}
