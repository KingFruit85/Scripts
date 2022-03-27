using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SimpleRoomInfo
{
    public bool UpDoor;
    public bool DownDoor;
    public bool LeftDoor;
    public bool RightDoor;
    public bool isStartRoom;
    public int MapPositionX;
    public int MapPositionY;
    public List<string> availbleDoors = new List<string>(){"UP", "DOWN","LEFT","RIGHT"};
    public bool isUsed;
    public bool placed;
    public bool AlreadyPassedThrough;
    public string RoomType;
    public List<string> RoomTypes = new List<string>(){"Standard", "Puzzle", "Trap", "LoreRoom", "Prize"};

    // Default constructor
    public SimpleRoomInfo(int x, int y, string r)
    {
        this.UpDoor = false;
        this.DownDoor = false;
        this.LeftDoor = false;
        this.RightDoor = false;
        this.isStartRoom = false;
        this.MapPositionX = x;
        this.MapPositionY = y;
        this.AlreadyPassedThrough = false;
        this.RoomType = RoomTypes[UnityEngine.Random.Range(0,RoomTypes.Count)];
    }

    public void OpenDoor(string door)
    {
        switch (door)
        {
            default:
            case "UP": UpDoor = true;break;
            case "DOWN": DownDoor = true;break;
            case "LEFT": LeftDoor = true;break;
            case "RIGHT": RightDoor = true;break;
        }
    }
}

public class Map : MonoBehaviour
{
    public SimpleRoomInfo[,] map;
    public int mapLength = 25;
    public int MapHeight = 25;
    public Vector3 CurrentMapPosition;
    private int lastMapX;
    private int lastMapY;
    public GameObject cameraBox;
    private GameManager gameManager;



    void Awake()
    {
        map = new SimpleRoomInfo[mapLength,MapHeight];
        int x = mapLength / 2; 
        int y = MapHeight / 2; 
        cameraBox = Resources.Load("CameraBox") as GameObject;
        GameObject camBox = Instantiate(cameraBox,Vector3.zero,Quaternion.identity) as GameObject;
        camBox.name = "CameraBox";
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //Set start room
        FillMapWithRooms();
        CreatePathThroughRooms();
        SpawnRoomsInGameSpace();
    }

