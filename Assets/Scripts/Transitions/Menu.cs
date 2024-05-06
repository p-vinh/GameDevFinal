using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private LevelLoader levelLoader;

    public void MainMenu()
    {
        StartCoroutine(FadeTransition("StartScreen"));
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Respawn()
    {
        PlayerStats.Instance.ResetStats();
        StartCoroutine(FadeTransition("ProceduralGeneration"));
    }

    private IEnumerator FadeTransition(string sceneName)
    {
        levelLoader.fadeTransition();

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(sceneName);
    }

}
