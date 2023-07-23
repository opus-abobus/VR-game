using System;
using System.Collections;
using UnityEngine;

public class GameSettingsManager : MonoBehaviour, Bootstrap.IBootstrap {
    [SerializeField]
    private Transform _gameSettingsRoot;

    public BerriesSettings BerriesSettings { get; private set; }
    public BananasSettings BananasSettings { get; private set; }
    public CoconutsSettings CoconutsSettings { get; private set; }
    public FoodSettings FoodSettings { get; private set; }
    public EvacSettings EvacSettings { get; private set; }
    public PlayerSettings PlayerSettings { get; private set; }

    private bool _berriesSettingsInitialized = false;
    private bool _bananasSettingsInitialized = false;
    private bool _coconutsSettingsInitialized = false;
    private bool _foodSettingsInitialized = false;
    private bool _evacSettingsInitialized = false;
    private bool _playerSettingsInitialized = false;

    private static GameSettingsManager _instance;
    public static GameSettingsManager Instance { get { return _instance; } }

    public bool HasInitialized { get; private set; } = false;

    public event Action Initialized;

    public void Initialize() {
        if (_instance == null) {
            _instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }

        if (_gameSettingsRoot == null) {
            throw new NullReferenceException("Game settings root was null.");
        }

        StartCoroutine(InitProcess()); 
    }

    IEnumerator InitProcess() {
        while (BerriesSettings == null) {
            BerriesSettings = _gameSettingsRoot.GetComponentInChildren<BerriesSettings>();
            yield return null;
        }
        BerriesSettings.Awaked += OnBerriesSettingsAwaked;
        BerriesSettings.Init();

        while (BananasSettings == null) {
            BananasSettings = _gameSettingsRoot.GetComponentInChildren<BananasSettings>();
            yield return null;
        }
        BananasSettings.Awaked += OnBananasSettingsAwaked;
        BananasSettings.Init();

        while (CoconutsSettings == null) {
            CoconutsSettings = _gameSettingsRoot.GetComponentInChildren<CoconutsSettings>();
            yield return null;
        }
        CoconutsSettings.Awaked += OnCoconutsSettingsAwaked;
        CoconutsSettings.Init();

        while (FoodSettings == null) {
            FoodSettings = _gameSettingsRoot.GetComponentInChildren<FoodSettings>();
            yield return null;
        }
        FoodSettings.Awaked += OnFoodSettingsAwaked;
        FoodSettings.Init();

        while (EvacSettings == null) {
            EvacSettings = _gameSettingsRoot.GetComponentInChildren<EvacSettings>();
            yield return null;
        }
        EvacSettings.Awaked += OnEvacSettingsAwaked;
        EvacSettings.Init();

        while (PlayerSettings == null) {
            PlayerSettings = _gameSettingsRoot.GetComponentInChildren<PlayerSettings>();
            yield return null;
        }
        PlayerSettings.Awaked += OnPlayerSettingsAwaked;
        PlayerSettings.Init();

        while (true) {
            yield return null;
            if (!_berriesSettingsInitialized) continue;
            if (!_bananasSettingsInitialized) continue;
            if (!_coconutsSettingsInitialized) continue;
            if (!_foodSettingsInitialized) continue;
            if (!_evacSettingsInitialized) continue;
            if (!_playerSettingsInitialized) continue;
            break;
        }
        print("all settings has been awakened (should be initialized).");

        Initialized?.Invoke();

        HasInitialized = true;

        UnsubscribeAll();

        yield return null;
    }

    void OnPlayerSettingsAwaked() {
        _playerSettingsInitialized = true;
    }

    void OnEvacSettingsAwaked() {
        _evacSettingsInitialized = true;
    }

    void OnFoodSettingsAwaked() {
        _foodSettingsInitialized = true;
    }

    void OnCoconutsSettingsAwaked() {
        _coconutsSettingsInitialized = true;
    }

    void OnBananasSettingsAwaked() {
        _bananasSettingsInitialized = true;
    }

    void OnBerriesSettingsAwaked() {
        _berriesSettingsInitialized = true;
    }

    void UnsubscribeAll() {
        BerriesSettings.Awaked -= OnBerriesSettingsAwaked;
        BananasSettings.Awaked -= OnBananasSettingsAwaked; 
        CoconutsSettings.Awaked -= OnCoconutsSettingsAwaked;
        FoodSettings.Awaked -= OnFoodSettingsAwaked;
        EvacSettings.Awaked -= OnEvacSettingsAwaked;
        PlayerSettings.Awaked -= OnPlayerSettingsAwaked;
    }
}
