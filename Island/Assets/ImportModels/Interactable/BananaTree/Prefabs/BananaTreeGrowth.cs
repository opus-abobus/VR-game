using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BananaTreeGrowth : MonoBehaviour
{
    //max scale = 1.75

    [SerializeField] bool allowGrowth = true;
    [Tooltip("Коэффициент роста: начальный рост к концу роста увеличится на это число")]
    [SerializeField, Range(1f, 10f)] float GrowthRatio = 3;

    [SerializeField] bool useGlobalSettings = true;
    [SerializeField] GameSettings globalSettings;

    [SerializeField] bool useRandomGrowthTime = true;
    [SerializeField] float timeToGrowthInSeconds = 5;
    [SerializeField] float MinTimeToGrowthInSeconds = 3;
    [SerializeField] float MaxTimeToGrowthInSeconds = 10;

    BananaRipening ripeningInstance = null;

    Vector3 startScale, endScale;
    IEnumerator _growningProcess;

    private void Awake() {
        if (useGlobalSettings && globalSettings != null) {
            if (globalSettings.useRandomBananaTreeStartScale) 
                startScale = Vector3.one * Random.Range(globalSettings.minBananaTreeScale, globalSettings.maxBananaTreeScale);
            else
                startScale = Vector3.one * globalSettings.minBananaTreeScale;
            transform.localScale = startScale;
            endScale = Vector3.one * globalSettings.maxBananaTreeScale;

            MinTimeToGrowthInSeconds = globalSettings.minTimeToGrowthInSeconds;
            MaxTimeToGrowthInSeconds = globalSettings.maxTimeToGrowthInSeconds;
        }
        else {
            startScale = transform.localScale;
            endScale = startScale * GrowthRatio;
        }  
        
        if (startScale.y >= endScale.y) this.enabled = false;

        ripeningInstance = GetComponent<BananaRipening>();

        if (allowGrowth) {
            if (startScale.y >= 1.3) ripeningInstance.enabled = true;
            else ripeningInstance.enabled = false;

            if (useRandomGrowthTime) {
                timeToGrowthInSeconds = Random.Range(MinTimeToGrowthInSeconds, MaxTimeToGrowthInSeconds);
            }
            _growningProcess = GrowningProcess(); StartCoroutine(_growningProcess);
        }
        else this.enabled = false; 
    }

    [SerializeField, Range(0, 1)] float growningProgress = 0;
    private void Update() {
        if (growningProgress >= 1) {
            StopCoroutine(_growningProcess);
            ripeningInstance.enabled = true;
            Destroy(this);
        }
    }
    IEnumerator GrowningProcess() {
        while (true) {
            growningProgress += Time.deltaTime / timeToGrowthInSeconds;
            transform.localScale = startScale + (endScale - startScale) * growningProgress;
            yield return null;
        }
    }
}
