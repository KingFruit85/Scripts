using System.Collections;
using System.Collections.Generic;
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

        // Default constructor
        public SimpleRoomInfo(int x, int y)
        {
            this.UpDoor = false;
            this.DownDoor = false;
            this.LeftDoor = false;
            this.RightDoor = false;
            this.isStartRoom = false;
            this.MapPositionX = x;
            this.MapPositionY = y;
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
    public int mapLength = 10;
    public int MapHeight = 10;
    public Vector3 CurrentMapPosition;
    private int lastMapX;
    private int lastMapY;
    private bool buildingPath;

    void Awake()
    {
        map = new SimpleRoomInfo[mapLength,MapHeight];
        int x = mapLength / 2; 
        int y = MapHeight / 2; 

        //Set start room
        FillMapWithRooms();
        CreatePathThroughRooms();
        SpawnRoomsInGameSpace();
    }

    /// <summary> Takes a room and opens 1 or more doors at random </summary>
    public void OpenOneOrMoreDoors(SimpleRoomInfo room)
    {
        int numberOfDoorsToOpen = Random.Range(0,room.availbleDoors.Count);
        for (int i = 0; i <= numberOfDoorsToOpen; i++)
        {
            string door = room.availbleDoors[Random.Range(0,room.availbleDoors.Count)];
            room.OpenDoor(door);
            room.availbleDoors.Remove(door);
        }
    }

    /// <summary> Spawns a room into gamespace and open any valid doors </summary>
    public void SpawnRoom(int x, int y, Vector3 worldPos, int roomNumber)
    {
        GameObject TemplateRoom = Resources.Load("SimpleRoom") as GameObject;
        GameObject _newRoom = Instantiate(TemplateRoom,worldPos,Quaternion.identity);
                   _newRoom.name = $"X:{x} Y:{y} RoomNumber {roomNumber}";
                   _newRoom.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = roomNumber.ToString();
                   _newRoom.transform.parent = GameObject.Find("Rooms").transform;
                   if (map[x,y].UpDoor) _newRoom.GetComponent<SimpleRoom>().OpenDoor("UP");
                   if (map[x,y].DownDoor) _newRoom.GetComponent<SimpleRoom>().OpenDoor("DOWN");
                   if (map[x,y].LeftDoor) _newRoom.GetComponent<SimpleRoom>().OpenDoor("LEFT");
                   if (map[x,y].RightDoor) _newRoom.GetComponent<SimpleRoom>().OpenDoor("RIGHT");
        
        // If start room spawn player
        if (map[x,y].isStartRoom)
        {
            Instantiate(Resources.Load("Player Variant 1"),worldPos,Quaternion.identity);
            var camera = GameObject.Find("Main Camera");
            camera.transform.position = worldPos;
        } 
    }

    /// <summary> Bulk fills the 2D map array with SimpleRoomInfoObjects </summary>
    public void FillMapWithRooms()
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                CurrentMapPosition = new Vector3(x,y,0);
                map[x,y] = new SimpleRoomInfo(x,y);
            }
        }
    }
    /// <summary> Checks if the provided room is valid and in the array  </summary>
    public bool CheckIfRoomIsValid(SimpleRoomInfo[,] mapPos)
    {
        // Check iof potential rooms are in the bounds of the array
        try
        {
             var x = mapPos;
        }
        catch (System.IndexOutOfRangeException)
        {
            return false;
        }
        return true;
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
        string door = room.availbleDoors[Random.Range(0,room.availbleDoors.Count)];
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
        var currentRoom = map[Random.Range(0,(map.GetLength(0)-1)),Random.Range(0,(map.GetLength(1)-1))];
        Debug.Log($"X:{currentRoom.MapPositionX}, Y:{currentRoom.MapPositionY}");
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
                    SpawnRoom( x, y, worldPos, roomNumber );
                    lastMapX = x;
                    lastMapY = y;
                    roomNumber++;
                }
            }
        }
    }
}