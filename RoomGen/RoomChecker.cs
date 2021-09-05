using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChecker : MonoBehaviour
{
    public bool IsTouchingWall = false;
    public bool IsTouchingOpenDoorway = false;
    public bool Up;
    public bool Down;
    public bool Right;
    public bool Left;
    
    public string Name;

    void Awake()
    {
        if (Up)
        {
            Name = "up";
        }

        if (Down)
        {
            Name = "down";
        }

        if (Right)
        {
            Name = "right";
        }

        if (Left)
        {
            Name = "left";
        }
    }

    public string GetName()
    {
        return Name;
    }

    public string GetOpposite(string name)
    {
        switch (name)
        {
            default:
            case "up": return "down";
            case "down": return "up";
            case "left": return "right";
            case "right": return "left";
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "OpenDoorwayCollider")
        {
            IsTouchingOpenDoorway = true;
            GetComponentInParent<RoomSpawner>().AddRequiredDoor(Name);
            // Need to get the other rooms exit trigger and disable it, this should be the exit we are leading into
            var otherRoom = other.transform.parent.parent.gameObject;

            var triggersToBeDisabled = GetComponentInParent<RoomSpawner>().requiredRoomAndExitTriggerToBeDisabled;

            if (triggersToBeDisabled == null)
            {
                throw new ArgumentNullException(nameof(triggersToBeDisabled));
            }

            if (!triggersToBeDisabled.ContainsKey(otherRoom))
            {
                triggersToBeDisabled.Add(otherRoom,GetOpposite(Name));
            }




        }

        if (other.gameObject.tag == "Wall")
        {
            IsTouchingWall = true;
            GetComponentInParent<RoomSpawner>().AddBlockedDoor(Name);
            GetComponentInParent<RoomSpawner>().RemoveValidExit(Name);
        }
    }

}
