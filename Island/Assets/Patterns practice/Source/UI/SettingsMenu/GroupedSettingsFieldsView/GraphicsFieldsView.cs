using UnityEngine;

namespace UI.SettingsManagement {
    public class GraphicsFieldView : MonoBehaviour {
        [SerializeField] public DropdownFieldView screenRes;
        [SerializeField] public DropdownFieldView screenRatio;
        [SerializeField] public DropdownFieldView displayMode;

        [SerializeField] public DropdownFieldView graphicsPreset;
    }
}
