using UnityEngine;
using UnityEngine.AddressableAssets;

public class CocountSplit : MonoBehaviour
{
    [SerializeField] 
    private GameObject _coconutUp, _coconutDown;

    [SerializeField] private AssetReferenceGameObject _coconutUpper, _coconutLower;

    [SerializeField]
    private Transform coconutBroken;

    private int _splitChance = 4;

    private bool _wasDetachedFromHand;
    public bool WasDetachedFromHand {
        get { return _wasDetachedFromHand; }
        set { _wasDetachedFromHand = value; }
    }
    
    private void OnCollisionEnter(Collision collision) {
        if (_wasDetachedFromHand) {
            if (Random.Range(0, _splitChance) == 0) {    // когда равно 0, то кокос должен расколоться
                _coconutDown.SetActive(true);
                _coconutUp.SetActive(true);

                GameObjectsRegistries.Instance.RegisterObject(_coconutDown, _coconutLower.AssetGUID, new Component[]
                {
                    _coconutDown.transform, _coconutDown.GetComponent<Collider>(), _coconutDown.GetComponent<Rigidbody>()
                });

                GameObjectsRegistries.Instance.RegisterObject(_coconutUp, _coconutUpper.AssetGUID, new Component[]
                {
                    _coconutUp.transform, _coconutUp.GetComponent<Collider>(), _coconutUp.GetComponent<Rigidbody>()
                });

                _coconutDown.transform.parent = coconutBroken;
                _coconutUp.transform.parent = coconutBroken;
                coconutBroken.parent = transform.parent;

                GameObjectsRegistries.Instance.UnregisterObject(this.gameObject);

                Destroy(this.gameObject);
            }
            else {  // иначе увеличить шанс раскалывания
                _splitChance -= 1;
            }
            _wasDetachedFromHand = false;
        }
    }
}
