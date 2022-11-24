using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOfDeathRoom : MonoBehaviour
{
    public enum CardinalDirections
    {
        North,
        East,
        South,
        West
    }
    public GameObject[] floorTiles;
    public List<List<GameObject>> VerticalRows;
    public List<List<GameObject>> HorizontalRows;
    public int currentRow = 0;
    public int safeTile = 0;
    public int ogSafeTile = 0;
    public int currentDirection = 0;

    void Start()
    {

        safeTile = UnityEngine.Random.Range(1, 8);
        ogSafeTile = safeTile;
        floorTiles = GetComponent<SimpleRoom>().floorTiles;
        VerticalRows = new List<List<GameObject>>()
        {
            new List<GameObject>(),
            new List<GameObject>(),
            new List<GameObject>(),
            new List<GameObject>(),
            new List<GameObject>(),
            new List<GameObject>(),
            new List<GameObject>(),
            new List<GameObject>(),
        };

        HorizontalRows = new List<List<GameObject>>()
        {
            new List<GameObject>(),
            new List<GameObject>(),
            new List<GameObject>(),
            new List<GameObject>(),
            new List<GameObject>(),
            new List<GameObject>(),
            new List<GameObject>(),
            new List<GameObject>(),
        };

        foreach (var tile in floorTiles)
        {
            int value;
            Int32.TryParse(tile.name, out value);

            if (value >= 1 && value <= 8) VerticalRows[0].Add(tile);
            if (value >= 9 && value <= 16) VerticalRows[1].Add(tile);
            if (value >= 17 && value <= 24) VerticalRows[2].Add(tile);
            if (value >= 25 && value <= 32) VerticalRows[3].Add(tile);
            if (value >= 33 && value <= 40) VerticalRows[4].Add(tile);
            if (value >= 41 && value <= 48) VerticalRows[5].Add(tile);
            if (value >= 49 && value <= 56) VerticalRows[6].Add(tile);
            if (value >= 57 && value <= 64) VerticalRows[7].Add(tile);
        }
        foreach (var tile in floorTiles)
        {
            int value;
            Int32.TryParse(tile.name, out value);

            if (value == 1 || value == 9 || value == 17 || value == 25 || value == 33 || value == 41 || value == 49 || value == 57)
            {
                HorizontalRows[0].Add(tile);
            }
            if (value == 2 || value == 10 || value == 18 || value == 26 || value == 34 || value == 42 || value == 50 || value == 58)
            {
                HorizontalRows[1].Add(tile);
            }
            if (value == 3 || value == 11 || value == 19 || value == 27 || value == 35 || value == 43 || value == 51 || value == 59)
            {
                HorizontalRows[2].Add(tile);
            }
            if (value == 4 || value == 12 || value == 20 || value == 28 || value == 36 || value == 44 || value == 52 || value == 60)
            {
                HorizontalRows[3].Add(tile);
            }
            if (value == 5 || value == 13 || value == 21 || value == 29 || value == 37 || value == 45 || value == 53 || value == 61)
            {
                HorizontalRows[4].Add(tile);
            }
            if (value == 6 || value == 14 || value == 22 || value == 30 || value == 38 || value == 46 || value == 54 || value == 62)
            {
                HorizontalRows[5].Add(tile);
            }
            if (value == 7 || value == 15 || value == 23 || value == 31 || value == 39 || value == 47 || value == 55 || value == 63)
            {
                HorizontalRows[6].Add(tile);
            }
            if (value == 8 || value == 16 || value == 24 || value == 32 || value == 40 || value == 48 || value == 56 || value == 64)
            {
                HorizontalRows[7].Add(tile);
            }
        }

        StartCoroutine(Pulse(VerticalRows[currentRow], safeTile));
    }

    IEnumerator Pulse(List<GameObject> tiles, int safeTile)
    {
        foreach (var tile in tiles)
        {
            // Tiles are named 1,2,3 etc in left to right, top to bottom order
            if (currentDirection == 0 || currentDirection == 1)
            {
                if (Int32.Parse(tile.name) == safeTile) continue;
                tile.GetComponent<FloorTile>().StartPulsingTiles();
            }

            if (currentDirection == 2 || currentDirection == 3)
            {
                var convertedSafeTile = HorizontalRows[currentRow][safeTile].name;
                if (tile.name == convertedSafeTile) continue;
                tile.GetComponent<FloorTile>().StartPulsingTiles();
            }

        }

        yield return new WaitForSeconds(0.3f);

        foreach (var tile in tiles)
        {
            tile.GetComponent<FloorTile>().StopPulsingTiles();
        }

        switch (currentDirection)
        {
            case 0: // SOUTH
                if (currentRow == 7)
                {
                    currentDirection = 1;
                    safeTile -= 8;
                    currentRow--;
                    StartCoroutine(Pulse(VerticalRows[currentRow], safeTile));
                }
                else
                {
                    currentRow++;
                    safeTile += 8;
                    StartCoroutine(Pulse(VerticalRows[currentRow], safeTile));
                }
                break;
            case 1: // NORTH
                if (currentRow == 0)
                {
                    currentDirection = 2;
                    safeTile = ogSafeTile;
                    StartCoroutine(Pulse(HorizontalRows[currentRow], safeTile));
                }
                else
                {
                    currentRow--;
                    safeTile -= 8;
                    StartCoroutine(Pulse(VerticalRows[currentRow], safeTile));
                }
                break;
            case 2: // WEST
                if (currentRow == 7)
                {
                    currentRow--;
                    currentDirection = 3;
                    safeTile = ogSafeTile;
                    StartCoroutine(Pulse(HorizontalRows[currentRow], safeTile));
                }
                else
                {
                    currentRow++;
                    StartCoroutine(Pulse(HorizontalRows[currentRow], safeTile));
                }
                break;
            case 3: // EAST
                if (currentRow == 0)
                {
                    currentDirection = 0;
                    safeTile = ogSafeTile;
                    StartCoroutine(Pulse(VerticalRows[currentRow], safeTile));
                }
                else
                {
                    currentRow--;
                    StartCoroutine(Pulse(HorizontalRows[currentRow], safeTile));
                }
                break;
        }


    }

    void MoveRightToLeft()
    {

    }


}
