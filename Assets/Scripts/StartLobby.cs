using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLobby : MonoBehaviour
{
    // TODO: Wait till everything is generated/PAUSE THE TIMER before moving player to start point
    public Transform startPoint;
    void Awake()
    {
        Transform player = GameObject.FindWithTag("Player").transform;
        player.position = startPoint.position;
    }
}
