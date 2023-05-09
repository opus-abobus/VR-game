using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] baseFootstepSounds;
    [SerializeField] private AudioClip[] darkGroundFootstepSounds;
    [SerializeField] private AudioClip[] darkMountainsFootstepSounds;
    [SerializeField] private AudioClip[] darkSandFootstepSounds;
    [SerializeField] private AudioClip[] grassFootstepSounds;
    [SerializeField] private AudioClip[] groundFootstepSounds;
    [SerializeField] private AudioClip[] lightGrassFootstepSounds;
    [SerializeField] private AudioClip[] mountainsFootstepSounds;
    [SerializeField] private AudioClip[] sandFootstepSounds;
    [SerializeField] private AudioClip[] woodFootstepSounds;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float footstepDelay = 0.5f;

    [SerializeField] float minVolume = 0.3f;
    [SerializeField] float maxVolume = 0.5f;

    private AudioSource audioSource;
    private float lastFootstepTime;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (Time.time - lastFootstepTime >= footstepDelay)
            {
                if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 0.5f, groundLayer))
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
                                footstepSounds = baseFootstepSounds;
                                break;
                            case "DarkGround (Instance)":
                                footstepSounds = darkGroundFootstepSounds;
                                break;
                            case "DarkMountains (Instance)":
                                footstepSounds = darkMountainsFootstepSounds;
                                break;
                            case "DarkSand (Instance)":
                                footstepSounds = darkSandFootstepSounds;
                                break;
                            case "Grass (Instance)":
                                footstepSounds = grassFootstepSounds;
                                break;
                            case "Ground (Instance)":
                                footstepSounds = groundFootstepSounds;
                                break;
                            case "LightGrass (Instance)":
                                footstepSounds = lightGrassFootstepSounds;
                                break;
                            case "Mountains (Instance)":
                                footstepSounds = mountainsFootstepSounds;
                                break;
                            case "Sand (Instance)":
                                footstepSounds = sandFootstepSounds;
                                break;
                            case "Wood (Instance)":
                                footstepSounds = woodFootstepSounds;
                                break;  
                            default:
                                return;
                        }

                        if (footstepSounds.Length > 0)
                        {
                            int randomIndex = Random.Range(0, footstepSounds.Length);
                            float randomVolume = Random.Range(minVolume, maxVolume);

                            AudioClip footstepSound = footstepSounds[randomIndex];
                            audioSource.PlayOneShot(footstepSound, randomVolume);
                            lastFootstepTime = Time.time;
                        }
                    }
                }
            }
        }
    }
}