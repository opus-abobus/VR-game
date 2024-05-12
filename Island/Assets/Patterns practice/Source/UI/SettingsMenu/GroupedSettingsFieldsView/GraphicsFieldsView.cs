using TMPro;
using UnityEngine;

namespace UI.SettingsManagement {
    public class GraphicsFieldView : MonoBehaviour {
        [SerializeField] public TMP_Dropdown screenRes;
        [SerializeField] public TMP_Dropdown screenRatio;
        [SerializeField] public TMP_Dropdown displayMode;

        [SerializeField] public TMP_Dropdown graphicsPreset;
    }
}
