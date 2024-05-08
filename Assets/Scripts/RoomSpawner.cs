using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public GameObject[] roomPrefabs;
    public GameObject[] deadEndRoomPrefabs;
    public GameObject spawnRoomPrefab;
    public GameObject bossRoomPrefab;
    public GameObject wallPrefab;

    public enum CardinalDir { NorthRoomConnection, EastRoomConnection, SouthRoomConnection, WestRoomConnection }
    public int numRooms = 0;
    public int roomFails = 0;

    public Vector3 spawnRoomFinalPosition;
    private GameObject enemySpawner;

    public List<GameObject> successfullySpawnedRooms = new List<GameObject>();
    public List<GameObject> availableConnectors = new List<GameObject>();
    private List<GameObject> availableBossConnectors = new List<GameObject>();
    private List<GameObject> doors = new List<GameObject>();

    void Start()
    {
        enemySpawner = GameObject.Find("EnemySpawner");
        enemySpawner.SetActive(false);
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
            if (availableConnectors.Count == 0)
            {
                Debug.Log("No available connectors");
                break;
            }

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
            // Get the position of the alignment point in world space
            Vector3 connectionPointOffset = newRoomConnectionPoint.position - roomPrefab.transform.position;

            // Get the position of the old room connection point
            Vector3 oldRoomConnectionPointPosition = oldRoomConnectionPoint.transform.position;

            // Calculate the new room position by subtracting the connection point offset from the old room connection point position
            Vector3 newRoomPosition = oldRoomConnectionPointPosition - connectionPointOffset;

            GameObject newRoom = Instantiate(roomPrefab, newRoomPosition, roomPrefab.transform.rotation);

            // ================COLLIDER OVERLAP CHECK================

            // Check if the new room's collider is overlapping with any of the successfully spawned rooms' colliders
            bool overlap = CheckForOverlap(newRoom);

            // If the room sare overlapping, destroy it and try again
            if (overlap)
            {
                // If the room fails to generate 50 times, skip it
                if (roomFails > 50)
                {
                    Debug.Log("ROOM FAILED TO GENERATE");
                    // yield return new WaitForSeconds(2f);
                    Destroy(newRoom);
                    roomFails = 0;
                    continue;
                }

                // yield return new WaitForSeconds(2f);
                Destroy(newRoom);
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
        FillUnusedConnections();
        enemySpawner.SetActive(true);
        yield return null;
    }


    private void SpawnBossRoom()
    {
        for (int i = 0; i < 1; i++)
        {
            // Select a random room prefab from the array
            GameObject bRoomPrefab = bossRoomPrefab;

            // Select a random connection point from the list and remove it from the list
            if (availableConnectors.Count == 0)
            {
                Debug.Log("No available connectors");
                break;
            }

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

            Vector3 connectionPointOffset = bossRoomConnectionPoint.position - bRoomPrefab.transform.position;

            // Get the position of the old room connection point
            Vector3 oldRoomBossConnectionPointPosition = oldRoomBossConnectionPoint.transform.position;

            // Calculate the new room position by subtracting the connection point offset from the old room connection point position
            Vector3 bossRoomPosition = oldRoomBossConnectionPointPosition - connectionPointOffset;
            GameObject newBossRoom = Instantiate(bRoomPrefab, bossRoomPosition, bRoomPrefab.transform.rotation);

            // Get the composite collider of the new boss room
            bool overlap = CheckForOverlap(newBossRoom);

            // If the rooms are overlapping, destroy the boss room and try again
            if (overlap)
            {
                // If the boss room fails to generate 50 times, skip it
                if (roomFails > 50)
                {
                    Debug.Log("ROOM FAILED TO GENERATE");
                    Destroy(newBossRoom);
                    roomFails = 0;
                    continue;
                }

                Destroy(newBossRoom);
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
            string oppositeConnectorName = GetOppositeConnectorName(connector);

            // Convert opposite connector name to an enum value
            CardinalDir oppositeConnectorDir = (CardinalDir)System.Enum.Parse(typeof(CardinalDir), oppositeConnectorName);
            GameObject deadEndRoom = deadEndRoomPrefabs[(int)oppositeConnectorDir]; // Get the corresponding dead end room

            Transform newRoomConnectPoint = deadEndRoom.transform.Find("RoomConnectionPoints");
            Transform deadEndRoomConnectionPoint = newRoomConnectPoint.transform.Find(oppositeConnectorName);

            if (deadEndRoomConnectionPoint != null)
            {
                Vector3 connectionPointOffset = deadEndRoomConnectionPoint.position - deadEndRoom.transform.position;
                Vector3 connectorPosition = connector.transform.position;
                Vector3 deadEndRoomPosition = connectorPosition - connectionPointOffset;

                GameObject room = Instantiate(deadEndRoom, deadEndRoomPosition, deadEndRoom.transform.rotation);

                bool overlap = CheckForOverlap(room);

                if (overlap)
                {
                    Destroy(room);
                    GenerateWall(connector.transform, connector);
                }
            }
            else
            {
                // Fill the unused connection with a wall/door
                GenerateWall(connector.transform, connector);
            }
        }

        availableConnectors.Clear();
    }

    private void GenerateWall(Transform connectionPoint, GameObject connector)
    {
        string directionName = connector.name;

        if (directionName == "NorthRoomConnection" || directionName == "SouthRoomConnection")
        {
            connectionPoint.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (directionName == "EastRoomConnection" || directionName == "WestRoomConnection")
        {
            connectionPoint.transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        connectionPoint.transform.position = new Vector3(connectionPoint.transform.position.x, connectionPoint.transform.position.y + 2f, connectionPoint.transform.position.z);

        Instantiate(wallPrefab, connectionPoint.transform.position, connectionPoint.transform.rotation);
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

    private bool CheckForOverlap(GameObject room)
    {
        Transform roomTransform = room.transform.Find("Collider");
        BoxCollider roomCollider = roomTransform.GetComponentInChildren<BoxCollider>();

        // Check if the new boss room's collider is overlapping with any of the successfully spawned rooms' colliders
        bool overlap = false;
        foreach (GameObject successfulRoom in successfullySpawnedRooms)
        {
            Transform transform = successfulRoom.transform.Find("Collider");
            BoxCollider successfulRoomCollider = transform.GetComponentInChildren<BoxCollider>();

            if (roomCollider != null && successfulRoomCollider != null)
            {
                Collider[] hitColliders = Physics.OverlapBox(roomCollider.bounds.center, roomCollider.bounds.extents, Quaternion.identity, LayerMask.GetMask("RoomCollider"));

                if (hitColliders.Length > 1)
                {
                    overlap = true;
                    break;
                }

                Collider[] overlappingColliders = Physics.OverlapBox(roomCollider.bounds.center, roomCollider.bounds.extents, Quaternion.identity, LayerMask.GetMask("Door"));
                foreach (Collider collider in overlappingColliders)
                {
                    Door door = collider.GetComponent<Door>();
                    if (door != null)
                    {
                        Debug.Log("Destroying door " + door.gameObject.name + " because of overlap");
                        Destroy(door.gameObject);
                    }
                }

            }
        }
        return overlap;
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
}