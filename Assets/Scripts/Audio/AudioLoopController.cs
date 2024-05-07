using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoopController : MonoBehaviour {
    public AudioSource audioSource;
    public float playTime = 120.0f; // Set the play time to 2mins
    public float startTime = 0.0f; // Set the start time to 0 seconds
    public bool play = false;
    public float fadeInSpeed = 0.1f;
    public float maxVolume = 0.34f;
    private float audioVolume = 0.0f;
    private bool keepFadingIn = false;
    private bool keepFadingOut = false;

    private void Start() {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        // Play the audio clip initially
        if (play == true) {
            audioSource.time = startTime;
            audioSource.Play();
        }//end if

        // Check if audio source time is less than the play time
        // if (audioSource.time < playTime) {
        //     // Set the audio source time to the play time
        //     playTime = audioSource.clip.length;
        // }//end if
    }//end Start()

    private void Update() {
        // Check if the audio clip has finished playing
        if (play == true) {
            if (!audioSource.isPlaying) {
                // Restart the audio clip
                audioSource.time = startTime;
                FadeInCaller();
                audioSource.Play();
            }//end if

            if (audioSource.time >= playTime) {
                // Stop the audio clip
                audioSource.Stop();

                audioSource.time = startTime;
                audioSource.Play();
            }//end if
        } else if (play == false && audioSource.isPlaying) {
            // Stop the audio clip
            FadeOutCaller();
            audioSource.Stop();
        }//end while
    }//end Update()

    public void FadeInCaller() {
        StartCoroutine(FadeIn());
    }//end FadeInCaller()

    public void FadeOutCaller() {
        StartCoroutine(FadeOut());
    }//end FadeOutCaller()

    // Fade in the Song Audio
    public IEnumerator FadeIn() {
        keepFadingIn = true;
        keepFadingOut = false;

        audioSource.volume = 0.0f;
        audioVolume = audioSource.volume;

        while (audioSource.volume < maxVolume && keepFadingIn) {
            audioVolume += fadeInSpeed * Time.deltaTime;
            audioSource.volume = audioVolume;
            yield return new WaitForSeconds(0.1f);
        }//end while
    }//end FadeIn()

    // Fade out the Song Audio
    public IEnumerator FadeOut() {
        keepFadingIn = false;
        keepFadingOut = true;

        audioVolume = audioSource.volume;

        while (audioSource.volume >= 0.0f && keepFadingOut) {
            audioVolume -= fadeInSpeed * Time.deltaTime;
            audioSource.volume = audioVolume;
            yield return new WaitForSeconds(0.1f);
        }//end while
    }//end FadeOut()
}//end AudioLoopController

