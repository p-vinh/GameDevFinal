using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionUI : MonoBehaviour
{

    public GameObject Tutorial;
    // Start is called before the first frame update
    void Start()
    {
        Tutorial.SetActive(false);
        Invoke("turonONUI",1f);
        Invoke("turnOffUI", 8f);

    }
    

    private void turnOffUI()
    {
        Tutorial.SetActive(false);
    }

    private void turonONUI()
    {
        Tutorial.SetActive(true);
    }
}
