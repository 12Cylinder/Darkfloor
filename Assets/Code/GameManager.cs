using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    #region player spawning
    public bool generationComplete = false;
    [System.Serializable]
    public class SpawnInfo
    {
        public GameObject PlayerPrefab;
        public Transform SpawnPoint;
        public Camera Cam;
    }

    public SpawnInfo Player1 = new SpawnInfo();
    public SpawnInfo Player2 = new SpawnInfo();

    public void startGameAfterGeneration()
    {
        generationComplete = true;

        GameObject newPlayer = Instantiate(Player1.PlayerPrefab, Player1.SpawnPoint.position, Player1.PlayerPrefab.transform.rotation);
        GameObject newCamera = Instantiate(Player1.Cam.gameObject, transform.position, Player1.Cam.transform.rotation);
        newCamera.GetComponent<CameraFollow>().target = newPlayer;
        newPlayer.GetComponent<PlayerMapper>().GM = this.gameObject;
        MapOffset[0].GetComponent<MapBuilder>().target = newPlayer;
        SpawnedPlayer1 = newPlayer;

        newPlayer = Instantiate(Player2.PlayerPrefab, Player2.SpawnPoint.position, Player2.PlayerPrefab.transform.rotation);
        newCamera = Instantiate(Player2.Cam.gameObject, transform.position, Player2.Cam.transform.rotation);
        newCamera.GetComponent<CameraFollow>().target = newPlayer;
        newPlayer.GetComponent<PlayerMapper>().GM = this.gameObject;
        MapOffset[1].GetComponent<MapBuilder>().target = newPlayer;
        SpawnedPlayer2 = newPlayer;

        newPlayer = null;
        newCamera = null;
    }
    #endregion
    #region enemy spawning
    public GameObject SpawnedPlayer1;
    public GameObject SpawnedPlayer2;

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

    public IEnumerable spawnEnemy(GameObject go)
    {
        bool pointFound = false;
        int i = 0;
        while (!pointFound)
        {
            i = Random.Range(0, enemySpawnPoints.Length - 1);
            if (Vector3.Distance(enemySpawnPoints[i], SpawnedPlayer1.transform.position) > minSpawnDistance && Vector3.Distance(enemySpawnPoints[i], SpawnedPlayer2.transform.position) > minSpawnDistance)
            {
                pointFound = true;
            }
        }

        Instantiate(enemyPrefabs[0], enemySpawnPoints[i], enemyPrefabs[0].transform.rotation);

        return null;
    }
    #endregion

    public GameObject[] MapOffset;

    private void Update()
    {
        if (enemySpawnPoints.Length > 0)
        {
            if (Time.time > lastSpawnTime + enemySpawnRate)
            {
                lastSpawnTime = Time.time;
                StartCoroutine("spawnEnemy", enemyPrefabs[0]);
            }
        }
    }

    private void addMapPixel(Vector3 pos)
    {
        foreach (GameObject go in MapOffset)
        {
            go.SendMessage("AddPixel", pos);
        }
    }

    private void addMapPixelBossRoom(Vector3 pos)
    {
        foreach (GameObject go in MapOffset)
        {
            go.SendMessage("AddPixelBossRoom", pos);
        }
    }
}
