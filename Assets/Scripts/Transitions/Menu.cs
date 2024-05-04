using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private LevelLoader levelLoader;

    public void MainMenu()
    {
        StartCoroutine(FadeTransition("MainMenu"));
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Respawn()
    {
        // TODO Reset Player's Stats
        
        StartCoroutine(FadeTransition("Dungeon"));
    }

    private IEnumerator FadeTransition(string sceneName)
    {
        levelLoader.fadeTransition();

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(sceneName);
    }

}
