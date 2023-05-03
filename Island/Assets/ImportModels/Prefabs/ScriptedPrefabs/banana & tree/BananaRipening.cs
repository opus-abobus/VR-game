using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

/* -----------------------
 * Алгоритм созревания плодов
 * -----------------------*/

public class BananaRipening : MonoBehaviour
{
    [SerializeField] GameObject Branch;
    [SerializeField] GameObject BananaPart;
    [SerializeField] GameObject UnripeBananas;
    [SerializeField] bool allowRipening = true;

    bool isRipening;
    BananaTreeStates treeState;

    private void Awake() {
        if (allowRipening) {
            if (Branch == null) { Debug.LogError("Отсутствует ссылка на ветку"); return; }
            if (BananaPart == null) { Debug.LogError("Отсутствует ссылка на зрелый плод"); return; }
            if (UnripeBananas == null) { Debug.LogError("Отсутствует ссылка на незрелый плод"); return; }
            
            BananaPart_prefab = Instantiate(BananaPart, BananaPart.transform.position, BananaPart.transform.rotation, Branch.transform);
            BananaPart_prefab.SetActive(false);

            DetermineRipeState();
        }
        else GetComponent<BananaRipening>().enabled = false;
    }

    [Range(0, 1), SerializeField] float ripeProgressPhase = 0;
    [SerializeField] float phaseTimeInSeconds = 4;

    float _elapsedPhaseTime = 0;
    private void Update() {
        if (allowRipening) {
            RipeProcess();
        }
    }

    GameObject BananaPart_prefab = null;
    void RipeProcess() {
        if (treeState == BananaTreeStates.ripe && !isBananasFallen) { return; }
        if (treeState == BananaTreeStates.ripe && isBananasFallen) { treeState = BananaTreeStates.fallen; _elapsedPhaseTime = 0; return; }

        _elapsedPhaseTime += Time.deltaTime;
        ripeProgressPhase = _elapsedPhaseTime / phaseTimeInSeconds;
        bool isNextPhase = ripeProgressPhase > 1f ? true : false;

        if (isNextPhase) {
            switch (treeState) {
                case BananaTreeStates.empty: {
                        treeState = BananaTreeStates.hasUnripePart;
                        Branch.SetActive(false);
                        BananaPart.SetActive(false);
                        UnripeBananas.SetActive(true);
                        break;
                    }
                case BananaTreeStates.hasUnripePart: {
                        treeState = BananaTreeStates.ripe;
                        UnripeBananas.SetActive(false);
                        BananaPart.SetActive(true);
                        Branch.SetActive(true);
                        break;
                    }
                case BananaTreeStates.fallen: {
                        if (BananaPart == null) { BananaPart = Instantiate(BananaPart_prefab, BananaPart_prefab.transform.position, BananaPart_prefab.transform.rotation, Branch.transform); }

                        treeState = BananaTreeStates.empty;
                        isBananasFallen = false;
                        BananaPart.SetActive(false);
                        Branch.SetActive(false);
                        UnripeBananas.SetActive(false);
                        break;
                    }
            }
            _elapsedPhaseTime = 0;
        }
    }

    enum BananaTreeStates {
        empty,
        hasUnripePart,
        hasEmptyBranch,
        ripe,
        fallen
    }

    [NonSerialized] public bool isBananasFallen = false;
    void DetermineRipeState() {
        bool isActiveUnripe = UnripeBananas.activeInHierarchy;

        if (isBananasFallen) { treeState = BananaTreeStates.fallen; return; }
        if (isActiveUnripe) { treeState = BananaTreeStates.hasUnripePart; return; }
        if (BananaPart.activeInHierarchy) { treeState = BananaTreeStates.ripe; return; }

        bool isActiveBranch = Branch.activeInHierarchy;
        if (isActiveBranch) { treeState = BananaTreeStates.hasEmptyBranch; return; }
        if (!(isActiveBranch || isActiveUnripe)) { treeState = BananaTreeStates.empty; return; }
        else Debug.LogAssertion("Состояние дерева не было определено");
    }
}
