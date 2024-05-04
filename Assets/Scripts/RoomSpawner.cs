using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    // TODO: Work with team for boss spawn room
    public GameObject[] roomPrefabs;
    public GameObject[] deadEndRoomPrefabs;
    public GameObject spawnRoomPrefab;
    public GameObject bossRoomPrefab;
    public GameObject wallPrefab;
    public GameObject sacrificeDoorPrefab;

    public int numRooms = 0;
    public float minRoomDistance;
    public int roomFails = 0;

    public Vector3 spawnRoomFinalPosition;

    public List<GameObject> successfullySpawnedRooms = new List<GameObject>();
    public List<GameObject> availableConnectors = new List<GameObject>();
    private List<GameObject> availableBossConnectors = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnRooms());
    }

    public IEnumerator SpawnRooms()
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

            if (newRoomConnectionPoint == null)
            {
                Debug.Log("Opposite Connector Not Found");
                // If the room fails to generate 50 times, skip it
                if (roomFails > 50)
                {
                    Debug.Log("ROOM FAILED TO GENERATE");
                    roomFails = 0;
                    continue;
                }

                i--;
                roomFails++;
                continue;
            }

            GameObject newRoomConnectionPointGO = newRoomConnectionPoint.gameObject; // The gameObject of the corresponding connection point child

            // ================OFFSET CALCULATION================
            // Get the position of the alignment point relative to the room prefab
            // Vector3 connectionPointOffset = newRoomConnectionPoint.localPosition;
            // // Get the position of the old room connection point
            // Vector3 oldRoomConnectionPointPosition = oldRoomConnectionPoint.transform.position;
            // // Calculate the new room position by subtracting the connection point offset from the old room connection point position
            // Vector3 newRoomPosition = oldRoomConnectionPointPosition - connectionPointOffset;

            // // Spawn the room prefab at the aligned position
            // GameObject newRoom = Instantiate(roomPrefab, newRoomPosition, Quaternion.identity);
            // Get the position of the alignment point in world space
            Vector3 connectionPointOffset = newRoomConnectionPoint.position - roomPrefab.transform.position;

            // Get the position of the old room connection point
            Vector3 oldRoomConnectionPointPosition = oldRoomConnectionPoint.transform.position;

            // Calculate the new room position by subtracting the connection point offset from the old room connection point position
            Vector3 newRoomPosition = oldRoomConnectionPointPosition - connectionPointOffset;

            GameObject newRoom = Instantiate(roomPrefab, newRoomPosition, roomPrefab.transform.rotation);

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
                    yield return new WaitForSeconds(1f);
                    DestroyRoom(newRoom);
                    roomFails = 0;
                    continue;
                }

                yield return new WaitForSeconds(1f);
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

            roomFails = 0;
        }
        Debug.Log("Successfully Spawned Rooms: " + successfullySpawnedRooms.Count);

        SpawnBossRoom();
        yield return null;
    }


    private void SpawnBossRoom()
    {
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

            if (bossRoomConnectionPoint == null)
            {
                Debug.Log("Opposite Connector Not Found");
                // If the boss room fails to generate 50 times, skip it
                if (roomFails > 50)
                {
                    Debug.Log("ROOM FAILED TO GENERATE");
                    roomFails = 0;
                    continue;
                }

                i--;
                roomFails++;
                continue;
            }

            GameObject bossRoomConnectionPointGO = bossRoomConnectionPoint.gameObject;

            // Vector3 connectionPointOffset = bossRoomConnectionPoint.localPosition;
            // Vector3 oldRoomBossConnectionPointPosition = oldRoomBossConnectionPoint.transform.position;
            // Vector3 bossRoomPosition = oldRoomBossConnectionPointPosition - connectionPointOffset;
            // GameObject newBossRoom = Instantiate(bRoomPrefab, bossRoomPosition, Quaternion.identity);

            Vector3 connectionPointOffset = bossRoomConnectionPoint.position - bRoomPrefab.transform.position;

            // Get the position of the old room connection point
            Vector3 oldRoomBossConnectionPointPosition = oldRoomBossConnectionPoint.transform.position;

            // Calculate the new room position by subtracting the connection point offset from the old room connection point position
            Vector3 bossRoomPosition = oldRoomBossConnectionPointPosition - connectionPointOffset;
            GameObject newBossRoom = Instantiate(bRoomPrefab, bossRoomPosition, bRoomPrefab.transform.rotation);

            // Get the composite collider of the new boss room
            Transform newBossRoomTransform = newBossRoom.transform.Find("Collider");
            BoxCollider bossRoomCollider = newBossRoomTransform.GetComponentInChildren<BoxCollider>();

            // Check if the new boss room's collider is overlapping with any of the successfully spawned rooms' colliders
            bool overlap = false;
            foreach (GameObject successfulRoom in successfullySpawnedRooms)
            {
                Transform transform = successfulRoom.transform.Find("Collider");
                BoxCollider successfulRoomCollider = transform.GetComponentInChildren<BoxCollider>();

                if (bossRoomCollider != null && successfulRoomCollider != null)
                {
                    // Check if the rooms are too close to each other
                    float distance = Vector3.Distance(bossRoomCollider.bounds.center, successfulRoomCollider.bounds.center);
                    if (distance < minRoomDistance)
                    {
                        Debug.Log("Distance: " + distance);
                        overlap = true;
                        break;
                    }

                    Collider[] hitColliders = Physics.OverlapBox(bossRoomCollider.bounds.center, bossRoomCollider.bounds.extents, Quaternion.identity, LayerMask.GetMask("RoomCollider"));

                    if (hitColliders.Length > 1)
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

            // Add the new boss room's connectors to the available list, except for the one used for connection
            AddRoomConnectors(newBossRoom, bossRoomConnectionPointGO);

            roomFails = 0;
        }
    }

    private void FillUnusedConnections()
    {
        // Check to see if the unused connection can be filled with a dead end room
        // If it can, fill it with a dead end room
        // else, fill it with a wall/door to sacrifice the room
        foreach (GameObject connector in availableConnectors)
        {
            // Get the opposite connector
            string oppositeConnectorName = GetOppositeConnectorName(connector);

            // Get opposite connection room from the list
            // TODO: Change to dead end room prefab/Logic to get the corresponding dead end room

            // if (deadEndRoomConnectionPoint != null)
            // {
            //     GameObject deadEndRoomConnectionPointGO = deadEndRoomConnectionPoint.gameObject;

            //     Vector3 connectionPointOffset = deadEndRoomConnectionPoint.position - roomPrefabs[0].transform.position;
            //     Vector3 connectorPosition = connector.transform.position;
            //     Vector3 deadEndRoomPosition = connectorPosition - connectionPointOffset;

            //     GameObject deadEndRoom = Instantiate(roomPrefabs[0], deadEndRoomPosition, roomPrefabs[0].transform.rotation);

            //     availableConnectors.Remove(connector);
            //     AddRoomConnectors(deadEndRoom, deadEndRoomConnectionPointGO);
            // }
            // else
            // {
            //     // Fill the unused connection with a wall/door
            //     // GameObject wall = Instantiate(wallPrefab, connector.transform.position, Quaternion.identity);
            //     // availableConnectors.Remove(connector);
            // }
        }
    }
    private void AddRoomConnectors(GameObject inputRoom, GameObject usedConnector)
    {
        GameObject roomConnectionPoints = inputRoom.transform.Find("RoomConnectionPoints").gameObject;
        for (int i = 0; i < roomConnectionPoints.transform.childCount; i++)
        {
            Transform connector = roomConnectionPoints.transform.GetChild(i);
            if (connector != usedConnector)
            {
                // Makes sure that the connector is not overlapping with another room to add it to the available list
                Collider[] nearbyRooms = Physics.OverlapSphere(connector.position, 1f, LayerMask.GetMask("RoomCollider"));

                // If there are less or equal to one room (itself), add the connector to the available list
                if (nearbyRooms.Length <= 1)
                {
                    GameObject connectorObject = connector.gameObject;

                    availableConnectors.Add(connectorObject);
                    availableBossConnectors.Add(connectorObject);
                }
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
        Destroy(roomToDelete);
    }
}