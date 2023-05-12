using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadScreen : MonoBehaviour
{
    Color image;

    [SerializeField] HungerSystem hungerSystem;

    private void Start()
    {
        image = GetComponent<SpriteRenderer>().color;
    }

    /*IEnumerator VisibleSprite()
    {
        for (float f = 0.05f; f <= 1f; f += 0.05f)
        {
            Color color = image.color;
            color.a = f;
            image.color = new Color(255, 255, 255, f);
            yield return new WaitForSeconds(0.05f);
        }
    }*/

    public void StartVisible()
    {
        if (hungerSystem.IsGameOver)
        {
            StartCoroutine("VisibleSprite");
        }
    }
}