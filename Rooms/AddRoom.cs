using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddRoom : MonoBehaviour
{
    private RoomTemplates templates;
    public bool up,down,left,right;
    public bool x;

    
    public string[] roomExits;
    public bool isExitRoom;
    public bool exitSpawned;
    public GameObject exitPoint;
    public GameObject playerSpawnPoint;
    public GameObject room;
    public string roomType;


    public GameObject LeftToggleDoor;
    public GameObject RightToggleDoor;
    public GameObject UpToggleDoor;
    public GameObject DownToggleDoor;

    public GameObject LeftDoorwayCollider;
    public GameObject RightDoorwayCollider;
    public GameObject UpDoorwayCollider;
    public GameObject DownDoorwayCollider;

    public GameObject[] upToggleWalls;
    public GameObject[] downToggleWalls;
    public GameObject[] leftToggleWalls;
    public GameObject[] rightToggleWalls;

    public GameObject upSpawner;
    public GameObject downSpawner;
    public GameObject leftSpawner;
    public GameObject rightSpawner;

    public Transform[] objectSpawners;


    Scene scene;

    void Awake()            
    {

        if (up)
        {
            string up = "up";
            roomExits.Append(up);
        }
        if (down)
        {
            string down = "down";
            roomExits.Append(down);
        }
        if (left)
        {
            string left = "left";
            roomExits.Append(left);
        }
        if (right)
        {
            string right = "right";
            roomExits.Append(right);
        }

        objectSpawners = GetComponentsInChildren<Transform>().Where(t => t.tag == "ObjectSpawner").ToArray();


    }
    void Start()
    {  
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "PCG")
        {
            templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
            room = this.gameObject;
            templates.rooms.Add(room);
            transform.parent = GameObject.Find("Rooms").transform;
        }

        isExitRoom = false;
        exitSpawned = false;
    }

    public string[] GetRoomExits()
    {
        return roomExits;
    }

    public Transform[] GetObjectSpawners()
    {
        return objectSpawners;
    }

    public Vector3 GetObjectSpawnerPosition(string spawner)
    {
        return objectSpawners.Where(o => o.name == spawner).ToList()[0].position;
    }


    public void SpawnExit()
    {
        if (!exitSpawned)
        {
            var exitSquare = Instantiate(Resources.Load("ExitSquare") as GameObject, new Vector3(exitPoint.transform.position.x,exitPoint.transform.position.y,0), Quaternion.identity);
            
            if(scene.name == "shop")
            {
                exitSquare.GetComponent<ExitTile>().isShopLevel = true;
            }

            exitSpawned = true;
        }
    }

    public void SpawnWinGameExit()
    {
        var exitSquare = Instantiate(Resources.Load("WinSquare") as GameObject, new Vector3(exitPoint.transform.position.x,exitPoint.transform.position.y,0), Quaternion.identity);
        exitSpawned = true;

    }

    public void RemoveToggleDoor(string door)
    {
        switch (door)
        {
            default: throw new System.Exception("cannot find toggle door to remove");
            case "up":
                // Remove toggle tile and wall tiles
                if (UpToggleDoor != null)
                {
                    UpToggleDoor.SetActive(false);
                    // Enable door collider box collider
                    UpDoorwayCollider.GetComponent<BoxCollider2D>().enabled = true;
                    //Switch the wall sprites on either side of the door to it's alternative sprite
                    foreach (var wall in upToggleWalls)
                    {
                        wall.GetComponent<Wall>().setAltSprite();
                    }
                }
                break;

            case "down":
                if (DownToggleDoor != null)
                {
                    DownToggleDoor.SetActive(false);
                    DownDoorwayCollider.GetComponent<BoxCollider2D>().enabled = true;
                    foreach (var wall in downToggleWalls)
                    {
                        wall.GetComponent<Wall>().setAltSprite();
                    }
                }
                break;

            case "left":
                if (LeftToggleDoor != null)
                {
                    LeftToggleDoor.SetActive(false);
                    LeftDoorwayCollider.GetComponent<BoxCollider2D>().enabled = true;
                    foreach (var wall in leftToggleWalls)
                    {
                        wall.GetComponent<Wall>().setAltSprite();
                    }
                }
                break;

            case "right":
                if (RightToggleDoor != null)
                {
                    RightToggleDoor.SetActive(false);
                    RightDoorwayCollider.GetComponent<BoxCollider2D>().enabled = true;
                    foreach (var wall in rightToggleWalls)
                    {
                        wall.GetComponent<Wall>().setAltSprite();
                    }
                }
                break;
    }

    // track room spawners
    //if they spawn mark as such
    //if they don't spawn anything, is this because they were destroyed or because we hit the room limit
    // if we hit the room limit then we need to spawn blank rooms to block off the open exits for the last set of rooms spawned with open exits



}
}
