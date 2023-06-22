using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class InventoryManager : MonoBehaviour
{
    public GameObject panelWithSlots;
    public GameObject canvas;

    private void Awake() {
        canvas.SetActive(false);
    }

    [HideInInspector] public bool isInvenoryOpen = false;
    private void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            if (isInvenoryOpen) {
                isInvenoryOpen = false;
                canvas.SetActive(false);
            }
            else {
                isInvenoryOpen = true;
                canvas.SetActive(true);
            }
        }
    }
}
