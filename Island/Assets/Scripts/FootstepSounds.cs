using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class FootstepSounds : MonoBehaviour
{
    [SerializeField] private AudioClip[] sandFootstepSounds;
    [SerializeField] private AudioClip[] groundFootstepSounds;
    [SerializeField] private AudioClip[] grassFootstepSounds;
    [SerializeField] private AudioClip[] rockFootstepSounds;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private KeyCode[] movementKeys = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
    [SerializeField] private float footstepDelay = 0.5f;

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
                    string groundMaterialName = hitInfo.collider.GetComponent<Renderer>().material.name;
                    AudioClip[] footstepSounds;

                    Console.Write(groundMaterialName);

                    switch (groundMaterialName)
                    {
                        case "Sand":
                            footstepSounds = sandFootstepSounds;
                            break;
                        case "Ground":
                            footstepSounds = groundFootstepSounds;
                            break;
                        case "Grass":
                            footstepSounds = grassFootstepSounds;
                            break;
                        case "Base":
                            footstepSounds = rockFootstepSounds;
                            break;
                        default:
                            return;
                    }

                    if (footstepSounds.Length > 0)
                    {
                        int randomIndex = UnityEngine.Random.Range(0, footstepSounds.Length);
                        AudioClip footstepSound = footstepSounds[randomIndex];
                        audioSource.PlayOneShot(footstepSound);
                        lastFootstepTime = Time.time;
                    }
                }
            }
        }
    }
}