using UnityEngine;

public class Berry : MonoBehaviour
{
    // Столкнулась ли с чем-нибудь или упала ягода 
    private bool _isFallen;
    public bool IsFallen { get { return _isFallen; } }

    private void Awake() {
        _isFallen = false;
    }

    private void OnCollisionEnter(Collision collision) {
        if (!_isFallen) {
            GetComponent<Rigidbody>().useGravity = true;
            _isFallen = true;

            Destroy(this);
        }
    }
}
