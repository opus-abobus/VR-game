using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataPersistence;
using System.Linq;
using TMPro;
using System;
using System.Reflection;

namespace UI.SettingsManagement {
    public class SettingsDataModel : MonoBehaviour {

        [SerializeField] private SettingsView _view;

        private SettingsData _settingsData;

        //public event Action ViewInitialized;

        public event Action DataModified;

        public FieldInfo[] fieldsInfo;

        public void Init(SettingsData settingsData) {
            _settingsData = settingsData;

            fieldsInfo = _settingsData.GetType().GetFields();

            UpdateWholeView();
        }

        // ÃŒ∆ÕŒ —ƒ≈À¿“‹ —ÀŒ¬¿–‹ “»œ¿ <BaseFieldView, FieldInfo> !!!!
        public void UpdateWholeView() {

            foreach (BaseFieldView fieldView in _view.fields) {

                foreach (FieldInfo fieldInfo in fieldsInfo) {

                    SaveFieldAttribute attribute = Attribute.GetCustomAttribute(fieldInfo, typeof(SaveFieldAttribute)) as SaveFieldAttribute;

                    if (attribute != null && attribute.fieldName == fieldView.fieldName) {
                        SetFieldView(fieldView, fieldInfo);
                    }
                }
            }

            //ViewInitialized?.Invoke();
        }

        private void SetFieldView(BaseFieldView fieldView, FieldInfo fieldInfo, bool setConstraints = true) {

            ConstraintFieldAttribute attribute = null;
            if (setConstraints) {
                attribute = Attribute.GetCustomAttribute(fieldInfo, typeof(ConstraintFieldAttribute)) as ConstraintFieldAttribute;
            }

            if (fieldView is SliderFieldView sliderFieldView) {
                
                if (sliderFieldView.slider.wholeNumbers) {
                    
                    if (attribute != null && attribute.constraintsInt != null) {
                        sliderFieldView.slider.minValue = attribute.constraintsInt.minValue;
                        sliderFieldView.slider.maxValue = attribute.constraintsInt.maxValue;
                    }

                    sliderFieldView.slider.value = (int) fieldInfo.GetValue(_settingsData);
                }
                else {

                    if (attribute != null && attribute.constraintsFloat != null) {
                        sliderFieldView.slider.minValue = attribute.constraintsFloat.minValue;
                        sliderFieldView.slider.maxValue = attribute.constraintsFloat.maxValue;
                    }

                    sliderFieldView.slider.value = (float) fieldInfo.GetValue(_settingsData);
                }

                sliderFieldView.valueText.text = sliderFieldView.slider.value.ToString("F3");
            }
            // !!!
            else if (fieldView is DropdownFieldView dropdownFieldView) {
                dropdownFieldView.dropdown.value = (int) fieldInfo.GetValue(_settingsData);
            }

            else if (fieldView is ToggleFieldView toggleFieldView) {
                toggleFieldView.toggle.isOn = (bool) fieldInfo.GetValue(_settingsData);
            }

            else if (fieldView is KeyBindFieldView keyBindFieldView) {
                keyBindFieldView.inputField.text = fieldInfo.GetValue(_settingsData).ToString();
            }
        }

        public void SaveChanges_WHOLE_SEARCH() {

            foreach (BaseFieldView fieldView in _view.fields) {

                foreach (FieldInfo fieldInfo in fieldsInfo) {

                    SaveFieldAttribute attribute = Attribute.GetCustomAttribute(fieldInfo, typeof(SaveFieldAttribute)) as SaveFieldAttribute;

                    if (attribute != null && attribute.fieldName == fieldView.fieldName) {
                        SetFieldData(fieldInfo, fieldView);
                    }
                }
            }

            WriteDataOnDisk();
        }

        private void SetFieldData(FieldInfo fieldInfo, BaseFieldView fieldView) {

            // FieldInfo.SetValue does not writing data on the original struct, it just makes a copy
            // So instead of doing that we need to use class instead of struct or call FieldInfo.SetValueDirect and use this:
            TypedReference dataRef = __makeref(_settingsData);

            if (fieldView is SliderFieldView sliderFieldView) {
                
                if (sliderFieldView.slider.wholeNumbers) {
                    fieldInfo.SetValueDirect(dataRef, (int) sliderFieldView.slider.value);
                }
                else {
                    fieldInfo.SetValueDirect(dataRef, sliderFieldView.slider.value);
                }
            }
            // !!!
            else if (fieldView is DropdownFieldView dropdownFieldView) {
                fieldInfo.SetValueDirect(dataRef, dropdownFieldView.dropdown.value);
            }

            else if (fieldView is ToggleFieldView toggleFieldView) {
                fieldInfo.SetValueDirect(dataRef, toggleFieldView.toggle.isOn);
            }
            // ???
            else if (fieldView is KeyBindFieldView keyBindFieldView) {
                fieldInfo.SetValueDirect(dataRef,
                    (KeyCode) Enum.Parse(typeof(KeyCode), keyBindFieldView.inputField.text));
            }
        }

        public void UpdateViewText(BaseFieldView fieldView) {

            if (fieldView is SliderFieldView sliderFieldView) {
                sliderFieldView.valueText.text = sliderFieldView.slider.value.ToString("F3");
            }
        }

        // Õ¿ƒŒ “ŒÀ‹ Œ »«Ã≈Õ≈ÕÕ€≈ œŒÀﬂ
        public void DiscardChanges(List<BaseFieldView> unsavedViews) {

            foreach (var view in unsavedViews) {

                foreach (FieldInfo fieldInfo in fieldsInfo) {

                    SaveFieldAttribute attribute = Attribute.GetCustomAttribute(fieldInfo, typeof(SaveFieldAttribute)) as SaveFieldAttribute;

                    if (attribute != null && attribute.fieldName == view.fieldName) {
                        SetFieldView(view, fieldInfo, false);
                    }
                }
            }

            unsavedViews.Clear();
        }

        private void WriteDataOnDisk() {
            _settingsData = AppManager.Instance.DataManager.SaveSettings(ref _settingsData);

            DataModified?.Invoke();
        }
    }
}
