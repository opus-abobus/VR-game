using System;
using System.Collections;
using UnityEngine;
using static DataPersistence.Gameplay.BananaTreeManagerData;

public class BananaTreeGrowth : MonoBehaviour
{
    public event Action AllowRipening;

    [SerializeField] private bool _allowGrowth = true;

    [SerializeField, Range(0, 1)] private float _growningProgress = 0;

    private Vector3 _startScale, _endScale;

    private float _timeToGrowthInSeconds;

    public bool HasGrown { get; private set; }

    [SerializeField, Range(0, 1)] private float _startRipeningMoment;

    public void SetData(GrowthData data)
    {
        if (data != null)
        {
            _allowGrowth = data.allowGrowth;
            HasGrown = data.hasGrown;
            _growningProgress = data.growthProgress;
            _timeToGrowthInSeconds = data.timeToGrowthInSeconds;
            _startScale = data.startScale;
            _endScale = data.endScale;
            _startRipeningMoment = data.startRipeningMoment;

            transform.localScale = _startScale + (_endScale - _startScale) * _growningProgress;
        }
    }

    public GrowthData GetData()
    {
        return new GrowthData(_allowGrowth, _growningProgress, _startScale, _endScale,
            _timeToGrowthInSeconds, HasGrown, _startRipeningMoment);
    }

    public void Init()
    {
        WorldSettings.IBananaTreeSettings bananasSettings = GameSettingsManager.Instance.ActiveWorldSettings;
        _startRipeningMoment = bananasSettings.StartRipeningMoment;

        if (bananasSettings.UseRandomTreeStartScale)
        {
            _startScale = Vector3.one * UnityEngine.Random.Range(bananasSettings.MinTreeScale, bananasSettings.MaxTreeScale);
        }
        else
        {
            _startScale = Vector3.one * bananasSettings.MinTreeScale;
        }

        transform.localScale = _startScale;
        _endScale = Vector3.one * bananasSettings.MaxTreeScale;

        if (bananasSettings.UseRandomGrowthTime)
        {
            _timeToGrowthInSeconds = UnityEngine.Random.Range(bananasSettings.MinTimeToGrowthInSeconds, bananasSettings.MaxTimeToGrowthInSeconds);
        }
        else
        {
            _timeToGrowthInSeconds = bananasSettings.TimeToGrowthInSeconds;
        }

        if (bananasSettings.UseRandomStartGrowthProgress)
        {
            _growningProgress = UnityEngine.Random.Range(bananasSettings.MinStartGrowthProgress, bananasSettings.MaxStartGrowthProgress);
        }
        else
        {
            _growningProgress = bananasSettings.StartGrowthProgres;
        }
    }

    private bool _allowRipening = false;

    public void StartGrowth()
    {
        if (_growningProgress >= _startRipeningMoment)
        {
            _allowRipening = true;
            AllowRipening?.Invoke();
        }

        if (HasGrown || !_allowGrowth) return;

        StartCoroutine(GrowningProcess());
    }

    IEnumerator GrowningProcess()
    {
        while (true)
        {
            if (_growningProgress >= 1)
            {
                HasGrown = true;
                _growningProgress = 1.0f;
                break;
            }

            _growningProgress += Time.deltaTime / _timeToGrowthInSeconds;
            transform.localScale = _startScale + (_endScale - _startScale) * _growningProgress;

            if (!_allowRipening)
            {
                if (_growningProgress >= _startRipeningMoment)
                {
                    AllowRipening?.Invoke();
                    _allowRipening = true;
                }
            }

            yield return null;
        }
    }
}
