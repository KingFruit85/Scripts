using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRoomInfo
    {
        public bool liveRoom;
        public bool UpDoor;
        public bool DownDoor;
        public bool LeftDoor;
        public bool RightDoor;

        // Default constructor
        public SimpleRoomInfo()
        {
            this.UpDoor = false;
            this.DownDoor = false;
            this.LeftDoor = false;
            this.RightDoor = false;
            this.liveRoom = false;
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
    public Vector3 CurrentSpawnPosition = new Vector3(0,0,0);
    private int lastMapX;
    private int lastMapY;

    void Awake()
    {
        map = new SimpleRoomInfo[10,10];
        int x = mapLength / 2; 
        int y = MapHeight / 2; 

        //Set start room
        FillMapWithRooms();
        SpawnRoomsInGameSpace();
    }

    /// <summary> Takes a room and opens 1-4 doors at random </summary>
    public void OpenOneOrMoreDoors(SimpleRoomInfo room)
    {
        int numberOfDoorsToOpen = Random.Range(0,4);
        List<string> availbleDoors = new List<string>(){"UP", "DOWN","LEFT","RIGHT"};

        for (int i = 0; i <= numberOfDoorsToOpen; i++)
        {
            string door = availbleDoors[Random.Range(0,availbleDoors.Count)];
            room.OpenDoor(door);
            availbleDoors.Remove(door);
        }
    }

    public void SpawnRoom(int x, int y, Vector3 worldPos)
    {
        GameObject TemplateRoom = Resources.Load("SimpleRoom") as GameObject;
        GameObject _newRoom = Instantiate(TemplateRoom,worldPos,Quaternion.identity);
                   _newRoom.transform.parent = GameObject.Find("Rooms").transform;
                   if (map[x,y].UpDoor) _newRoom.GetComponent<SimpleRoom>().OpenDoor("UP");
                   if (map[x,y].DownDoor) _newRoom.GetComponent<SimpleRoom>().OpenDoor("DOWN");
                   if (map[x,y].LeftDoor) _newRoom.GetComponent<SimpleRoom>().OpenDoor("LEFT");
                   if (map[x,y].RightDoor) _newRoom.GetComponent<SimpleRoom>().OpenDoor("RIGHT");
    }

    public void FillMapWithRooms()
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                CurrentMapPosition = new Vector3(x,y,0);
                map[x,y] = new SimpleRoomInfo();
                OpenOneOrMoreDoors(map[x,y]);
            }
        }
    }

    public Vector3 ConvertMapPositionToWorldPosition(int x, int y)
    {
        return new Vector3((x*10),(y*10),0);
    }

    public void SpawnRoomsInGameSpace()
    {
        
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[x, y] != null)
                {
                    var worldPos = ConvertMapPositionToWorldPosition(x,y);
                    SpawnRoom( x, y, worldPos );
                    lastMapX = x;
                    lastMapY = y;
                }
            }
        }
    }
}
