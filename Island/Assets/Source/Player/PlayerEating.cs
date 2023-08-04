using System;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class PlayerEating : MonoBehaviour, Bootstrap.IBootstrap
{
    public event Action OnInitialized;

    [SerializeField] 
    private Hand _hand;

/*    [SerializeField] 
    private Transform _playerCamera;*/

    private List<string> _tagsWithEatableObjects = new List<string> { "berry", "banana", "cocount", "coconut" };

    [SerializeField]
    private HungerSystem _hungerSystem;

    private WorldSettings.INutritionSettings _settings;

    private Dictionary<string, float> nutVals = new Dictionary<string, float>();

    void Bootstrap.IBootstrap.Initialize() {
        /*        if (_playerCamera == null) { Debug.LogAssertion("player camera null reference exception"); }
                else { gameObject.transform.position = _playerCamera.position; }*/

        _settings = GameSettingsManager.Instance.ActiveWorldSettings;

        foreach (var tag in _tagsWithEatableObjects) {
            float val = 0;
            switch (tag) {
                case "berry": {
                        val = _settings.BerryNutrVal;
                        break;
                    }
                case "cocount":
                case "coconut": {
                        val = _settings.CoconutNutrVal;
                        break;
                    }
                case "banana": {
                        val = _settings.BananaNutrVal;
                        break;
                    }
            }
            nutVals.Add(tag, val);
        }

        if (_hungerSystem == null) { Debug.LogError("hungerSystem instance null reference exception"); }

        OnInitialized?.Invoke();
    }

    private void OnTriggerEnter(Collider other) {
        var tag = other.tag;
        foreach (var item in _tagsWithEatableObjects) {
            
            if (tag == item) {
                _hungerSystem.Satiety += nutVals[tag];
                Destroy(other.gameObject);
            }
        }
    }

}
