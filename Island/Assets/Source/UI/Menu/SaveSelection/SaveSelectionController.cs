using DataPersistence;
using DataPersistence.Gameplay;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class SaveSelectionController : MonoBehaviour
    {
        [SerializeField] private Transform _itemsRoot;
        [SerializeField] private GameObject _itemPrefab;

        private Dictionary<GameObject, GameplayData> _savesDictionary;

        private void Awake()
        {
            _savesDictionary = new Dictionary<GameObject, GameplayData>();

            InitSavesList();
        }

        private void InitSavesList()
        {
            GameplayData[] data = AppManager.Instance.DataManager.GetSaves();
            if (data == null)
            {
                return;
            }

            foreach (var item in data)
            {
                GameObject itemObject = Instantiate(_itemPrefab, _itemsRoot);
                itemObject.GetComponentInChildren<TextMeshProUGUI>().text = item.displayName;
                itemObject.GetComponent<SaveItemHandler>().DoubleClick += OnItemDoubleClick;

                _savesDictionary.Add(itemObject, item);
            }
        }

        private void OnItemDoubleClick(GameObject sender)
        {
            CurrentSessionDataManager.Instance.SetData(_savesDictionary[sender]);
            AppManager.Instance.LoadLevel();
        }

        private void OnDestroy()
        {
            foreach (var key in _savesDictionary.Keys)
            {
                key.GetComponent<SaveItemHandler>().DoubleClick -= OnItemDoubleClick;
            }
        }
    }
}
