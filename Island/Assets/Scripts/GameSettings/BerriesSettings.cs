using UnityEngine;

public class BerriesSettings : GameSettings
{
    [Header("Настройки спавна и респавна ягод")]

    [SerializeField, Range(0, 100), Tooltip("Минимальное количество ягод на старте игры")]
    private int _minBerriesOnStart;
    public int MinBerriesOnStart { 
        get {  return _minBerriesOnStart; } 
        private set {
            if (value < 0) value = 0;
            if (value > _maxBerriesOnStart) value = _maxBerriesOnStart;

            _minBerriesOnStart = value;
        }
    }


    [SerializeField, Range(0, 100), Tooltip("Максимальное количество ягод на старте игры")]
    private int _maxBerriesOnStart;
    public int MaxBerriesOnStart { 
        get { return _maxBerriesOnStart; } 
        private set {
            if (value < 0) value = 0;
            if (value < _minBerriesOnStart) value = _minBerriesOnStart;

            _maxBerriesOnStart = value; 
        }
    }


    [SerializeField, Range(1, 100000), Tooltip("Тайм-аут, \"заморозка\" респавна упавшей/собранной ягоды в секундах")]
    private int _timeoutBerryRespawnInSeconds;
    public int TimeoutBerryRespawnInSeconds { 
        get {
            return _timeoutBerryRespawnInSeconds; 
        } 
        private set { 
            _timeoutBerryRespawnInSeconds = value; 
        }
    }


    [SerializeField, Range(1, 100000), Tooltip("Время, после которого появится новая ягода после подбора/опадания старой")]
    private int _timeToRespawnBerryInSeconds;
    public int TimeToRespawnBerryInSeconds { 
        get { 
            return _timeToRespawnBerryInSeconds; 
        } 
        private set { 
            _timeToRespawnBerryInSeconds = value; 
        }
    }


    [SerializeField, Tooltip("Рандомизировать время респавна")]
    private bool _useRandomBerryRespawnTime;
    public bool UseRandomBerryRespawnTime { 
        get { 
            return _useRandomBerryRespawnTime; 
        } 
        private set { 
            _useRandomBerryRespawnTime = value; 
        }
    }


    [SerializeField, Range(1, 100000), Tooltip("Минимальное время респавна ягоды в секундах")]
    private int _minTimeToRespawnBerryInSeconds;
    public int MinTimeToRespawnBerryInSeconds { 
        get { 
            return _minTimeToRespawnBerryInSeconds; 
        } 
        private set { 
            _minTimeToRespawnBerryInSeconds = value; 
        }
    }


    [SerializeField, Range(1, 100000), Tooltip("Максимальное время респавна ягоды в секундах")]
    private int _maxTimeToRespawnBerryInSeconds;
    public int MaxTimeToRespawnBerryInSeconds { 
        get { 
            return _maxTimeToRespawnBerryInSeconds; 
        } 
        private set { 
            _maxTimeToRespawnBerryInSeconds = value; 
        } 
    }
}
