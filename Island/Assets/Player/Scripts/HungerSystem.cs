using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerSystem : MonoBehaviour
{
    [SerializeField, Tooltip("Сколько секунд пройдет, чтобы вычесть один процент сытости")] int secondsPerPercent = 2;
    [SerializeField, Tooltip("Начальная сытость в процентах"), Range(1, 100)] int startSatiety = 70;

    int satiety;
    public int Satiety {
        get { return satiety; }
        set { satiety = value; }
    }

    IEnumerator _StarvingProcess;
    private void Awake() {
        satiety = startSatiety;
        _StarvingProcess = StarvingProcess();
        StartCoroutine(_StarvingProcess);
    }
    IEnumerator StarvingProcess() {
        yield return new WaitForSeconds(secondsPerPercent);
        while (true) {
            satiety -= 1;
            yield return new WaitForSeconds(secondsPerPercent);
        }
    }
    private void Update() {
        if (satiety == 0) {
            StopCoroutine(_StarvingProcess);
            StartCoroutine(ShowScreenOfPlayerDeath());
        }
        if (satiety > 100) { satiety = 100; }
    }
    IEnumerator ShowScreenOfPlayerDeath() {
        Debug.Log("Player died from starving");
        yield return null;
    }
}
