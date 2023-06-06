using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvacuationSystem : MonoBehaviour
{
    public static EvacuationSystem instance;
    public bool useGlobalSettings = true;

    public int rocketChance = 25;
    public int sosRocksChance = 1;
    public int bonfireChance = 5;
    public int bonfireDuration = 100;

    List<EvacItem> evacItems;
    bool isEvacuated = false;

    private void Awake() {
        instance = this;
        //GameSettings.instance.sosRocksChance = 100;
        //print("fsd: " + GameSettings.instance.sosRocksChance);
        /*GameSettings.instance.bonfireChance = 25;
        print("chance: " + GameSettings.instance.bonfireChance + "\ndur: " + GameSettings.instance.bonfireDuration);*/

    }

    public class EvacItem {
        int duration;
        int evacChance;
        bool isActive;

        Bonfire bonfire = null;

        TypesOfItems type;

        public enum TypesOfItems {
            rocket, bonfire, sosRocks
        }

        public EvacItem(TypesOfItems type, Bonfire bonfire = null, bool useGlobalSettings = true) {
            this.type = type;

            switch (type) {
                case TypesOfItems.sosRocks: {
                        duration = -1;
                        isActive = false;
                        if (useGlobalSettings) {
                            evacChance = GameSettings.instance.sosRocksChance;
                        }
                        else {
                            evacChance = instance.sosRocksChance;
                        }
                        break;
                    }
                case TypesOfItems.rocket: {
                        duration = 0;
                        isActive = false;
                        if (useGlobalSettings) {
                            evacChance = GameSettings.instance.rocketChance;
                        }
                        else {
                            evacChance = instance.rocketChance;
                        }
                        break;
                    }
                case TypesOfItems.bonfire: {
                        isActive = false;
                        this.bonfire = bonfire;
                        if (useGlobalSettings) {
                            duration = GameSettings.instance.bonfireDuration;
                            evacChance = GameSettings.instance.bonfireChance;
                        }
                        else {
                            duration = instance.bonfireDuration;
                            evacChance = instance.bonfireChance;
                        }
                        break;
                    }
            }
        }

        IEnumerator EvacuationProcess() {
            yield return new WaitForSeconds(1);
            int elapsedTime = 0;
            while (true) {
                RouletteWheelSelection();

                if (instance.isEvacuated) {
                    if (bonfire != null) { bonfire.particleSystem.Stop(); bonfire.audioSource.Stop(); bonfire.isFired = false; }
                    break;
                }

                if (elapsedTime >= duration && duration != -1) {
                    if (bonfire != null) { bonfire.particleSystem.Stop(); bonfire.audioSource.Stop(); bonfire.isFired = false; }
                    break;
                }
                yield return new WaitForSeconds(1);
                elapsedTime += 1;
                yield return null;
            }
            isActive = false;
            instance.evacItems.Remove(this);
        }

        void RouletteWheelSelection() {
            int failChance = 100 - evacChance;
            int rnd = Random.Range(0, evacChance + failChance);
            //print("chance: " + rnd);
            print("type: " + type);
            while (rnd >= 0) {
                rnd -= evacChance;
                if (rnd <= 0) {
                    // sucess!
                    instance.isEvacuated = true;
                    print("Evacuated by " + type);
                    break;
                }
                rnd -= failChance;
            }
        }
        public void ActivateItem() {
            isActive = true;
            instance.StartCoroutine(EvacuationProcess());
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
