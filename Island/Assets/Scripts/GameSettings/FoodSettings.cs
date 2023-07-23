using UnityEngine;

public class FoodSettings : GameSettings
{
    [Header("Параметры питательности объектов еды")]

    [SerializeField, Range(0, 100)]
    private int _nutritionalValue_Berry = 5;
    public int BerryNutrVal { get { return _nutritionalValue_Berry; } private set { _nutritionalValue_Berry = value; } }

    [SerializeField, Range(0, 100)]
    private int _nutritionalValue_Coconut = 5;
    public int CoconutNutrVal { get { return _nutritionalValue_Coconut; } private set { _nutritionalValue_Coconut = value; } }

    [SerializeField, Range(0, 100)] 
    private int _nutritionalValue_Banana = 5;
    public int BananaNutrVal { get { return _nutritionalValue_Banana; } private set { _nutritionalValue_Banana = value; } }
}
