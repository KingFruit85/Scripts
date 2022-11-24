using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwarmRooms : MonoBehaviour
{
    public SimpleRoom room;
    public DoorController doorController;
    private Shaker cameraShaker;
    public EnemySpawner enemySpawner;
    public AudioManager audioManager;
    public List<string> enemies;
    public Collider2D[] enemiesInRoom;
    public bool playerInRoom;
    public LayerMask enemyLayer;
    private int playerLayer;
    public GameObject topLeft;
    public GameObject bottomRight;

    public int enemyLimit = 25;
    public int MaxActiveEnemies = 6;
    public int tracker = 0;

    void Start()
    {
        room = GetComponent<SimpleRoom>();
        gameObject.AddComponent<EnemySpawner>();
        enemySpawner = GetComponent<EnemySpawner>();
        doorController = gameObject.transform.Find("DoorController").GetComponent<DoorController>();
        cameraShaker = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Shaker>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();


        topLeft = doorController.topLeft;
        bottomRight = doorController.bottomRight;
        enemyLayer = LayerMask.GetMask("enemies");
        playerLayer = LayerMask.GetMask("Player");

        doorController.OpenByMobDeath = true;

        // Populate the enemy list
        enemies = enemySpawner.GetEnemies(enemyLimit);

        // Spawn a rune at each spawn location
        // 1
        var spawnerOneTile = room.floorTiles
        .Where(t => t.name == "10")
        .FirstOrDefault();

        var spawnerTwoTile = room.floorTiles
        .Where(t => t.name == "15")
        .FirstOrDefault();

        var spawnerThreeTile = room.floorTiles
        .Where(t => t.name == "50")
        .FirstOrDefault();

        var spawnerFourTile = room.floorTiles
        .Where(t => t.name == "55")
        .FirstOrDefault();

        var flameBowl1Tile = room.floorTiles
        .Where(t => t.name == "19")
        .FirstOrDefault();
        var flameBowl2Tile = room.floorTiles
        .Where(t => t.name == "22")
        .FirstOrDefault();
        var flameBowl3Tile = room.floorTiles
        .Where(t => t.name == "43")
        .FirstOrDefault();
        var flameBowl4Tile = room.floorTiles
        .Where(t => t.name == "46")
        .FirstOrDefault();

        GameObject flameBowl1 = Instantiate(Resources.Load("FlameBowl"), flameBowl1Tile.transform.position, Quaternion.identity) as GameObject;
        GameObject flameBowl2 = Instantiate(Resources.Load("FlameBowl"), flameBowl2Tile.transform.position, Quaternion.identity) as GameObject;
        GameObject flameBowl3 = Instantiate(Resources.Load("FlameBowl"), flameBowl3Tile.transform.position, Quaternion.identity) as GameObject;
        GameObject flameBowl4 = Instantiate(Resources.Load("FlameBowl"), flameBowl4Tile.transform.position, Quaternion.identity) as GameObject;
        flameBowl1.GetComponent<FlameBowl>().startLit = true;
        flameBowl2.GetComponent<FlameBowl>().startLit = true;
        flameBowl3.GetComponent<FlameBowl>().startLit = true;
        flameBowl4.GetComponent<FlameBowl>().startLit = true;

        var roomCenter = transform.Find("RoomCenter").transform.position;
        GameObject pentagram = Instantiate(Resources.Load("pentagram"), roomCenter, Quaternion.identity) as GameObject;
        pentagram.transform.parent = gameObject.transform;
        GameObject roomOverlay = Instantiate(Resources.Load("SwarmRoomOverlay"), roomCenter, Quaternion.identity) as GameObject;
        roomOverlay.transform.parent = gameObject.transform;

        GameObject spawnerOne = Instantiate(Resources.Load("SwarmSpawn1"), spawnerOneTile.transform.position, Quaternion.identity) as GameObject;
        GameObject spawnerTwo = Instantiate(Resources.Load("SwarmSpawn2"), spawnerTwoTile.transform.position, Quaternion.identity) as GameObject;
        GameObject spawnerThree = Instantiate(Resources.Load("SwarmSpawn3"), spawnerThreeTile.transform.position, Quaternion.identity) as GameObject;
        GameObject spawnerFour = Instantiate(Resources.Load("SwarmSpawn4"), spawnerFourTile.transform.position, Quaternion.identity) as GameObject;

        spawnerOne.transform.parent = gameObject.transform;
        spawnerTwo.transform.parent = gameObject.transform;
        spawnerThree.transform.parent = gameObject.transform;
        spawnerFour.transform.parent = gameObject.transform;

        tracker = 0;

    }

    void Update()
    {
        enemiesInRoom = Physics2D.OverlapAreaAll(topLeft.transform.position, bottomRight.transform.position, enemyLayer);
        playerInRoom = Physics2D.OverlapAreaAll(topLeft.transform.position, bottomRight.transform.position, playerLayer).Any();

        if (playerInRoom && enemiesInRoom.Length > 0)
        {
            audioManager.ReverseGameMusic(true);
            cameraShaker.ActivateShake();
        }

        if (enemiesInRoom.Length == 0)
        {
            audioManager.ReverseGameMusic(false);
            cameraShaker.DeactivateShake();
        }

        if (enemiesInRoom.Length < MaxActiveEnemies && tracker <= enemyLimit)
        {
            var spawnTiles = new List<string>() { "10", "15", "50", "55" };
            var r = UnityEngine.Random.Range(0, 3);
            var tile = spawnTiles[r];
            var spawnTile = room.floorTiles
                .Where(t => t.name == tile)
                .FirstOrDefault();

            var enemyToSpawn = enemies[UnityEngine.Random.Range(0, enemies.Count)];
            GameObject enemy = Instantiate(Resources.Load(enemyToSpawn), spawnTile.transform.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = gameObject.transform;
            GameObject spawnEffect = Instantiate(Resources.Load("SpawnEffect"), enemy.transform.position, Quaternion.identity) as GameObject;
            spawnEffect.transform.parent = gameObject.transform;
            tracker++;
        }
    }
}