using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataPersistence;
using TMPro;
using System;
using System.Reflection;

namespace UI.SettingsManagement
{
    public class SettingsDataModel : MonoBehaviour
    {
        [SerializeField] private SettingsView _view;

        private SettingsData _settingsData;

        public event Action DataModified;

        public FieldInfo[] fieldsInfo;

        public void Init(SettingsData settingsData)
        {
            _settingsData = settingsData;

            fieldsInfo = _settingsData.GetType().GetFields();

            UpdateWholeView();
        }

        // ÃŒ∆ÕŒ —ƒ≈À¿“‹ —ÀŒ¬¿–‹ “»œ¿ <BaseFieldView, FieldInfo> !!!!
        public void UpdateWholeView()
        {
            foreach (BaseFieldView fieldView in _view.fields)
            {
                foreach (FieldInfo fieldInfo in fieldsInfo)
                {
                    SaveFieldAttribute attribute = Attribute.GetCustomAttribute(fieldInfo, typeof(SaveFieldAttribute)) as SaveFieldAttribute;

                    if (attribute != null && attribute.fieldName == fieldView.fieldName)
                    {
                        SetFieldView(fieldView, fieldInfo);
                    }
                }
            }
        }

        private void SetFieldView(BaseFieldView fieldView, FieldInfo fieldInfo, bool setConstraints = true)
        {
            ConstraintFieldAttribute attribute = null;
            if (setConstraints)
            {
                attribute = Attribute.GetCustomAttribute(fieldInfo, typeof(ConstraintFieldAttribute)) as ConstraintFieldAttribute;
            }

            if (fieldView is SliderFieldView sliderFieldView)
            {
                if (sliderFieldView.slider.wholeNumbers)
                {
                    if (attribute != null && attribute.constraintsInt != null)
                    {
                        sliderFieldView.slider.minValue = attribute.constraintsInt.minValue;
                        sliderFieldView.slider.maxValue = attribute.constraintsInt.maxValue;
                    }

                    sliderFieldView.slider.value = (int) fieldInfo.GetValue(_settingsData);
                }
                else
                {
                    if (attribute != null && attribute.constraintsFloat != null)
                    {
                        sliderFieldView.slider.minValue = attribute.constraintsFloat.minValue;
                        sliderFieldView.slider.maxValue = attribute.constraintsFloat.maxValue;
                    }

                    sliderFieldView.slider.value = (float) fieldInfo.GetValue(_settingsData);
                }

                sliderFieldView.valueText.text = sliderFieldView.slider.value.ToString(sliderFieldView.valueFormat);
            }

            else if (fieldView is DropdownFieldView dropdownFieldView)
            {
                SetDropdown(dropdownFieldView, fieldInfo);
            }

            else if (fieldView is ToggleFieldView toggleFieldView)
            {
                toggleFieldView.toggle.isOn = (bool) fieldInfo.GetValue(_settingsData);
            }

            else if (fieldView is KeyBindFieldView keyBindFieldView)
            {
                keyBindFieldView.keyBind = (KeyCode) fieldInfo.GetValue(_settingsData);
                keyBindFieldView.inputField.text = keyBindFieldView.keyBind.ToString();
            }
        }

        [Obsolete]
        public void SaveChanges_WHOLE_SEARCH()
        {
            foreach (BaseFieldView fieldView in _view.fields)
            {
                foreach (FieldInfo fieldInfo in fieldsInfo)
                {
                    SaveFieldAttribute attribute = Attribute.GetCustomAttribute(fieldInfo, typeof(SaveFieldAttribute)) as SaveFieldAttribute;
                    if (attribute != null && attribute.fieldName == fieldView.fieldName)
                    {
                        SetDataFromView(fieldInfo, fieldView);
                    }
                }
            }

            WriteDataOnDisk();
        }

        public void SaveChanges(List<BaseFieldView> unsavedFields)
        {
            foreach (var fieldView in unsavedFields)
            {
                foreach (FieldInfo fieldInfo in fieldsInfo)
                {
                    SaveFieldAttribute attribute = Attribute.GetCustomAttribute(fieldInfo, typeof(SaveFieldAttribute)) as SaveFieldAttribute;
                    if (attribute != null && attribute.fieldName == fieldView.fieldName)
                    {
                        SetDataFromView(fieldInfo, fieldView);
                    }
                }
            }

            unsavedFields.Clear();

            WriteDataOnDisk();
        }

