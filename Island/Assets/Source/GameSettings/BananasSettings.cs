using UnityEngine;

public class BananasSettings : GameSettings
{
    [Header("Настройки роста банановых пальм")]

    [SerializeField]
    private bool _useRandomTreeStartScale = true;
    public bool UseRandomTreeStartScale { get { return _useRandomTreeStartScale; } private set { _useRandomTreeStartScale = value; } }

    [SerializeField, Range(0.5f, 1)]
    private float _minTreeScale = 1;
    public float MinTreeScale { get { return _minTreeScale; } private set { _minTreeScale = value; } }

    [SerializeField, Range(1.1f, 2)]
    private float _maxTreeScale = 1.74f;
    public float MaxTreeScale { get { return _maxTreeScale; } private set { _maxTreeScale = value; } }

    [SerializeField, Range(0, 100000)]
    private float _timeToGrowthInSeconds;
    public float TimeToGrowthInSeconds { get { return _timeToGrowthInSeconds; } private set { _timeToGrowthInSeconds = value; } }
    [SerializeField]
    private bool _useRandomGrowthTime = true;
    public bool UseRandomGrowthTime { get { return _useRandomGrowthTime; } private set { _useRandomGrowthTime = value; } }
    [SerializeField, Range(1, 100000)] 
    private float _minTimeToGrowthInSeconds;
    public float MinTimeToGrowthInSeconds { get { return _minTimeToGrowthInSeconds; } private set { _minTimeToGrowthInSeconds = value; } }
    [SerializeField, Range(2, 100000)]
    private float _maxTimeToGrowthInSeconds;
    public float MaxTimeToGrowthInSeconds { get { return _maxTimeToGrowthInSeconds; } private set { _maxTimeToGrowthInSeconds = value; } }


    [Header("Настройки скорости созревания банановых пальм")]

    [SerializeField]
    private bool _useRandomRipePhaseDuration;
    public bool UseRandomRipePhaseDuration { get {  return _useRandomRipePhaseDuration; } private set { _useRandomRipePhaseDuration = value; } }

    [SerializeField, Range(1, 100000)]
    private float _ripePhaseDurationInSeconds = 4;
    public float RipePhaseDurationInSeconds { get { return _ripePhaseDurationInSeconds; } private set { _ripePhaseDurationInSeconds = value; } }
    
    [SerializeField, Range(1, 100000)]
    private float _minPhaseRipeDurationInSeconds;
    public float MinPhaseRipeDurationInSeconds { get { return _minPhaseRipeDurationInSeconds; } private set { _minPhaseRipeDurationInSeconds = value; } }

    [SerializeField, Range(1, 100000)] 
    private float _maxPhaseRipeDurationInSeconds;
    public float MaxPhaseRipeDurationInSeconds { get { return _maxPhaseRipeDurationInSeconds; } private set { _maxPhaseRipeDurationInSeconds = value; } }

    [SerializeField, Range(0, 1), Tooltip("С этого значения роста дерева начнется цикл созревания плодов")]
    private float startRipeningMoment = 0.8f;
    public float StartRipeningMoment { get { return startRipeningMoment; } private set { startRipeningMoment = value; } }


    [Header("Настройки урожайности банановых пальм")]

    [SerializeField, Range(1, 100)] 
    private int _minBananasToDrop = 1;
    public int MinBananasToDrop { get { return _minBananasToDrop; } private set { _minBananasToDrop = value; } }

    [SerializeField, Range(1, 100)] 
    private int _maxBananasToDrop = 10;
    public int MaxBananasToDrop { get { return _maxBananasToDrop; } private set { _maxBananasToDrop = value; } }
}
