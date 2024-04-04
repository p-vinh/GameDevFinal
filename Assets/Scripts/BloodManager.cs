using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodManager : MonoBehaviour
{
    private Dictionary<string, float> bloodTypeDropPercentages = new Dictionary<string, float>
    {
        { "A", 0.42f },
        { "B", 0.11f },
        { "AB", 0.04f },
        { "O", 0.43f }
    };

    private string playerBloodType;
    // Start is called before the first frame update
    void Start()
    {
        // The player manager will be a singleton that holds the player's blood type
        playerBloodType = PlayerManager.Instance.playerBloodType;
    }

    void Update()
    {
        float random = Random.Range(0.0f, 1.0f);

        foreach (KeyValuePair<string, float> bloodTypeDropPercentage in bloodTypeDropPercentages)
        {
            if (random < bloodTypeDropPercentage.Value)
            {
                if (playerBloodType == bloodTypeDropPercentage.Key)
                {
                    Debug.Log("Player got blood type " + playerBloodType);
                }
                else
                {
                    Debug.Log("Player did not get blood type " + playerBloodType);
                }
                break;
            }
            random -= bloodTypeDropPercentage.Value;
        }        


    }
}
