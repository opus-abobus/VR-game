using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public static GameObject ground;

    private void Awake() {
        ground = GameObject.FindGameObjectWithTag("ground");
        if (ground == null) { Debug.LogError("ground null reference exception"); }
    }
}
