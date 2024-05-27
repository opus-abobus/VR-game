using DataPersistence.Gameplay;
using System;
using System.Collections;
using UnityEngine;

public class BananaTreeGrowth : MonoBehaviour
{
    public event Action AllowRipening;

    [SerializeField]
    private bool _allowGrowth = true;

    [SerializeField, Range(0, 1)]
    private float _growningProgress = 0;

    private Vector3 _startScale, _endScale;

    private float _timeToGrowthInSeconds;

    public bool HasGrown { get; private set; }
    public bool HasInitialized { get; private set; } = false;

    private float _startRipeningMoment;

    public void SetData(BananaTreeData.BananaGrowthData data)
    {
        _allowGrowth = data.allowGrowth;
        HasGrown = data.hasGrown;
        _growningProgress = data.growthProgress;
        _timeToGrowthInSeconds = data.timeToGrowthInSeconds;
        _startScale = data.startScale;
        _endScale = data.endScale;

        transform.localScale = _startScale + (_endScale - _startScale) * _growningProgress;
    }

    public BananaTreeData.BananaGrowthData GetData()
    {
        return new BananaTreeData.BananaGrowthData(_allowGrowth, _growningProgress, _startScale, _endScale, 
            _timeToGrowthInSeconds, HasGrown);
    }

    public void Init(BananaTreeData.BananaGrowthData data) {
        WorldSettings.IBananaTreeSettings bananasSettings = GameSettingsManager.Instance.ActiveWorldSettings;
        _startRipeningMoment = bananasSettings.StartRipeningMoment;

        if (data != null)
        {
            SetData(data);
        }
        else
        {
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
        }

        HasInitialized = true;
    }

    public void StartGrowth() {
        if (HasGrown)
        {
            AllowRipening?.Invoke();
        }

        if (HasGrown || !_allowGrowth) return;

/*        if (!HasInitialized) {
            Init();
        }*/

        StartCoroutine(GrowningProcess());
    }

    IEnumerator GrowningProcess() {
        bool ripeningFlag = false;

        while (true) {
            if (_growningProgress >= 1) {
                HasGrown = true;

                break;
            }

            _growningProgress += Time.deltaTime / _timeToGrowthInSeconds;
            transform.localScale = _startScale + (_endScale - _startScale) * _growningProgress;

            if (!ripeningFlag) {
                if (_growningProgress >= _startRipeningMoment) {
                    AllowRipening?.Invoke();
                    ripeningFlag = true;
                }
            }

            yield return null;
        }
    }
}
