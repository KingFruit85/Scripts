using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static System.Math;

public class RoomSpawner : MonoBehaviour
{
    public RoomChecker[] myRoomCheckers;
    public Dictionary<GameObject,string> requiredRoomAndExitTriggerToBeDisabled = new Dictionary<GameObject, string>();
    public RoomTemplates templates;
    public List<string> validSpawnRoomExits = new List<string>();
    public List<string> invalidExits = new List<string>();
    public List<string> requiredDoors = new List<string>();
    public int myIndex;
    private GameObject templateRoom;
    private GameObject tunnelRoom;
    private bool isTunnel = false;
    public GameManager GameManager { get; private set; }
    public GameObject otherSpawner;
    public GameObject myRoomsDestroyer;

    void Awake()
    {
        myRoomCheckers = GetComponentsInChildren<RoomChecker>();
    }

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        templateRoom = templates.templateRoom;
        tunnelRoom = templates.tunnelRoom;

        GameManager  = GameObject.Find("GameManager").GetComponent<GameManager>();

        myIndex = templates.GetRoomSpawnerIndex();
        myRoomsDestroyer.GetComponent<Destroyer>().myIndex = myIndex;

        foreach (var checker in myRoomCheckers)
        {
            validSpawnRoomExits.Add(checker.GetName());
        }

        

    }

    public void RemoveValidExit(string exit)
    {
        validSpawnRoomExits = validSpawnRoomExits.Where( e => e != exit).ToList();
    }


    public void AddRequiredDoor(string name)
    {
        requiredDoors.Add(name);
        validSpawnRoomExits = validSpawnRoomExits.Where(e => e != name).ToList();
    }

    public void AddBlockedDoor(string name)
    {
        if (!invalidExits.Contains(name))
        {
            invalidExits.Add(name);
        }
    }

    public string PickRandomDoorToOpen()
    {
        List<string> remainingDoors = new List<string>(){"up","down","left","right"};

        foreach (var door in requiredDoors)
        {
            remainingDoors = remainingDoors.Where(d => d != door).ToList();
        }

        remainingDoors = remainingDoors.OrderBy(item => Random.value).ToList();

        return remainingDoors[0];
    }

    public void DecorateFloorTiles(List<GameObject> floorTiles)
    {
        foreach (var tile in floorTiles)
        {
            var r = Random.Range(0,4);
            if (r == 1)
            {
                var dressings = GameObject.Find("Room Sprites").GetComponent<RoomSprites>().floorDressing;
                var randomDressing = dressings[Random.Range(0,dressings.Length - 1)];

                // Move to the next element in the foreach loop if we hit a null value
                if (!randomDressing)
                {
                    continue; 
                }

                // Place the dressing int he game world 
                GameObject newGO = new GameObject();
                newGO.transform.position = tile.transform.position;
                newGO.AddComponent<SpriteRenderer>();
                newGO.GetComponent<SpriteRenderer>().sprite = randomDressing;
                newGO.GetComponent<SpriteRenderer>().sortingOrder = 1;
                newGO.transform.parent = tile.transform.parent;

            }
        }
    }


    public void SpawnRoom(string comingFromDirection)
    {
        // If we have hit the room limit, spawn closed room with miniboss
        if (templates.spawnMiniBossRoom)
        {
            GameObject _newRoom = Instantiate(templateRoom,transform.position,Quaternion.identity) as GameObject;
            _newRoom.name =  "TemplateRoom" + gameObject.GetInstanceID();  

            switch (comingFromDirection)
        {
            default: throw new System.Exception("invalid opening direction");

            case "up":
                _newRoom.GetComponent<AddRoom>().OpenToggleDoor("down");
                _newRoom.GetComponent<AddRoom>().DisableSpawner("down");
                DisableExitRoomTrigger(_newRoom,"down");
                break;

            case "down":
                _newRoom.GetComponent<AddRoom>().OpenToggleDoor("up");
                _newRoom.GetComponent<AddRoom>().DisableSpawner("up");
                DisableExitRoomTrigger(_newRoom,"up");
                break;

            case "left":
                _newRoom.GetComponent<AddRoom>().OpenToggleDoor("right");
                _newRoom.GetComponent<AddRoom>().DisableSpawner("right");
                DisableExitRoomTrigger(_newRoom,"right");
                break;

            case "right":
                _newRoom.GetComponent<AddRoom>().OpenToggleDoor("left");
                _newRoom.GetComponent<AddRoom>().DisableSpawner("left");
                DisableExitRoomTrigger(_newRoom,"left");
                break;
        }
        
        // Check to make sure we only spawn one miniboss per level
        if (!GameManager.miniBossKilled)
        {
            GameObject GB = Instantiate(Resources.Load("Ghost"),_newRoom.transform.position,Quaternion.identity) as GameObject;
            GB.GetComponent<Health>().isBoss = true;
            GB.transform.localScale += new Vector3(2f,2f,0);
            GB.GetComponent<SpriteRenderer>().color = Color.red;
            GB.GetComponent<Ghost>().moveSpeed += 3;
            GB.transform.parent = _newRoom.transform;
        }
        }
        // NON-MINIBOSS ROOM
        else
        {

        validSpawnRoomExits = validSpawnRoomExits.Where(e => e != comingFromDirection).ToList();

        // Check and destroy other spawners in the same spawn location
        if (otherSpawner)
        {
            var otherSpawnersRequired = otherSpawner.gameObject.GetComponent<RoomSpawner>().requiredDoors;
            var otherSpawnersInvalid = otherSpawner.gameObject.GetComponent<RoomSpawner>().invalidExits;
            
            // Grab the other spawners data just in case
            foreach (var door in otherSpawnersRequired)
            {
               if (!requiredDoors.Contains(door))
               {
                   requiredDoors.Add(door);
               } 
            }

            foreach (var exit in otherSpawnersInvalid)
            {
                if (!invalidExits.Contains(exit))
                {
                    invalidExits.Add(exit);
                }
            }

            otherSpawner.SetActive(false);
        }

        

        // Spawn the room
        // Maybe add weights to the room types so tunnels are less common
        GameObject _newRoom;
        switch (Random.Range(0,1))
        {
            default:
                _newRoom = Instantiate(templateRoom,transform.position,Quaternion.identity) as GameObject;
                _newRoom.name =  "TemplateRoom" + gameObject.GetInstanceID();
                break;
            case 0: 
                _newRoom = Instantiate(templateRoom,transform.position,Quaternion.identity) as GameObject;
                _newRoom.name =  "TemplateRoom" + gameObject.GetInstanceID();
                break;
            // case 1:
            //     _newRoom = Instantiate(tunnelRoom,transform.position,Quaternion.identity) as GameObject;
            //     _newRoom.name =  "TunnelRoom" + gameObject.GetInstanceID();
            //     isTunnel = true;
            //     break;
        }

        // Decorate tiles
        var allChildren = _newRoom.GetComponentsInChildren<Transform>();
        var floorTiles = new List<GameObject>();

        foreach (var child in allChildren)
        {
            if (child.tag == "Floor")
            {
                floorTiles.Add(child.gameObject);
            }
        }

        DecorateFloorTiles(floorTiles);

        // Here is where I need to disable the opposing rooms exit trigger also

        // for each required door we have we also need to disable the opposite (ie up -> down ) exit trigger in the other room

        if (requiredRoomAndExitTriggerToBeDisabled.Count > 0)
        {
            foreach (var e in requiredRoomAndExitTriggerToBeDisabled)
            {
                Debug.Log(e.Key + " " + e.Value);
                DisableExitRoomTrigger(e.Key,e.Value);
            }
        }

        // Door we enter from

        switch (comingFromDirection)
        {
            default: throw new System.Exception("invalid opening direction");

            case "up":
                _newRoom.GetComponent<AddRoom>().OpenToggleDoor("down");
                _newRoom.GetComponent<AddRoom>().DisableSpawner("down");
                DisableExitRoomTrigger(_newRoom,"down");
                break;

            case "down":
                _newRoom.GetComponent<AddRoom>().OpenToggleDoor("up");
                _newRoom.GetComponent<AddRoom>().DisableSpawner("up");
                DisableExitRoomTrigger(_newRoom,"up");
                break;

            case "left":
                _newRoom.GetComponent<AddRoom>().OpenToggleDoor("right");
                _newRoom.GetComponent<AddRoom>().DisableSpawner("right");
                DisableExitRoomTrigger(_newRoom,"right");
                break;

            case "right":
                _newRoom.GetComponent<AddRoom>().OpenToggleDoor("left");
                _newRoom.GetComponent<AddRoom>().DisableSpawner("left");
                DisableExitRoomTrigger(_newRoom,"left");
                break;
        }

        // Open one extra door
        var doorToOpen = PickRandomDoorToOpen();
        _newRoom.GetComponent<AddRoom>().OpenToggleDoor(doorToOpen);

        if (!requiredDoors.Contains(doorToOpen))
        {
            requiredDoors.Add(doorToOpen);
        }

        // Make sure exits leading to walls are closed
        foreach (var door in invalidExits)
        {
            Debug.Log(door);
            _newRoom.GetComponent<AddRoom>().CloseToggleDoor(door);
            _newRoom.GetComponent<AddRoom>().DisableSpawner(door);
            // CHANGE BACK FROM ALT WALL SPRITES
        }

        // Remove room spawners from closed exits
        List<GameObject> closedDoors = new List<GameObject>();
        for (int i = 0; i < _newRoom.transform.childCount; i++)
        {
            GameObject child = _newRoom.transform.GetChild(i).gameObject;
            if (child.name == "Doors")
            {
                for (int c = 0; c < child.transform.childCount; c++)
                {
                    GameObject exit = child.transform.GetChild(c).gameObject;
                    if (exit.activeSelf)
                    {
                        closedDoors.Add(exit);
                    }
                }
            }
        }

        foreach (var door in closedDoors)
        {
            switch (door.name)
            {
                default:
                case "LeftToggleDoor": _newRoom.GetComponent<AddRoom>().DisableSpawner("left");break;
                case "RightToggleDoor":_newRoom.GetComponent<AddRoom>().DisableSpawner("right");break;
                case "UpToggleDoor":_newRoom.GetComponent<AddRoom>().DisableSpawner("up");break;
                case "DownToggleDoor":_newRoom.GetComponent<AddRoom>().DisableSpawner("down");break;
            }
        }

        }

    }

    public void DisableExitRoomTrigger(GameObject room, string exittriggerToDisable)
    {
        for (int i = 0; i < room.transform.childCount; i++)
        {
            GameObject child = room.transform.GetChild(i).gameObject;
            if (child.name == "RoomExits")
            {
                for (int c = 0; c < child.transform.childCount; c++)
                {
                    GameObject exit = child.transform.GetChild(c).gameObject;
                    if (exit.name == exittriggerToDisable)
                    {
                        Destroy(exit);
                    }
                }
            }
        }
    }

    // public void SpawnTemplateRoom(string comingFromDirection)
    // {

    //     // If we are spawning a room into a space with an existing spawner that hasn't been triggered
    //     // Destroy the other spawner
    //     if (otherSpawner)
    //     {
    //         otherSpawner.SetActive(false);
    //     }

    //     // Spawn Room
    //     GameObject _newRoom = Instantiate(templateRoom,transform.position,Quaternion.identity) as GameObject;
    //     _newRoom.name =  "TemplateRoom" + gameObject.GetInstanceID();
    //     // Add to rooms layer
    //     _newRoom.layer = 16;
    //     // Tidy away into the rooms "folder"
    //     _newRoom.transform.parent = GameObject.Find("Rooms").transform;

    //     //Open the door we are entering from
    //     switch (comingFromDirection)
    //     {
    //         default: throw new System.Exception("invalid opening direction");
    //         case "up":
    //             _newRoom.GetComponent<AddRoom>().OpenToggleDoor("down");
                  
    //             break;

    //         case "down":_newRoom.GetComponent<AddRoom>().OpenToggleDoor("up");break;

    //         case "left":_newRoom.GetComponent<AddRoom>().OpenToggleDoor("right");break;

    //         case "right":_newRoom.GetComponent<AddRoom>().OpenToggleDoor("left");break;
    //     }

    //     // Open other Required Doors
    //     foreach (var door in requiredDoors)
    //     {
    //         _newRoom.GetComponent<AddRoom>().OpenToggleDoor(door);
    //     }

    //     // Select a random number of valid doors to open
    //     randomNumberOfValidDoorsToOpen = Random.Range(0, validSpawnRoomExits.Count);
    //     // Shuffle the exits
    //     shuffledValidSpawnRoomExits = validSpawnRoomExits.OrderBy(item => Random.value).ToList();
    //     var validSpawnerList = new List<string>();

    //     // Open the doors
    //     _newRoom.GetComponent<AddRoom>().OpenToggleDoor(shuffledValidSpawnRoomExits[0]);
    //     validSpawnerList.Add(shuffledValidSpawnRoomExits[0]);

    //     // Disable exits and spawners for invalid exits
    //     if (_newRoom.GetComponent<AddRoom>().LeftToggleDoor)
    //     {
    //         _newRoom.GetComponent<AddRoom>().leftSpawner.SetActive(false);   
    //     }
    //     if (_newRoom.GetComponent<AddRoom>().RightToggleDoor)
    //     {
    //         _newRoom.GetComponent<AddRoom>().rightSpawner.SetActive(false);   
    //     }
    //     if (_newRoom.GetComponent<AddRoom>().UpToggleDoor)
    //     {
    //         _newRoom.GetComponent<AddRoom>().upSpawner.SetActive(false);   
    //     }
    //     if (_newRoom.GetComponent<AddRoom>().DownToggleDoor)
    //     {
    //         _newRoom.GetComponent<AddRoom>().downSpawner.SetActive(false);   
    //     }

    //     // We don't want to active a spawner in an existing room
    //     shuffledValidSpawnRoomExits.Remove(openingDirection);

    //     // Remove exit trigger that lead to the room we came from

    //     string exitToRemove;

    //     switch (openingDirection)
    //     {
    //         default:
    //         case "up": exitToRemove = "UpExit";break;
    //         case "down": exitToRemove = "DownExit";break;
    //         case "left": exitToRemove = "LeftExit";break;
    //         case "right": exitToRemove = "RightExit";break;
    //     }

    //     for (int i = 0; i < _newRoom.transform.childCount; i++)
    //     {
    //         GameObject child = _newRoom.transform.GetChild(i).gameObject;
    //         if (child.name == "RoomExits")
    //         {
    //             for (int c = 0; c < child.transform.childCount; c++)
    //             {
    //                 GameObject exit = child.transform.GetChild(c).gameObject;
    //                 if (exit.name == exitToRemove)
    //                 {
    //                     Destroy(exit);
    //                 }
    //             }
    //         }
    //     }

    //     // Activate new room spawners
    //     if (templates.startBuildingLevel && gameObject.name != "StartRoom")
    //     {
    //         foreach (var exit in validSpawnerList)
    //         {
    //             switch (exit)
    //             {
    //                 default: throw new System.Exception("invalid exit");
    //                 case "up": _newRoom.GetComponent<AddRoom>().upSpawner.SetActive(true);break;
    //                 case "down": _newRoom.GetComponent<AddRoom>().downSpawner.SetActive(true);break;
    //                 case "left": _newRoom.GetComponent<AddRoom>().leftSpawner.SetActive(true);break;
    //                 case "right": _newRoom.GetComponent<AddRoom>().rightSpawner.SetActive(true);break;
    //             }
    //         }
    //     }

    //     // Remove spawners we're not going to use
    //     if (transform.parent.parent.name != "StartRoom(Clone)")
    //     {
    //         gameObject.GetComponentInParent<AddRoom>().RemoveUnusedSpawners();
    //     }

    //     Destroy(gameObject);

    //     // // Get random wall prefab
    //     // var wall = templates.roomWalls[Random.Range(0,templates.roomWalls.Length)];

    //     // if (wall.name == "1x1Wall")
    //     // {
    //     //     var arrowTraps = wall.GetComponent<Wall>().attachedTraps;

    //     //     switch (Random.Range(0,1))
    //     //                 {
    //     //                     default: throw new System.Exception("number not in range");
    //     //                     case 0:
    //     //                         for (int i = 0; i < 0; i++)
    //     //                         {
    //     //                             arrowTraps[Random.Range(0,arrowTraps.Length)].SetActive(true);
    //     //                         }
    //     //                         break;
    //     //                     case 1:
    //     //                         for (int i = 0; i < 1; i++)
    //     //                         {
    //     //                             arrowTraps[Random.Range(0,arrowTraps.Length)].SetActive(true);
    //     //                         }
    //     //                         break;
                                
    //     //                 }
    //     // }
    //     // // get object spawners
    //     // var objectSpawners = _newRoom.GetComponent<AddRoom>().objectSpawners;

    //     // // Get random object spawner

    //     // var spawnPoint = objectSpawners[Random.Range(0,objectSpawners.Length)];

    //     // objectSpawners = objectSpawners.Where(o => o != spawnPoint).ToArray();

    //     // var spawnPoint2 = objectSpawners[Random.Range(0,objectSpawners.Length)];


    //     // // Instantiate prefabs

    //     // GameObject wall1 = Instantiate(wall,spawnPoint.position,Quaternion.identity) as GameObject;
    //     // wall1.transform.parent = _newRoom.transform;
    //     // wall1.layer = 16;
    //     // GameObject wall2 = Instantiate(wall,spawnPoint2.position,Quaternion.identity) as GameObject;
    //     // wall2.transform.parent = _newRoom.transform;
    //     // wall2.layer = 16;
    // }

    void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.name == "Destroyer")
        {
            // Remove roomexit trigger
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "SpawnPoint")
        {
            otherSpawner = other.gameObject;
        }
    }

}



