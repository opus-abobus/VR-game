using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOS_Manager : MonoBehaviour
{
    [SerializeField] GameObject[] rocks;
    Placeholding[] placeholdings;

    int placeholderCount = 0;

    [HideInInspector] public static float dayTimeChance = 0.1f;
    [HideInInspector] public static float nightTimeChance = 0.01f;

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

    [HideInInspector] public bool isSOSLayedOut = false;
    IEnumerator SOS_controller() {
        while (true) {
            if (transform.childCount == placeholderCount) {
                isSOSLayedOut = true;
                EvacuationSystem.Instance.AddEvacItem(EvacuationSystem.EvacItem.TypesOfItems.sosRocks);

                Array.Clear(placeholdings, 0, placeholdings.Length);

                break;
            }
            yield return null;
        }
    }
}
