using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class BananaPool : MonoBehaviour
{
    public static BananaPool Instance { get; private set; }

    [SerializeField] private Transform _root;
    [SerializeField] private int _poolSize;

    [SerializeField] private AssetReferenceGameObject _bananaPrefabRef;
    private GameObject _bananaPrefab;

    public string GetAssetGUID()
    {
        return _bananaPrefabRef.AssetGUID;
    }

    private Stack<GameObject> _pool;

    public void Init()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        _bananaPrefab = AddressableItems.Instance.GetPrefabByGUID(_bananaPrefabRef.AssetGUID);
        InitPool();
    }

    private void InitPool()
    {
        if (_poolSize > 0)
        {
            _pool = new Stack<GameObject>(_poolSize);
            for (int i = 0; i < _poolSize; i++)
            {
                _pool.Push(Instantiate(_bananaPrefab, _root));
                _pool.Peek().SetActive(false);
            }
        }
        else
        {
            _pool = new Stack<GameObject>(0);
        }
    }

    private void Refill()
    {
        if (_pool.Count < _poolSize)
        {
            _pool.Push(Instantiate(_bananaPrefab, _root));
            _pool.Peek().SetActive(false);
        }
    }

    public GameObject Get()
    {
        if (_pool.Count > 0)
        {
            Refill();

            return _pool.Pop();
        }
        else
        {
            return Instantiate(_bananaPrefab);
        }
    }

    public void Return(GameObject banana)
    {
        banana.SetActive(false);
        banana.transform.parent = _root;
        _pool.Push(banana);
    }
}
