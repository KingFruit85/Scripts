using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardRoom : MonoBehaviour
{
    public SimpleRoom room;
    public DoorController doorController;
    public EnemySpawner enemySpawner;

    void Start()
    {
        room = GetComponent<SimpleRoom>();
        gameObject.AddComponent<EnemySpawner>();

        enemySpawner = GetComponent<EnemySpawner>();
        enemySpawner.SetEnemyCount(UnityEngine.Random.Range(0, 3));


        doorController = gameObject.transform.Find("DoorController").GetComponent<DoorController>();

        doorController.OpenByMobDeath = true;
        // Get the floor tiles we can spawn objects on
        var floorTiles = room.SpawnableFloorTiles;
        // And a random number of walls we're going to spawn
        var wallTilesToSpawn = UnityEngine.Random.Range(0, floorTiles.Length);

        // Spawn the wall tiles
        for (int i = 0; i <= wallTilesToSpawn; i++)
        {
            GameObject wall = Instantiate(Resources.Load("Wall"), floorTiles[i].transform.position, Quaternion.identity) as GameObject;
            wall.transform.parent = gameObject.transform.Find("Tiles");
            room.spawnedWallTiles.Add(wall);
            //Add to room contents array
            room.AddItemToRoomContents(wall.transform.localPosition, '#');

            // Spawn arrow traps on valid wall locations
            List<Vector3> validTrapWalls = new List<Vector3>()
                {
                    new Vector3(0.15f,0.15f,0),
                    new Vector3(-0.15f,0.15f,0),
                    new Vector3(-0.15f,-0.15f,0),
                    new Vector3(0.15f,-0.15f,0)
                };

            var wallPOS = wall.transform.localPosition;
            GameObject arrowTrap;

            if (validTrapWalls.Contains(wallPOS))
            {
                arrowTrap = Instantiate(Resources.Load("arrowTrap"), wall.transform.localPosition, Quaternion.identity) as GameObject;
                arrowTrap.transform.parent = gameObject.transform.Find("Tiles");

                if (wallPOS == new Vector3(0.15f, 0.15f, 0))
                {
                    arrowTrap.transform.position = new Vector3(wall.transform.position.x,
                                                               (wall.transform.position.y - 0.4f),
                                                               0);
                    arrowTrap.GetComponent<ArrowTrap>().active = true;
                    arrowTrap.GetComponent<ArrowTrap>().shootDown = true;
                }
                if (wallPOS == new Vector3(-0.15f, 0.15f, 0))
                {
                    arrowTrap.transform.position = new Vector3(wall.transform.position.x,
                                                               (wall.transform.position.y - 0.4f),
                                                               0);
                    arrowTrap.GetComponent<ArrowTrap>().active = true;
                    arrowTrap.GetComponent<ArrowTrap>().shootDown = true;
                }
                if (wallPOS == new Vector3(-0.15f, -0.15f, 0))
                {
                    arrowTrap.transform.rotation *= Quaternion.AngleAxis(180, transform.right);
                    arrowTrap.transform.position = new Vector3(wall.transform.position.x,
                                                               (wall.transform.position.y + 0.4f),
                                                               0);

                    arrowTrap.GetComponent<ArrowTrap>().active = true;
                    arrowTrap.GetComponent<ArrowTrap>().shootUp = true;
                }
                if (wallPOS == new Vector3(0.15f, -0.15f, 0))
                {
                    arrowTrap.transform.rotation *= Quaternion.AngleAxis(180, transform.right);
                    arrowTrap.transform.position = new Vector3(wall.transform.position.x,
                                                               (wall.transform.position.y + 0.4f),
                                                               0);
                    arrowTrap.GetComponent<ArrowTrap>().active = true;
                    arrowTrap.GetComponent<ArrowTrap>().shootUp = true;
                }
            }
        }
    }

}
