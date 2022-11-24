using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public List<GameObject> doors = new List<GameObject>();
    public Collider2D[] enemies;
    public EnemySpawner enemySpawner;
    public Collider2D player;
    public LayerMask enemyLayer;
    public LayerMask playerLayer;
    public GameObject topLeft;
    public GameObject bottomRight;
    public GameObject camAnchor;
    public bool OpenByMobDeath = false;
    public bool OpenByPuzzleComplete = false;
    public bool roomComplete = false;

    void Awake()
    {
        topLeft = transform.Find("TopLeft").gameObject;
        bottomRight = transform.Find("BottomRight").gameObject;
        GameObject.Find("CameraBox").transform.position = camAnchor.transform.position;
        // enemySpawner = transform.parent.Find("EnemySpawner").GetComponent<EnemySpawner>();

    }

    void Start()
    {
        enemies = Physics2D.OverlapAreaAll(topLeft.transform.position, bottomRight.transform.position, enemyLayer);

        // for each door in "Doors", if it's active, add it to the doors list
        var _doors = transform.parent.Find("Doors");
        if (_doors.GetChild(0).gameObject.activeSelf) doors.Add(_doors.GetChild(0).gameObject);
        if (_doors.GetChild(1).gameObject.activeSelf) doors.Add(_doors.GetChild(1).gameObject);
        if (_doors.GetChild(2).gameObject.activeSelf) doors.Add(_doors.GetChild(2).gameObject);
        if (_doors.GetChild(3).gameObject.activeSelf) doors.Add(_doors.GetChild(3).gameObject);


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

    public void killAllMobsInRoom()
    {
        foreach (var mob in enemies)
        {
            Destroy(mob.gameObject);
        }
    }

    void Update()
    {
        if (transform.parent.GetComponent<EnemySpawner>())
        {
            enemySpawner = transform.parent.GetComponent<EnemySpawner>();
        }
        if (!roomComplete && !player)
        {
            foreach (var enemy in enemies)
            {
                enemy.GetComponent<AIMovement>().enabled = false;
            }
        }

        // Handles player entering a new room
        player = Physics2D.OverlapArea(topLeft.transform.position, bottomRight.transform.position, playerLayer);

        enemies = Physics2D.OverlapAreaAll(topLeft.transform.position, bottomRight.transform.position, enemyLayer);

        // Player enters the room
        if (player)
        {
            foreach (var enemy in enemies)
            {
                enemy.GetComponent<AIMovement>().enabled = true;
            }
            // Detects player is in the room and moves the camera
            var cam = GameObject.FindGameObjectWithTag("MainCamera");
            cam.transform.position = camAnchor.transform.position;
            GameObject.Find("CameraBox").transform.position = camAnchor.transform.position;


            if (!roomComplete)
            {

                // Closes the door shortly after the player enters
                foreach (var door in doors)
                {
                    if (door.GetComponent<Door>().open)
                    {
                        Invoke("closeThisRoomsDoors", 0.4f);
                    }
                }
            }
        }


        // If doors open on puzzle solve
        if (OpenByPuzzleComplete)
        {

        }

        // If doors are opened on all mobs killed
        if (OpenByMobDeath)
        {
            if (enemies.Length <= 0)
            {
                roomComplete = true;
            }
        }

        if (roomComplete)
        {
            Invoke("openThisRoomsDoors", 1f);
        }

        // Doors open on other trigger?

        if (enemies.Length > 0)
        {
            foreach (var enemy in enemies)
            {
                enemy.GetComponent<Health>().currentRoom = transform.parent.gameObject;
            }
        }

    }
}