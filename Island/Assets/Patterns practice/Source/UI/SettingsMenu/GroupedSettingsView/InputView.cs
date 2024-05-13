using UnityEngine;

namespace UI.SettingsManagement {
    public class InputView : MonoBehaviour {
        [SerializeField] public SliderFieldView mouseSensitivityX;
        [SerializeField] public SliderFieldView mouseSensitivityY;

        [SerializeField] public KeyBindFieldView saveGame; 
    }
}
