using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial_Transition : MonoBehaviour
{
    public string transitionSceneName;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           Destroy(other.gameObject);
           SceneManager.LoadScene(transitionSceneName);
        }
    }
}
