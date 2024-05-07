using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using static BlankStudio.Constants.Constants;
using System.Linq;

public class RoomDetector : MonoBehaviour
{
    [SerializeField]
    private RoomType m_RoomType = RoomType.EntryPoint;

    public static Action<RoomType, Bounds, Transform> PlayerEntered;

    [SerializeField]
    private BoxCollider m_BoxCollider = null;

    [SerializeField]
    private BoxCollider[] doorColliders = null;

    [SerializeField]
    private VisitStatus m_VisitStatus = VisitStatus.NotVisited;

    private bool navMeshGenerated = false;
    public GameObject roomNavMesh;
    [SerializeField] private int enemyCount = 0;
    private bool isSpawned = false;

    void Start()
    {
        m_VisitStatus = VisitStatus.NotVisited;
        m_BoxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        CountEnemies();
        if (enemyCount <= 0 && isSpawned)
        {
            foreach (var door in doorColliders)
            {
                door.enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (m_VisitStatus == VisitStatus.Visited)
            {
                return;
            }
            m_VisitStatus = VisitStatus.Visited;
            if (roomNavMesh != null)
                GenerateNavMesh();

            foreach (var door in doorColliders)
                door.enabled = true;

            PlayerEntered?.Invoke(m_RoomType, m_BoxCollider.bounds, collision.gameObject.transform);
            isSpawned = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        print("left collided with player");
        if (other.CompareTag("Player") && roomNavMesh != null)
        {
            DisableNavMesh();
        }
    }

    void GenerateNavMesh()
    {
        if (!navMeshGenerated)
        {
            print("Enable navmesh for this room");
            NavMeshSurface surface = roomNavMesh.AddComponent<NavMeshSurface>();
            surface.collectObjects = CollectObjects.Children;
            surface.BuildNavMesh();
            navMeshGenerated = true;
        }
    }

    void DisableNavMesh()
    {
        if (navMeshGenerated)
        {
            print("Disable navmesh for this room");
            UnityEngine.AI.NavMesh.RemoveAllNavMeshData();
            Destroy(GetComponent<NavMeshSurface>());
            navMeshGenerated = false;
        }
    }

    void CountEnemies()
    {
        enemyCount = 0;
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (m_BoxCollider.bounds.Contains(enemy.transform.position))
            {
                enemyCount++;
            }
        }

        if (enemyCount <= 0 && isSpawned)
        {
            foreach (var door in doorColliders)
            {
                door.enabled = false;
            }
        }
    }
    /*  private VisitStatus GetRoomVisitStatus(RoomType roomType)
      {
          string key = roomType.ToString() + "_VisitStatus";
          if (PlayerPrefs.HasKey(key))
          {
              return (VisitStatus)PlayerPrefs.GetInt(key);
          }
          return VisitStatus.NotVisited; 
      }

      private void SetRoomVisitStatus(RoomType roomType, VisitStatus status)
      {
          string key = roomType.ToString() + "_VisitStatus";
          PlayerPrefs.SetInt(key, (int)status);
      }*/
}
