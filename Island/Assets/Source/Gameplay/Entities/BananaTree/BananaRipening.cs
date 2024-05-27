using DataPersistence.Gameplay;
using System;
using System.Collections;
using UnityEngine;

public class BananaRipening : MonoBehaviour
{
    [SerializeField]
    private GameObject _branch;

    [SerializeField]
    private GameObject _bananaPart;

    [SerializeField]
    private GameObject _unripeBananas;

    private bool _allowRipening = true;

    private RipePhase _ripePhase;

    private GameObject _bananaPartPrefab;

    [Range(0, 1), SerializeField]
    private float _ripeProgressPhase;

    private float _phaseTimeInSeconds;

    private bool _isBananasFallen = false;

    public bool HasInitialized { get; private set; } = false;

    private void OnAllowRipening()
    {
        _allowRipening = true;
    }

    public void SetData(BananaTreeData.BananaRipeningData data)
    {
        _allowRipening = data.allowRipening;
        _ripePhase = data.ripePhase;
        _ripeProgressPhase = data.ripeProgressPhase;
        _phaseTimeInSeconds = data.phaseTimeInSeconds;
        //_isBananasFallen = data.isBananaFallen;
        _isBananasFallen = false;

        if (_allowRipening)
        {
            _bananaPartPrefab = Instantiate(_bananaPart, _bananaPart.transform.position, _bananaPart.transform.rotation, _branch.transform);
            _bananaPartPrefab.SetActive(false);

            SetupForRipeState(_ripePhase);
            RipeProcess();
        }
        else
        {
            this.enabled = false;
        }
    }

    public BananaTreeData.BananaRipeningData GetData()
    {
        return new BananaTreeData.BananaRipeningData(_allowRipening, _ripePhase, _ripeProgressPhase, 
            _phaseTimeInSeconds, _isBananasFallen);
    }

    public void Init(BananaTreeData.BananaRipeningData data) {
        //---
        _bananaPart.GetComponent<BananaDrop>().FallenFruit += OnBananaDrop;
        GetComponent<BananaTreeGrowth>().AllowRipening += OnAllowRipening;
        //---

        if (data != null)
        {
            SetData(data);
        }
        else
        {
            SetRandomTreeState();

            WorldSettings.IBananaTreeSettings bananasSettings = GameSettingsManager.Instance.ActiveWorldSettings;
            if (bananasSettings.UseRandomRipePhaseDuration)
            {
                _phaseTimeInSeconds = UnityEngine.Random.Range(bananasSettings.MinPhaseRipeDurationInSeconds, bananasSettings.MaxPhaseRipeDurationInSeconds);
            }
            else
            {
                _phaseTimeInSeconds = bananasSettings.RipePhaseDurationInSeconds;
            }

            DetermineRipeState();
        }

        HasInitialized = true;
    }

    public void OnBananaDrop() {
        _bananaPart.GetComponent<BananaDrop>().FallenFruit -= OnBananaDrop;
        _isBananasFallen = true;
    }

    public void StartRipening() {
        if (!_allowRipening) return;

        StopCoroutine(RipeningProcess());
        StartCoroutine(RipeningProcess());
    }

    public void StopRipening() {
        StopCoroutine(RipeningProcess());
    }

    IEnumerator RipeningProcess() {
        while (true) {
            yield return null;
            if (!_allowRipening) {
                continue;
                //break;
            }

            RipeProcess();
            yield return null;
        }
    }

    void SetRandomTreeState() {
        switch (UnityEngine.Random.Range(1, 6)) {   
        // 0,4 - вероятность "пустого" дерева или незрелого плода, // 0,2 - вероятность созревшего плода
            case 1:
            case 2: {
                    // не имеет плода и ветки
                    _bananaPart.SetActive(true);
                    _branch.SetActive(false);
                    _unripeBananas.SetActive(false);
                    break;
                }
            case 3:
            case 4: {
                    // имеет несозревший плод
                    _unripeBananas.SetActive(true);
                    _bananaPart.SetActive(false);
                    _branch.SetActive(false);
                    break;
                }
            case 5: {
                    // имеет созревший плод
                    _unripeBananas.SetActive(false);
                    _bananaPart.SetActive(true);
                    _branch.SetActive(true);
                    break;
                }
        }
    }

    void RipeProcess() {
        if (_ripePhase == RipePhase.ripe && !_isBananasFallen) return;
        if (_ripePhase == RipePhase.ripe && _isBananasFallen) { 
            _ripePhase = RipePhase.fallen;
            return;
        }
        
        _ripeProgressPhase += Time.deltaTime / _phaseTimeInSeconds;

        if (_ripeProgressPhase > 1.0f) {
            switch (_ripePhase) {
                case RipePhase.empty: {
                        _ripePhase = RipePhase.hasUnripePart;
                        _branch.SetActive(false);
                        _bananaPart.SetActive(false);
                        _unripeBananas.SetActive(true);
                        break;
                    }
                case RipePhase.hasUnripePart: {
                        _ripePhase = RipePhase.ripe;
                        _unripeBananas.SetActive(false);
                        _bananaPart.SetActive(true);
                        _branch.SetActive(true);
                        break;
                    }
                case RipePhase.fallen: {
                        if (_bananaPart == null) {
                            _bananaPart = Instantiate(_bananaPartPrefab, _bananaPartPrefab.transform.position, _bananaPartPrefab.transform.rotation, _branch.transform);
                        }

                        _ripePhase = RipePhase.empty;
                        _isBananasFallen = false;
                        _bananaPart.SetActive(false);
                        _branch.SetActive(false);
                        _unripeBananas.SetActive(false);
                        break;
                    }
            }
            _ripeProgressPhase = 0;
        }
    }

    public enum RipePhase {
        empty,
        hasUnripePart,
        hasEmptyBranch,
        ripe,
        fallen
    }

    void DetermineRipeState() {
        bool isActiveUnripe = _unripeBananas.activeInHierarchy;

        if (_isBananasFallen) { _ripePhase = RipePhase.fallen; return; }
        if (isActiveUnripe) { _ripePhase = RipePhase.hasUnripePart; return; }
        if (_bananaPart.activeInHierarchy) { _ripePhase = RipePhase.ripe; return; }

        bool isActiveBranch = _branch.activeInHierarchy;

        if (isActiveBranch) { _ripePhase = RipePhase.hasEmptyBranch; return; }
        if (!(isActiveBranch || isActiveUnripe)) { _ripePhase = RipePhase.empty; return; }
        else Debug.LogAssertion("Состояние дерева не было определено");
    }

    private void SetupForRipeState(RipePhase phase)
    {
        _bananaPart.SetActive(false);
        _branch.SetActive(false);
        _unripeBananas.SetActive(false);

        switch (phase)
        {
            case RipePhase.hasUnripePart:
                {
                    _unripeBananas.SetActive(true);
                    _branch.SetActive(true);
                    break;
                }
            case RipePhase.fallen:
                {
                    _branch.SetActive(true);
                    _ripePhase = RipePhase.empty;
                    break;
                }
            case RipePhase.ripe:
                {
                    _branch.SetActive(true);
                    _bananaPart.SetActive(true);
                    break;
                }
            case RipePhase.empty:
                {
                    _branch.SetActive(true);
                    break;
                }
        }
    }
}
