using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinSoundController : MonoBehaviour {
    public AudioSource winSound;
    public float playTime = 5.0f;

    // Start is called before the first frame update
    void Start() {
        winSound.Play();

        if (winSound.time >= playTime) {
            winSound.Stop();
            winSound.time = 0.0f;
        }//end if
    }//end Start()

    // Update is called once per frame
    void Update() {
        if (winSound.time >= playTime) {
            winSound.Stop();
            winSound.time = 0.0f;
        }//end if
    }//end Update()
}//end WinSoundController