        private void SetDataFromView(FieldInfo fieldInfo, BaseFieldView fieldView)
        {
            // FieldInfo.SetValue does not writing data on the original struct, it just makes a copy
            // So instead of doing that we need to use class instead of struct or call FieldInfo.SetValueDirect and use this:
            TypedReference dataRef = __makeref(_settingsData);

            if (fieldView is SliderFieldView sliderFieldView)
            {

                if (sliderFieldView.slider.wholeNumbers)
                {
                    fieldInfo.SetValueDirect(dataRef, (int) sliderFieldView.slider.value);
                }
                else
                {
                    fieldInfo.SetValueDirect(dataRef, sliderFieldView.slider.value);
                }
            }

            else if (fieldView is DropdownFieldView dropdownFieldView)
            {
                SaveFieldAttribute attribute = Attribute.GetCustomAttribute(fieldInfo, typeof(SaveFieldAttribute)) as SaveFieldAttribute;
                if (attribute != null) 
                {
                    switch (attribute.fieldName)
                    {
                        case FieldName.ScreenResolution:
                            {
                                string[] resParts = dropdownFieldView.dropdown.options[dropdownFieldView.dropdown.value].text
                                    .Split(new char[] { 'x', '@' });

                                Resolution resolution = new Resolution();
                                resolution.width = int.Parse(resParts[0].Trim());
                                resolution.height = int.Parse(resParts[1].Trim());
                                resolution.refreshRateRatio = Screen.currentResolution.refreshRateRatio;
                                //int.Parse(resParts[2].Trim().Replace("Hz", "").Trim());

                                fieldInfo.SetValueDirect(dataRef, resolution);

                                Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode, resolution.refreshRateRatio);

                                break;
                            }
                        case FieldName.FullscreenMode:
                            {
                                FullScreenMode fSM = (FullScreenMode) Enum.Parse(typeof(FullScreenMode), 
                                    dropdownFieldView.dropdown.options[dropdownFieldView.dropdown.value].text);
                                fieldInfo.SetValueDirect(dataRef, fSM);

                                Screen.fullScreenMode = fSM;

                                break;
                            }
                    }
                }
            }

            else if (fieldView is ToggleFieldView toggleFieldView)
            {
                fieldInfo.SetValueDirect(dataRef, toggleFieldView.toggle.isOn);
            }

            else if (fieldView is KeyBindFieldView keyBindFieldView)
            {
                fieldInfo.SetValueDirect(dataRef, keyBindFieldView.keyBind);
            }
        }

        public void UpdateViewText(BaseFieldView fieldView)
        {
            if (fieldView is SliderFieldView sliderFieldView)
            {
                sliderFieldView.valueText.text = sliderFieldView.slider.value.ToString(sliderFieldView.valueFormat);
            }
        }

        public void DiscardChanges(List<BaseFieldView> unsavedViews)
        {
            foreach (var view in unsavedViews)
            {
                foreach (FieldInfo fieldInfo in fieldsInfo)
                {
                    SaveFieldAttribute attribute = Attribute.GetCustomAttribute(fieldInfo, typeof(SaveFieldAttribute)) as SaveFieldAttribute;

                    if (attribute != null && attribute.fieldName == view.fieldName)
                    {
                        SetFieldView(view, fieldInfo, false);
                    }
                }
            }

            unsavedViews.Clear();
        }

        private void WriteDataOnDisk()
        {
            _settingsData = AppManager.Instance.DataManager.SaveSettings(ref _settingsData);

            DataModified?.Invoke();
        }

        private void SetDropdown(DropdownFieldView dropdownFieldView, FieldInfo fieldInfo)
        {
            dropdownFieldView.dropdown.ClearOptions();

            SaveFieldAttribute attribute = Attribute.GetCustomAttribute(fieldInfo, typeof(SaveFieldAttribute)) as SaveFieldAttribute;
            if (attribute != null)
            {
                switch (attribute.fieldName)
                {
                    case FieldName.ScreenResolution:
                        {
                            // perhaps, it's not necessary to save the current resolution or load it since it is already saved when applied
                            Resolution savedRes = (Resolution) fieldInfo.GetValue(_settingsData);

                            Resolution[] supportedRess = Screen.resolutions;

                            int i = -1;
                            bool isCurrentRes = false;

                            foreach (Resolution resolution in supportedRess)
                            {
                                dropdownFieldView.dropdown.options.Add(new TMP_Dropdown.OptionData(resolution.ToString(), null));

                                if (!isCurrentRes)
                                {
                                    ++i;
                                    if (resolution.Equals(savedRes))
                                        isCurrentRes = true;
                                }
                            }

                            dropdownFieldView.dropdown.value = i;

                            break;
                        }
                    case FieldName.FullscreenMode:
                        {
                            dropdownFieldView.dropdown.AddOptions(new List<TMP_Dropdown.OptionData>{
                                new TMP_Dropdown.OptionData(FullScreenMode.FullScreenWindow.ToString(), null),
                                new TMP_Dropdown.OptionData(FullScreenMode.ExclusiveFullScreen.ToString(), null),
                                new TMP_Dropdown.OptionData(FullScreenMode.Windowed.ToString(), null)
                            });

                            FullScreenMode savedFSM = (FullScreenMode) fieldInfo.GetValue(_settingsData);
                            if (savedFSM == FullScreenMode.FullScreenWindow)
                                dropdownFieldView.dropdown.value = 0;
                            else if (savedFSM == FullScreenMode.ExclusiveFullScreen)
                                dropdownFieldView.dropdown.value = 1;
                            else if (savedFSM == FullScreenMode.Windowed)
                                dropdownFieldView.dropdown.value = 2;

                            break;
                        }
                }
            }
        }
    }
}
