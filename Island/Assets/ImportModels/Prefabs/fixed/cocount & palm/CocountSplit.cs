using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CocountSplit : MonoBehaviour
{
    [SerializeField] GameObject coconutUp, coconutDown;

    int splitChance = 5;
    public int SplitChance {
        get { return splitChance; } 
        set { splitChance = value; }
    }

    bool wasDetachedFromHand;
    public bool WasDetachedFromHand {
        get { return wasDetachedFromHand; }
        set {
            wasDetachedFromHand = value;
        }
    }

    
    private void OnCollisionEnter(Collision collision) {
        if (wasDetachedFromHand) {
            if (Random.Range(0, splitChance) == 0) {    // когда равно 0, то кокос должен расколоться
                coconutDown.SetActive(true);
                coconutUp.SetActive(true);
                coconutDown.transform.parent = transform.parent;
                coconutUp.transform.parent = transform.parent;
                Destroy(this.gameObject);
            }
            else {  // иначе увеличить шанс раскалывания
                splitChance -= 1;
            }
            wasDetachedFromHand = false;
        }
    }
}
