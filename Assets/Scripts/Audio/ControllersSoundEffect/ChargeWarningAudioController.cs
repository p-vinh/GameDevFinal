using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeWarningAudioController : MonoBehaviour {
    public AudioSource audioSource;
    public float playTime = 2.0f;
    public bool play = false;
    private float audioVolume;
    public float fadeOutSpeed = 1.0f;
    private bool keepFadingOut = false;
    private bool isFadingOut = false;

    // Start is called before the first frame update
    void Start() {
        audioSource = GetComponent<AudioSource>();
        audioVolume = audioSource.volume;
    }//end Start()

    // Update is called once per frame
    void Update() {
        if (play == true) {
            if (!audioSource.isPlaying) {
                audioSource.Play();
            }//end if
            
            if (audioSource.time >= playTime && !isFadingOut) {
                FadeOutCaller();
                isFadingOut = true;
                Debug.Log("Fading Out");

                if (audioVolume <= 0.0f) {
                    audioSource.Stop();
                    audioSource.time = 0.0f;
                    keepFadingOut = false;
                    isFadingOut = false;
                    play = false;
                    Debug.Log("Audio Stopped");
                }//end if
            }//end if
        }//end if
    }//end Update()

    // Fade Out Caller Helper Method
    public void FadeOutCaller() {
        StartCoroutine(FadeOut());
    }//end FadeOutCaller()

    // Fade out the Song Audio
    public IEnumerator FadeOut() {
        keepFadingOut = true;

        audioVolume = audioSource.volume;

        while (audioSource.volume >= 0.0f && keepFadingOut) {
            audioVolume -= fadeOutSpeed * Time.deltaTime;
            audioSource.volume = audioVolume;
            yield return null;
        }//end while

        isFadingOut = false;
    }//end FadeOut()
}//end ChargeWarningAudioController
