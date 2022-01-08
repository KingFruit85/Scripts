using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject[] doors;
    public Collider2D[] enemies;
    public Collider2D player;
    private Collider2D[] spawnedItems;
    public LayerMask enemyLayer;
    public LayerMask playerLayer;
    public LayerMask items;
    public LayerMask itemSpawners;
    public GameObject[] flameBowls;
    public GameObject[] exitTriggers;
    public GameObject topLeft;
    public GameObject bottomRight;
    public GameObject camAnchor;
    public List<GameObject> itemSpawnPoints;
    public bool canSpawn = false;
    public GameObject itemToSpawn;
    public float spawnDelay = 15.0f;
    public float spawnElapsedTime = 0.0f;
    public bool itemSpawned;
    public bool allEnemiesKilled;

    void Awake()
    {
        topLeft = transform.Find("TopLeft").gameObject;
        bottomRight = transform.Find("BottomRight").gameObject;
        GameObject.Find("CameraBox").transform.position = camAnchor.transform.position;
    }

    void Start()
    {

        var spawners = Physics2D.OverlapAreaAll(topLeft.transform.position, bottomRight.transform.position, itemSpawners);
        enemies = Physics2D.OverlapAreaAll(topLeft.transform.position, bottomRight.transform.position, enemyLayer);

        // Some rooms may not have enemies
        // if (enemies.Length > 0)
        // {
        //     allEnemiesKilled = false;
        // }
        // else
        // {
        //     allEnemiesKilled = true;
        // }

        foreach (var spawn in spawners)
        {
            itemSpawnPoints.Add(spawn.gameObject);
        }

        if (itemSpawnPoints.Count > 0)
        {
            canSpawn = true;
        }

    }

    public void KillAllEnemiesInRoom()
    {
        foreach (var enemy in enemies)
        {
            Debug.Log($"Destroyed {enemy.name}");
            Destroy(enemy);
        }
    }

    public void SetExitTriggers(GameObject[] exits)
    {
        exitTriggers = exits;
    }

    public void closeThisRoomsDoors()
    {
        foreach (var door in doors)
        {
            door.GetComponent<Door>().CloseDoor();
        }
    }

    public void openThisRoomsDoors()
    {
        foreach (var door in doors)
        {
            door.GetComponent<Door>().OpenDoor();
        }
    }


    void Update()
    {

        enemies = Physics2D.OverlapAreaAll(topLeft.transform.position, bottomRight.transform.position, enemyLayer);
        spawnedItems = Physics2D.OverlapAreaAll(topLeft.transform.position, bottomRight.transform.position, items);
        player = Physics2D.OverlapArea(topLeft.transform.position, bottomRight.transform.position, playerLayer);

        if (spawnedItems.Length == 0)
        {
            itemSpawned = false;
        }

        if (enemies.Length <= 0)
        {
            allEnemiesKilled = true;
            Invoke("openThisRoomsDoors", 1f);
            // Enable exit triggers
            foreach (var exit in exitTriggers)
            {
                if (exit != null)
                {
                    exit.SetActive(true);
                }
            }

        }

        if (player && !allEnemiesKilled)
        {
            // Closes the door shortly after the player enters
            foreach (var door in doors)
            {
                if (door.GetComponent<Door>().open)
                {
                    Invoke("closeThisRoomsDoors",0.4f);
                }
            }
        }

        if (player)
        {
            // Detects player is in the room and moves the camera
            var cam = GameObject.FindGameObjectWithTag("MainCamera");
            cam.transform.position = camAnchor.transform.position;
            GameObject.Find("CameraBox").transform.position = camAnchor.transform.position;
            
            var spawner = transform.parent.Find("EnemySpawner").GetComponent<EnemySpawner>();
            
            if (spawner.canSpawn)
            {
                spawner.SpawnEnemies();
            }
        }

        if (enemies.Length > 0)
        {
            foreach (var enemy in enemies)
            {
                enemy.GetComponent<Health>().currentRoom = transform.parent.gameObject;
            }
        }

        if (canSpawn)
        {
            spawnElapsedTime += Time.deltaTime;
        }

        if(canSpawn && !itemSpawned && itemToSpawn != null)
        {
            if (spawnElapsedTime >= spawnDelay)
            {
                spawnElapsedTime = 0;
                spawnItemAtRandomPoint();
            }
        }

        
    }

    void spawnItemAtRandomPoint()
    {
        var r = Random.Range(0,itemSpawnPoints.Count -1);
        Instantiate(itemToSpawn,itemSpawnPoints[r].transform.position,Quaternion.identity);
        itemSpawned = true;
    }
}
