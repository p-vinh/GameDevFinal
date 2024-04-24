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
    // void Update() {
    //     // If the door is clicked, play the audio
    //     if (Input.GetMouseButtonDown(0)) {
    //         OnMouseDown();
    //     }//end if
    // }//end Update()

    // // Play the audio when the door is clicked
    // void OnMouseDown() {
    //     StartCoroutine(PlayForSeconds(playTime));
    // }//end OnMouseDown()

    // Play the audio for a set amount of time (0.06 seconds)
    IEnumerator PlayForSeconds(float seconds) {
        audioSource.Play();
        yield return new WaitForSeconds(seconds);
        audioSource.Stop();
    }//end PlayForSeconds()
}//end DoorController
