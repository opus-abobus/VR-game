using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaTreeGrowth : MonoBehaviour
{
    [SerializeField] bool allowGrowth = true;
    [SerializeField, Range(0.01f, 50)] float growthMultiplier = 1f;

    BananaRipening ripeningInstance = null;
    private void Awake() {
        ripeningInstance = GetComponent<BananaRipening>();

        if (allowGrowth) {
            ripeningInstance.enabled = false;
        }
    }
}
