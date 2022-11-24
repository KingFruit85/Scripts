using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRoom : MonoBehaviour
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
        var spawnLocations = room.SpawnableFloorTiles;
        var r = UnityEngine.Random.Range(0, spawnLocations.Length); // Get number of traps to place
        var trapLocations = new List<int>();

        var picked = UnityEngine.Random.Range(0, spawnLocations.Length);
        trapLocations.Add(picked); // add the first trap tile

        for (int i = 0; i <= (r - 1); i++)
        {
            var t = UnityEngine.Random.Range(0, spawnLocations.Length);
            if (!(t < (picked - 4)) || !(t > (picked + 4)))
            {
                trapLocations.Add(t);
            }
            else
            {
                i--;
            }
        }

        foreach (var location in trapLocations)
        {
            GameObject trapTile = Instantiate(Resources.Load("trapTile"), spawnLocations[location].transform.position, Quaternion.identity) as GameObject;
            trapTile.transform.parent = gameObject.transform.Find("Tiles");
            trapTile.name = $"TrapTile {location}";

            float lastDistance = 999999.00f;
            Vector3 tileToPlaceArrowTrap = new Vector3(69, 69, 69);

            // Find the closest wall tile
            foreach (var walltile in room.wallTiles)
            {
                var distanceBetweenTiles = Vector3.Distance(walltile.transform.position, trapTile.transform.position);
                if (distanceBetweenTiles <= lastDistance)
                {
                    tileToPlaceArrowTrap = walltile.transform.position;
                    lastDistance = distanceBetweenTiles;
                }
            }
            // Place the arrow trap on the closest tile
            GameObject arrowTrap = Instantiate(Resources.Load("arrowTrap"), tileToPlaceArrowTrap, Quaternion.identity) as GameObject;
            trapTile.GetComponent<TrapTile>().MyTrap = arrowTrap;
            arrowTrap.GetComponent<SpriteRenderer>().enabled = false;

            arrowTrap.transform.LookAt(tileToPlaceArrowTrap);
            arrowTrap.transform.parent = gameObject.transform;
        }
    }

}
