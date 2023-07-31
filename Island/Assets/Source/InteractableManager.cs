using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class InteractableManager : MonoBehaviour
{
    public Sprite spriteInInvenory;
    
    bool isPickedUp = false;
    public bool IsPickedUp {
        get { return isPickedUp; }
        set { isPickedUp = value; }
    }
}
