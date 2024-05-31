using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DataPersistence.Gameplay
{
    public class PersistenceController : MonoBehaviour
    {
        [SerializeField] private AssetReferenceGameObject _prefabRef;

        private void Start()
        {
            GameObjectsRegistries.Instance.RegisterObject(gameObject, _prefabRef.AssetGUID, GetComponents(typeof(Component)));
        }

        private void OnDestroy()
        {
            GameObjectsRegistries.Instance.UnregisterObject(gameObject);
        }
    }
}