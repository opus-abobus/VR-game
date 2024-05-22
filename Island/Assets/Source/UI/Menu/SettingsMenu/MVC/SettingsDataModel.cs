using DataPersistence;
using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace UI.SettingsManagement
{
    public class SettingsDataModel : MonoBehaviour
    {
        [SerializeField] private SettingsView _view;

        private SettingsData _settingsData;

        private Dictionary<BaseFieldView, FieldInfo> _fieldsData;

        public void Init(SettingsData settingsData)
        {
            _settingsData = settingsData;

            InitFieldsData(_settingsData.GetType().GetFields());

            UpdateWholeView();
        }

        private void InitFieldsData(FieldInfo[] fieldsInfo)
        {
            _fieldsData = new Dictionary<BaseFieldView, FieldInfo>();

            foreach (BaseFieldView fieldView in _view.fields)
            {
                foreach (FieldInfo fieldInfo in fieldsInfo)
                {
                    SaveFieldAttribute attribute = Attribute.GetCustomAttribute(fieldInfo, typeof(SaveFieldAttribute)) as SaveFieldAttribute;
                    if (attribute != null && attribute.fieldName == fieldView.fieldName)
                    {
                        if (!_fieldsData.ContainsKey(fieldView))
                            _fieldsData.Add(fieldView, fieldInfo);
                    }
                }
            }
        }

        public void UpdateWholeView()
        {
            foreach (KeyValuePair<BaseFieldView, FieldInfo> fieldData in _fieldsData)
            {
                SetFieldView(fieldData.Key, true);
            }
        }

        private void SetFieldView(BaseFieldView fieldView, bool setConstraints = true)
        {
            ConstraintFieldAttribute attribute = null;
            if (setConstraints)
            {
                attribute = Attribute.GetCustomAttribute(_fieldsData[fieldView], typeof(ConstraintFieldAttribute)) as ConstraintFieldAttribute;
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

                    sliderFieldView.slider.value = (int) _fieldsData[fieldView].GetValue(_settingsData);
                }
                else
                {
                    if (attribute != null && attribute.constraintsFloat != null)
                    {
                        sliderFieldView.slider.minValue = attribute.constraintsFloat.minValue;
                        sliderFieldView.slider.maxValue = attribute.constraintsFloat.maxValue;
                    }

                    sliderFieldView.slider.value = (float) _fieldsData[fieldView].GetValue(_settingsData);
                }

                sliderFieldView.valueText.text = sliderFieldView.slider.value.ToString(sliderFieldView.valueFormat);
            }

            else if (fieldView is DropdownFieldView dropdownFieldView)
            {
                SetDropdown(dropdownFieldView);
            }

            else if (fieldView is ToggleFieldView toggleFieldView)
            {
                toggleFieldView.toggle.isOn = (bool) _fieldsData[fieldView].GetValue(_settingsData);
            }

            else if (fieldView is KeyBindFieldView keyBindFieldView)
            {
                keyBindFieldView.keyBind = (KeyCode) _fieldsData[fieldView].GetValue(_settingsData);
                keyBindFieldView.inputField.text = keyBindFieldView.keyBind.ToString();
            }
        }

        [Obsolete("Use SaveChanges() instead.")]
        public void SaveChanges_WHOLE_SEARCH()
        {
            foreach (KeyValuePair<BaseFieldView, FieldInfo> fieldData in _fieldsData)
            {
                SetDataFromView(fieldData.Key);
            }

            WriteDataOnDisk();
        }

        public void SaveChanges(List<BaseFieldView> unsavedViews)
        {
            for (int i = 0; i < unsavedViews.Count; i++)
            {
                SetDataFromView(unsavedViews[i]);
            }

            unsavedViews.Clear();

            WriteDataOnDisk();
        }

        private void SetDataFromView(BaseFieldView fieldView)
        {
            // FieldInfo.SetValue does not writing data on the original struct, it just makes a copy
            // So instead of doing that we need to use class instead of struct or call FieldInfo.SetValueDirect and use this:
            TypedReference dataRef = __makeref(_settingsData);

            if (fieldView is SliderFieldView sliderFieldView)
            {

                if (sliderFieldView.slider.wholeNumbers)
                {
                    _fieldsData[fieldView].SetValueDirect(dataRef, (int) sliderFieldView.slider.value);
                }
                else
                {
                    _fieldsData[fieldView].SetValueDirect(dataRef, sliderFieldView.slider.value);
                }
            }

            else if (fieldView is DropdownFieldView dropdownFieldView)
            {
                SaveFieldAttribute attribute = Attribute.GetCustomAttribute(_fieldsData[fieldView], typeof(SaveFieldAttribute)) as SaveFieldAttribute;
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

                                _fieldsData[fieldView].SetValueDirect(dataRef, resolution);

                                Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode, resolution.refreshRateRatio);

                                break;
                            }
                        case FieldName.FullscreenMode:
                            {
                                FullScreenMode fSM = (FullScreenMode) Enum.Parse(typeof(FullScreenMode),
                                    dropdownFieldView.dropdown.options[dropdownFieldView.dropdown.value].text);
                                _fieldsData[fieldView].SetValueDirect(dataRef, fSM);

                                Screen.fullScreenMode = fSM;

                                break;
                            }
                    }
                }
            }

            else if (fieldView is ToggleFieldView toggleFieldView)
            {
                _fieldsData[fieldView].SetValueDirect(dataRef, toggleFieldView.toggle.isOn);
            }

            else if (fieldView is KeyBindFieldView keyBindFieldView)
            {
                _fieldsData[fieldView].SetValueDirect(dataRef, keyBindFieldView.keyBind);
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
            if (unsavedViews == null || unsavedViews.Count == 0)
                return;

            for (int i = 0; i < unsavedViews.Count; i++)
            {
                SetFieldView(unsavedViews[i], false);
            }

            unsavedViews.Clear();
        }

        private void WriteDataOnDisk()
        {
            _settingsData = AppManager.Instance.DataManager.SaveSettings(ref _settingsData);
        }

        private void SetDropdown(DropdownFieldView dropdownFieldView)
        {
            dropdownFieldView.dropdown.ClearOptions();

            SaveFieldAttribute attribute = Attribute.GetCustomAttribute(_fieldsData[dropdownFieldView], typeof(SaveFieldAttribute)) as SaveFieldAttribute;
            if (attribute != null)
            {
                switch (attribute.fieldName)
                {
                    case FieldName.ScreenResolution:
                        {
                            // perhaps, it's not necessary to save the current resolution or load it since it is already saved when applied
                            //Resolution savedRes = (Resolution) _fieldsData[dropdownFieldView].GetValue(_settingsData);
                            Resolution currentRes = Screen.currentResolution;

                            Resolution[] supportedRess = Screen.resolutions;

                            int i = -1;
                            bool isCurrentRes = false;

                            foreach (Resolution resolution in supportedRess)
                            {
                                dropdownFieldView.dropdown.options.Add(new TMP_Dropdown.OptionData(resolution.ToString(), null));

                                if (!isCurrentRes)
                                {
                                    ++i;
                                    if (resolution.Equals(currentRes))
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

                            FullScreenMode savedFSM = (FullScreenMode) _fieldsData[dropdownFieldView].GetValue(_settingsData);
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

        public bool IsValueChanged(BaseFieldView updatedView)
        {
            if (updatedView is SliderFieldView sliderFieldView)
            {
                if (sliderFieldView.slider.wholeNumbers)
                {
                    if (!(sliderFieldView.slider.value == (int) _fieldsData[updatedView].GetValue(_settingsData)))
                        return true;
                }
                else
                {
                    if (!(sliderFieldView.slider.value == (float) _fieldsData[updatedView].GetValue(_settingsData)))
                        return true;
                }
            }

            else if (updatedView is DropdownFieldView dropdownFieldView)
            {
                SaveFieldAttribute attribute = Attribute.GetCustomAttribute(_fieldsData[dropdownFieldView], typeof(SaveFieldAttribute)) as SaveFieldAttribute;
                if (attribute != null)
                {
                    switch (attribute.fieldName)
                    {
                        case FieldName.ScreenResolution:
                            {
                                string dropdownText = dropdownFieldView.dropdown.options[dropdownFieldView.dropdown.value].text;
                                //Resolution res = (Resolution) _fieldsData[dropdownFieldView].GetValue(_settingsData);
                                Resolution res = Screen.currentResolution;

                                if (!(dropdownText == res.ToString()))
                                {
                                    return true;
                                }

                                break;
                            }
                        case FieldName.FullscreenMode:
                            {
                                string dropdownText = dropdownFieldView.dropdown.options[dropdownFieldView.dropdown.value].text;
                                //FullScreenMode fSM = (FullScreenMode) _fieldsData[dropdownFieldView].GetValue(_settingsData);
                                FullScreenMode fSM = Screen.fullScreenMode;

                                if (!(dropdownText == fSM.ToString()))
                                {
                                    return true;
                                }

                                break;
                            }
                    }
                }
            }

            else if (updatedView is ToggleFieldView toggleFieldView)
            {
                if (!(toggleFieldView.toggle.isOn == (bool) _fieldsData[updatedView].GetValue(_settingsData)))
                    return true;
            }

            else if (updatedView is KeyBindFieldView keyBindFieldView)
            {
                if (!(keyBindFieldView.keyBind == (KeyCode) _fieldsData[updatedView].GetValue(_settingsData)))
                    return true;
            }

            return false;
        }
    }
}