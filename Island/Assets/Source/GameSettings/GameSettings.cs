using System;
using System.Collections;
using UnityEngine;

public abstract class GameSettings : MonoBehaviour {
    public event Action Awaked;

    private bool _hasAwakened = false;

    private void Awake() {
        _hasAwakened = true;
    }

    public void Init() {
        StartCoroutine(InitProcess());
    }

    IEnumerator InitProcess() {
        while (!_hasAwakened) {
            yield return null;
        }

        Awaked?.Invoke();
    }
}
