using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static BlankStudio.Constants.Constants;
using DG.Tweening;
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemySpawnerData m_EnemySpawnerData;
    [SerializeField] private List<GameObject> m_Enemies = new List<GameObject>();
    [SerializeField] private GameObject m_EnemySpawnFX = null;
    [SerializeField] private RoomType m_CurrentRoomType = RoomType.Room_1;
    [SerializeField] private int m_SpawnerCount = 0;
    [SerializeField] private GameObject RoomEnemies;
    public static Action<bool> lockAllDoors;
    [SerializeField] private int m_EnemyCount = 0;

    private List<Vector3> m_SpawnPoints = new List<Vector3>();

    private void OnEnable()
    {
        RoomDetector.PlayerEntered += SpawnEnemies;
        EnemyAI.EnemyDied += EnemyDied;
    }

    private void OnDisable()
    {
        RoomDetector.PlayerEntered -= SpawnEnemies; 
        EnemyAI.EnemyDied -= EnemyDied;
    }

    void Update()
    {
        if (m_EnemyCount <= 0)
        {
            lockAllDoors?.Invoke(false);
        }
    }

    private void SpawnEnemies(RoomType roomType, Bounds roomBounds, Transform roomTransform)
    {
        EnemyTypeData enemyData = m_EnemySpawnerData.GetEnemyProperties(roomType);
        if (enemyData == null)
        {
            Debug.LogWarning("No enemy data found for the current room type.");
            return;
        } 

        m_Enemies = enemyData._EnemyPrefeb;
        m_SpawnerCount = enemyData._EnemyCount <= 0 ? 10 : enemyData._EnemyCount;
        GenerateRandomCoordinates(m_SpawnerCount, roomBounds, roomTransform); 
    }

    private void GenerateRandomCoordinates(int spawnCount, Bounds bounds, Transform roomTransform)
    {
        HashSet<Vector3> existingSpawnPoints = new HashSet<Vector3>(m_SpawnPoints);
        Debug.Log("Coordinates generated");

        while (m_SpawnPoints.Count < spawnCount)
        {
            Vector3 position = GetCoordinates(bounds);
            position.y = roomTransform.position.y;

            if (existingSpawnPoints.Add(position)) // Add returns false if the point already exists
            {
                m_SpawnPoints.Add(position);
            }
        }

        lockAllDoors?.Invoke(true);
        GenerateEnemies();
    }

    private Vector3 GetCoordinates(Bounds bounds)
    {
        return new Vector3(
            UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
            0,
            UnityEngine.Random.Range(bounds.min.z, bounds.max.z)
        );
    }
        
    private void GenerateEnemies()
    {
        for (int i = 0; i < m_SpawnerCount; i++)
        {
            GameObject enemyPrefab = m_Enemies[UnityEngine.Random.Range(0, m_Enemies.Count)];
            StartCoroutine(AnimateEnemies(enemyPrefab, i));    
        }

        m_EnemyCount = m_SpawnerCount;
    }

    private IEnumerator AnimateEnemies(GameObject enemy, int i) 
    {
        GameObject fx = Instantiate(m_EnemySpawnFX, m_SpawnPoints[i], m_EnemySpawnFX.transform.rotation);
        yield return new WaitForSeconds(0.2f);
        GameObject enemyClone = Instantiate(enemy, m_SpawnPoints[i], Quaternion.identity, RoomEnemies.transform); // TODO: Spawn enemies under parent object per room
        Vector3 originalScale = enemy.transform.localScale;
        enemyClone.transform.localScale = Vector3.zero; 
        enemyClone.transform.DOScale(originalScale, 0.5f);
        yield return new WaitForEndOfFrame();
        Destroy(fx);
    }

    private void EnemyDied()
    {
        m_EnemyCount--;
    }
}
