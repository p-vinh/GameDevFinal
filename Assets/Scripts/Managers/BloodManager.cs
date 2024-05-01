using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodManager : MonoBehaviour
{
    private Dictionary<string, float> bloodTypeDropPercentages = new Dictionary<string, float>
    {
        { "A", 100f },
        { "B", 0.11f },
        { "AB", 0.04f },
        { "O", 0.43f }
    };

    private string playerBloodType;

    [SerializeField] private GameObject bloodDropPrefab;

    void Start()
    {
        playerBloodType = PlayerStats.Instance.bloodType.ToString();
    }

    public void OnEnemyDeath(Transform enemyTransform)
    {
        float random = Random.Range(0.0f, 1.0f);

        foreach (KeyValuePair<string, float> bloodTypeDropPercentage in bloodTypeDropPercentages)
        {
            if (random < bloodTypeDropPercentage.Value)
            {
                if (playerBloodType == bloodTypeDropPercentage.Key)
                {
                    Debug.Log("Enemy dropped blood type " + playerBloodType);
                    Instantiate(bloodDropPrefab, enemyTransform.position, Quaternion.identity);
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
