using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEatability : MonoBehaviour
{
    private bool _isEatable = false;
    public bool IsEatable {
        get { return _isEatable; }
        set { _isEatable = value; }
    }

    //[SerializeField] GameSettings globalSettings;

    //public bool useGlobalSettings = true;

    public float nutritionalValue = (float) 5 / 100;

    private void Awake() {
        SetNutritionalValue();
    }
    void SetNutritionalValue() {
        switch (this.gameObject.tag) {
            case "berry": {
                    nutritionalValue = (float) GameSettingsManager.Instance.FoodSettings.BerryNutrVal / 100;
                    break;
                }
            case "banana": {
                    nutritionalValue = (float) GameSettingsManager.Instance.FoodSettings.BananaNutrVal / 100;
                    break;
                }
            case "cocount":
            case "coconut": {
                    nutritionalValue = (float) GameSettingsManager.Instance.FoodSettings.CoconutNutrVal / 100;
                    break;
                }
        }
    }
}
