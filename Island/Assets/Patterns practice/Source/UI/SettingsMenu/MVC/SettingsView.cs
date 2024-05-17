using System;
using UnityEngine;

namespace UI.SettingsManagement {
    public class SettingsView : MonoBehaviour {

        [SerializeField] public BaseFieldView[] fields;

        public void AddListeners() {
            foreach (BaseFieldView fieldView in fields) {

                if (fieldView is ToggleFieldView) {
                    (fieldView as ToggleFieldView).ValueChanged += OnValueChanged;
                }

                else if (fieldView is SliderFieldView) {
                    (fieldView as SliderFieldView).ValueChanged += OnValueChanged;
                }

                else if (fieldView is DropdownFieldView) {
                    (fieldView as DropdownFieldView).ValueChanged += OnValueChanged;
                }

                else if (fieldView is KeyBindFieldView) {
                    (fieldView as KeyBindFieldView).ValueChanged += OnValueChanged;
                }

                fieldView.Subscribe();
            }
        }

        public void RemoveListeners() {
            foreach (BaseFieldView fieldView in fields) {
                fieldView.Unsubscribe();
            }
        }

        public event Action<BaseFieldView> FieldViewUpdated;

        private void OnValueChanged<T>(T fieldView) where T : BaseFieldView {
            FieldViewUpdated?.Invoke(fieldView);
        }
    }
}
