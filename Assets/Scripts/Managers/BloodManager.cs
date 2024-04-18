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
    void Start()
    {
        playerBloodType = PlayerStats.Instance.bloodType.ToString();
    }

    public void OnEnemyDeath()
    {
        float random = Random.Range(0.0f, 1.0f);

        foreach (KeyValuePair<string, float> bloodTypeDropPercentage in bloodTypeDropPercentages)
        {
            if (random < bloodTypeDropPercentage.Value)
            {
                if (playerBloodType == bloodTypeDropPercentage.Key)
                {
                    Debug.Log("Enemy dropped blood type " + playerBloodType);
                }
                else
                {
                    Debug.Log("Enemy did not drop blood type " + playerBloodType);
                }
                break;
            }
            random -= bloodTypeDropPercentage.Value;
        }
    }
}
