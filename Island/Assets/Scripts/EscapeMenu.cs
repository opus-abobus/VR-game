using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour
{
    private void Awake() {
        startTimeScale = Time.timeScale;
        AudioListener.pause = false;
    }

    private bool pauseGame;
    [SerializeField] GameObject pauseGameMenu;
    [SerializeField] GameObject overlay;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HungerSystem hungerSystem = new HungerSystem();

            if (!hungerSystem.IsGameOver)
            {
                if (pauseGame)
                {
                    Resume();
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                else
                {
                    Pause();
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
        }
    }

    public void Resume()
    {
        pauseGameMenu.SetActive(false);
        Time.timeScale = startTimeScale;
        pauseGame = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

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
