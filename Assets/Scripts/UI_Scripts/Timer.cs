using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float maxTime;
    public float currentTime; //In seconds
    public float timeToRed; //In seconds, what limit should the text go red
    public TextMeshProUGUI timerText;
    public bool pauseTimer = false;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(!pauseTimer)
        {
            if(currentTime > timeToRed)
            {
                currentTime -= Time.deltaTime;
                SetTimerText(currentTime);
            }
            else if(currentTime <= timeToRed && currentTime > 0)
            {
                timerText.color = Color.red;
                currentTime -= Time.deltaTime;
                SetTimerText(currentTime);
            }
            else
            {
                currentTime = 0;
                timerText.color = Color.red;
                timerText.text = string.Format("{0:00} : {1:00}", 0, 0);

                Invoke("transitionToNextScene",3.0f);

            }
        }

    }

    private void PauseTimer() //Call this function here if you want to pause and unpause the timer
    {
        pauseTimer = !pauseTimer;
    }

    private void SetTimerText(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    private void transitionToNextScene()
    {
        PauseTimer();
        SceneManager.LoadScene("GameOver");
    }
}
