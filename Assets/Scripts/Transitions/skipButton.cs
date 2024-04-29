using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class skipButton : MonoBehaviour
{
    public void changeScene(string transitionSceneName)
    {
        SceneManager.LoadScene(transitionSceneName);
    }
}
