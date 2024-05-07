using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicAttackSoundController : MonoBehaviour {
    public AudioSource attackSound;
    public float playTime = 1.0f;
    public float startTime = 0.0f;
    public bool play = false;

    // Start is called before the first frame update
    void Start() {
        attackSound = GetComponent<AudioSource>();
    }//end Start()

    // Update is called once per frame
    void Update() {
        if (play == true) {
            if (!attackSound.isPlaying) {
                attackSound.time = startTime;
                attackSound.Play();
            }//end if

            if (attackSound.time >= playTime) {
                attackSound.Stop();
                attackSound.time = startTime;
                play = false;
            }//end if
        }//end if
    }//end Update()
}//end MimicAttackSoundController
