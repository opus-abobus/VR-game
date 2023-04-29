using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BananaTreeGrowth : MonoBehaviour
{
    [SerializeField] bool allowGrowth = true;
    [SerializeField, Range(0.01f, 1)] float growthSpeed = 1f;
    [SerializeField] bool randomizeSpeed = true;
    [SerializeField] int changeSpeedAfterSeconds = 5;

    BananaRipening ripeningInstance = null;
    Animation animation;
    private void Awake() {
        ripeningInstance = GetComponent<BananaRipening>();
        animation = GetComponent<Animation>();

        if (allowGrowth) {
            ripeningInstance.enabled = false;
            StartCoroutine(ChangeAnimationSpeed());
        }
        else
            animation.enabled = false;
        //StartCoroutine(FirstRoutine());
    }

    Vector3 _oldScale;
    bool isAnimEnded = false;
    private void Update() {
        if (transform.localScale == _oldScale) {
            isAnimEnded = true;
        }
        _oldScale = transform.localScale;
        //if (_oldScale == null) _oldScale = transform.localScale;
        Debug.Log(isAnimEnded);
    }
    int i = 0;
    IEnumerator ChangeAnimationSpeed() {
        print("cr call " + i);
        i++;
        while (!isAnimEnded) {
            yield return new WaitForSeconds(changeSpeedAfterSeconds);
            foreach (AnimationState state in animation) {
                if (!randomizeSpeed)
                    state.speed = growthSpeed;
                else {
                    state.speed = Random.Range(0.1f, 1);
                }
            }
            yield return null;
        }
    }
    /*IEnumerator StartRipening() {

    }*/
}
