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

    public float nutritionalValue = (float) 5 / 100;

    private void Awake() {
        SetNutritionalValue();
    }
    void SetNutritionalValue() {
        switch (this.gameObject.tag) {
            case "berry": {
                    if (useGlobalSettings && globalSettings != null) {
                        nutritionalValue = (float) globalSettings.nutritionalValue_Berry / 100;
                    }
                    break;
                }
            case "banana": {
                    if (useGlobalSettings && globalSettings != null) {
                        nutritionalValue = (float) globalSettings.nutritionalValue_Banana / 100;
                    }
                    break;
                }
            case "cocount":
            case "coconut": {
                    if (useGlobalSettings && globalSettings != null) {
                        nutritionalValue = (float) globalSettings.nutritionalValue_Coconut / 100;
                    }
                    break;
                }
        }
    }
}