    /// <summary> Spawns a room into gamespace and open any valid doors </summary>
    public void SpawnRoom(int x, int y, Vector3 worldPos, int RoomNumber)
    {
        GameObject TemplateRoom = Resources.Load("SimpleRoom") as GameObject;
        GameObject _newRoom = Instantiate(TemplateRoom,worldPos,Quaternion.identity);

            // Set room name, room type and attach to parent
            _newRoom.name = $"X:{x} Y:{y} Room {RoomNumber}";
            var room = _newRoom.GetComponent<SimpleRoom>();
            room.RoomType = map[x,y].RoomType;
            // Set the event that will open the room doors, this changes based on the room type
            var doorController = _newRoom.transform.Find("DoorController").GetComponent<DoorController>();
            switch (room.RoomType)
            {
                case "Standard": doorController.OpenByMobDeath = true;break;
                case "Puzzle" : 
                    doorController.OpenByMobDeath = false;
                    doorController.OpenByPuzzleComplete = true;
                    break;
                case "Trap" : doorController.OpenByMobDeath = true;break;
                case "LoreRoom": doorController.OpenByMobDeath = true;break;
                case "Prize": doorController.OpenByMobDeath = true;break;
            }

            switch (room.RoomType)
            {
                default: throw new Exception("RoomType Unknown");
                case "Standard": room.EnemyCount = UnityEngine.Random.Range(0,3); break;
                case "Puzzle": room.EnemyCount = UnityEngine.Random.Range(0,3); break;
                case "Trap": room.EnemyCount = UnityEngine.Random.Range(0,3); break;
                case "LoreRoom": room.EnemyCount = UnityEngine.Random.Range(0,3); break;
                case "Prize": room.EnemyCount = UnityEngine.Random.Range(1,5); break;
            }
            _newRoom.transform.parent = GameObject.Find("Rooms").transform;
            
            // Open Doors
            if (map[x,y].UpDoor) room.OpenDoor("UP");
            if (map[x,y].DownDoor) room.OpenDoor("DOWN");
            if (map[x,y].LeftDoor) room.OpenDoor("LEFT");
            if (map[x,y].RightDoor) room.OpenDoor("RIGHT");

        
        // If start room remove any enemies and spawn player
        if (RoomNumber == 0)
        {
            _newRoom.name += " START ROOM";
            room.RoomType = "StartRoom";
            GameObject player = Instantiate(Resources.Load("Player Variant 1"),_newRoom.transform.position,Quaternion.identity) as GameObject;
            player.name = "Player";
            var camera = GameObject.Find("Main Camera");
            camera.transform.position = _newRoom.transform.position;
            doorController.roomComplete = true;
            
            // Disable the enemy spawner
            _newRoom.transform.Find("EnemySpawner").GetComponent<EnemySpawner>().enabled = false;

            // Debug: Spawn kill square
            Instantiate(Resources.Load("KillSquare"),room.ExitTile.transform.position,Quaternion.identity);

            // Place level specific terminal with new lore
            if (gameManager.currentGameLevel == 1)
            {

                GameObject terminal = Instantiate(Resources.Load("InteractableRune1"),room.ReturnTerminalSpawnLocation(),Quaternion.identity) as GameObject;
                terminal.transform.parent = _newRoom.transform.Find("Tiles");
                terminal.transform.position = _newRoom.transform.Find("CameraAnchor").transform.position;

            }

        } 

        // If end room remove any enemies and spawn mini boss and exit tile
        if (RoomNumber == GetTotalValidRooms()-1)
        {
            _newRoom.name += " END ROOM";
            room.RoomType = "EndRoom";
            GameObject mb = Instantiate(Resources.Load("GhostMiniBoss"),_newRoom.transform.position,Quaternion.identity) as GameObject;
            room.SpawnExitTile();
            mb.transform.parent = _newRoom.transform;
            // Disable the enemy spawner
            _newRoom.transform.Find("EnemySpawner").GetComponent<EnemySpawner>().enabled = false;

        }

        

        // Configure Standard room
        if (room.RoomType == "Standard")
        {

            // Get the floor tiles we can spawn objects on
            var floorTiles = room.SpawnableFloorTiles;
            // And a random number of walls we're going to spawn
            var wallTilesToSpawn = UnityEngine.Random.Range(0,floorTiles.Length);

            // Spawn the wall tiles
            for (int i = 0; i <= wallTilesToSpawn; i++)
            {
                GameObject wall = Instantiate(Resources.Load("Wall"),floorTiles[i].transform.position,Quaternion.identity) as GameObject;
                wall.transform.parent = _newRoom.transform.Find("Tiles");
                room.spawnedWallTiles.Add(wall);
                //Add to room contents array
                room.AddItemToRoomContents(wall.transform.localPosition,'#');

                // Spawn arrow traps on valid wall locations
                List<Vector3> validTrapWalls = new List<Vector3>()
                {
                    new Vector3(0.15f,0.15f,0),
                    new Vector3(-0.15f,0.15f,0),
                    new Vector3(-0.15f,-0.15f,0),
                    new Vector3(0.15f,-0.15f,0)
                };

                var wallPOS = wall.transform.localPosition;
                GameObject arrowTrap;

                if (validTrapWalls.Contains(wallPOS))
                {
                    arrowTrap = Instantiate(Resources.Load("arrowTrap"), wall.transform.localPosition,Quaternion.identity) as GameObject;
                    arrowTrap.transform.parent = _newRoom.transform.Find("Tiles");

                    if (wallPOS == new Vector3(0.15f,0.15f,0))
                    {
                        arrowTrap.transform.position = new Vector3(wall.transform.position.x,
                                                                   (wall.transform.position.y - 0.4f),
                                                                   0);
                        arrowTrap.GetComponent<ArrowTrap>().active = true;
                        arrowTrap.GetComponent<ArrowTrap>().shootDown = true;
                    }
                    if (wallPOS == new Vector3(-0.15f,0.15f,0))
                    {
                        arrowTrap.transform.position = new Vector3(wall.transform.position.x,
                                                                   (wall.transform.position.y - 0.4f),
                                                                   0);
                        arrowTrap.GetComponent<ArrowTrap>().active = true;
                        arrowTrap.GetComponent<ArrowTrap>().shootDown = true; 
                                            }
                    if (wallPOS == new Vector3(-0.15f,-0.15f,0))
                    {
                        arrowTrap.transform.rotation *= Quaternion.AngleAxis( 180, transform.right );
                        arrowTrap.transform.position = new Vector3(wall.transform.position.x,
                                                                   (wall.transform.position.y + 0.4f),
                                                                   0);

                        arrowTrap.GetComponent<ArrowTrap>().active = true;
                        arrowTrap.GetComponent<ArrowTrap>().shootUp = true;
                    }
                    if (wallPOS == new Vector3(0.15f,-0.15f,0))
                    {
                        arrowTrap.transform.rotation *= Quaternion.AngleAxis( 180, transform.right );
                        arrowTrap.transform.position = new Vector3(wall.transform.position.x,
                                                                   (wall.transform.position.y + 0.4f),
                                                                   0);
                        arrowTrap.GetComponent<ArrowTrap>().active = true;
                        arrowTrap.GetComponent<ArrowTrap>().shootUp = true;
                    }
                }
            }
        }

        // Configure Prize Room
        if (room.RoomType == "Prize")
        {
            // Get the random floor tile to spawn a chest on
            var chestSpawnLocation = room.SpawnableFloorTiles[UnityEngine.Random.Range(0,room.SpawnableFloorTiles.Length)].transform;
            // Spawn a chest on a random tile
            
            GameObject bars = Instantiate(Resources.Load("verticalBars"),chestSpawnLocation.position,Quaternion.identity) as GameObject;
            bars.transform.parent = _newRoom.transform.Find("Tiles");
            GameObject chest = Instantiate(Resources.Load("chest"),chestSpawnLocation.position,Quaternion.identity) as GameObject;
            chest.transform.parent = _newRoom.transform.Find("Tiles");
            //Add to room contents array
            room.AddItemToRoomContents(chest.transform.localPosition,'C');
        }

        // Configure Trap Room
        if (room.RoomType == "Trap")
        {
            var spawnLocations = _newRoom.GetComponent<SimpleRoom>().SpawnableFloorTiles;
            var r = UnityEngine.Random.Range(0,spawnLocations.Length);
            
            // GameObject trap = Instantiate(Resources.Load("arrowTurret"),spawnLocations[r].transform.position,Quaternion.identity) as GameObject;
            // trap.transform.parent = _newRoom.transform.Find("Tiles");
        }

        // Configure Lore Room
        if (room.RoomType == "LoreRoom")
        {
            var spawnLocations = _newRoom.GetComponent<SimpleRoom>().SpawnableFloorTiles;
            var r = UnityEngine.Random.Range(0,spawnLocations.Length);
            
            GameObject terminal = Instantiate(Resources.Load("InteractableRune1"),spawnLocations[r].transform.position,Quaternion.identity) as GameObject;
        }

        // Configure Puzzle Room
        if (room.RoomType == "Puzzle")
        {
            var runeTiles = room.runeTiles;
            var pillarTiles = room.pillarTiles;
            var chestTile = room.puzzleChestSpawnLocation;
            // Just one type of puzzle room right now

            // Spawn puzzle pieces
            // -Chest
            var adjustedChestPosition = new Vector3((0.5f + chestTile.transform.position.x),(-0.5f + chestTile.transform.position.y),0);
            GameObject chest = Instantiate(Resources.Load("chest")as GameObject,adjustedChestPosition,Quaternion.identity);
            chest.transform.parent = _newRoom.transform.Find("Tiles");

            // Barrier
            GameObject barrier = Instantiate(Resources.Load("verticalBars")as GameObject,adjustedChestPosition,Quaternion.identity);
            barrier.transform.parent = _newRoom.transform.Find("Tiles");
            barrier.GetComponent<Barrier>().SetBarrierUnlockMethod(1);

            // Pillars
            foreach (var tile in pillarTiles)
            {
                GameObject wall = Instantiate(Resources.Load("Wall") as GameObject,tile.transform.position,Quaternion.identity) as GameObject;
                wall.transform.parent = _newRoom.transform.Find("Tiles");
                // Flame bowls and arrow traps
                GameObject flameBowl = Instantiate(Resources.Load("FlameBowl") as GameObject,tile.transform.position,Quaternion.identity) as GameObject;
                flameBowl.transform.parent = _newRoom.transform.Find("Tiles");

                if (flameBowl.transform.localPosition == new Vector3(-0.25f, 0.25f, 0))
                {
                    flameBowl.name = "flameBowl1";
                    GameObject arrowTrap = Instantiate(Resources.Load("arrowTrap") as GameObject,_newRoom.GetComponent<SimpleRoom>().arrowTrap1Position.transform.position,Quaternion.identity) as GameObject;
                    arrowTrap.transform.parent = _newRoom.transform.Find("Tiles");
                    arrowTrap.name = "arrowTrap1";
                    arrowTrap.GetComponent<ArrowTrap>().shootRight = true;
                    
                    arrowTrap.transform.rotation *= Quaternion.AngleAxis( 90, transform.forward );
                    arrowTrap.transform.position += new Vector3(0.4f,0f,0f);
 
                }    
                    
                if (flameBowl.transform.localPosition == new Vector3(0.25f, 0.25f, 0))
                {
                    flameBowl.name = "flameBowl2";  
                    GameObject arrowTrap = Instantiate(Resources.Load("arrowTrap") as GameObject,_newRoom.GetComponent<SimpleRoom>().arrowTrap2Position.transform.position,Quaternion.identity) as GameObject;
                    arrowTrap.transform.parent = _newRoom.transform.Find("Tiles");
                    arrowTrap.name = "arrowTrap2";
                    arrowTrap.GetComponent<ArrowTrap>().shootLeft = true;
                    
                    arrowTrap.transform.rotation *= Quaternion.AngleAxis( -90, transform.forward );
                    arrowTrap.transform.position += new Vector3(-0.4f,0f,0f);
                } 

                if (flameBowl.transform.localPosition == new Vector3(-0.25f, -0.15f, 0))
                {
                    flameBowl.name = "flameBowl3";
                    GameObject arrowTrap = Instantiate(Resources.Load("arrowTrap") as GameObject,_newRoom.GetComponent<SimpleRoom>().arrowTrap3Position.transform.position,Quaternion.identity) as GameObject;
                    arrowTrap.transform.parent = _newRoom.transform.Find("Tiles");
                    arrowTrap.name = "arrowTrap3";
                    arrowTrap.GetComponent<ArrowTrap>().shootRight = true;
                    
                    arrowTrap.transform.rotation *= Quaternion.AngleAxis( 90, transform.forward );
                    arrowTrap.transform.position += new Vector3(0.4f,0f,0f);
                } 


                if (flameBowl.transform.localPosition == new Vector3(0.25f, -0.15f, 0))
                {
                    flameBowl.name = "flameBowl4";
                    GameObject arrowTrap = Instantiate(Resources.Load("arrowTrap") as GameObject,_newRoom.GetComponent<SimpleRoom>().arrowTrap4Position.transform.position,Quaternion.identity) as GameObject;
                    arrowTrap.transform.parent = _newRoom.transform.Find("Tiles");
                    arrowTrap.name = "arrowTrap4";
                    arrowTrap.GetComponent<ArrowTrap>().shootLeft = true;
                    
                    arrowTrap.transform.rotation *= Quaternion.AngleAxis( -90, transform.forward );
                    arrowTrap.transform.position += new Vector3(-0.4f,0f,0f);
                } 


                room.AddItemToRoomContents(tile.transform.localPosition,'P');
            }

            // -Runes - randomised placement
            List<GameObject> runes = new List<GameObject>()
            {
                Resources.Load("BlueTile") as GameObject,
                Resources.Load("GreenTile") as GameObject,
                Resources.Load("RedTile") as GameObject,
                Resources.Load("TealTile") as GameObject
            };

            foreach (var rune in runeTiles)
            {
                System.Random rnd = new System.Random();
                int r = rnd.Next(runes.Count);

                var randomRune = runes[r];
                GameObject _rune = Instantiate(randomRune,rune.transform.position,Quaternion.identity) as GameObject;
                _rune.GetComponent<Rune>().myUnlock = barrier;
                _rune.transform.parent = _newRoom.transform.Find("Tiles");
                room.AddItemToRoomContents(rune.transform.localPosition,'R');
                runes.RemoveAt(r);

                // How to link the runes to the flamebowls????
                if (_rune.transform.localPosition == new Vector3(-0.25f, 0.15f, 0))
                {
                    _rune.GetComponent<Rune>().flameBowl = _rune.transform.parent.Find("flameBowl1").GetComponent<FlameBowl>();
                    _rune.GetComponent<Rune>().myTrap = _rune.transform.parent.Find("arrowTrap1").gameObject;
                }

                if (_rune.transform.localPosition == new Vector3(0.25f, 0.15f, 0))
                {
                    _rune.GetComponent<Rune>().flameBowl = _rune.transform.parent.Find("flameBowl2").GetComponent<FlameBowl>();
                    _rune.GetComponent<Rune>().myTrap = _rune.transform.parent.Find("arrowTrap2").gameObject;
                }
                
                if (_rune.transform.localPosition == new Vector3(-0.25f, -0.25f, 0))
                {
                    _rune.GetComponent<Rune>().flameBowl = _rune.transform.parent.Find("flameBowl3").GetComponent<FlameBowl>();
                    _rune.GetComponent<Rune>().myTrap = _rune.transform.parent.Find("arrowTrap3").gameObject;
                }
                
                if (_rune.transform.localPosition == new Vector3(0.25f, -0.25f, 0))
                {
                    _rune.GetComponent<Rune>().flameBowl = _rune.transform.parent.Find("flameBowl4").GetComponent<FlameBowl>();
                    _rune.GetComponent<Rune>().myTrap = _rune.transform.parent.Find("arrowTrap4").gameObject;
                }
                
            }


            barrier.GetComponent<Barrier>().redRune = _newRoom.transform.Find("Tiles").Find("RedTile(Clone)").gameObject;
            barrier.GetComponent<Barrier>().blueRune = _newRoom.transform.Find("Tiles").Find("BlueTile(Clone)").gameObject;
            barrier.GetComponent<Barrier>().greenRune = _newRoom.transform.Find("Tiles").Find("GreenTile(Clone)").gameObject;
            barrier.GetComponent<Barrier>().tealRune = _newRoom.transform.Find("Tiles").Find("TealTile(Clone)").gameObject;
        }
        
        foreach (var w in room.spawnedWallTiles)
        {
            // For some reason floor tiles were getting slightly changed when instanciated, for example something
            // with a y of 0.05 in the prefab would instantiated with  0.500001, this was breaking the conversion in 
            // SetTileSprite so this was the fix I came up with.
            var convertedX = float.Parse(w.transform.localPosition.x.ToString("0.##"));
            var convertedY = float.Parse(w.transform.localPosition.y.ToString("0.##"));
            w.GetComponent<Wall>().SetTileSprite(new Vector3(convertedX,convertedY,0));
        }

        room.SaveRoomLayoutToFile(x, y);
        
    }

