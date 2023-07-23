using UnityEngine;

public class CoconutsSettings : GameSettings
{
    [Header("Настройка спавна и респавна кокосов")]

    [SerializeField, Range(0, 100)] 
    private int _minCoconutsOnStart = 0;
    public int MinCoconutsOnStart { get { return _minCoconutsOnStart; } private set { _minCoconutsOnStart = value; } }

    [SerializeField,  Range(0, 100)] 
    private int _maxCoconutsOnStart = 4;
    public int MaxCoconutsOnStart { get { return _maxCoconutsOnStart; } private set { _maxCoconutsOnStart = value; } }

    [SerializeField, Range(1, 100000)] 
    private int _minTimeToRespawnInSeconds;
    public int MinTimeToRespawnInSeconds { get { return _minTimeToRespawnInSeconds; } private set { _minTimeToRespawnInSeconds = value; } }

    [SerializeField, Range(1, 100000)] 
    private int _maxTimeToRespawnInSeconds;
    public int MaxTimeToRespawnInSeconds { get { return _maxTimeToRespawnInSeconds; } private set { _maxTimeToRespawnInSeconds = value; } }

    [SerializeField, Range(0, 1)]
    private float _chanceToSpawnOnStart = 0.4f;
    public float ChanceToSpawnOnStart { get { return _chanceToSpawnOnStart; } private set { _chanceToSpawnOnStart = value; } }
}
