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

    public int nutritionalValue = 5;
    [SerializeField] bool useDefaultNutritionalValues = false;

    private void Awake() {
        if (useDefaultNutritionalValues) SetDefaultNutritionalValues();
    }

    void SetDefaultNutritionalValues() {
        switch (this.gameObject.tag) {
            case "berry": {
                    nutritionalValue = 2;
                    break;
                }
            case "banana": {
                    nutritionalValue = 7;
                    break;
                }
            case "cocount": {
                    nutritionalValue = 5;
                    break;
                }
            case "coconut": {
                    nutritionalValue = 5;
                    break;
                }
        }
    }
}
