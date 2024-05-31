using DataPersistence.Gameplay;
using UnityEngine;
using UnityEngine.UI;

public class HungerSystem : MonoBehaviour
{
    [SerializeField, Tooltip("Начальная сытость"), Range(0, 1)]
    private float _startSatiety = 1;

    [SerializeField, Range(0, 1f)] private float _startHealth = 1;

    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _satietyBar;

    [SerializeField] private GameObject overlay;
    [SerializeField] private GameObject deathScreen;

    private bool isGameOver = false;
    public bool IsGameOver
    {
        get { return isGameOver; }
        set { isGameOver = value; }
    }

    [SerializeField]
    private float _satiety, _health;
    public float Satiety
    {
        get 
        { 
            return Mathf.Clamp01(_satiety); 
        }
        set 
        {
            _satiety = Mathf.Clamp01(value);
        }
    }

    [SerializeField] private LevelDataManager _levelDataManager;

    private WorldSettings _worldSettings;

    private float _healthDepletionRate, _satietyDepletionRate;
    private float _healthRestoreRateFromSatiety, _healthMaxThresholdFromSatiety,
        _healthIncreaseFromSatietyThreshold;

    public void Initialize(HungerSystemData data)
    {
        _levelDataManager.OnGameSave += OnGameSave;

        if (data != null)
        {
            _health = data.health;
            _satiety = data.satiety;
        }
        else
        {
            _satiety = _startSatiety;
            _health = _startHealth;
        }

        _worldSettings = GameSettingsManager.Instance.ActiveWorldSettings;

        _satietyDepletionRate = 1.0f / _worldSettings.satietyTime;
        _healthDepletionRate = 1.0f / _worldSettings.healthDecreaseTimeFromStarving;
        _healthRestoreRateFromSatiety = 1.0f / _worldSettings.healthIncreaseTimeFromSatiety;
        _healthMaxThresholdFromSatiety = _worldSettings.healthMaxThresholdFromSatiety;
        _healthIncreaseFromSatietyThreshold = _worldSettings.healthIncreaseFromSatietyThreshold;
    }

    private void OnGameSave(GameplayData data)
    {
        data.playerData.hungerSystemData = new HungerSystemData(_health, _satiety);
    }

    private void Start()
    {
        _healthBar.fillAmount = _health;
        _satietyBar.fillAmount = _satiety;
    }

    private bool isDead = false;
    private bool _allowDecreaseSatiety = true;

    private void Update()
    {
        if (isGameOver)
        {
            return;
        }

        if (_health < 0f)
        {
            isDead = true;
        }

        if (isDead && !isGameOver)
        {
            ShowScreenOfPlayerDeath();
            isGameOver = true;
            return;
        }

        if (_allowDecreaseSatiety)
        {
            _satiety = _satiety - _satietyDepletionRate * Time.deltaTime > 0 ? 
                _satiety - _satietyDepletionRate * Time.deltaTime : 0;
        }

        if (_satiety >= _healthIncreaseFromSatietyThreshold && _health < _healthIncreaseFromSatietyThreshold)
        {
            _health = Mathf.Clamp(_health + _healthRestoreRateFromSatiety * Time.deltaTime, 0, 
                _healthMaxThresholdFromSatiety);
        }
        else if (_satiety <= 0.001f)
        {
            _health -= _healthDepletionRate * Time.deltaTime;
        }

        _satietyBar.fillAmount = _satiety;
        _healthBar.fillAmount = _health;
    }

    private void ShowScreenOfPlayerDeath()
    {
        Debug.Log("Player died from starving");

        Time.timeScale = 0;
        AudioListener.pause = true;

        deathScreen.SetActive(true);
        overlay.SetActive(false);
    }

    private void OnDestroy()
    {
        _levelDataManager.OnGameSave -= OnGameSave;
    }
}