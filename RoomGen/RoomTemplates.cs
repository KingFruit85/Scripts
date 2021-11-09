using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] currentRoomCount;
    public GameObject startRoom;

    public GameObject templateRoom;
    public GameObject tunnelRoom;

    public List<GameObject> rooms;
    public bool spawnMiniBossRoom = false;

    public int roomSpawnerCount = 0;

    [SerializeField]
    private GameObject roomContainer;
    public bool startBuildingLevel;

    public GameObject[] activeSpawnPoints;

    public int GetRoomSpawnerIndex()
    {
        roomSpawnerCount ++;
        return roomSpawnerCount;
    }


    void Awake()
    {
        startBuildingLevel = true;
    }

    void Start()
    {
        roomContainer = new GameObject("Rooms");
        transform.parent = roomContainer.transform;

        if (SceneManager.GetActiveScene().name != "Lab")
        {
            var startRoomVar = Instantiate(startRoom, transform.position, Quaternion.identity);

            var playerVar = Instantiate(Resources.Load("Player Variant 1"), 
                        startRoomVar.GetComponent<AddRoom>().playerSpawnPoint.transform.position,
                        Quaternion.identity);
                playerVar.name = "Player";

            // GameObject.Find("Main Camera").GetComponent<FollowPlayer>().target = GameObject.FindGameObjectWithTag("Player").transform;  
        }
        else if (SceneManager.GetActiveScene().name == "Lab")
        {
            
            // var startRoomVar = Instantiate(lab, transform.position, Quaternion.identity);
            var Lab = GameObject.Find("Lab");
            var playerVar = Instantiate(Resources.Load("Player Variant 1"), 
                        Lab.GetComponent<AddRoom>().playerSpawnPoint.transform.position,
                        Quaternion.identity);
                playerVar.name = "Player";

            // GameObject.Find("Main Camera").GetComponent<FollowPlayer>().target = GameObject.FindGameObjectWithTag("Player").transform;  
        }

    }

    void GetSpawnPoints()
    {
        activeSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }

    void Update()
    {
        currentRoomCount = GameObject.FindGameObjectsWithTag("Room");

        if (currentRoomCount.Length >=15) 
        {
            spawnMiniBossRoom = true;
        }

        if (GameObject.Find("MiniBoss"))
        {
            var bossSpriteRenderer = GameObject.Find("MiniBoss").GetComponent<SpriteRenderer>();

            bossSpriteRenderer.color = new Color(249.0f / 255.0f,
                                                 13.0f/ 255.0f,
                                                 2.0f / 255.0f);
        }
    }
}