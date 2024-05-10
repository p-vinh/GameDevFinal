using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial_Transition : MonoBehaviour
{
    public string transitionSceneName;

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("Here"); 
        if (other.gameObject.CompareTag("Player"))
        { 
           Destroy(other.gameObject);
           SceneManager.LoadScene(transitionSceneName);
        }
    }
}
