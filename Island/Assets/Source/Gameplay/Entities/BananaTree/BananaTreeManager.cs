using DataPersistence.Gameplay;
using UnityEngine;
using static SpawnManager;

[RequireComponent(typeof(BananaTreeGrowth), typeof(BananaRipening))]
public class BananaTreeManager : MonoBehaviour, ISpawner
{
    private BananaRipening _bananaRipening;

    private BananaTreeGrowth _bananaTreeGrowth;

    [SerializeField] private BananaDrop _bananaDrop;

    private bool _allowRipening;

    public SpawnerData GetData()
    {
        return new BananaTreeManagerData(gameObject.name, _bananaRipening.GetData(), _bananaTreeGrowth.GetData());
    }

    public void SetData<TSpawnerData>(TSpawnerData tSpawnerData) where TSpawnerData : SpawnerData
    {
        var data = tSpawnerData as BananaTreeManagerData;

        if (data != null)
        {
            _bananaRipening.SetData(data.ripeningData);
            _bananaTreeGrowth.SetData(data.growthData);

            if (data.ripeningData.allowRipening == true)
            {
                OnAllowRipening();
            }
        }
        else
        {
            //_bananaRipening.SetData(null);
            //_bananaTreeGrowth.SetData(null);
        }
    }

    public void Init()
    {
        _bananaRipening = GetComponent<BananaRipening>();
        _bananaTreeGrowth = GetComponent<BananaTreeGrowth>();

        _bananaRipening.Init();

        _bananaTreeGrowth.AllowRipening += OnAllowRipening;

        _bananaTreeGrowth.Init();
    }

    void ISpawner.BeginSpawn() {
        _bananaTreeGrowth.StartGrowth();
    }

    void OnAllowRipening() {
        _bananaTreeGrowth.AllowRipening -= OnAllowRipening;

        _bananaRipening.StartRipening();
    }
}
