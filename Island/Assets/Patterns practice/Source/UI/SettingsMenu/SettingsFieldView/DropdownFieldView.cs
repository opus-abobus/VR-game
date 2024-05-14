using System;
using TMPro;
using UnityEngine;

namespace UI.SettingsManagement {
    public class DropdownFieldView : BaseFieldView {

        [SerializeField] public TMP_Dropdown dropdown;

        public event Action<DropdownFieldView> ValueChanged;

        public override void Subscribe() {
            dropdown.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(int value) {
            ValueChanged?.Invoke(this);
        }

        public override void Unsubscribe() {
            dropdown.onValueChanged.RemoveAllListeners();
        }
    }
}
