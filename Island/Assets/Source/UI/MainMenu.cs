using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void Quit()
    {
        Debug.Log("The game is closed");
        Application.Quit();
    }
}
