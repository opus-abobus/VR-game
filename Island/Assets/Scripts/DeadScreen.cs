using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadScreen : MonoBehaviour
{
    [SerializeField] HungerSystem hungerSystem;
    [SerializeField] Image image;

    private void Start()
    {

    }

    [SerializeField] private float fadeTime;
    private YieldInstruction fadeInstruction = new YieldInstruction();
    IEnumerator FadeIn(Image image)
    {
        float elapsedTime = 0.0f;
        Color c = image.color;
        while (elapsedTime < fadeTime)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsedTime / fadeTime);
            image.color = c;
        }
    }

    public void StartVisible()
    {
        if (hungerSystem.IsGameOver)
        {
            StartCoroutine(FadeIn(image));
        }
    }
}