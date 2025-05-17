using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Require adioSource to be attached to this gameObject
[RequireComponent(typeof(AudioSource))]

public class PlaySound : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip soundToPlay;
    public float volume = 1.0f; // Volume of the sound effect

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        
        audioSource.PlayOneShot(soundToPlay, volume); // Play the sound effect once
    }

}
