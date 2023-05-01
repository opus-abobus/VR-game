using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEatability : MonoBehaviour
{
    bool isEatable = false;
    public bool IsEatable {
        get { return isEatable; }
        set { isEatable = value; }
    }
}
