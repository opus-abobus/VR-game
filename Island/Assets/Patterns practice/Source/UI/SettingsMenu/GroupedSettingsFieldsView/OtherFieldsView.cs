using UnityEngine;

namespace UI.SettingsManagement {
    public class OtherFieldsView : MonoBehaviour {
        [SerializeField] public ToggleFieldView skipIntroVideo;
        [SerializeField] public SliderFieldView fieldOfView;

        [SerializeField] public DropdownFieldView UILanguage;

        [SerializeField] public ToggleFieldView autosave;
        [SerializeField] public ToggleFieldView saveOnExit;
        [SerializeField] public SliderFieldView autosaveInterval;
    }
}
