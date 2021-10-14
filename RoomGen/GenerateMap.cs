using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{

    public int DHeight = 50;
    public int DLength = 50;
    public int X = 0;
    public int Y = 0;
    public char[,] map;
    public GameObject TemplateRoom;

    void Start()
    {

        map = new char[DLength,DHeight];
        ResetMap('X', map);
        X = DLength / 2;
        Y = DHeight / 2;
        // map[X,Y] = '0'; // Used for random walk algo
        bool spawnPlayer = true;

        for (int walk = 1; walk <= 5000; walk++)
        {
            // Deciding if we'r moving on the x or y axis
            var moveDirection = (UnityEngine.Random.Range(0,2) == 0) ? "x":"y";

            // Decide if we're moving forwards or backwards
            // 0 = Backwards
            // 1 = Forwards
            var forwardOrBackward = (UnityEngine.Random.Range(0,2) == 0) ? 0 : 1;

            // Check if this direction is valid and moves if it is
            bool canMoveUp = true;
            bool canMoveDown = true;
            bool canMoveLeft = true;
            bool canMoveRight = true;

            if (moveDirection == "x" && forwardOrBackward == 0)
            {
                if (CheckValid(X - 1,Y,map))
                {
                    X--;
                    map[X,Y] = '0';
                    continue;
                }
                else
                {
                    canMoveLeft = false;
                    goto CheckOtherDirections;
                }
            }
            if (moveDirection == "x" && forwardOrBackward == 1)
            {
                if (CheckValid(X + 1, Y, map))
                {
                    X++;
                    map[X,Y] = '0';
                    continue;
                }    
                else
                {
                    canMoveRight = false;
                    goto CheckOtherDirections;
                }                
            }
            if (moveDirection == "y" && forwardOrBackward == 0)
            {
                if (CheckValid(X, Y - 1, map))
                {
                    Y--;
                    map[X,Y] = '0';
                    continue;
                }
                else
                {
                    canMoveDown = false;
                    goto CheckOtherDirections;
                }
            }
            if (moveDirection == "y" && forwardOrBackward == 1)
            {
                if (CheckValid(X, Y + 1, map))
                {
                    Y++;
                    map[X,Y] = '0';
                    continue;
                }
                else
                {
                    canMoveUp = false;
                    goto CheckOtherDirections;
                }
            }

            CheckOtherDirections:
            // If it's not then the other 3 options
            if (canMoveUp && CheckValid(X,Y+1,map))
            {
                Y++;
                map[X,Y] = '0';
                continue;
            }
            else
            {
                    canMoveUp = false;
            }

            if (canMoveDown && CheckValid(X,Y-1,map))
            {
                Y--;
                map[X,Y] = '0';
                continue;
            }
            else
            {
                canMoveDown = false;
            }

            if (canMoveLeft && CheckValid(X-1,Y,map))
            {
                X--;
                map[X,Y] = '0';
                continue;
            }
            else
            {
                canMoveLeft = false;
            }

            if (canMoveRight && CheckValid(X+1,Y,map))
            {
                X++;
                map[X,Y] = '0';
                continue;
            }
            else
            {
                canMoveRight = false;
            }

            if (!canMoveUp && !canMoveDown && !canMoveLeft && !canMoveRight)
            {
                map[X,Y] = 'E';

                X = UnityEngine.Random.Range(1,DLength -1);
                Y = UnityEngine.Random.Range(1,DHeight -1);

                if(CheckValid(X,Y,map))
                {
                    continue;
                }
                else
                {
                    goto End;
                }
            }
        }

        End:

        for (int i = 0; i <= map.GetUpperBound(0); i++)  
            {  
                for (int j = 0; j <= map.GetUpperBound(1); j++)  
                {  
                    if (map[i,j] == 'E')
                    {
                        // Pick a direction and check if we encounter at least 2 wall tiles
                        TryCreateValidPath(map, 0, i, j);
                        TryCreateValidPath(map, 1, i, j);
                        TryCreateValidPath(map, 2, i, j);
                        TryCreateValidPath(map, 3, i, j);
                    }  
                }  
            }

        for (int i = 0; i <= map.GetUpperBound(0); i++)  
        {  
            for (int j = 0; j <= map.GetUpperBound(1); j++)  
            {  
                if (i == 0 || i == DHeight-1)
                {
                    map[i,j] = 'E';
                }
                if (j == 0 || j == DLength -1)
                {
                    map[i,j] = 'E';
                }
            }  
        }    

        // Clear out single blocks
            for (int i = 0; i <= map.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= map.GetUpperBound(1); j++)
                {
                    if (map[i, j] == 'X')
                    {   // If all adjacent squares are floor tiles then remove wall
                        if (
                               CheckForWalls(i-1, j+1, map) &&
                               CheckForWalls(i, j+1, map) &&
                               CheckForWalls(i+1, j+1, map) &&
                               CheckForWalls(i-1, j, map) &&
                               CheckForWalls(i+1, j, map) &&
                               CheckForWalls(i-1, j-1, map) &&
                               CheckForWalls(i, j-1, map) &&
                               CheckForWalls(i+1, j-1, map)
                            )
                        {
                            if (   
                                    map[i - 1, j + 1] == '0'
                                    && map[i, j + 1] == '0'
                                    && map[i + 1, j + 1] == '0'
                                    && map[i - 1, j] == '0'
                                    && map[i + 1, j] == '0'
                                    && map[i - 1, j - 1] == '0'
                                    && map[i, j - 1] == '0'
                                    && map[i + 1, j - 1] == '0'
                                )
                            {
                                map[i, j] = '0';
                            }
                            
                        }
                    }
                }
            }
            ///
            CarveOut10by10Rooms(map,4);

            ///
        
        for (int i = 0; i <= map.GetUpperBound(0); i++)  
        {  
            for (int j = 0; j <= map.GetUpperBound(1); j++)  
            {   
                // Floor
                if (map[i,j] == '0')
                {
                    GameObject x = Instantiate(Resources.Load("GenericFloor"), new Vector2(i,j),Quaternion.identity) as GameObject;
                    x.transform.parent = GameObject.Find("FloorTiles").transform;

                    if (spawnPlayer)
                    {
                        GameObject player = Instantiate(Resources.Load("Player Variant 1"), x.transform.position,Quaternion.identity) as GameObject;
                        spawnPlayer = false;
                    }
                }
                // Wall
                else if (map[i,j] == 'X')
                {
                    GameObject x = Instantiate(Resources.Load("GenericWall"), new Vector2(i,j),Quaternion.identity) as GameObject;
                    x.GetComponent<Wall>().topLeftCorner = true;
                    x.transform.parent = GameObject.Find("WallTiles").transform;
                }

                // Map borders
                else if (map[i,j] == 'E')
                {
                    GameObject x = Instantiate(Resources.Load("GenericWall"), new Vector2(i,j),Quaternion.identity) as GameObject;
                    x.transform.parent = GameObject.Find("WallTiles").transform;
                }

                else if (map[i,j] == 'C')
                {
                    GameObject x = Instantiate(TemplateRoom, new Vector2(i - .5f,j- .5f),Quaternion.identity) as GameObject;

                    x.GetComponent<AddRoom>().OpenAllDoors(true);
                }

                
            }  
    }


}

