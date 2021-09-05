using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyRoomDetector : MonoBehaviour
{
    public RoomTemplates templates;
    public bool canSpawn = false;
    public GameObject closedRooms;
    public bool isTouchingRoom;


    void Awake()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
    }
    void Update()
    {
        if(templates.startBuildingLevel == false)
        {
            canSpawn = true;
        }
    }

    // Check if object is touching nay layers/other game objects

   }
