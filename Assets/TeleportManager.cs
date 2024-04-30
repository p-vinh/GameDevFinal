using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    public GameObject intialSpawnPoint;
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            TeleportPlayer();
        }
    }

    public void TeleportPlayer()
    {
        if (gameObject.CompareTag("SacrificeRoom"))
        {
            Vector3 newPos = new Vector3(PlayerStats.Instance.LastDoorEntered.x, PlayerStats.Instance.LastDoorEntered.y, PlayerStats.Instance.LastDoorEntered.z - 2f);
            player.transform.position = newPos;
        }
        else
        {
            PlayerStats.Instance.LastDoorEntered = player.transform.position;
            player.transform.position = intialSpawnPoint.transform.position;
        }

    }

}
