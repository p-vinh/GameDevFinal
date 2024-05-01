using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacrificeMenu : MonoBehaviour
{
    private GameObject player;
    public GameObject canvas;
    private bool isPlayerNear = false;
    void Start()
    {
        canvas.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {

        if (isPlayerNear)
        {
            ShowMenu();
        }

        if (!isPlayerNear)
        {
            CloseMenu();
        }
    }

    public void ShowMenu()
    {
        canvas.SetActive(true);
    }

    public void CloseMenu()
    {
        canvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }

}
