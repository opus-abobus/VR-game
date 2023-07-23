using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvacuationSystem : MonoBehaviour
{
    private static EvacuationSystem _instance;
    public static EvacuationSystem Instance { get { return _instance; } }

    public bool useGlobalSettings = true;

    public int rocketChance = 25;
    public int sosRocksChance = 1;
    public int bonfireChance = 5;
    public int bonfireDuration = 100;

    public int chanceTickRate = 60;

    [SerializeField]
    private EvacSettings _evacSettings;

    List<EvacItem> evacItems;
    [HideInInspector] public bool isEvacuated = false;

    private void Awake() {
        if (_instance == null) {
            _instance = this;
        }
        else {
            Destroy(this);
        }

        //GameSettings.instance.sosRocksChance = 100;
        //print("fsd: " + GameSettings.instance.sosRocksChance);
        /*GameSettings.instance.bonfireChance = 25;
        print("chance: " + GameSettings.instance.bonfireChance + "\ndur: " + GameSettings.instance.bonfireDuration);*/

    }

    public class EvacItem {
        int duration;
        int evacChance;
        //bool isActive;

        Bonfire bonfire = null;

        TypesOfItems type;

        public enum TypesOfItems {
            rocket, bonfire, sosRocks
        }

        public EvacItem(TypesOfItems type, Bonfire bonfire = null, bool useGlobalSettings = true) {
            this.type = type;

            if (useGlobalSettings) _instance.chanceTickRate = _instance._evacSettings.ChanceTickRateInSeconds;

            switch (type) {
                case TypesOfItems.sosRocks: {
                        duration = -1;
                        //isActive = false;
                        if (useGlobalSettings) {
                            evacChance = _instance._evacSettings.SosRocksChance;
                        }
                        else {
                            evacChance = _instance.sosRocksChance;
                        }
                        break;
                    }
                case TypesOfItems.rocket: {
                        duration = 0;
                        //isActive = false;
                        if (useGlobalSettings) {
                            evacChance = _instance._evacSettings.RocketChance;
                        }
                        else {
                            evacChance = _instance.rocketChance;
                        }
                        break;
                    }
                case TypesOfItems.bonfire: {
                        //isActive = false;
                        this.bonfire = bonfire;
                        if (useGlobalSettings) {
                            duration = _instance._evacSettings.BonfireDuration;
                            evacChance = _instance._evacSettings.BonfireChance;
                        }
                        else {
                            duration = _instance.bonfireDuration;
                            evacChance = _instance.bonfireChance;
                        }
                        break;
                    }
            }
            print("type: " + type + "   chance: " + evacChance);
        }

        IEnumerator EvacuationProcess() {
            yield return new WaitForSeconds(1);
            int elapsedTime = 0;
            while (true) {
                RouletteWheelSelection();

                if (_instance.isEvacuated) {
                    if (bonfire != null) { bonfire._particleSystem.Stop(); bonfire._audioSource.Stop(); bonfire._isFired = false; }
                    break;
                }

                yield return new WaitForSeconds(_instance.chanceTickRate);
                elapsedTime += _instance.chanceTickRate;

                if (elapsedTime >= duration && duration != -1) {
                    if (bonfire != null) { bonfire._particleSystem.Stop(); bonfire._audioSource.Stop(); bonfire._isFired = false; }
                    break;
                }

                yield return null;
            }
            //isActive = false;
            _instance.evacItems.Remove(this);
        }

        void RouletteWheelSelection() {
            int failChance = 100 - evacChance;
            int rnd = UnityEngine.Random.Range(0, evacChance + failChance);
            //print("chance: " + rnd);
            print("type: " + type);
            while (rnd >= 0) {
                rnd -= evacChance;
                if (rnd <= 0) {
                    // sucess!
                    _instance.isEvacuated = true;
                    print("Evacuated by " + type);
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
        if (evacItems == null || evacItems.Count == 0) {
            evacItems = new List<EvacItem>();
        }

        EvacItem item = new EvacItem(type, bonfire);
        evacItems.Add(item);
        item.ActivateItem();
    }
}
