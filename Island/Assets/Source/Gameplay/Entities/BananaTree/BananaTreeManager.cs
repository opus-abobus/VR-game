using UnityEngine;
using static SpawnManager;

[RequireComponent(typeof(BananaTreeGrowth), typeof(BananaRipening))]
public class BananaTreeManager : MonoBehaviour, ISpawner
{
    //[SerializeField]
    private BananaRipening _bananaRipening;

    //[SerializeField]
    private BananaTreeGrowth _bananaTreeGrowth;

    private ISpawner _spawner;

    private void Awake() {
        _spawner = this;
    }

    private bool _hasInitialized = false;
    bool ISpawner.HasInitialized { get { return _hasInitialized; } }

    private static bool _hasStarted = false;
    public static bool HasStarted { get { return _hasStarted; } }

    private void Start() {
        _hasStarted = true;
    }

    void ISpawner.Init() {
        _bananaRipening = GetComponent<BananaRipening>();
        _bananaTreeGrowth = GetComponent<BananaTreeGrowth>();

        _bananaRipening.Init();
        _bananaTreeGrowth.Init();

        _bananaTreeGrowth.AllowRipening += OnAllowRipening;

        _hasInitialized = true;
    }

    void ISpawner.BeginSpawn() {
        if (!_hasInitialized) {
            _spawner.Init();
        }
        _bananaTreeGrowth.StartGrowth();
    }

    void OnAllowRipening() {
        _bananaTreeGrowth.AllowRipening -= OnAllowRipening;

        _bananaRipening.StartRipening();
    }
}
