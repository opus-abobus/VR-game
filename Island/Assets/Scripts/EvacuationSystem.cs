using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvacuationSystem : MonoBehaviour
{
    public GameObject[] _SOSLetters;
    public GameTime gameTime;

    SOS_Manager[] managers;
    private void Awake() {
        managers = new SOS_Manager[_SOSLetters.Length];
        int i = 0;
        foreach (var obj in _SOSLetters) {
            managers.SetValue(obj.GetComponent<SOS_Manager>(), i);
            i++;
        }
    }

    void Update()
    {
        foreach (var manager in managers) {
            if (manager.isSOSLayedOut) {
                AttemptToEscape();
            }
        }
    }

    void AttemptToEscape() {
        
    }
}
