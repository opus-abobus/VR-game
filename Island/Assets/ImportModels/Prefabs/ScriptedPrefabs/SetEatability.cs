using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEatability : MonoBehaviour
{
    bool isEatable = false;
    public bool IsEatable {
        get { return isEatable; }
        set { isEatable = value; }
    }

    [SerializeField] GameSettings globalSettings;
    public bool useGlobalSettings = true;

    public int nutritionalValue = 5;

    private void Awake() {
        SetNutritionalValue();
    }
    void SetNutritionalValue() {
        switch (this.gameObject.tag) {
            case "berry": {
                    if (useGlobalSettings && globalSettings != null) {
                        nutritionalValue = globalSettings.nutritionalValue_Berry;
                    }
                    break;
                }
            case "banana": {
                    if (useGlobalSettings && globalSettings != null) {
                        nutritionalValue = globalSettings.nutritionalValue_Banana;
                    }
                    break;
                }
            case "cocount":
            case "coconut": {
                    if (useGlobalSettings && globalSettings != null) {
                        nutritionalValue = globalSettings.nutritionalValue_Coconut;
                    }
                    break;
                }
        }
    }
}
