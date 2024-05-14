using System;
using TMPro;
using UnityEngine;

namespace UI.SettingsManagement {
    public class KeyBindFieldView : BaseFieldView {

        [SerializeField] public TMP_InputField inputField;
        [SerializeField] public TextMeshProUGUI placeholder;
        [SerializeField] public TextMeshProUGUI text;

        public event Action<KeyBindFieldView> ValueChanged;

        public override void Subscribe() {
            inputField.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(string value) {
            ValueChanged?.Invoke(this);
        }

        public override void Unsubscribe() {
            inputField.onValueChanged.RemoveAllListeners();
        }
    }
}
