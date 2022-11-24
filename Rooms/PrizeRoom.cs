using UnityEngine;

public class PrizeRoom : MonoBehaviour
{
    public SimpleRoom room;
    public DoorController doorController;
    public EnemySpawner enemySpawner;

    void Start()
    {
        room = GetComponent<SimpleRoom>();
        gameObject.AddComponent<EnemySpawner>();
        enemySpawner = GetComponent<EnemySpawner>();
        enemySpawner.SetEnemyCount(UnityEngine.Random.Range(3, 5));

        doorController = gameObject.transform.Find("DoorController").GetComponent<DoorController>();
        doorController.OpenByMobDeath = true;

        // Get the random floor tile to spawn a chest on
        var chestSpawnLocation = room.SpawnableFloorTiles[UnityEngine.Random.Range(0, room.SpawnableFloorTiles.Length)].transform;

        // Spawn a chest on a random tile
        GameObject bars = Instantiate(Resources.Load("verticalBars"), chestSpawnLocation.position, Quaternion.identity) as GameObject;
        bars.transform.parent = gameObject.transform.Find("Tiles");

        GameObject chest = Instantiate(Resources.Load("chest"), chestSpawnLocation.position, Quaternion.identity) as GameObject;
        chest.transform.parent = gameObject.transform.Find("Tiles");

        //Add to room contents array
        room.AddItemToRoomContents(chest.transform.localPosition, 'C');
    }
}
