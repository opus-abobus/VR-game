using UnityEngine;

namespace UI.SettingsManagement {
    public class InputFieldsView : MonoBehaviour {
        [SerializeField] public SliderFieldView mouseSensitivityX;
        [SerializeField] public SliderFieldView mouseSensitivityY;

        [SerializeField] public KeyBindFieldView saveGame; 
    }
}
