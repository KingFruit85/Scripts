using System.Linq;
using UnityEngine;

public class StartRoom : MonoBehaviour
{
    public SimpleRoom room;
    public DoorController doorController;
    public GameManager gameManager;
    void Start()
    {
        room = GetComponent<SimpleRoom>();
        doorController = gameObject.transform.Find("DoorController").GetComponent<DoorController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        gameObject.transform.name += " START ROOM";

        var flameBowlTile = room.floorTiles
        .Where(t => t.name == "46")
        .FirstOrDefault();

        GameObject flamebowl = Instantiate(Resources.Load("FlameBowl"), flameBowlTile.transform.position, Quaternion.identity) as GameObject;
        flamebowl.GetComponent<FlameBowl>().startLit = true;

        flamebowl.GetComponent<FlameBowl>().Light();

        GameObject player = Instantiate(Resources.Load("Player Variant 1"), gameObject.transform.position, Quaternion.identity) as GameObject;
        player.name = "Player";
        var camera = GameObject.Find("Main Camera");
        camera.transform.position = gameObject.transform.position;

        doorController.roomComplete = true;

        // Debug: Spawn kill square 
        Instantiate(Resources.Load("KillSquare"), room.ExitTile.transform.position, Quaternion.identity);

        // Place level specific terminal with new lore
        if (gameManager.currentGameLevel == 1)
        {

            GameObject terminal = Instantiate(Resources.Load("InteractableRune1"), room.ReturnTerminalSpawnLocation(), Quaternion.identity) as GameObject;
            terminal.transform.parent = gameObject.transform.Find("Tiles");
            terminal.transform.position = gameObject.transform.Find("CameraAnchor").transform.position;

        }

    }
}
