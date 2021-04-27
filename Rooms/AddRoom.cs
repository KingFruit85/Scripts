using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    private RoomTemplates templates;
    public bool up,down,left,right;
    public bool isExitRoom;
    public bool exitSpawned;
    public GameObject exitPoint;

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        templates.rooms.Add(this.gameObject);
        transform.parent = GameObject.Find("Rooms").transform;

        isExitRoom = false;
        exitSpawned = false;
    }

    public void SpawnExit()
    {
        if (!exitSpawned)
        {
            Instantiate(Resources.Load("ExitSquare") as GameObject, new Vector3(exitPoint.transform.position.x,exitPoint.transform.position.y,0), Quaternion.identity);
            exitSpawned = true;
        }

    }
}
