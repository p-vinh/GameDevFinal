using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    // Reference to enemy prefab
    public GameObject enemy;
    public GameObject chargeEnemy1;
    public GameObject chargeEnemy2;
    public GameObject mageEnemy;
    public GameObject chestEnemy;
    public GameObject rangeEnemy;
    /*
        ADD IN ANY OTHER ENEMY TYPES HERE
        THEN ATTACH GAME OBJECT
    */


    // Array of all spawn points in map
    public Transform[] spawnPoints;

    // Wave interval
    public float waveInterval = 30f;

    // Start is called before the first frame update
    void Start()
    {
        // Empty
        /* TEST */
        SpawnOne();
        SpawnTwo();
        SpawnThree();
        SpawnFour();
        
    }

    // Update is called once per frame
    void Update()
    {
        /*()    CREATE IF STATEMENTS SO THAT WHEN PLAYER TPS/ENTERS NEW CHAMBER
                IT WILL CALL THE SPAWN FUNCTION FOR THAT CHAMBER
        // Check condition for chamber 1 (either by tp check or by position check, some sort of bool)
        if player is in chamber1 {
            SpawnOne();
        }
        // Check condition for chamber 2
        if player is in chamber2 {
            SpawnTwo();
        }
        // Check condition for chamber 3
        if player is in chamber3 {
            SpawnThree();
        }
        // Check condition for chamber 4
        if player is in chamber4 {
            SpawnFour();
        }

        // Check condition for chamber 5
        if player is in chamber5 {
            SpawnFive();
        }
        */
    }

    // Chamber 1
    void SpawnOne() {
        Instantiate(enemy, spawnPoints[0].position, spawnPoints[0].rotation);
        Instantiate(enemy, spawnPoints[1].position, spawnPoints[1].rotation);
        Instantiate(enemy, spawnPoints[2].position, spawnPoints[2].rotation);
    }

    // Chamber 2
    void SpawnTwo() {
        Instantiate(chargeEnemy1, spawnPoints[3].position, spawnPoints[3].rotation);
        Instantiate(chestEnemy, spawnPoints[4].position, spawnPoints[4].rotation);
        Instantiate(chestEnemy, spawnPoints[5].position, spawnPoints[5].rotation);
        Instantiate(chargeEnemy2, spawnPoints[6].position, spawnPoints[6].rotation);
        Instantiate(rangeEnemy, spawnPoints[7].position, spawnPoints[7].rotation);
        Instantiate(rangeEnemy, spawnPoints[8].position, spawnPoints[8].rotation);
    }

    // Chamber 3
    void SpawnThree() {
        Instantiate(mageEnemy, spawnPoints[9].position, spawnPoints[9].rotation);
        Instantiate(mageEnemy, spawnPoints[10].position, spawnPoints[10].rotation);
        Instantiate(enemy, spawnPoints[11].position, spawnPoints[11].rotation);
        Instantiate(enemy, spawnPoints[12].position, spawnPoints[12].rotation);
    }

    // Chamber 4
    void SpawnFour() {
        Instantiate(enemy, spawnPoints[13].position, spawnPoints[13].rotation);
        Instantiate(chargeEnemy1, spawnPoints[14].position, spawnPoints[14].rotation);
        Instantiate(chestEnemy, spawnPoints[15].position, spawnPoints[15].rotation);
        Instantiate(rangeEnemy, spawnPoints[16].position, spawnPoints[16].rotation);
        Instantiate(mageEnemy, spawnPoints[17].position, spawnPoints[17].rotation);
        Instantiate(enemy, spawnPoints[18].position, spawnPoints[18].rotation);
        Instantiate(enemy, spawnPoints[19].position, spawnPoints[19].rotation);
    }
}
