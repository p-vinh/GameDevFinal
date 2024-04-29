using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start() {
        audioSource = GetComponent<AudioSource>();
    }//end Start()

    // Update is called once per frame
    void Update() {
        // Check if the left mouse button was clicked
        if (Input.GetMouseButtonDown(0)) {
            // Create a ray from the mouse cursor on screen in the direction of the camera
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // Perform the raycast
            if (Physics.Raycast(ray, out hit)) {
                // If the raycast hit this game object
                if (hit.transform == this.transform) {
                    // Play the audio
                    audioSource.Play();
                }//end if
            }//end if
        }//end if
    }//end Update()
}//end Test