    /// <summary> Bulk fills the 2D map array with SimpleRoomInfoObjects </summary>
    public void FillMapWithRooms()
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x,y] == null || !map[x,y].placed)
                {

                    map[x,y] = new SimpleRoomInfo(x,y,"10X10Room");
                    map[x,y].placed = true;
                }
                CurrentMapPosition = new Vector3(x,y,0);
            }
        }
    }

    /// <summary> Removes room exits that would lead out of bounds of the array  </summary>
    public void RemoveInvalidExits(SimpleRoomInfo room)
    {
        // If room is on the edge of map, remove doors that would lead off the map
        if (room.MapPositionY == 0) room.availbleDoors.Remove("DOWN");
        if (room.MapPositionY == (map.GetLength(1) -1) ) room.availbleDoors.Remove("UP");
        if (room.MapPositionX == 0) room.availbleDoors.Remove("LEFT");
        if (room.MapPositionX == (map.GetLength(0) -1) ) room.availbleDoors.Remove("RIGHT");
     }

    /// <summary> Opens a random valid door and the doorway of the adjacent room to link both rooms. Returns the adjacent room </summary>
    public SimpleRoomInfo OpenRandomValidDoor(SimpleRoomInfo room)
    {
        RemoveInvalidExits(room);

        // Return a random valid door
        start:
        string door = room.availbleDoors[UnityEngine.Random.Range(0,room.availbleDoors.Count)];
        switch (door)
        {
            default:Debug.Log("No rooms remaining"); break;
            case "UP":
                if (map[(room.MapPositionX),(room.MapPositionY+1)].AlreadyPassedThrough)
                {
                    room.availbleDoors.Remove("UP");
                    goto start;
                }break; 

            case "DOWN":
                if (map[(room.MapPositionX),(room.MapPositionY-1)].AlreadyPassedThrough)
                {
                    room.availbleDoors.Remove("DOWN");
                    goto start;
                }break; 

            case "LEFT":   
                if (map[(room.MapPositionX-1),(room.MapPositionY)].AlreadyPassedThrough)
                {
                    room.availbleDoors.Remove("LEFT");
                    goto start;
                }break;    

            case "RIGHT":  
                if (map[(room.MapPositionX+1),(room.MapPositionY)].AlreadyPassedThrough)
                {
                    room.availbleDoors.Remove("RIGHT");
                    goto start;
                }break;           
        }
        
        room.OpenDoor(door);
        // Open the adjacent rooms door
        SimpleRoomInfo connectingRoom = map[0,0];
        string oppositeDoor = "";
        switch (door)
        {
            case "UP": 
                connectingRoom = map[room.MapPositionX,(room.MapPositionY + 1)];
                oppositeDoor = "DOWN";
                break;
            case "DOWN": 
                connectingRoom = map[room.MapPositionX,(room.MapPositionY - 1)];
                oppositeDoor = "UP";
                break;
            case "LEFT": 
                connectingRoom = map[(room.MapPositionX - 1),room.MapPositionY];
                oppositeDoor = "RIGHT";
                break;
            case "RIGHT": 
                connectingRoom = map[(room.MapPositionX + 1),room.MapPositionY];
                oppositeDoor = "LEFT";
                break;
        }
        connectingRoom.OpenDoor(oppositeDoor);

        return connectingRoom;

    }
    /// <summary> Creates a random path though the map </summary>
    public void CreatePathThroughRooms()
    {
        // Pick a random start room
        var currentRoom = map[UnityEngine.Random.Range(0,(map.GetLength(0)-1)),UnityEngine.Random.Range(0,(map.GetLength(1)-1))];
        currentRoom.isStartRoom = true;

        // Need to track the current room and the next room
        for (int i = 0; i < (map.GetLength(0) * map.GetLength(1)); i++)
        {
            currentRoom.isUsed = true;
            currentRoom = OpenRandomValidDoor(currentRoom);
        }
    }
    
    /// <summary> Converts the position in the 2D array to the game space </summary>
    public Vector3 ConvertMapPositionToWorldPosition(int x, int y)
    {
        return new Vector3((x*10),(y*10),0);
    }

    public int GetTotalValidRooms()
    {
        int totalValidRooms = 0;

        for (int y = 0; y < map.GetLength(0); y++)
                {
                    for (int x = 0; x < map.GetLength(1); x++)
                    {
                        if (map[x, y] != null && map[x, y].isUsed)
                        {
                            totalValidRooms++;
                        }
                    }
                }
        return totalValidRooms;
    }


    /// <summary> Iterates over the 2D map array and spawns any valid rooms into gamespace </summary>
    public void SpawnRoomsInGameSpace()
    {
        int roomNumber = 0;
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[x, y] != null && map[x, y].isUsed)
                {
                    var worldPos = ConvertMapPositionToWorldPosition(x,y);
                    SpawnRoom( x, y, worldPos, roomNumber);
                    lastMapX = x;
                    lastMapY = y;
                    roomNumber++;
                }
            }
        }
    }
}