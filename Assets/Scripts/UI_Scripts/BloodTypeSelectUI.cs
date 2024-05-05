using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BloodTypeSelectUI : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI infoText;
    public GameObject acceptButton;

    private string choosenBloodType;

    [SerializeField] private LevelLoader levelLoader;

    // Start is called before the first frame update
    void Start()
    {
        // string title = "";
        // string info = "";
        acceptButton.SetActive(false);
    }

    //Update info text
    public void updateInfo(string bloodType)
    {
        acceptButton.SetActive(true);

        if (bloodType == "A")
        {
            title.text = "Blood Type A";
            title.color = Color.red;
            infoText.text = "By choosing this, enemies will MORE LIKELY drop blood bags";
            choosenBloodType = "A";
        }
        else if(bloodType == "B")
        {
            title.text = "Blood Type B";
            title.color = Color.red;
            infoText.text = "By choosing this, enemies will LESS LIKELY drop blood bags";
            choosenBloodType = "B";
        }
        else if(bloodType == "AB")
        {
            title.text = "Blood Type AB";
            title.color = Color.red;
            infoText.text = "By choosing this, enemies will RARELY drop blood bags \nNot recommended for beginners";
            choosenBloodType = "AB";
        }
        else if(bloodType == "O")
        {

            title.text = "Blood Type O";
            title.color = Color.red;
            infoText.text = "By choosing this, enemies will MODERATELY drop blood bags";
            choosenBloodType = "O";
        }

    }

    public void updatePlayerStats()
    {
        if(choosenBloodType == "A") // 
        {
            PlayerStats.Instance.bloodType = PlayerStats.BloodType.A;
            
        }
        else if(choosenBloodType == "B")
        {
            PlayerStats.Instance.bloodType = PlayerStats.BloodType.B;
        }
        else if( choosenBloodType == "AB")
        {
            PlayerStats.Instance.bloodType = PlayerStats.BloodType.AB;
        
        }
        else if(choosenBloodType == "O")
        {
            PlayerStats.Instance.bloodType = PlayerStats.BloodType.O;
            
        }

        print(PlayerStats.Instance.bloodType);

        StartCoroutine(FadeTransition("FirstStoryBoard"));


    }


    private IEnumerator FadeTransition(string sceneName)
    {
        print("HERE");
        levelLoader.fadeTransition();

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(sceneName);
    }
}
