using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemies;
    public SimpleRoom simpR;
    public GameManager GameManager;
    public GameObject spawnRoom;
    public GameObject[] spawnableTiles;
    public bool canSpawn = true;

    public List<Vector2> _availbleTiles;
    public List<Vector2> availbleTiles;

    void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        simpR = transform.parent.GetComponent<SimpleRoom>();

        if (GameManager.currentGameLevel == 1)
        {
            enemies.Add(Resources.Load("Worm") as GameObject);
            enemies.Add(Resources.Load("Ghost") as GameObject);
        }

    }

    public List<Vector2> GetValidSpawns()
    {
        _availbleTiles = simpR.GetAllTilesOfType('░');
        availbleTiles = new List<Vector2>();


        foreach (var tile in _availbleTiles)
        {
            availbleTiles.Add(simpR.ArrayToWorldPOS(tile));
        } 

        return availbleTiles;
    }

    public void SpawnEnemies()
    {
        if (canSpawn)
        {
            var spawnPoints = GetValidSpawns();
            

            for (int i = 0; i <= simpR.EnemyCount; i++)
            {
                var posA = spawnPoints[Random.Range(0,(spawnPoints.Count -1))];
                GameObject mob = Instantiate(enemies[Random.Range(0,(enemies.Count - 1))], posA, Quaternion.identity);
                mob.transform.parent = transform.parent.Find("Tiles").transform;
                
                mob.transform.localPosition = posA;

            }
            canSpawn = false;
        }
        
    }
}
