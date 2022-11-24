using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<string> enemies = new List<string>() { "Worm", "Ghost" };
    public int enemyCount;
    public SimpleRoom simpR;
    public GameManager GameManager;
    public DoorController doorController;

    public List<Vector2> _availbleTiles;
    public List<Vector2> availbleTiles;

    void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        simpR = GetComponent<SimpleRoom>();
        doorController = transform.Find("DoorController").GetComponent<DoorController>();

        var spawnPoints = GetValidSpawns();

        for (int i = 0; i <= enemyCount; i++)
        {
            var posA = spawnPoints[Random.Range(0, (spawnPoints.Count - 1))];
            var mobToSpawn = enemies[Random.Range(0, (enemies.Count))];
            GameObject mob = Instantiate(Resources.Load<GameObject>(mobToSpawn), posA, Quaternion.identity);
            mob.transform.parent = transform.Find("Tiles").transform;
            mob.transform.localPosition = posA;
        }
    }

    public void SetEnemyCount(int noOfEnemiesToSpawn)
    {
        enemyCount = noOfEnemiesToSpawn;
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

    public List<string> GetEnemies(int count = 1)
    {
        var results = new List<string>();
        for (int i = 0; i < count; i++)
        {
            var r = UnityEngine.Random.Range(0, 2);
            results.Add(enemies[r]);
        }
        return results;
    }
}
