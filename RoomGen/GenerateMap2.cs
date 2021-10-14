using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GenerateMap2 : MonoBehaviour
{
  
    private int MapHeight = 26;
    private int MapLength = 26;

    [SerializeField]
    private int[] currentSpawnPoint = new int[2] {1,1};

    // 0 = floor
    // 1 = wall
    public int[,] map;


    void Start()
    {
        map = new int[MapLength,MapHeight];
        // Fill out the 2D array with 1's
        InitializeMap();
        CreateMap();
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
            }
        }
    }

    void CreateMap()
    {
        for (int i = 0; i < 20; i++)
        {
            var X = UnityEngine.Random.Range(3,10);
            var Y = UnityEngine.Random.Range(3,5);
            CreateRoom(currentSpawnPoint,X,Y);
        }
    }
    
    
    
    
    
    bool CheckValid(int X, int Y)
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
    
    
    void CreateRoom(int[] startPoint, int length, int height)
    {
        var X = startPoint[0];
        var Y = startPoint[1];

        if (CheckValid(X,Y) && CheckValid((X + length + 1),(Y + length + 1)))
        {
            for (int i = X; i <= X + (length - 1); i++)
            {
                for (int j = Y; j <= Y + (height - 1); j++)
                {
                    map[i,j] = 0;
                }
            }
        }
        else
        {
            Debug.Log("Room position not valid");
        }

        // Check if we can place to the right
        if (CheckValid((X + length + 1) ,(Y)) && CheckValid((X + (length * 2) + 1) ,(Y)) )
        {   
            // Sets the new room spawn point
            currentSpawnPoint = new int[2]{(X + length + 1) ,Y};
        }

        // Check if we can place left
        else if (CheckValid((X - (length + 1)), Y ))
        {
           currentSpawnPoint = new int[2]{(X - (length + 1)) ,Y}; 
        }

        // Check if we can place up
        else if (CheckValid(X , Y + (height + 1)) && CheckValid(X, (Y + (height * 2) + 1)))
        {
           currentSpawnPoint = new int[2]{X ,(Y + length + 1)}; 
        }

        else
        {
            return;
        }

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
