using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BananaTreeGrowth : MonoBehaviour
{
    [SerializeField] bool allowGrowth = true;
    [Tooltip("Коэффициент роста: начальный рост к концу роста увеличится на это число")]
    [SerializeField, Range(1f, 10f)] float GrowthRatio = 3;

    [SerializeField] bool useRandomGrowthTime = true;
    [SerializeField] float timeToGrowthInSeconds = 5;
    [SerializeField] float MinTimeToGrowthInSeconds = 3;
    [SerializeField] float MaxTimeToGrowthInSeconds = 10;

    BananaRipening ripeningInstance = null;

    Vector3 startScale, endScale;
    Vector3 deltaScale;
    IEnumerator _growningProcess;

    private void Awake() {
        startScale = transform.localScale;
        endScale = startScale * GrowthRatio;

        if (startScale.y >= endScale.y) this.enabled = false;

        ripeningInstance = GetComponent<BananaRipening>();

        if (allowGrowth) {
            ripeningInstance.enabled = false;

            if (useRandomGrowthTime) {
                timeToGrowthInSeconds = Random.Range(MinTimeToGrowthInSeconds, MaxTimeToGrowthInSeconds);
            }
            deltaScale = endScale - startScale;
            _growningProcess = GrowningProcess(); StartCoroutine(_growningProcess);
        }
    }

    [SerializeField, Range(0, 1)] float growningProgress = 0;
    private void Update() {
        if (growningProgress >= 1) {
            StopCoroutine(_growningProcess);
            ripeningInstance.enabled = true;
            this.enabled = false;
        }
    }
    IEnumerator GrowningProcess() {
        while (true) {
            growningProgress += Time.deltaTime / timeToGrowthInSeconds;
            transform.localScale = startScale + deltaScale * growningProgress;
            yield return null;
        }
    }
}
