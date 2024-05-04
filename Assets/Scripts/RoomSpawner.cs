using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    // TODO: Work with team for boss spawn room
    public GameObject[] roomPrefabs;  // An array of room prefabs to be spawned
    public GameObject spawnRoomPrefab;
    // public GameObject bossRoomPrefab;

    public int numRooms = 0;  
    public float minRoomDistance;
    public int roomFails = 0;

    public Vector3 spawnRoomFinalPosition;

    public List<GameObject> successfullySpawnedRooms = new List<GameObject>();
    private List<GameObject> availableConnectors = new List<GameObject>();
    private List<GameObject> availableBossConnectors = new List<GameObject>();

    void Start()
    {
        SpawnRooms();
    }

    public void SpawnRooms()
    {
        // Spawn the spawn room prefab
        GameObject spawnRoom = Instantiate(spawnRoomPrefab, transform.position, Quaternion.identity);
        successfullySpawnedRooms.Add(spawnRoom);
        AddRoomConnectors(spawnRoom, null);

        // Spawn in the remainder of the rooms
        for (int i = 0; i < numRooms; i++)
        {
            // Select a random room prefab from the array
            int randomRoomIndex = Random.Range(0, roomPrefabs.Length);
            GameObject roomPrefab = roomPrefabs[randomRoomIndex];

            // Select a random connection point from the list and remove it from the list
            GameObject oldRoomConnectionPoint = availableConnectors[Random.Range(0, availableConnectors.Count)];

            // Get the opposite connector
            string oppositeConnectorName = GetOppositeConnectorName(oldRoomConnectionPoint); // Name of room
            Transform newRoomConnectPoints = roomPrefab.transform.Find("RoomConnectionPoints");
            Transform newRoomConnectionPoint = newRoomConnectPoints.transform.Find(oppositeConnectorName); // The corresponding connection point child
            GameObject newRoomConnectionPointGO = newRoomConnectionPoint.gameObject; // The gameObject of the corresponding connection point child

            // ================OFFSET CALCULATION================
            // Get the position of the alignment point relative to the room prefab
            Vector3 connectionPointOffset = newRoomConnectionPoint.localPosition;
            // Get the position of the old room connection point
            Vector3 oldRoomConnectionPointPosition = oldRoomConnectionPoint.transform.position;
            // Calculate the new room position by subtracting the connection point offset from the old room connection point position
            Vector3 newRoomPosition = oldRoomConnectionPointPosition - connectionPointOffset;

            // Spawn the room prefab at the aligned position
            GameObject newRoom = Instantiate(roomPrefab, newRoomPosition, Quaternion.identity);

            // ================COLLIDER OVERLAP CHECK================

            // Get the box collider of the new room
            Transform newRoomTransform = newRoom.transform.Find("Collider");
            BoxCollider newRoomCollider = newRoomTransform.GetComponentInChildren<BoxCollider>();

            // TODO: May have bugs by checking colliders
            // Check if the new room's collider is overlapping with any of the successfully spawned rooms' colliders
            bool overlap = false;
            foreach (GameObject successfulRoom in successfullySpawnedRooms)
            {
                Transform prefabTransform = successfulRoom.transform.Find("Collider");
                BoxCollider successfulRoomCollider = prefabTransform.GetComponentInChildren<BoxCollider>();

                if (newRoomCollider != null && successfulRoomCollider != null)
                {
                    // Check if the rooms are too close to each other
                    float distance = Vector3.Distance(newRoomCollider.bounds.center, successfulRoomCollider.bounds.center);
                    if (distance < minRoomDistance)
                    {
                        Debug.Log("Distance: " + distance);
                        overlap = true;
                        break;
                    }

                    Collider[] hitColliders = Physics.OverlapBox(newRoomCollider.bounds.center, newRoomCollider.bounds.extents, Quaternion.identity, LayerMask.GetMask("RoomCollider"));

                    if (hitColliders.Length > 1)
                    {
                        overlap = true;
                        break;
                    }
                }
            }

            // If the room sare overlapping, destroy it and try again
            if (overlap)
            {
                // If the room fails to generate 50 times, skip it
                if (roomFails > 50)
                {
                    Debug.Log("ROOM FAILED TO GENERATE");
                    DestroyRoom(newRoom);
                    roomFails = 0;
                    continue;
                }

                DestroyRoom(newRoom);
                i--;
                roomFails++;
                continue;
            }

            // Add the new room to the list of successfully spawned rooms and continue with the rest of the code
            successfullySpawnedRooms.Add(newRoom);

            availableConnectors.Remove(oldRoomConnectionPoint);
            availableBossConnectors.Remove(oldRoomConnectionPoint);

            // Add the new room's connectors to the available list, except for the one used for connection
            AddRoomConnectors(newRoom, newRoomConnectionPointGO);

            // Set the new room as the old room's neighbor
            // GameObject oldRoomParent = oldRoomConnectionPoint.transform.root.gameObject;
            // RoomController oldRoomController = oldRoomParent.GetComponent<RoomController>();
            // RoomController newRoomController = newRoom.GetComponent<RoomController>();
            // oldRoomController.AddNeighoringRoom(newRoom);
            // oldRoomController.AddUnpathedNeighboringRoom(newRoom);
            // newRoomController.AddNeighoringRoom(oldRoomParent);
            // newRoomController.AddUnpathedNeighboringRoom(oldRoomParent);

            roomFails = 0;
        }
        Debug.Log("Successfully Spawned Rooms: " + successfullySpawnedRooms.Count);

        // SpawnBossRoom();
    }

    /*
    private void SpawnBossRoom()
    {
        // Spawn in the remainder of the rooms
        for (int i = 0; i < 1; i++)
        {
            // Select a random room prefab from the array
            GameObject bRoomPrefab = bossRoomPrefab;

            // Select a random connection point from the list and remove it from the list
            GameObject oldRoomBossConnectionPoint = availableBossConnectors[Random.Range(0, availableBossConnectors.Count)];

            // Get the opposite connector
            string oppositeConnectorName = GetOppositeConnectorName(oldRoomBossConnectionPoint); // Name of room
            Transform bossRoomConnectPoints = bRoomPrefab.transform.Find("RoomConnectionPoints");
            Transform bossRoomConnectionPoint = bossRoomConnectPoints.transform.Find(oppositeConnectorName); // The corresponding connection point child
            GameObject bossRoomConnectionPointGO = bossRoomConnectionPoint.gameObject; // The gameObject of the corresponding connection point child

            // Get the position of the alignment point relative to the room prefab
            Vector3 connectionPointOffset = bossRoomConnectionPoint.localPosition;

            // Get the position of the old room connection point
            Vector3 oldRoomBossConnectionPointPosition = oldRoomBossConnectionPoint.transform.position;

            // Calculate the boss room position by subtracting the connection point offset from the old room connection point position
            Vector3 bossRoomPosition = oldRoomBossConnectionPointPosition - connectionPointOffset;

            // Spawn the boss room prefab at the aligned position
            GameObject newBossRoom = Instantiate(bRoomPrefab, bossRoomPosition, Quaternion.identity);

            // Get the composite collider of the new boss room
            CompositeCollider2D bossRoomCollider = null;
            Transform gridTransform = newBossRoom.transform.Find("Grid");
            if (gridTransform != null)
            {
                Transform roomColliderTransform = gridTransform.Find("RoomGenerationCollider");
                if (roomColliderTransform != null)
                {
                    bossRoomCollider = roomColliderTransform.GetComponent<CompositeCollider2D>();
                }
            }

            // Check if the new boss room's collider is overlapping with any of the successfully spawned rooms' colliders
            bool overlap = false;
            foreach (GameObject successfulRoom in successfullySpawnedRooms)
            {
                CompositeCollider2D successfulRoomCollider = null;
                gridTransform = successfulRoom.transform.Find("Grid");
                if (gridTransform != null)
                {
                    Transform roomColliderTransform = gridTransform.Find("RoomGenerationCollider");
                    if (roomColliderTransform != null)
                    {
                        successfulRoomCollider = roomColliderTransform.GetComponent<CompositeCollider2D>();
                    }
                }

                if (bossRoomCollider != null && successfulRoomCollider != null)
                {
                    Vector2 bossRoomMin = bossRoomCollider.bounds.min;
                    Vector2 bossRoomMax = bossRoomCollider.bounds.max;
                    Vector2 successfulRoomMin = successfulRoomCollider.bounds.min;
                    Vector2 successfulRoomMax = successfulRoomCollider.bounds.max;

                    // Check if the rooms are too close to each other
                    float distance = Vector2.Distance(bossRoomCollider.bounds.center, successfulRoomCollider.bounds.center);
                    if (distance < minRoomDistance)
                    {
                        overlap = true;
                        break;
                    }

                    if (bossRoomMax.x > successfulRoomMin.x && bossRoomMin.x < successfulRoomMax.x &&
                        bossRoomMax.y > successfulRoomMin.y && bossRoomMin.y < successfulRoomMax.y)
                    {
                        overlap = true;
                        break;
                    }
                }
            }

            // If the rooms are overlapping, destroy the boss room and try again
            if (overlap)
            {
                // If the boss room fails to generate 50 times, skip it
                if (roomFails > 50)
                {
                    Debug.Log("ROOM FAILED TO GENERATE");
                    DestroyRoom(newBossRoom);
                    roomFails = 0;
                    continue;
                }

                DestroyRoom(newBossRoom);
                i--;
                roomFails++;
                continue;
            }

            // Add the new boss room to the list of successfully spawned rooms and continue with the rest of the code
            successfullySpawnedRooms.Add(newBossRoom);

            // Remove the used connector
            availableConnectors.Remove(oldRoomBossConnectionPoint);
            //availableBossConnectors.Remove(oldRoomConnectionPoint);

            // Add the new boss room's connectors to the available list, except for the one used for connection
            AddRoomConnectors(newBossRoom, bossRoomConnectionPointGO);

            // Set the new boss room as the old room's neighbor
            // GameObject oldRoomParent = oldRoomBossConnectionPoint.transform.root.gameObject;
            // RoomController oldRoomController = oldRoomParent.GetComponent<RoomController>();
            // RoomController newRoomController = newBossRoom.GetComponent<RoomController>();
            // oldRoomController.AddNeighoringRoom(newBossRoom);
            // oldRoomController.AddUnpathedNeighboringRoom(newBossRoom);
            // newRoomController.AddNeighoringRoom(oldRoomParent);
            // newRoomController.AddUnpathedNeighboringRoom(oldRoomParent);


            roomFails = 0;
        }
    }
    */

    private void AddRoomConnectors(GameObject inputRoom, GameObject usedConnector)
    {
        GameObject roomConnectionPoints = inputRoom.transform.Find("RoomConnectionPoints").gameObject;
        for (int i = 0; i < roomConnectionPoints.transform.childCount; i++)
        {
            Transform connector = roomConnectionPoints.transform.GetChild(i);
            if (connector != usedConnector)
            {
                GameObject connectorObject = connector.gameObject;

                availableConnectors.Add(connectorObject);
                // availableBossConnectors.Add(connectorObject);
            }
        }
    }

    private string GetOppositeConnectorName(GameObject inputConnector)
    {
        if (inputConnector.name == "NorthRoomConnection")
        {
            return "SouthRoomConnection";
        }
        else if (inputConnector.name == "EastRoomConnection")
        {
            return "WestRoomConnection";
        }
        else if (inputConnector.name == "WestRoomConnection")
        {
            return "EastRoomConnection";
        }
        else if (inputConnector.name == "SouthRoomConnection")
        {
            return "NorthRoomConnection";
        }
        else
        {
            return null;
        }
    }

    private void DestroyRoom(GameObject roomToDelete)
    {
        // Transform gridTransform = roomToDelete.transform.Find("Grid");
        // Transform roomColliderTransform = gridTransform.transform.Find("RoomGenerationCollider");

        // if (roomColliderTransform != null)
        // {
        //     GameObject generationCollider = roomColliderTransform.gameObject;
        //     // Remove the collider from the A* pathfinding graph
        //     Destroy(generationCollider);
        // }

        Destroy(roomToDelete);
    }
}