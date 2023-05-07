using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOS_Manager : MonoBehaviour
{
    [SerializeField] GameObject[] rocks;
    Placeholding[] placeholdings;

    int placeholderCount = 0;

    private void Awake() {
        placeholdings = new Placeholding[rocks.Length];
        int i = 0;
        foreach (GameObject rock in rocks) {
            placeholdings.SetValue(rock.GetComponent<Placeholding>(), i);
            placeholdings[i].OnPlaceholding += UpdatePlaceholderCount;
            i++;
        }
        StartCoroutine(SOS_controller());
    }

    void UpdatePlaceholderCount() {
        placeholderCount++;
    }
    IEnumerator SOS_controller() {
        while (true) {
            if (transform.childCount == placeholderCount) {
                print("layed out!");
                break;
            }
            yield return null;
        } 
    }
}
