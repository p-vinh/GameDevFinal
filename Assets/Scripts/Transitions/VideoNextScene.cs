using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoNextScene : MonoBehaviour
{
    [SerializeField]
    VideoPlayer myVideoPlayer;
    public string transitionSceneName;

    // Start is called before the first frame update
    void Start()
    {
        myVideoPlayer.loopPointReached += transitionToNextScene;
    }

    void transitionToNextScene(VideoPlayer vp)
    {
        print("Video is finished...transitioning....");
        SceneManager.LoadScene(transitionSceneName);
    }


}
