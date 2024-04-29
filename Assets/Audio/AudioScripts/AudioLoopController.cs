using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoopController : MonoBehaviour {
    public AudioSource audioSource;
    public float playTime = 120.0f; // Set the play time to 2mins
    public float startTime = 0.0f; // Set the start time to 0 seconds

    private void Start() {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        audioSource.time = startTime;

        // Play the audio clip initially
        audioSource.Play();

        // Check if audio source time is less than the play time
        // if (audioSource.time < playTime) {
        //     // Set the audio source time to the play time
        //     playTime = audioSource.clip.length;
        // }//end if
    }//end Start()

    private void Update() {
        // Check if the audio clip has finished playing
        if (audioSource.time >= playTime) {
            // Stop the audio clip
            audioSource.Stop();

            audioSource.time = startTime;
            audioSource.Play();
        }//end if

        if (!audioSource.isPlaying) {
            // Restart the audio clip
            audioSource.time = startTime;
            audioSource.Play();
        }//end if
    }//end Update()
}//end AudioLoopController

