using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSensors : MonoBehaviour {
    public Collider collider;
    public Transform player;
    public float sightRange = 15.0f;
    public AudioSource audioSource; // Main Audio Source for Room
    public AudioLoopController audioLoopController;

    // (If Sacrifice is main, this is Main Battle. 
    // If Alt Battle is main, this is Main Battle.
    // And If Main Battle is main this is Alt Battle.)
    public AudioSource audioSource2; // First Other Audio Source 
    public AudioLoopController audioLoopController2;
    public AudioSource audioSource3; // Second Other Audio Source
    public AudioLoopController audioLoopController3;
    
    // Position Variables
    private float playerX;
    private float playerZ;
    private float colliderX;
    private float colliderZ;
    private Vector3 colliderPosition;
    private float xDistance;
    private float zDistance;
    private float distance;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindWithTag("Player").transform;

        // Get the player's position in the x axis compared to the collider's sight range
        playerX = player.transform.position.x;
        playerZ = player.transform.position.z;
        colliderX = collider.transform.position.x;
        colliderZ = collider.transform.position.z;
        colliderPosition = collider.transform.position;
        
        // Get the x and z distance between the player and the collider
        xDistance = Mathf.Abs(playerX - colliderX);
        zDistance = Mathf.Abs(playerZ - colliderZ);
        // Get the distance between the player and the collider
        distance = Mathf.Sqrt(Mathf.Pow(xDistance, 2) + Mathf.Pow(zDistance, 2));
    }//end Start()

    // Update is called once per frame
    void Update() {
        CheckSightRange();
    }//end Update()

    void CheckSightRange() {
        // Get the player's position in the x axis compared to the collider's sight range
        playerX = player.transform.position.x;
        playerZ = player.transform.position.z;
        colliderX = collider.transform.position.x;
        colliderZ = collider.transform.position.z;
        colliderPosition = collider.transform.position;
        
        // Get the x and z distance between the player and the collider
        xDistance = Mathf.Abs(playerX - colliderX);
        zDistance = Mathf.Abs(playerZ - colliderZ);
        // Get the distance between the player and the collider
        distance = Mathf.Sqrt(Mathf.Pow(xDistance, 2) + Mathf.Pow(zDistance, 2));

        // Check if the player is within the collider's sight range
        if (distance <= sightRange) {
            if (audioLoopController2.play == true) {
                audioLoopController2.play = false;
            } else if (audioLoopController3.play == true) {
                audioLoopController3.play = false;
            }//end if
            
            if (!audioLoopController.play) {
                audioLoopController.play = true;
            }//end if
        }//end if
    }//end CheckSightRange()

    void OnDrawGizmosSelected() {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }//end OnDrawGizmosSelected()
}//end HallwayOneSound
