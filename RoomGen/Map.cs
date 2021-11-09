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
    
    void Awake()
    {
        map = new SimpleRoomInfo[10,10];
        int x = mapLength / 2; 
        int y = MapHeight / 2; 

        //Set start room
        map[x,y] = new SimpleRoomInfo();
        OpenOneOrMoreDoors(map[x,y]);
        SpawnRooms();
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

    public void SpawnRoom(int x, int y)
    {
        GameObject TemplateRoom = Resources.Load("SimpleRoom") as GameObject;
        GameObject _newRoom = Instantiate(TemplateRoom,new Vector3(x,y,0),Quaternion.identity);
                   _newRoom.transform.parent = GameObject.Find("Rooms").transform;
                   if (map[x,y].UpDoor) _newRoom.GetComponent<SimpleRoom>().OpenDoor("UP");
                   if (map[x,y].DownDoor) _newRoom.GetComponent<SimpleRoom>().OpenDoor("DOWN");
                   if (map[x,y].LeftDoor) _newRoom.GetComponent<SimpleRoom>().OpenDoor("LEFT");
                   if (map[x,y].RightDoor) _newRoom.GetComponent<SimpleRoom>().OpenDoor("RIGHT");
    }

    public void SpawnRooms()
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x, y] != null)
                {
                    SpawnRoom( x, y );
                }
            }
        }
    }
}
