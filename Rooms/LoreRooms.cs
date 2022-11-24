using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreRooms : MonoBehaviour
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
        var spawnLocations = gameObject.transform.GetComponent<SimpleRoom>().SpawnableFloorTiles;
        var r = UnityEngine.Random.Range(0, spawnLocations.Length);

        GameObject terminal = Instantiate(Resources.Load("InteractableRune1"), spawnLocations[r].transform.position, Quaternion.identity) as GameObject;
        terminal.transform.parent = gameObject.transform;
    }

}
