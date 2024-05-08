using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThemeSongManager : MonoBehaviour {
    void Awake() {
        // Get the ThemeSong GameObject and the Current Scene
        GameObject themeSong = GameObject.FindGameObjectWithTag("ThemeSong");
        AudioLoopController audioLoopController = themeSong.GetComponent<AudioLoopController>();
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        // If the scene is the ProceduralGeneration scene, destroy the theme song
        if (sceneName == "StartScreen") {
            audioLoopController.play = true;
        } else if (sceneName == "FirstStoryBoard" || sceneName == "SecondStoryBoard") {
            audioLoopController.play = false;
        } else if (sceneName == "Introduction") {
            audioLoopController.play = true;
        } else if (sceneName == "ProceduralGeneration") {
            Destroy(themeSong);
        }//end if
        // If the scene is before the ProceduralGeneration scene, keep the theme song
        DontDestroyOnLoad(themeSong);
    }//end Awake()

    void Update() {
        // Get the ThemeSong GameObject and the Current Scene
        GameObject themeSong = GameObject.FindGameObjectWithTag("ThemeSong");
        AudioLoopController audioLoopController = themeSong.GetComponent<AudioLoopController>();
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        // If the scene is the ProceduralGeneration scene, destroy the theme song
        if (sceneName == "StartScreen") {
            audioLoopController.play = true;
        } else if (sceneName == "FirstStoryBoard" || sceneName == "SecondStoryBoard") {
            audioLoopController.play = false;
        } else if (sceneName == "Introduction") {
            audioLoopController.play = true;
        } else if (sceneName == "ProceduralGeneration") {
            Destroy(themeSong);
        }//end if
        // If the scene is before the ProceduralGeneration scene, keep the theme song
        DontDestroyOnLoad(themeSong);
    }//end Update()
}//end ThemeSongManager
