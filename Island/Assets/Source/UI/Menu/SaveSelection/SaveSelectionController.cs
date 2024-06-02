using DataPersistence;
using DataPersistence.Gameplay;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SaveSelectionController : MonoBehaviour
    {
        [SerializeField] private Transform _itemsRoot;
        [SerializeField] private GameObject _itemPrefab;

        [SerializeField] private Image _image;
        private Sprite _noScreenCaptureSprite;
        [SerializeField] private int _imageSpriteWidth = 600;
        [SerializeField] private int _imageSpriteHeight = 600;

        private Dictionary<GameObject, GameplayData> _savesDictionary;

        private void Awake()
        {
            _noScreenCaptureSprite = _image.sprite;

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

                itemObject.GetComponent<SaveItemHandler>().SingleClick += OnSingleClick;
                itemObject.GetComponent<SaveItemHandler>().DoubleClick += OnItemDoubleClick;

                _savesDictionary.Add(itemObject, item);
            }
        }

        private void OnSingleClick(GameObject sender)
        {
            var savedCapture = AppManager.Instance.DataManager.GetScreenCaptureFromSave(_savesDictionary[sender], _imageSpriteWidth, _imageSpriteHeight);

            if (savedCapture != null)
            {
                _image.sprite = GetSprite(savedCapture);
            }
            else
            {
                _image.sprite = _noScreenCaptureSprite;
            }
        }

        private Sprite GetSprite(Texture2D texture2D)
        {
            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
            return sprite;
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
