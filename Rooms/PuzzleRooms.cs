using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRooms : MonoBehaviour
{
    public SimpleRoom room;
    public DoorController doorController;
    public EnemySpawner enemySpawner;
    void Start()
    {
        gameObject.AddComponent<EnemySpawner>();
        enemySpawner = GetComponent<EnemySpawner>();
        enemySpawner.SetEnemyCount(UnityEngine.Random.Range(0, 3));

        room = GetComponent<SimpleRoom>();
        doorController = gameObject.transform.Find("DoorController").GetComponent<DoorController>();
        var _newRoom = gameObject.transform;

        doorController.OpenByMobDeath = false;
        doorController.OpenByPuzzleComplete = true;

        var runeTiles = room.runeTiles;
        var pillarTiles = room.pillarTiles;
        var chestTile = room.puzzleChestSpawnLocation;
        // Just one type of puzzle room right now

        // Spawn puzzle pieces
        // -Chest
        var adjustedChestPosition = new Vector3((0.5f + chestTile.transform.position.x), (-0.5f + chestTile.transform.position.y), 0);
        GameObject chest = Instantiate(Resources.Load("chest") as GameObject, adjustedChestPosition, Quaternion.identity);
        chest.transform.parent = _newRoom.transform.Find("Tiles");

        // Barrier
        GameObject barrier = Instantiate(Resources.Load("verticalBars") as GameObject, adjustedChestPosition, Quaternion.identity);
        barrier.transform.parent = _newRoom.transform.Find("Tiles");
        barrier.GetComponent<Barrier>().SetBarrierUnlockMethod(1);

        // Pillars
        foreach (var tile in pillarTiles)
        {
            GameObject wall = Instantiate(Resources.Load("Wall") as GameObject, tile.transform.position, Quaternion.identity) as GameObject;
            wall.transform.parent = _newRoom.transform.Find("Tiles");
            // Flame bowls and arrow traps
            GameObject flameBowl = Instantiate(Resources.Load("FlameBowl") as GameObject, tile.transform.position, Quaternion.identity) as GameObject;
            flameBowl.transform.parent = _newRoom.transform.Find("Tiles");

            if (flameBowl.transform.localPosition == new Vector3(-0.25f, 0.25f, 0))
            {
                flameBowl.name = "flameBowl1";
                GameObject arrowTrap = Instantiate(Resources.Load("arrowTrap") as GameObject, _newRoom.GetComponent<SimpleRoom>().arrowTrap1Position.transform.position, Quaternion.identity) as GameObject;
                arrowTrap.transform.parent = _newRoom.transform.Find("Tiles");
                arrowTrap.name = "arrowTrap1";
                arrowTrap.GetComponent<ArrowTrap>().shootRight = true;

                arrowTrap.transform.rotation *= Quaternion.AngleAxis(90, transform.forward);
                arrowTrap.transform.position += new Vector3(0.4f, 0f, 0f);

            }

            if (flameBowl.transform.localPosition == new Vector3(0.25f, 0.25f, 0))
            {
                flameBowl.name = "flameBowl2";
                GameObject arrowTrap = Instantiate(Resources.Load("arrowTrap") as GameObject, _newRoom.GetComponent<SimpleRoom>().arrowTrap2Position.transform.position, Quaternion.identity) as GameObject;
                arrowTrap.transform.parent = _newRoom.transform.Find("Tiles");
                arrowTrap.name = "arrowTrap2";
                arrowTrap.GetComponent<ArrowTrap>().shootLeft = true;

                arrowTrap.transform.rotation *= Quaternion.AngleAxis(-90, transform.forward);
                arrowTrap.transform.position += new Vector3(-0.4f, 0f, 0f);
            }

            if (flameBowl.transform.localPosition == new Vector3(-0.25f, -0.15f, 0))
            {
                flameBowl.name = "flameBowl3";
                GameObject arrowTrap = Instantiate(Resources.Load("arrowTrap") as GameObject, _newRoom.GetComponent<SimpleRoom>().arrowTrap3Position.transform.position, Quaternion.identity) as GameObject;
                arrowTrap.transform.parent = _newRoom.transform.Find("Tiles");
                arrowTrap.name = "arrowTrap3";
                arrowTrap.GetComponent<ArrowTrap>().shootRight = true;

                arrowTrap.transform.rotation *= Quaternion.AngleAxis(90, transform.forward);
                arrowTrap.transform.position += new Vector3(0.4f, 0f, 0f);
            }


            if (flameBowl.transform.localPosition == new Vector3(0.25f, -0.15f, 0))
            {
                flameBowl.name = "flameBowl4";
                GameObject arrowTrap = Instantiate(Resources.Load("arrowTrap") as GameObject, _newRoom.GetComponent<SimpleRoom>().arrowTrap4Position.transform.position, Quaternion.identity) as GameObject;
                arrowTrap.transform.parent = _newRoom.transform.Find("Tiles");
                arrowTrap.name = "arrowTrap4";
                arrowTrap.GetComponent<ArrowTrap>().shootLeft = true;

                arrowTrap.transform.rotation *= Quaternion.AngleAxis(-90, transform.forward);
                arrowTrap.transform.position += new Vector3(-0.4f, 0f, 0f);
            }


            room.AddItemToRoomContents(tile.transform.localPosition, 'P');
        }

        // -Runes - randomised placement
        List<GameObject> runes = new List<GameObject>()
            {
                Resources.Load("BlueTile") as GameObject,
                Resources.Load("GreenTile") as GameObject,
                Resources.Load("RedTile") as GameObject,
                Resources.Load("TealTile") as GameObject
            };

        foreach (var rune in runeTiles)
        {
            System.Random rnd = new System.Random();
            int r = rnd.Next(runes.Count);

            var randomRune = runes[r];
            GameObject _rune = Instantiate(randomRune, rune.transform.position, Quaternion.identity) as GameObject;
            _rune.GetComponent<Rune>().myUnlock = barrier;
            _rune.transform.parent = _newRoom.transform.Find("Tiles");
            room.AddItemToRoomContents(rune.transform.localPosition, 'R');
            runes.RemoveAt(r);

            // How to link the runes to the flamebowls????
            if (_rune.transform.localPosition == new Vector3(-0.25f, 0.15f, 0))
            {
                _rune.GetComponent<Rune>().flameBowl = _rune.transform.parent.Find("flameBowl1").GetComponent<FlameBowl>();
                _rune.GetComponent<Rune>().myTrap = _rune.transform.parent.Find("arrowTrap1").gameObject;
            }

            if (_rune.transform.localPosition == new Vector3(0.25f, 0.15f, 0))
            {
                _rune.GetComponent<Rune>().flameBowl = _rune.transform.parent.Find("flameBowl2").GetComponent<FlameBowl>();
                _rune.GetComponent<Rune>().myTrap = _rune.transform.parent.Find("arrowTrap2").gameObject;
            }

            if (_rune.transform.localPosition == new Vector3(-0.25f, -0.25f, 0))
            {
                _rune.GetComponent<Rune>().flameBowl = _rune.transform.parent.Find("flameBowl3").GetComponent<FlameBowl>();
                _rune.GetComponent<Rune>().myTrap = _rune.transform.parent.Find("arrowTrap3").gameObject;
            }

            if (_rune.transform.localPosition == new Vector3(0.25f, -0.25f, 0))
            {
                _rune.GetComponent<Rune>().flameBowl = _rune.transform.parent.Find("flameBowl4").GetComponent<FlameBowl>();
                _rune.GetComponent<Rune>().myTrap = _rune.transform.parent.Find("arrowTrap4").gameObject;
            }

        }


        barrier.GetComponent<Barrier>().redRune = _newRoom.transform.Find("Tiles").Find("RedTile(Clone)").gameObject;
        barrier.GetComponent<Barrier>().blueRune = _newRoom.transform.Find("Tiles").Find("BlueTile(Clone)").gameObject;
        barrier.GetComponent<Barrier>().greenRune = _newRoom.transform.Find("Tiles").Find("GreenTile(Clone)").gameObject;
        barrier.GetComponent<Barrier>().tealRune = _newRoom.transform.Find("Tiles").Find("TealTile(Clone)").gameObject;

    }

}
