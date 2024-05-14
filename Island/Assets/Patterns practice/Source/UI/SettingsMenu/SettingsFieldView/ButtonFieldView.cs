using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SettingsManagement {
    public class ButtonFieldView : BaseFieldView {

        [SerializeField] public Button button;

        public event Action<ButtonFieldView> Clicked;
        public override void Subscribe() {
            button.onClick.AddListener(OnClicked);
        }

        public override void Unsubscribe() {
            button.onClick.RemoveAllListeners();
        }

        private void OnClicked() {
            Clicked?.Invoke(this);
        }
    }
}
