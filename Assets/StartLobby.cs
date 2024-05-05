using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLobby : MonoBehaviour
{
    public Transform startPoint;
    void Awake()
    {
        Transform player = GameObject.FindWithTag("Player").transform;
        player.position = startPoint.position;
    }
}