private void TryCreateValidPath(char[,] map, int direction, int X, int Y)
        {
            // 0 = UP
            // 1 = DOWN
            // 2 = LEFT
            // 3 = RIGHT

            int counter = 0;
            int secondCounter = 0;

            switch (direction)
            {
                case 0:
                    try
                        {
                            for (int i = 0; i <= DHeight; i++)  
                            {  
                                if (counter == 2)
                                {
                                    for (int j = 0; j <= 100; j++)
                                    {
                                        if (map[X-j,Y] == 'X')
                                        {
                                            secondCounter++;
                                        }
                                        if (secondCounter <= 2)
                                        {
                                        map[X-j,Y] = '0';  
                                        }
                                    }
                                }
                                if (map[X-i,Y] == 'X')
                                {
                                    counter++;
                                }
                            }
                        }
                        catch (System.IndexOutOfRangeException)
                        {
                        }break;
                case 1:
                    try
                    {
                        for (int i = 0; i <= 100; i++)  
                        {  
                            if (counter == 2)
                            {
                                for (int j = 0; j <= 100; j++)
                                {
                                    if (map[X+j,Y] == 'X')
                                    {
                                        secondCounter++;
                                    }
                                    if (secondCounter <= 2)
                                    {
                                    map[X+j,Y] = '0';  
                                    }
                                }
                            }
                            if (map[X+i,Y] == 'X')
                            {
                                counter++;
                            }
                        }
                    }
                    catch (System.IndexOutOfRangeException)
                    {
                    }break;
                
                case 2:
                    try
                    {
                        for (int i = 0; i <= 100; i++)  
                        {  
                            if (counter == 2)
                            {
                                for (int j = 0; j <= 100; j++)
                                {
                                    if (map[X,Y-j] == 'X')
                                    {
                                        secondCounter++;
                                    }
                                    if (secondCounter <= 2)
                                    {
                                    map[X,Y-j] = '0';  
                                    }
                                }
                            }
                            if (map[X,Y-i] == 'X')
                            {
                                counter++;
                            }
                        }
                    }
                    catch (System.IndexOutOfRangeException)
                    {
                    }break;

                    case 3:
                    try
                    {
                        for (int i = 0; i <= 100; i++)  
                        {  
                            if (counter == 2)
                            {
                                for (int j = 0; j <= 100; j++)
                                {
                                    if (map[X,Y+j] == 'X')
                                    {
                                        secondCounter++;
                                    }
                                    if (secondCounter <= 2)
                                    {
                                    map[X,Y+j] = '0';  
                                    }
                                }
                            }
                            if (map[X,Y+i] == 'X')
                            {
                                counter++;
                            }
                        }
                    }
                    catch (System.IndexOutOfRangeException)
                    {
                    }break;

            }
        }

    private void ResetMap(char c, char[,] map)
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                map[i, j] = c;
            }
        }
    }

    private bool CheckValid(int x, int y, char[,] map)
    {
        // A negative number would be outside the bounds of the array
        if (x < 0 && y < 0)
        {
            return false;
        }

        // Return false if axis values are larger then the dimentions of the given array
        if (x > map.GetLength(0) || y > map.GetLength(1))
        {   
            return false;
        }

        // Returns false if were at the edge of the array
        if (x == map.GetLength(0) && y == map.GetLength(1))
        {
            return false;
        }

        try
        {
            // If position is valid and is not currently set to anything, return true
            if (map[x, y] == '0')
            {
                return false;
            }
        }
        catch (System.IndexOutOfRangeException)
        {
            return false;
        }
        
        // If position is valid and is not currently set to anything, return true
        if (map[x, y] == 'X')
        {
            return true;
        }

        return false;
    }

    private void CarveOut10by10Rooms(char[,] map, int roomsToSpawn)
    {
        

        for (int i = 1; i <= roomsToSpawn; i++)
        {
            Pick:
            //pick an X/Y coord
            X = UnityEngine.Random.Range(1,DLength);
            Y = UnityEngine.Random.Range(1,DHeight);
            if (CheckValid((X + 10),Y,map) && CheckValid(X,(Y + 10),map))
            {
                for (int y = 0; y <= 9; y++)
                {
                    for (int x = 0; x <= 9; x++)
                    {
                        map[(X + x),(Y + y)] = '0';
                    }
                }
            }
            else
            {
                goto Pick;
            }
            map[(X +5),(Y+5)] = 'C'; 

            Tunnel(X, Y, "Left", 20, map);
            Tunnel(X, Y, "Right", 20, map);
            Tunnel(X, Y, "Up", 20, map);
            Tunnel(X, Y, "Down", 20, map);

            X = UnityEngine.Random.Range(1,DLength);
            Y = UnityEngine.Random.Range(1,DHeight);

        }
    }

    private void Tunnel(int x, int y, string direction, int howFarToTunnel, char [,] map)
    {
        switch (direction)
        {
            case "Left":
                var LX = 1;

                for (int i = 0; i <= howFarToTunnel; i++)
                {
                    if (CheckValid((x-LX),(y+4),map))
                    {
                        map[(x-LX),(y+4)] = '0';
                        map[(x-LX),(y+5)] = '0';
                        LX ++;
                    }
                }break;

            case "Right":
                var RX = 10;

                for (int i = 0; i <= (RX + howFarToTunnel); i++)
                {
                    if (CheckValid((x+RX),(y+4),map))
                    {
                        map[(x+RX),(y+4)] = '0';
                        map[(x+RX),(y+5)] = '0';
                        RX ++;
                    }
                }break;

            case "Up":
                var UX = 10;

                for (int i = 0; i <= (UX + howFarToTunnel); i++)
                {
                    if (CheckValid((x+4),(y+UX),map))
                    {
                        map[(x+4),(y+UX)] = '0';
                        map[(x+5),(y+UX)] = '0';
                        UX ++;
                    }
                }break;

            case "Down":
                var DX = 1;

                for (int i = 0; i <= (DX + howFarToTunnel); i++)
                {
                    if (CheckValid((x+4),(y-DX),map))
                    {
                        map[(x+4),(y-DX)] = '0';
                        map[(x+5),(y-DX)] = '0';
                        DX ++;
                    }
                }break;
        }

    }

    private bool CheckForWalls(int x, int y, char[,] map)
    {
        // A negative number would be outside the bounds of the array
        if (x < 0 && y < 0)
        {
            return false;
        }

        // Return false if axis values are larger then the dimentions of the given array
        if (x >= map.GetLength(0) && y >= map.GetLength(1))
        {
            return false;
        }

        try
        {
            // If position is valid and is a wall return false
            if (map[x, y] == 'X')
            {
                return false;
            }
        }
        catch (System.IndexOutOfRangeException)
        {
            return false;
        }
        
        try
        {
            // If position is a floor tile
            if (map[x, y] == '0')
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
    
}
