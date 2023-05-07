using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour
{
    private void Awake() {
        timeScale = Time.timeScale;
    }

    private bool pauseGame;
    [SerializeField] GameObject pauseGameMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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

    public void Resume()
    {
        pauseGameMenu.SetActive(false);
        Time.timeScale = timeScale;
        pauseGame = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    float timeScale;
    public void Pause()
    {
        pauseGameMenu.SetActive(true);
        Time.timeScale = 0;
        pauseGame = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = timeScale;
        SceneManager.LoadScene("Menu");
    }
}
