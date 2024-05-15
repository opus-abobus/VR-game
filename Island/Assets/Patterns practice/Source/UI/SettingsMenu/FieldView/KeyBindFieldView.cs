using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.SettingsManagement
{
    public class KeyBindFieldView : BaseFieldView
    {
        [SerializeField] public TMP_InputField inputField;

        // theese fields not really needed
        [SerializeField] public TextMeshProUGUI placeholder;
        [SerializeField] public TextMeshProUGUI text;

        [HideInInspector] public KeyCode keyBind;

        private bool _isWaitingForKey = false;

        public event Action<KeyBindFieldView> ValueChanged;

        public override void Subscribe()
        {
            inputField.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(string value)
        {
            ValueChanged?.Invoke(this);
        }

        public override void Unsubscribe()
        {
            inputField.onValueChanged.RemoveAllListeners();
        }

        private void Awake()
        {
            inputField.text = keyBind.ToString();

            inputField.caretWidth = 0;

            inputField.onSelect.AddListener(OnSelect);
            inputField.onDeselect.AddListener(OnDeselect);
        }

        private void OnSelect(string value)
        {
            _isWaitingForKey = true;
            inputField.text = string.Empty;
        }

        private void OnDeselect(string value)
        {
            _isWaitingForKey = false;
            inputField.text = keyBind.ToString();
        }

        private void OnGUI()
        {
            if (_isWaitingForKey && Event.current.isKey)
            {
                keyBind = Event.current.keyCode;
                inputField.text = keyBind.ToString();
                _isWaitingForKey = false;

                inputField.DeactivateInputField();

                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        private void OnDestroy()
        {
            inputField.onSelect.RemoveListener(OnSelect);
            inputField.onDeselect.RemoveListener(OnDeselect);
        }
    }
}
