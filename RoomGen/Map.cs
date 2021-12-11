using System.Collections.Generic;
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
    public GameObject cameraBox;

    void Awake()
    {
        map = new SimpleRoomInfo[mapLength,MapHeight];
        int x = mapLength / 2; 
        int y = MapHeight / 2; 
        cameraBox = Resources.Load("CameraBox") as GameObject;
        GameObject camBox = Instantiate(cameraBox,Vector3.zero,Quaternion.identity) as GameObject;
        camBox.name = "CameraBox";

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
                   _newRoom.name = $"X:{x} Y:{y} Room {RoomNumber}";
                   _newRoom.transform.parent = GameObject.Find("Rooms").transform;
                   _newRoom.GetComponent<SimpleRoom>().EnemyCount = Random.Range(0,4);
                   
                   if (map[x,y].UpDoor) _newRoom.GetComponent<SimpleRoom>().OpenDoor("UP");
                   if (map[x,y].DownDoor) _newRoom.GetComponent<SimpleRoom>().OpenDoor("DOWN");
                   if (map[x,y].LeftDoor) _newRoom.GetComponent<SimpleRoom>().OpenDoor("LEFT");
                   if (map[x,y].RightDoor) _newRoom.GetComponent<SimpleRoom>().OpenDoor("RIGHT");

        
        // If start room remove any enemies and spawn player
        if (RoomNumber == 0)
        {
            _newRoom.name += " START ROOM";
            _newRoom.GetComponent<SimpleRoom>().EnemySpawner.GetComponent<EnemySpawner>().canSpawn = false;
            Instantiate(Resources.Load("Player Variant 1"),_newRoom.transform.position,Quaternion.identity);
            var camera = GameObject.Find("Main Camera");
            camera.transform.position = _newRoom.transform.position;
        } 

        if (RoomNumber == GetTotalValidRooms()-1)
        {
            _newRoom.name += " END ROOM";
            GameObject GhostBoss = Instantiate(Resources.Load("Ghost"),_newRoom.transform.position,Quaternion.identity) as GameObject;
            GhostBoss.transform.parent = _newRoom.transform;
            GhostBoss.name = "GhostBoss";
            GhostBoss.tag = "GhostBoss";
            GhostBoss.GetComponent<Health>().isBoss = true;
        }
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
        string door = room.availbleDoors[Random.Range(0,room.availbleDoors.Count)];
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
        var currentRoom = map[Random.Range(0,(map.GetLength(0)-1)),Random.Range(0,(map.GetLength(1)-1))];
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