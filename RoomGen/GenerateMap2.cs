using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GenerateMap2 : MonoBehaviour
{
  
    private int MapHeight = 51;
    private int MapLength = 51;
    private const int MaxRoomHeight = 10;
    private const int MaxRoomLength = 10;

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
    private int[] nextSpawnPoint = new int[2]{1,1};


    // 0 = floor
    // 1 = wall
    public int[,] map;
    

    void Start()
    {
        map = new int[MapLength,MapHeight];
        // Fill out the 2D array with 1's
        InitializeMap();
        CreateRooms();

        PlaceTiles();
        var c = 1;
        foreach (var room in Rooms)
        {
            Debug.Log($"Start Point {c} X{room.StartPoint[0]},Y{room.StartPoint[1]} Length:{room.Length} Height:{room.Height}");
            c++;
        }
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
            }
        }
    }

    void CreateRooms()
    {
        for (int i = 0; i <= 25; i++)
        {
            // Get new room dimentions
            var length = UnityEngine.Random.Range(5,MaxRoomLength);
            var height = UnityEngine.Random.Range(5,MaxRoomHeight);
            Debug.Log($"Create Rooms currentSpawnPoint = X:{currentSpawnPoint[0]} Y:{currentSpawnPoint[1]}::: nextSpawnPoint = X:{nextSpawnPoint[0]} Y:{nextSpawnPoint[1]}");

            // Using default start point
            if (i == 0)
            {   
                Debug.Log($"Hit first loop {i}");
                CreateRoom(currentSpawnPoint,length,height);
            }
            else
            {
                Debug.Log($"Hit {i} Loop");
                currentSpawnPoint = nextSpawnPoint;
                Debug.Log($"Create Rooms currentSpawnPoint = X:{currentSpawnPoint[0]} Y:{currentSpawnPoint[1]}::: nextSpawnPoint = X:{nextSpawnPoint[0]} Y:{nextSpawnPoint[1]}");
                SetNextSpawnPoint(currentSpawnPoint,length,height);
                CreateRoom(nextSpawnPoint,length,height);
            }
        }
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

        // If the position a wall tile return true 
        try
        {
             if (map[X,Y] == 1)
             {
                 return true;
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
                    if (CheckIfSpawnPointIsValid(i,j) && map[i,j] != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
    }
    
    void SetNextSpawnPoint(int[] currentSpawnPoint, int newRoomLength, int newRoomHeight)
    {
        int X = currentSpawnPoint[0];
        int Y = currentSpawnPoint[1];

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
                nextSpawnPoint = new int[2]{RightSpawn,Y};
                goto End;
            }            
        }
        // Check Up
        if (CheckIfSpawnPointIsValid(X, UpSpawn))
        {
            if (CheckIfNewRoomIsValid(new int[2]{X,UpSpawn},newRoomLength,newRoomHeight))
            {
                nextSpawnPoint = new int[2]{X,UpSpawn};
                goto End;
            }            
        }
        // Check Left
        if (CheckIfSpawnPointIsValid(LeftSpawn, Y))
        {
            if (CheckIfNewRoomIsValid(new int[2]{LeftSpawn,Y},newRoomLength,newRoomHeight))
            {
                nextSpawnPoint = new int[2]{LeftSpawn,Y};
                goto End;
            }            
        }

        End:
        // currentSpawnPoint = nextSpawnPoint;
        Debug.Log($"HitEnd! currentSpawnPOint = X:{currentSpawnPoint[0]} Y:{currentSpawnPoint[1]}...nextSpawnPoint = X:{nextSpawnPoint[0]} Y:{nextSpawnPoint[1]}");
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
        Debug.Log("Room added to rooms list");
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