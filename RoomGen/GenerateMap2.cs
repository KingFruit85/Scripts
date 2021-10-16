using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GenerateMap2 : MonoBehaviour
{
  
    private int MapHeight = 55;
    private int MapLength = 55;
    private const int MaxRoomHeight = 15;
    private const int MaxRoomLength = 15;

    public struct Room
    {
        public int[] StartPoint;
        public int Length;
        public int Height;

        public Room(int[] startPoint, int length,int height)
        {
           StartPoint = startPoint;
           Length = length;
           Height = height; 
        }
    }

    private List<Room> Rooms = new List<Room>();

    [SerializeField]
    public int[] currentSpawnPoint = new int[2] {1,1};

    // 0 = floor
    // 1 = wall
    public int[,] map;
    

    void Start()
    {
        map = new int[MapLength,MapHeight];
        // Fill out the 2D array with 1's
        InitializeMap();
        CreateRooms();
        CreateCorridors();
        PlaceTiles();
    }

    

    void PlaceTiles()
    {
        for (int i = 0; i <= map.GetUpperBound(0); i++)  
        {  
            for (int j = 0; j <= map.GetUpperBound(1); j++)  
            { 
                // Floor Tiles
                if (map[i,j] == 0)
                {
                    GameObject x = Instantiate(Resources.Load("GenericFloor"), new Vector2(i,j),Quaternion.identity) as GameObject;
                    x.transform.parent = GameObject.Find("FloorTiles").transform;
                }
                //Wall Tiles
                if (map[i,j] == 1)
                {
                    GameObject x = Instantiate(Resources.Load("GenericWall"), new Vector2(i,j),Quaternion.identity) as GameObject;
                    x.transform.parent = GameObject.Find("WallTiles").transform;
                    x.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = $"{i} , {j}";
                }

                if (map[i,j] == 6)
                {
                    GameObject x = Instantiate(Resources.Load("BlueTile"), new Vector2(i,j),Quaternion.identity) as GameObject;
                }
            }
        }
    }

    void CreateRooms()
    {
        for (int i = 0; i <= 24; i++)
        {
            // Get new room dimentions
            var length = UnityEngine.Random.Range(5,MaxRoomLength);
            var height = UnityEngine.Random.Range(5,MaxRoomHeight);
            Debug.Log($" loop:{i} currentSpawnPoint = X{currentSpawnPoint[0]}, Y:{currentSpawnPoint[1]}");

            // Using default start point
            if (i == 0)
            {   
                CreateRoom(currentSpawnPoint,length,height);
                GameObject x = Instantiate(Resources.Load("Player Variant 1"), new Vector2(currentSpawnPoint[0],currentSpawnPoint[1]),Quaternion.identity) as GameObject;
            }
            else
            {
                SetNextSpawnPoint(currentSpawnPoint,length,height);
            }
        }
    }

    private void CreateCorridors()
    {
        // get the list of rooms
        var rooms = Rooms;
        // for each room get a wall that has an adjacent room

        foreach (var room in rooms)
        {
            room.StartPoint[0] += MaxRoomLength;
            room.StartPoint[1] += MaxRoomHeight/2;
            map[room.StartPoint[0],room.StartPoint[1]] = 6;
        }
        // try to tunnel a path to it

    }
    
    
    bool CheckIfSpawnPointIsValid(int X, int Y)
    {   
        // A negative number is out of bounds of the array
        if (X < 0 || Y < 0)
        {
            return false;
        }

        // Number is larger than the max bounds of the array
        if (X > map.GetLength(0) || Y > map.GetLength(1)) 
        {
            return false;
        }

        // Returns false if were at the edge of the array
        if (X == map.GetLength(0) || Y == map.GetLength(1))
        {
            return false;
        }

        // If the position is a wall tile return true 
        try
        {
             if (map[X,Y] == 1)
             {
                 return true;
             }
             else if (map[X,Y] == 0)
             {
                Debug.Log("Error, trying to spawn on an existing floor tile");
                return false;
             }
        }
        catch (System.IndexOutOfRangeException)
        {
            return false;
        }
        return false;
    }

    bool CheckIfNewRoomIsValid(int[] newSpawnPoint, int newRoomLength, int newRoomHeight)
    {
        // check each tile of the proposed new room to make sure it's valid and not already a floor tile
            for (int i = newSpawnPoint[0]; i <= newSpawnPoint[0] + (newRoomLength -1); i++)
            {
                for (int j = newSpawnPoint[1]; j <= newSpawnPoint[1] + (newRoomHeight -1); j++)
                {
                    if (CheckIfSpawnPointIsValid(i,j))
                    {
                        Debug.Log("New room is valid");
                        return true;
                    }
                }
            }
            Debug.Log("New Room is not valid");
            return false;
    }
    
    void SetNextSpawnPoint(int[] spawnPoint, int newRoomLength, int newRoomHeight)
    {
        int X = spawnPoint[0];
        int Y = spawnPoint[1];

        int RightSpawn = X + MaxRoomLength + 1;
        int UpSpawn = Y + MaxRoomHeight + 1;
        int LeftSpawn = X - MaxRoomLength - 1;
        
        // Check Right
        // Checks if there is a valid starting point to the right by taking the current spawn point and moving 
        // right the max possible length of a room + 1 (to allow for wall buffer and a corridor to be added later)
        if (CheckIfSpawnPointIsValid(RightSpawn, Y))
        {
            if (CheckIfNewRoomIsValid(new int[2]{RightSpawn,Y},newRoomLength,newRoomHeight))
            {
                currentSpawnPoint = new int[2]{RightSpawn,Y};
                CreateRoom(currentSpawnPoint,newRoomLength,newRoomHeight);
                return;
            }            
        }
        
        // Check Left
        else if (CheckIfSpawnPointIsValid(LeftSpawn, Y))
        {
            if (CheckIfNewRoomIsValid(new int[2]{LeftSpawn,Y},newRoomLength,newRoomHeight))
            {
                currentSpawnPoint = new int[2]{LeftSpawn,Y};
                CreateRoom(currentSpawnPoint,newRoomLength,newRoomHeight);
                return;
            }            
        }
        
        // Check Up
        else if (CheckIfSpawnPointIsValid(X, UpSpawn))
        {
            if (CheckIfNewRoomIsValid(new int[2]{X,UpSpawn},newRoomLength,newRoomHeight))
            {
                currentSpawnPoint = new int[2]{X,UpSpawn};
                CreateRoom(currentSpawnPoint,newRoomLength,newRoomHeight);
                return;
            }            
        }
        
        else
        {
            Debug.Log("Cannot find a suitable spawn point");
            return;
        }

    }
    
    void CreateRoom(int[] startPoint, int length, int height)
    {
        var X = startPoint[0];
        var Y = startPoint[1];

        // SetNextSpawnPoint() has done all the validity checking needed by this point
        for (int i = X; i <= X + (length - 1); i++)
            {
                for (int j = Y; j <= Y + (height - 1); j++)
                {
                    map[i,j] = 0;
                }
            }

        // Save the room 
        Rooms.Add(new Room(startPoint,length,height));
    }
    
    void InitializeMap()
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                map[i, j] = 1;
            }
        }
    }
}