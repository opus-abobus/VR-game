using UnityEngine;

public class AmbientAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    private void Start()
    {
        _audioSource.volume = AppManager.Instance.DataManager.SettingsData.MusicVolume;
    }
}
