using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This should only be attached to the Sacrifice Door GameObject
public class SacrificeSongController : MonoBehaviour {
    // Variables //
    private GameObject door;
    public AudioSource srcSacrifice;
    private AudioLoopController audioLoopControllerSacrifice;
    public AudioSource srcMaze;
    public AudioSource srcNight;
    private AudioLoopController audioLoopControllerMaze;
    private AudioLoopController audioLoopControllerNight;
    private bool isPlaying = false;
    private AudioSource lastPlayedBattleSong;
    private BattleSongSwitchController battleSongSwitchController;
    private AudioLoopController audioLoopControllerLastBattleSong;

    // Start is called before the first frame update
    void Start() {
        door = GetComponent<GameObject>();
        audioLoopControllerSacrifice = srcSacrifice.GetComponent<AudioLoopController>();
        audioLoopControllerMaze = srcMaze.GetComponent<AudioLoopController>();
        audioLoopControllerNight = srcNight.GetComponent<AudioLoopController>();
        battleSongSwitchController = GetComponent<BattleSongSwitchController>();
        lastPlayedBattleSong = battleSongSwitchController.lastPlayed;
        audioLoopControllerLastBattleSong = lastPlayedBattleSong.GetComponent<AudioLoopController>();
    }//end Start()

    // Update is called once per frame
    void Update() {
        // If Player clicks on the door // TODO: Make sure this will work with the player's mouse click
        if (door.tag != "Interact" && Input.GetMouseButtonDown(0)) {
            // Check if this is the first time the player is clicking on the door
            if (!isPlaying) {
                lastPlayedBattleSong = battleSongSwitchController.lastPlayed; // Save the last battle song
            }//end if

            // Check if the player is going back through the door
            if (isPlaying) {
                // Stop the sacrifice audio source if it is playing
                audioLoopControllerSacrifice.play = false;
                isPlaying = false;
                // Play the last played battle song
                audioLoopControllerLastBattleSong.play = true;
                battleSongSwitchController.lastPlayed = lastPlayedBattleSong;
            }//end if

            // Play the sacrifice audio source and stop the Battle Audio
            audioLoopControllerSacrifice.play = true;
            audioLoopControllerMaze.play = false;
            audioLoopControllerNight.play = false;
            isPlaying = true;
        }//end if
    }//end Update()
}//end SacrificeSongController
