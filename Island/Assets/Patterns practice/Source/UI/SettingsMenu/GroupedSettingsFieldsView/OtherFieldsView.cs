using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SettingsManagement {
    public class OtherFieldsView : MonoBehaviour {
        [SerializeField] public Toggle skipIntroVideo;
        [SerializeField] public SliderFieldView fieldOfView;

        [SerializeField] public TMP_Dropdown UILanguage;

        [SerializeField] public Toggle autosave;
        [SerializeField] public Toggle saveOnExit;
        [SerializeField] public SliderFieldView autosaveInterval;
    }
}
