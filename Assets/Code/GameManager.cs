using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    #region player spawning
    public int roomsToGenerate = 500;
    public bool generationComplete = false;
    

    [System.Serializable]
    public class SpawnInfo
    {
        public string Name;
        public GameObject PlayerPrefab;
    }

    public GameObject CameraPrefab;
    public SpawnInfo[] Players;
    public List<GameObject> SpawnedPlayers;
    public List<Camera> SpawnedCameras;
    public GameObject[] Spawnpoints;

    private int viewPorts;

    public void startGame()
    {
        generationComplete = true;
        for (int i = 0; i < Players.Length; i++)
        {
            GameObject newPlayer = Instantiate(Players[i].PlayerPrefab, Spawnpoints[i].transform.position, Players[i].PlayerPrefab.transform.rotation);
            newPlayer.name = Players[i].Name;
            newPlayer.GetComponent<PlayerMapper>().GM = this.gameObject;
            newPlayer.GetComponent<PlayerController>().player = i + 1;

            GameObject newCamera = Instantiate(CameraPrefab, transform.position, CameraPrefab.transform.rotation);
            newCamera.GetComponent<CameraFollow>().target = newPlayer;
            SpawnedPlayers.Add(newPlayer);
            SpawnedCameras.Add(newCamera.GetComponent<Camera>());
        }

        if(SpawnedCameras.Count == 1)
        {
            SpawnedCameras[0].rect = new Rect(0, 0, 1, 1);
        }
        else if(SpawnedCameras.Count == 2)
        {
            SpawnedCameras[0].rect = new Rect(0, 0, .5f, 1);
            SpawnedCameras[1].rect = new Rect(.5f, 0, .5f, 1);
        }
        else if(SpawnedCameras.Count == 3)
        {
            SpawnedCameras[0].rect = new Rect(0, .5f, .5f, .5f);
            SpawnedCameras[1].rect = new Rect(.5f, .5f, .5f, .5f);
            SpawnedCameras[2].rect = new Rect(0, 0, .5f, .5f);
        }
        else if (SpawnedCameras.Count == 4)
        {
            SpawnedCameras[0].rect = new Rect(0, .5f, .5f, .5f);
            SpawnedCameras[1].rect = new Rect(.5f, .5f, .5f, .5f);
            SpawnedCameras[2].rect = new Rect(0, 0, .5f, .5f);
            SpawnedCameras[3].rect = new Rect(.5f, 0, .5f, .5f);
        }
    }

    public void endGame()
    {
        SpawnedPlayers.Clear();
        SpawnedCameras.Clear();
    }

    #endregion

    #region enemy spawning

    public Vector3[] enemySpawnPoints;
    public int enemySpawnRate = 15;
    public int maxSpawnDistance = 100;
    public int minSpawnDistance = 30;
    public GameObject[] enemyPrefabs;

    private float lastSpawnTime;

    public void setNewSpawnPoints(List<Vector3> points)
    {
        enemySpawnPoints = points.ToArray();
    }

    #endregion

    private void Update()
    {
        if (enemySpawnPoints.Length > 0)
        {
            if (Time.time > lastSpawnTime )
            {
                lastSpawnTime = Time.time + enemySpawnRate;
                SpawnEnemies();
            }
        }
    }

    private void SpawnEnemies()
    {
        foreach (Vector3 pos in enemySpawnPoints)
        {
            bool[] distanceGood = new bool[SpawnedPlayers.Count];
            bool pointGood = true;
            for (int i = 0; i < SpawnedPlayers.Count; i++)
            {
                distanceGood[i] = (Vector3.Distance(pos, SpawnedPlayers[i].transform.position) > minSpawnDistance && Vector3.Distance(pos, SpawnedPlayers[i].transform.position) < maxSpawnDistance);
            }
            foreach(bool b in distanceGood)
            {
                if(b == false)
                {
                    pointGood = false;
                }
            }

            if (pointGood)
            {
                Instantiate(enemyPrefabs[0], pos, enemyPrefabs[0].transform.rotation);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        foreach(Vector3 pos in enemySpawnPoints)
        {
            Gizmos.DrawIcon(pos, "spawn_icon.png", true);
        }
    }
}
