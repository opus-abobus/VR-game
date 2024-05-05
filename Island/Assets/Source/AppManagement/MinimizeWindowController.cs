using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimizeWindowController : MonoBehaviour
{

    [SerializeField]
    private bool _disableSoundOnUnfocusing = true;

    [SerializeField]
    private bool _pauseOnUnfocusing = true;

    void Update()
    {
        if (!Application.isFocused) {

            if (_disableSoundOnUnfocusing) {
                AudioListener.pause = true;
            }

            if (_pauseOnUnfocusing) {
                // pause game
            }
        }
        else {
            AudioListener.pause = false;
            
            // resume game
        }
    }
}
