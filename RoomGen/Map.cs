using System;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRoomInfo
{
    public bool UpDoor;
    public bool DownDoor;
    public bool LeftDoor;
    public bool RightDoor;
    public bool IsStartRoom;
    public int MapPositionX;
    public int MapPositionY;
    public enum WallDirection
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
    public List<WallDirection> ValidWalls = new List<WallDirection>()
        {
            WallDirection.UP,
            WallDirection.DOWN,
            WallDirection.LEFT,
            WallDirection.RIGHT
        };

    public bool isUsed;
    public bool placed;
    public string RoomType;

    // Default constructor
    public SimpleRoomInfo(int x, int y, string r, string roomType)
    {
        this.UpDoor = false;
        this.DownDoor = false;
        this.LeftDoor = false;
        this.RightDoor = false;
        this.IsStartRoom = false;
        this.MapPositionX = x;
        this.MapPositionY = y;
        this.RoomType = roomType;
    }

    public void OpenDoorInWall(SimpleRoomInfo.WallDirection wall)
    {
        switch (wall)
        {
            case SimpleRoomInfo.WallDirection.UP: UpDoor = true; break;
            case SimpleRoomInfo.WallDirection.DOWN: DownDoor = true; break;
            case SimpleRoomInfo.WallDirection.LEFT: LeftDoor = true; break;
            case SimpleRoomInfo.WallDirection.RIGHT: RightDoor = true; break;
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
    private Rigidbody2D rb2D;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Awake()
    {
        map = new SimpleRoomInfo[mapLength, MapHeight];
        int x = mapLength / 2;
        int y = MapHeight / 2;
        cameraBox = Resources.Load("CameraBox") as GameObject;
        GameObject camBox = Instantiate(cameraBox, Vector3.zero, Quaternion.identity) as GameObject;
        camBox.name = "CameraBox";
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //Set start room
        FillMapWithRooms();
        CreatePathThroughRooms();
        SpawnRoomsInGameSpace();
        // clean up any doors leading to nowhere
    }

    /// <summary> Spawns a room into game space and open any valid doors </summary>
    public void SpawnRoom(int x, int y, Vector3 worldPos, int RoomNumber)
    {
        GameObject TemplateRoom = Resources.Load("SimpleRoom") as GameObject;
        GameObject _newRoom = Instantiate(TemplateRoom, worldPos, Quaternion.identity);

        // Set room name, room type and attach to parent
        _newRoom.name = $"X:{x} Y:{y} Room {RoomNumber}";
        var room = _newRoom.GetComponent<SimpleRoom>();
        room.RoomType = map[x, y].RoomType;
        // Set the event that will open the room doors, this changes based on the room type
        var doorController = _newRoom.transform.Find("DoorController").GetComponent<DoorController>();
        _newRoom.transform.parent = GameObject.Find("Rooms").transform;

        // Open Doors
        if (map[x, y].UpDoor) room.OpenDoor("UP");
        if (map[x, y].DownDoor) room.OpenDoor("DOWN");
        if (map[x, y].LeftDoor) room.OpenDoor("LEFT");
        if (map[x, y].RightDoor) room.OpenDoor("RIGHT");


        // If start room remove any enemies and spawn player
        if (RoomNumber == 0)
        {
            room.RoomType = "StartRoom";
            room.gameObject.AddComponent<StartRoom>();
        }

        // If end room remove any enemies and spawn mini boss and exit tile
        if (RoomNumber == GetTotalValidRooms() - 1)
        {
            room.RoomType = "EndRoom";
            room.gameObject.AddComponent<EndRoom>();
        }

        // Configure Standard room
        if (room.RoomType == "Standard")
        {
            room.gameObject.AddComponent<StandardRoom>();
        }

        // Configure Prize Room
        if (room.RoomType == "Prize")
        {
            room.gameObject.AddComponent<PrizeRoom>();
        }

        // Configure Trap Room
        if (room.RoomType == "Trap1")
        {
            room.gameObject.AddComponent<TrapRoom>();
        }

        if (room.RoomType == "Trap2")
        {
            doorController.OpenByMobDeath = true;
            room.gameObject.AddComponent<WallOfDeathRoom>();
        }

        // Configure Lore Room
        if (room.RoomType == "LoreRoom")
        {
            room.gameObject.AddComponent<LoreRooms>();
        }

        // Configure Puzzle Room
        if (room.RoomType == "Puzzle")
        {
            room.gameObject.AddComponent<PuzzleRooms>();
        }

        // Configure Puzzle Room
        if (room.RoomType == "Swarm")
        {
            room.gameObject.AddComponent<SwarmRooms>();
        }

        foreach (var w in room.spawnedWallTiles)
        {
            // For some reason floor tiles were getting slightly changed when instanciated, for example something
            // with a y of 0.05 in the prefab would instantiated with  0.500001, this was breaking the conversion in 
            // SetTileSprite so this was the fix I came up with.
            var convertedX = float.Parse(w.transform.localPosition.x.ToString("0.##"));
            var convertedY = float.Parse(w.transform.localPosition.y.ToString("0.##"));
            w.GetComponent<Wall>().SetTileSprite(new Vector3(convertedX, convertedY, 0));
        }

        room.SaveRoomLayoutToFile(x, y);

    }

    /// <summary> Bulk fills the 2D map array with SimpleRoomInfoObjects </summary>
    public void FillMapWithRooms()
    {
        var roomTypes = new List<string>() { "Standard", "Puzzle", "LoreRoom", "Prize", "Trap1", "Trap2", "Swarm" };

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x, y] == null || !map[x, y].placed)
                {
                    var roomType = roomTypes[UnityEngine.Random.Range(0, roomTypes.Count)];
                    if (roomType != "Standard")
                    {
                        roomTypes.RemoveAll(r => r == roomType);
                    } // this sucks

                    map[x, y] = new SimpleRoomInfo(x, y, "10X10Room", roomType);
                    map[x, y].placed = true;
                }
                CurrentMapPosition = new Vector3(x, y, 0);
            }
        }
    }

    /// <summary> Removes room exits that would lead out of bounds of the array  </summary>
    public void RemoveInvalidExits(SimpleRoomInfo room)
    {
        // If room is on the edge of map, remove doors that would lead off the map
        if (room.MapPositionY == 0) room.ValidWalls.Remove(SimpleRoomInfo.WallDirection.DOWN);
        if (room.MapPositionY == (map.GetLength(1) - 1)) room.ValidWalls.Remove(SimpleRoomInfo.WallDirection.UP);
        if (room.MapPositionX == 0) room.ValidWalls.Remove(SimpleRoomInfo.WallDirection.LEFT);
        if (room.MapPositionX == (map.GetLength(0) - 1)) room.ValidWalls.Remove(SimpleRoomInfo.WallDirection.RIGHT);
    }

    /// <summary> Opens a random valid door and the doorway of the adjacent room to link both rooms. Returns the adjacent room </summary>
    public SimpleRoomInfo OpenRandomValidDoor(SimpleRoomInfo roomBlueprint)
    {
        RemoveInvalidExits(roomBlueprint);

        // Return a random valid door
        var wall = roomBlueprint.ValidWalls[UnityEngine.Random.Range(0, roomBlueprint.ValidWalls.Count)];
        roomBlueprint.OpenDoorInWall(wall);
        // Open the adjacent rooms door
        SimpleRoomInfo connectingRoom = map[0, 0];
        var oppositeDoor = SimpleRoomInfo.WallDirection.UP;
        switch (wall)
        {
            case SimpleRoomInfo.WallDirection.UP:
                connectingRoom = map[roomBlueprint.MapPositionX, (roomBlueprint.MapPositionY + 1)];
                oppositeDoor = SimpleRoomInfo.WallDirection.DOWN;
                break;
            case SimpleRoomInfo.WallDirection.DOWN:
                connectingRoom = map[roomBlueprint.MapPositionX, (roomBlueprint.MapPositionY - 1)];
                oppositeDoor = SimpleRoomInfo.WallDirection.UP;
                break;
            case SimpleRoomInfo.WallDirection.LEFT:
                connectingRoom = map[(roomBlueprint.MapPositionX - 1), roomBlueprint.MapPositionY];
                oppositeDoor = SimpleRoomInfo.WallDirection.RIGHT;
                break;
            case SimpleRoomInfo.WallDirection.RIGHT:
                connectingRoom = map[(roomBlueprint.MapPositionX + 1), roomBlueprint.MapPositionY];
                oppositeDoor = SimpleRoomInfo.WallDirection.LEFT;
                break;
        }
        connectingRoom.OpenDoorInWall(oppositeDoor);

        return connectingRoom;

    }
    /// <summary> Creates a random path though the map </summary>
    public void CreatePathThroughRooms()
    {
        // Pick a random start room
        var currentRoom = map[UnityEngine.Random.Range(0, (map.GetLength(0) - 1)), UnityEngine.Random.Range(0, (map.GetLength(1) - 1))];
        currentRoom.IsStartRoom = true;

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
        return new Vector3((x * 10), (y * 10), 0);
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
                    var worldPos = ConvertMapPositionToWorldPosition(x, y);
                    SpawnRoom(x, y, worldPos, roomNumber);
                    lastMapX = x;
                    lastMapY = y;
                    roomNumber++;
                }
            }
        }
        Console.WriteLine("created a level");
        gameManager.timeTillDeath = roomNumber * 10;
    }
}