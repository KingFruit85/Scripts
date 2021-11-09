using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddRoom : MonoBehaviour
{
    private RoomTemplates templates;
    public bool isExitRoom;
    public bool exitSpawned;
    public GameObject exitPoint;
    public GameObject playerSpawnPoint;
    public GameObject room;

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

    public GameObject[] HorizontalTunnelTiles;
    public GameObject[] VerticalTunnelTiles;
    public GameObject[] LeftUpTunnelTiles;
    public GameObject[] LeftDownTunnelTiles;
    public GameObject[] RightUpTunnelTiles;
    public GameObject[] RightDownTunnelTiles;
    public GameObject[] AllTunnelTiles;


    public GameObject upSpawner;
    public GameObject downSpawner;
    public GameObject leftSpawner;
    public GameObject rightSpawner;

    public Transform[] objectSpawners;

    Scene scene;

    void Awake()            
    {
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

    public void RemoveUnusedSpawners()
    {
        if (LeftToggleDoor.activeSelf)
        {
            Destroy(leftSpawner);
        }
        if (RightToggleDoor.activeSelf)
        {
            Destroy(rightSpawner);
        }
        if (UpToggleDoor.activeSelf)
        {
            Destroy(upSpawner);
        }
        if (DownToggleDoor.activeSelf)
        {
            Destroy(downSpawner);
        }
    }

    public void DisableSpawner(string door)
    {
        switch (door)
        {
            default:
            case "up": 
                upSpawner.SetActive(false);
                Destroy(upSpawner);
                break;
            case "down": 
                downSpawner.SetActive(false);
                Destroy(downSpawner);
                break;
            case "left": 
                leftSpawner.SetActive(false);
                Destroy(leftSpawner);
                break;
            case "right": 
                rightSpawner.SetActive(false);
                Destroy(rightSpawner);
                break;
        }
    }

    public void OpenAllDoors(bool open)
    {
        if (open)
        {
            if (UpToggleDoor)
            {
                UpToggleDoor.SetActive(false);
                DownToggleDoor.SetActive(false);
                LeftToggleDoor.SetActive(false);
                RightToggleDoor.SetActive(false);

                UpDoorwayCollider.GetComponent<BoxCollider2D>().enabled = true;
                DownDoorwayCollider.GetComponent<BoxCollider2D>().enabled = true;
                LeftDoorwayCollider.GetComponent<BoxCollider2D>().enabled = true;
                RightDoorwayCollider.GetComponent<BoxCollider2D>().enabled = true;
            }
            else
            {
                UpToggleDoor.SetActive(true);
                DownToggleDoor.SetActive(true);
                LeftToggleDoor.SetActive(true);
                RightToggleDoor.SetActive(true);

                UpDoorwayCollider.GetComponent<BoxCollider2D>().enabled = false;
                DownDoorwayCollider.GetComponent<BoxCollider2D>().enabled = false;
                LeftDoorwayCollider.GetComponent<BoxCollider2D>().enabled = false;
                RightDoorwayCollider.GetComponent<BoxCollider2D>().enabled = false; 
            }
        }
    }

    public void CloseToggleDoor(string door)
    {
        switch (door)
        {
            default: throw new System.Exception("cannot find toggle door to remove");
            case "up":
                // Remove toggle tile and wall tiles
                if (UpToggleDoor != null)
                {
                    UpToggleDoor.SetActive(true);
                    // Disable door collider box collider
                    UpDoorwayCollider.GetComponent<BoxCollider2D>().enabled = false;
                    //Switch the wall sprites on either side of the door to it's alternative sprite
                    foreach (var wall in upToggleWalls)
                    {
                        wall.GetComponent<Wall>().setAltDefaultSprite();
                    }
                }
                break;

            case "down":
                if (DownToggleDoor != null)
                {
                    DownToggleDoor.SetActive(true);
                    DownDoorwayCollider.GetComponent<BoxCollider2D>().enabled = false;
                }
                foreach (var wall in downToggleWalls)
                    {
                        wall.GetComponent<Wall>().setAltDefaultSprite();
                    }
                break;

            case "left":
                if (LeftToggleDoor != null)
                {
                    LeftToggleDoor.SetActive(true);
                    LeftDoorwayCollider.GetComponent<BoxCollider2D>().enabled = false;
                    foreach (var wall in leftToggleWalls)
                    {
                        wall.GetComponent<Wall>().setAltDefaultSprite();
                    }
                }
                break;

            case "right":
                if (RightToggleDoor != null)
                {
                    RightToggleDoor.SetActive(true);
                    RightDoorwayCollider.GetComponent<BoxCollider2D>().enabled = false;
                }
                foreach (var wall in rightToggleWalls)
                    {
                        wall.GetComponent<Wall>().setAltDefaultSprite();
                    }
                break;
    }
    }

    public void OpenToggleDoor(string door)
    {
        switch (door)
        {
            default: throw new System.Exception(door + ": cannot find this toggle door to remove");
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

}
}
