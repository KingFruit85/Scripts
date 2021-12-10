using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemies;
    public GameManager GameManager;
    public GameObject spawnRoom;
    public bool canSpawn = true;

    void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (GameManager.currentGameLevel == 1)
        {
            enemies.Add(Resources.Load("Worm") as GameObject);
            enemies.Add(Resources.Load("Ghost") as GameObject);
        }

        if (canSpawn && enemies.Count > 0)
        {
            for (int i = 0; i < spawnRoom.GetComponent<SimpleRoom>().EnemyCount; i++)
            {
                var enemy = Instantiate(enemies[i],new Vector3(transform.position.x,transform.position.y,0),Quaternion.identity);
                enemy.transform.parent = spawnRoom.transform;
            }
        }
    }
}
