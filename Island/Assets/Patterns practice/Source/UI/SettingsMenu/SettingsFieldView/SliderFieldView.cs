using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UI.SettingsManagement {
    public class SliderFieldView : BaseFieldView {

        [SerializeField] public Slider slider;
        [SerializeField] public TextMeshProUGUI valueText;
        [SerializeField] private TextMeshProUGUI suffix;

        public event Action<SliderFieldView> ValueChanged;

        public override void Subscribe() {
            slider.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(float value) {
            ValueChanged?.Invoke(this);
        }

        public override void Unsubscribe() {
            slider.onValueChanged.RemoveAllListeners();
        }
    }
}
