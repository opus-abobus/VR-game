using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameScreen : MonoBehaviour
{
    [SerializeField] GameObject screen_survived;

    float startTimeScale;

    private void Awake() {
        screen_survived.SetActive(false);
        startTimeScale = Time.timeScale;
    }

    private void Update() {
        if (EvacuationSystem.Instance.isEvacuated && !isActiveScreen) {
            DisplayEndGameScreen();
        }

        if (isActiveScreen) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                screen_survived.SetActive(false);
                isActiveScreen = false;
                LoadMenu();
            }
        }
    }

    public void LoadMenu() {
        Time.timeScale = startTimeScale;
        SceneManager.LoadScene("Menu");
    }

    bool isActiveScreen = false;
    public void DisplayEndGameScreen() {
        if (EvacuationSystem.Instance.isEvacuated) {
            screen_survived.SetActive(true);
            isActiveScreen = true;
        }
    }
}
