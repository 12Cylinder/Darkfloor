using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour {

    public GameManager GM;
    public GameObject NavMesh;
    public Transform RoomsContainer;
    public Transform WallsContainer;
    public Transform DoorsContainer;

    public int maxRooms = 100;
    public int doorFrequency = 5;
    public int forwardPreference;

    public GameObject Spawnpoints;
    public GameObject[] SpawnpointsGM;
    public GameObject roomPrefab;
    public GameObject[] wallPrefabs;
    public GameObject floorPrefab;
    public GameObject doorwayPrefab;
    public GameObject ladderPrefab;
    public GameObject bossRoomPrefab;
    public GameObject startingRoomPrefab;
    public int spawnPointFrequency = 5;
    private Dictionary<GameObject, Vector3> rooms = new Dictionary<GameObject, Vector3>();

    private List<Vector3> spawnPositions = new List<Vector3>();
    private Vector3 lastPosition;
    private Vector3 position;
    private bool findNewPosition = false;
    private Vector3 furthestPosition = new Vector3(0, 0, 0);
    private Vector3 closestPosition = new Vector3(0, 0, 0);
    private string state;
    private string progress;

    private void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        maxRooms = GM.roomsToGenerate;
        StartCoroutine("GenerateDungeon", maxRooms);
    }

    private void OnGUI()
    {
        if (!GM.generationComplete)
        {
            GUI.color = Color.white;
            GUI.Label(new Rect(20, Screen.height - 20, 500, 20), state + " : " + progress);
        }
    }

    private IEnumerable GenerateDungeon(int max)
    {
        state = "Generating Rooms";
        GenerateRooms(max);
        GenerateBossRoom();
        GenerateStartingRoom();
        state = "Generating Walls";
        GenerateWalls();
        state = "Generating Doorways";
        GenerateDoorways();
        state = "Generating AI NavMesh";
        GenerateNavMesh();
        state = "Complete!";
        GM.setNewSpawnPoints(spawnPositions);
        GM.Spawnpoints = SpawnpointsGM;
        GM.startGame();
        rooms.Clear();
        spawnPositions.Clear();
        return null;
    }

    private void GenerateRooms(int max)
    {
        int i = 0;

        position = new Vector3(0, 0, 0);

        GameObject newRoom;
        newRoom = Instantiate(roomPrefab, position, roomPrefab.transform.rotation);
        newRoom.transform.SetParent(RoomsContainer);
        rooms.Add(newRoom, position);

        //Main Generator
        while (i < max)
        {
            if (findNewPosition)
            {
                int r = UnityEngine.Random.Range(0, rooms.Count - 1);
                Vector3[] roomPositionsArray = rooms.Values.ToArray();
                position = roomPositionsArray[r];
                findNewPosition = false;
            }
            else
            {
                List<Vector3> validPositions = new List<Vector3>();

                Vector3 checkPositionFront = position + new Vector3(0, 0, 10);
                Vector3 checkPositionBack = position + new Vector3(0, 0, -10);
                Vector3 checkPositionLeft = position + new Vector3(-10, 0, 0);
                Vector3 checkPositionRight = position + new Vector3(10, 0, 0);

                bool roomPresentFront = false;
                bool roomPresentBack = false;
                bool roomPresentLeft = false;
                bool roomPresentRight = false;

                foreach (KeyValuePair<GameObject, Vector3> kv in rooms)
                {
                    if (kv.Value == checkPositionFront)
                    {
                        roomPresentFront = true;
                    }
                    if (kv.Value == checkPositionBack)
                    {
                        roomPresentBack = true;
                    }
                    if (kv.Value == checkPositionLeft)
                    {
                        roomPresentLeft = true;
                    }
                    if (kv.Value == checkPositionRight)
                    {
                        roomPresentRight = true;
                    }
                }

                if (!roomPresentFront)
                {
                    validPositions.Add(checkPositionFront);
                }

                if (!roomPresentBack)
                {
                    validPositions.Add(checkPositionBack);
                }

                if (!roomPresentLeft)
                {
                    validPositions.Add(checkPositionLeft);
                }

                if (!roomPresentRight)
                {
                    validPositions.Add(checkPositionRight);
                }

                if (validPositions.Count > 0)
                {
                    Vector3 spawnPosition = new Vector3();

                    int newRoomPosition = 0;

                    if (!roomPresentFront)
                    {
                        int p = Random.Range(0, forwardPreference);
                        if (p == 0)
                        {
                            spawnPosition = checkPositionFront;
                        }
                        else
                        {
                            newRoomPosition = Random.Range(0, validPositions.Count);
                            spawnPosition = validPositions[newRoomPosition];
                        }
                    }
                    else
                    {
                        newRoomPosition = Random.Range(0, validPositions.Count);
                        spawnPosition = validPositions[newRoomPosition];
                    }

                    GameObject newRoomObject;
                    newRoomObject = Instantiate(roomPrefab, spawnPosition, roomPrefab.transform.rotation);
                    if (RoomsContainer)
                    {
                        newRoomObject.transform.SetParent(RoomsContainer);
                    }

                    rooms.Add(newRoomObject, newRoomObject.transform.position);

                    position = spawnPosition;

                    int r = Random.Range(0, spawnPointFrequency);
                    if(r == 0)
                    {
                        spawnPositions.Add(position + new Vector3(0, 2, 0));
                    }
                    i++;
                    progress = i.ToString() + " / " + max.ToString();
                }
                else
                {
                    findNewPosition = true;
                }
            }
        }
    }

    private void GenerateBossRoom()
    {
        foreach (KeyValuePair<GameObject, Vector3> kvp in rooms)
        {
            if (kvp.Value.z > furthestPosition.z)
            {
                furthestPosition = kvp.Value;
            }
        }
        GameObject newBossRoom = Instantiate(bossRoomPrefab, furthestPosition, ladderPrefab.transform.rotation);
        newBossRoom.transform.SetParent(transform);
    }

    private void GenerateStartingRoom()
    {
        foreach(KeyValuePair<GameObject, Vector3> kvp in rooms)
        {
            if(kvp.Value.z < closestPosition.z)
            {
                closestPosition = kvp.Value;
            }
        }
        Spawnpoints.transform.position = closestPosition;
        GameObject newStartingRoom = Instantiate(startingRoomPrefab, closestPosition, startingRoomPrefab.transform.rotation);
        Spawnpoints.transform.position = closestPosition + new Vector3(0, 0, -20);
    }

    private void GenerateWalls()
    {
        int i = 0;
        foreach (KeyValuePair<GameObject, Vector3> kvp in rooms)
        {
            Vector3 currentPosition = kvp.Value;
            bool roomFront = false;
            bool roomBack = false;
            bool roomLeft = false;
            bool roomRight = false;

            foreach (KeyValuePair<GameObject, Vector3> kvpNext in rooms)
            {
                if (kvp.Key != kvpNext.Key)
                {
                    if (kvp.Value + new Vector3(0, 0, 10) == kvpNext.Value)
                    {
                        roomFront = true;
                    }
                    if (kvp.Value + new Vector3(0, 0, -10) == kvpNext.Value)
                    {
                        roomBack = true;
                    }
                    if (kvp.Value + new Vector3(-10, 0, 0) == kvpNext.Value)
                    {
                        roomLeft = true;
                    }
                    if (kvp.Value + new Vector3(10, 0, 0) == kvpNext.Value)
                    {
                        roomRight = true;
                    }
                }
            }

            GameObject newWall;
            if (!roomFront)
            {
                if (currentPosition != furthestPosition)
                {
                    newWall = Instantiate(wallPrefabs[0], currentPosition, wallPrefabs[0].transform.rotation);
                    newWall.transform.parent = WallsContainer;
                }
            }
            if (!roomBack)
            {
                if (currentPosition != closestPosition)
                {
                    newWall = Instantiate(wallPrefabs[1], currentPosition, wallPrefabs[1].transform.rotation);
                    newWall.transform.parent = WallsContainer;
                }
            }
            if (!roomLeft)
            {
                newWall = Instantiate(wallPrefabs[2], currentPosition, wallPrefabs[2].transform.rotation);
                newWall.transform.parent = WallsContainer;
            }
            if (!roomRight)
            {
                newWall = Instantiate(wallPrefabs[3], currentPosition, wallPrefabs[3].transform.rotation);
                newWall.transform.parent = WallsContainer;
            }
            i++;
            progress = i.ToString();
        }
    }

    private void GenerateDoorways()
    {
        foreach (KeyValuePair<GameObject, Vector3> kvp in rooms)
        {
            Vector3 currentPosition = kvp.Value;
            bool roomForward = false;
            bool roomBack = false;
            bool roomLeft = false;
            bool roomRight = false;
            

            foreach (KeyValuePair<GameObject, Vector3> otherKvp in rooms)
            {
                if (otherKvp.Key != kvp.Key)
                {
                    if (otherKvp.Value == currentPosition + new Vector3(0, 0, 10))
                    {
                        roomForward = true;
                    }
                    if (otherKvp.Value == currentPosition + new Vector3(0, 0, -10))
                    {
                        roomBack = true;
                    }
                    if (otherKvp.Value == currentPosition + new Vector3(-10, 0, 0))
                    {
                        roomLeft = true;
                    }
                    if (otherKvp.Value == currentPosition + new Vector3(10, 0, 0))
                    {
                        roomRight = true;
                    }
                }
            }

            if (roomForward && roomBack && !roomLeft && !roomRight)
            {
                int p = Random.Range(0, doorFrequency);
                if (p == 0)
                {
                    GameObject newDoor;
                    newDoor = Instantiate(doorwayPrefab, currentPosition, doorwayPrefab.transform.rotation);
                    newDoor.transform.parent = DoorsContainer;
                }
            }
        }
    }

    void GenerateNavMesh()
    {
        NavMesh.SendMessage("BuildNavMesh");
    }
}
