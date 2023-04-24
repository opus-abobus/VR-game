using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GameTime : MonoBehaviour
{
    public float timeMultiplier = 1.0f;
    [SerializeField] Gradient LightGradient;
    [SerializeField] Gradient ambientLightGradient;

    [SerializeField, Range(1, 3600)] float timeDayInSeconds = 60;
    [SerializeField, Range(0f, 1f)] float timeProgress;

    [SerializeField] Light light;

    Vector3 defaultAngles;

    private void Start() {
        defaultAngles = light.transform.localEulerAngles;
    }
    private void Update() {
        if (Application.isPlaying)
            timeProgress += Time.deltaTime / timeDayInSeconds;
        light.transform.rotation = Quaternion.Euler(timeProgress * 360f, 180, 0);

        if (timeProgress > 1f)
            timeProgress = 0f;

        light.color = LightGradient.Evaluate(timeProgress);
        RenderSettings.ambientLight = ambientLightGradient.Evaluate(timeProgress);
    }
}
