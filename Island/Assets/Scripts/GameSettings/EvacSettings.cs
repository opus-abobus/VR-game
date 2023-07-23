using UnityEngine;

public class EvacSettings : GameSettings
{
    [Header("Параметры предметов для эвакуации")]

    [Range(0, 100)] 
    private int _rocketChance = 1;
    public int RocketChance { get { return _rocketChance; } private set {  _rocketChance = value; } }

    [Range(0, 100)] 
    private int _sosRocksChance = 1;
    public int SosRocksChance { get { return _sosRocksChance; } private set { _sosRocksChance = value; } }

    [Range(0, 100)] 
    private int _bonfireChance = 1;
    public int BonfireChance { get { return _bonfireChance; } private set { _bonfireChance = value; } }

    [SerializeField]
    private int _bonfireDuration = 100;
    public int BonfireDuration { get { return _bonfireDuration; } private set { _bonfireDuration = value; } }

    [SerializeField]
    private int _chanceTickRateInSeconds = 30;
    public int ChanceTickRateInSeconds { get { return _chanceTickRateInSeconds; } private set { _chanceTickRateInSeconds = value; } }

    [SerializeField]
    private int _signalGunsAmount = 1;
    public int SignalGunsAmount { get { return _signalGunsAmount; } private set { _signalGunsAmount = value; } }

    [SerializeField]
    private int _bonfiresAmount = 4;
    public int BonfiresAmount { get { return _bonfiresAmount; } private set { _bonfiresAmount = value; } }
}
