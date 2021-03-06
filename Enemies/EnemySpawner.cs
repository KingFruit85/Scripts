using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemies;
    public float waitTime = 1.5f;
    private int rand;
    public GameObject spawnRoom;

    void Start()
    {
        // Clears self up
        Destroy(gameObject, waitTime);
		rand = Random.Range(0, enemies.Length);

        if (enemies.Length > 0)
        {
            var enemy = Instantiate(enemies[rand],new Vector3(transform.position.x,transform.position.y,0),Quaternion.identity);
            enemy.transform.parent = spawnRoom.transform;
        }
        

    }
}
