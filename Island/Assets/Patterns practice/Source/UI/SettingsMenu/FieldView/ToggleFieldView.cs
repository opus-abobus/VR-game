using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SettingsManagement {
    public class ToggleFieldView : BaseFieldView {

        [SerializeField] public Toggle toggle;
        [SerializeField] public Text toggleText;

        public event Action<ToggleFieldView> ValueChanged;

        public override void Subscribe() {
            toggle.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(bool value) {
            ValueChanged?.Invoke(this);
        }

        public override void Unsubscribe() {
            toggle.onValueChanged.RemoveAllListeners();
        }
    }
}
