using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAudio : MonoBehaviour {
    public AudioSource audioSource;
    public float playTime = 6.0f; // Set the play time to 6 seconds

    // Start is called before the first frame update
    void Start() {
        audioSource = GetComponent<AudioSource>();
    }//end Start()

    // TODO: Link correctly to the Input Manager
    void Update() {
        // If the door is clicked, play the audio
        if (Input.GetMouseButtonDown(0)) {
            audioSource.Play();

            // Stop audio at playTime
            if (audioSource.time >= playTime) {
                audioSource.Stop();
                audioSource.time = 0.0f;
            }//end if
        }//end if
    }//end Update()
}//end DoorController
