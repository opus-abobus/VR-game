using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] _baseFootstepSounds;
    [SerializeField] private AudioClip[] _darkGroundFootstepSounds;
    [SerializeField] private AudioClip[] _darkMountainsFootstepSounds;
    [SerializeField] private AudioClip[] _darkSandFootstepSounds;
    [SerializeField] private AudioClip[] _grassFootstepSounds;
    [SerializeField] private AudioClip[] _groundFootstepSounds;
    [SerializeField] private AudioClip[] _lightGrassFootstepSounds;
    [SerializeField] private AudioClip[] _mountainsFootstepSounds;
    [SerializeField] private AudioClip[] _sandFootstepSounds;
    [SerializeField] private AudioClip[] _woodFootstepSounds;

    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _footstepDelay = 0.5f;

    [SerializeField] float _minVolume = 0.3f;
    [SerializeField] float _maxVolume = 0.5f;

    private AudioSource _audioSource;
    private float _lastFootstepTime;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (Time.time - _lastFootstepTime >= _footstepDelay)
            {
                if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 0.5f, _groundLayer))
                {
                    Renderer renderer = hitInfo.collider.gameObject.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        Material material = renderer.material;
                        AudioClip[] footstepSounds;

                        string materialName = material.name;

                        switch (materialName)
                        {

                            case "Base (Instance)":
                                footstepSounds = _baseFootstepSounds;
                                break;
                            case "DarkGround (Instance)":
                                footstepSounds = _darkGroundFootstepSounds;
                                break;
                            case "DarkMountains (Instance)":
                                footstepSounds = _darkMountainsFootstepSounds;
                                break;
                            case "DarkSand (Instance)":
                                footstepSounds = _darkSandFootstepSounds;
                                break;
                            case "Grass (Instance)":
                                footstepSounds = _grassFootstepSounds;
                                break;
                            case "Ground (Instance)":
                                footstepSounds = _groundFootstepSounds;
                                break;
                            case "LightGrass (Instance)":
                                footstepSounds = _lightGrassFootstepSounds;
                                break;
                            case "Mountains (Instance)":
                                footstepSounds = _mountainsFootstepSounds;
                                break;
                            case "Sand (Instance)":
                                footstepSounds = _sandFootstepSounds;
                                break;
                            case "Wood (Instance)":
                                footstepSounds = _woodFootstepSounds;
                                break;  
                            default:
                                return;
                        }

                        if (footstepSounds.Length > 0)
                        {
                            int randomIndex = Random.Range(0, footstepSounds.Length);
                            float randomVolume = Random.Range(_minVolume, _maxVolume);

                            AudioClip footstepSound = footstepSounds[randomIndex];
                            _audioSource.PlayOneShot(footstepSound, randomVolume);
                            _lastFootstepTime = Time.time;
                        }
                    }
                }
            }
        }
    }
}