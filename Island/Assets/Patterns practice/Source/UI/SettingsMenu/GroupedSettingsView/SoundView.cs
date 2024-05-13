using UnityEngine;

namespace UI.SettingsManagement {
    public class SoundView : MonoBehaviour {
        [SerializeField] public SliderFieldView totalVolume;
        [SerializeField] public SliderFieldView musicVolume;

        [SerializeField] public SliderFieldView playersStepsVolume;
    }
}
