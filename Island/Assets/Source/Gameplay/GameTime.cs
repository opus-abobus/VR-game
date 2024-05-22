using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

//[ExecuteInEditMode]
public class GameTime : MonoBehaviour
{
    public static GameTime instance;

    //[SerializeField] Gradient LightGradient;
    //[SerializeField] Gradient ambientLightGradient;

    [Tooltip("Длительность суток в секундах")]
    [SerializeField, Range(1, 3600)] float timeDayInSeconds = 60;
    [Tooltip("Время дня")]
    [SerializeField, Range(0f, 1f)] float timeProgress;
    
    //[SerializeField] float RotationSpeed = 1f;

    [SerializeField] AnimationCurve SunCurve;
    [SerializeField] AnimationCurve MoonCurve;
    [SerializeField] AnimationCurve SkyboxCurve;

    [SerializeField] Material DaySkybox;
    [SerializeField] Material NightSkybox;

    [SerializeField] Light Sun;
    [SerializeField] Light Moon;

    float sunIntensity, moonIntensity;

    Vector3 defaultAngles;
    //float _elapsedTime = 0f;
    private void Awake() {
        instance = this;
    }

    private void Start() {
        defaultAngles = Sun.transform.localEulerAngles;
        //DaySkybox.SetFloat("_Exposure", 1f);

        sunIntensity = Sun.intensity;
        moonIntensity = Moon.intensity;
    }
    private void Update() {
        //_elapsedTime += Time.deltaTime;
        timeProgress += Time.deltaTime / timeDayInSeconds;
        //Sun.transform.rotation = Quaternion.Euler(timeProgress * 360f, 180, 0);

        //DaySkybox.SetFloat("_Rotation", _elapsedTime * RotationSpeed);
        //DaySkybox.SetFloat("_Exposure", Mathf.Clamp(Mathf.Sin(_elapsedTime), 0.15f, 1f));

        if (timeProgress > 1f) timeProgress = 0f;

        RenderSettings.skybox.Lerp(NightSkybox, DaySkybox, SkyboxCurve.Evaluate(timeProgress));
        RenderSettings.sun = SkyboxCurve.Evaluate(timeProgress) > 0.1f ? Sun : Moon;
        DynamicGI.UpdateEnvironment();

        Sun.transform.localRotation = Quaternion.Euler(timeProgress * 360f, 180, 0);
        Moon.transform.localRotation = Quaternion.Euler(timeProgress * 360f + 180f, 180, 0);

        Sun.intensity = sunIntensity * SunCurve.Evaluate(timeProgress);
        Moon.intensity = moonIntensity * MoonCurve.Evaluate(timeProgress);

        //Sun.color = LightGradient.Evaluate(timeProgress);
        //RenderSettings.ambientLight = ambientLightGradient.Evaluate(timeProgress);
        //PrintTime();
    }

    /*void PrintTime() {
        int hours, minutes;

        hours = Mathf.FloorToInt(timeProgress / (1 / 24)); print("h = " + timeProgress / 24);
        if (hours == 24) hours = 0;

        minutes = Mathf.FloorToInt(timeProgress / (1 / 24 / 60)); print("m = " + (timeProgress / (1 / 24 / 60);
        if (minutes == 60) minutes = 0;

        string hoursPart = hours.ToString(); if (hours < 10) hoursPart = "0" + hours;
        string minutesPart = minutes.ToString(); if (minutes < 10) minutesPart = "0" + minutes;

        string timeStr = hoursPart + ":" + minutesPart;
        print("time: " + timeStr);
    }*/
}
