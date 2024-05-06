using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject focus; //this is to help disable it and renable

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                print("Pause");
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        focus.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    void Pause()
    {
        //TODO: Pause timer
        pauseMenuUI.SetActive(true);
        focus.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScreen");
    }

    public void QuitGame()
    {
        print("Quit game");
        Application.Quit();
    }
}
