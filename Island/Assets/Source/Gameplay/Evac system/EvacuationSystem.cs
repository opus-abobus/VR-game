using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvacuationSystem : MonoBehaviour, GameplayBootstrap.IBootstrap
{
    private static EvacuationSystem _instance;
    public static EvacuationSystem Instance { get { return _instance; } }

    public event Action OnInitialized; 

    public bool _useGlobalSettings = true;

    [SerializeField] private EndGameScreen _endGameScreen;

    public int _rocketChance = 25;
    public int _sosRocksChance = 1;
    public int _bonfireChance = 5;
    public int _bonfireDuration = 100;

    public int _chanceTickRate = 60;

    private WorldSettings.IEvacSettings _evacSettings;

    private List<EvacItem> _evacItems;

    [HideInInspector] 
    public bool _isEvacuated = false;

    void GameplayBootstrap.IBootstrap.Initialize() {
        if (_instance == null) {
            _instance = this;
        }
        else {
            Destroy(this);
        }

        _evacSettings = GameSettingsManager.Instance.ActiveWorldSettings;

        //GameSettings.instance.sosRocksChance = 100;
        //print("fsd: " + GameSettings.instance.sosRocksChance);
        /*GameSettings.instance.bonfireChance = 25;
        print("chance: " + GameSettings.instance.bonfireChance + "\ndur: " + GameSettings.instance.bonfireDuration);*/

        _endGameScreen.Initialize();
        
        OnInitialized?.Invoke();
    }

    public class EvacItem {
        int _duration;
        int _evacChance;
        //bool isActive;

        Bonfire _bonfire = null;

        TypesOfItems _type;

        public enum TypesOfItems {
            rocket, bonfire, sosRocks
        }

        public EvacItem(TypesOfItems type, Bonfire bonfire = null, bool useGlobalSettings = true) {
            this._type = type;

            if (useGlobalSettings) _instance._chanceTickRate = _instance._evacSettings.ChanceTickRateInSeconds;

            switch (type) {
                case TypesOfItems.sosRocks: {
                        _duration = -1;
                        //isActive = false;
                        if (useGlobalSettings) {
                            _evacChance = _instance._evacSettings.SosRocksChance;
                        }
                        else {
                            _evacChance = _instance._sosRocksChance;
                        }
                        break;
                    }
                case TypesOfItems.rocket: {
                        _duration = 0;
                        //isActive = false;
                        if (useGlobalSettings) {
                            _evacChance = _instance._evacSettings.RocketChance;
                        }
                        else {
                            _evacChance = _instance._rocketChance;
                        }
                        break;
                    }
                case TypesOfItems.bonfire: {
                        //isActive = false;
                        this._bonfire = bonfire;
                        if (useGlobalSettings) {
                            _duration = _instance._evacSettings.BonfireDuration;
                            _evacChance = _instance._evacSettings.BonfireChance;
                        }
                        else {
                            _duration = _instance._bonfireDuration;
                            _evacChance = _instance._bonfireChance;
                        }
                        break;
                    }
            }
            //print("type: " + type + "   chance: " + _evacChance);
        }

        IEnumerator EvacuationProcess() {
            yield return new WaitForSeconds(1);
            int elapsedTime = 0;
            while (true) {
                RouletteWheelSelection();

                if (_instance._isEvacuated) {
                    if (_bonfire != null) { _bonfire.ParticleSystem.Stop(); _bonfire.AudioSource.Stop(); _bonfire._isFired = false; }
                    break;
                }

                yield return new WaitForSeconds(_instance._chanceTickRate);
                elapsedTime += _instance._chanceTickRate;

                if (elapsedTime >= _duration && _duration != -1) {
                    if (_bonfire != null) { _bonfire.ParticleSystem.Stop(); _bonfire.AudioSource.Stop(); _bonfire._isFired = false; }
                    break;
                }

                yield return null;
            }
            //isActive = false;
            _instance._evacItems.Remove(this);
        }

        void RouletteWheelSelection() {
            int failChance = 100 - _evacChance;
            int rnd = UnityEngine.Random.Range(0, _evacChance + failChance);
            //print("chance: " + rnd);
            //print("type: " + _type);
            while (rnd >= 0) {
                rnd -= _evacChance;
                if (rnd <= 0) {
                    // sucess!
                    _instance._isEvacuated = true;
                    print("Evacuated by " + _type);
                    break;
                }
                rnd -= failChance;
            }
        }
        public void ActivateItem() {
            //isActive = true;
            _instance.StartCoroutine(EvacuationProcess());
        }
    }

    public void AddEvacItem(EvacItem.TypesOfItems type, Bonfire bonfire = null, bool useGlobalSettings = true) {
        if (_evacItems == null) {
            _evacItems = new List<EvacItem>();
        }

        EvacItem item = new EvacItem(type, bonfire);
        _evacItems.Add(item);
        item.ActivateItem();
    }
}
