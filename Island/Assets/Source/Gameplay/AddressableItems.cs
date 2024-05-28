using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableItems : MonoBehaviour
{
    [SerializeField] private AssetReferenceGameObject[] _prefabsReferences;
    //[SerializeField] private AssetLabelReference _itemsLabelReference;

    private Dictionary<string, GameObject> _cachedPrefabs;
    public GameObject GetPrefabByGUID(string GUID)
    {
        if (_cachedPrefabs == null || _cachedPrefabs.Count == 0)
            return null;

        if (!_cachedPrefabs.ContainsKey(GUID))
            return null;

        return _cachedPrefabs[GUID];
    }

    public static AddressableItems Instance { get; private set; }

    public void LoadAssets()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (_cachedPrefabs == null)
            _cachedPrefabs = new Dictionary<string, GameObject>();

        foreach (var prefab in _prefabsReferences)
        {
            LoadAssetSync(prefab);
        }
    }

    private GameObject LoadAssetSync(AssetReferenceGameObject assetRef)
    {
        var handle = assetRef.LoadAssetAsync<GameObject>();
        handle.WaitForCompletion();

        if (!_cachedPrefabs.ContainsKey(assetRef.AssetGUID) && handle.Status == AsyncOperationStatus.Succeeded)
            _cachedPrefabs.Add(assetRef.AssetGUID, handle.Result);

        return handle.Result;
    }

    private void LoadAssetsByLabelSync(AssetLabelReference labelReference)
    {
        var handle = Addressables.LoadResourceLocationsAsync(labelReference.labelString, typeof(GameObject));
        handle.WaitForCompletion();

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            foreach (var item in handle.Result)
            {
                var handleAsset = Addressables.LoadAssetAsync<GameObject>(item.PrimaryKey);
                handleAsset.WaitForCompletion();
                if (handleAsset.Status == AsyncOperationStatus.Succeeded)
                {
                    _cachedPrefabs.Add(item.InternalId, handleAsset.Result);
                    
                }
            }
        }
    }

    private async Task<GameObject> LoadAssetAsync(AssetReferenceGameObject assetRef)
    {
        var handle = assetRef.LoadAssetAsync<GameObject>();
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            return handle.Result;
        }
        else
        {
            return null;
        }
    }
}
