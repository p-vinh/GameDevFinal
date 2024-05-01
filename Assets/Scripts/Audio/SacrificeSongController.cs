using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacrificeSongController : MonoBehaviour {
    public AudioSource srcSacrifice;
    private AudioLoopController audioLoopControllerSacrifice;
    public AudioSource srcMaze;
    public AudioSource srcNight;
    private AudioLoopController audioLoopControllerMaze;
    private AudioLoopController audioLoopControllerNight;

    // Start is called before the first frame update
    void Start() {
        audioLoopControllerSacrifice = srcSacrifice.GetComponent<AudioLoopController>();
        audioLoopControllerMaze = srcMaze.GetComponent<AudioLoopController>();
        audioLoopControllerNight = srcNight.GetComponent<AudioLoopController>();
    }//end Start()

    // Update is called once per frame
    void Update() {
        // This only needs to close the Battle Audio. The BattleSongController knows which song is the last played.
        // If Player clicks on the door // TODO: Make sure this will work with the player's mouse click
        if (Input.GetMouseButtonDown(0)) {
            audioLoopControllerSacrifice.play = true;
            audioLoopControllerMaze.play = false;
            audioLoopControllerNight.play = false;
        }//end if
    }//end Update()
}//end SacrificeSongController
