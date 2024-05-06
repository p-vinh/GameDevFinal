using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class NavMeshGenerate : MonoBehaviour
{
  
    private bool navMeshGenerated = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("Collided with player");
            GenerateNavMesh();
            SpawnEnemies();
        }
    }

    void OnTriggerExit(Collider other)
    {
        print("left collided with player");
        if (other.CompareTag("Player"))
        {
            DisableNavMesh();
        }
    }

    void GenerateNavMesh()
    {
        if (!navMeshGenerated)
        {
            print("Enable navmesh for this room");
            NavMeshSurface surface = gameObject.AddComponent<NavMeshSurface>();
            surface.collectObjects = CollectObjects.All;
            surface.BuildNavMesh();
            navMeshGenerated = true;
        }
    }

    void SpawnEnemies()
    {
        print("Spawn enemies here");
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


}
