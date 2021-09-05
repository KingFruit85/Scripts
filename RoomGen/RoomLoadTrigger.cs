using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLoadTrigger : MonoBehaviour
{
    public GameObject LeftRoomSpawner;
    public GameObject RightRoomSpawner;
    public GameObject UpRoomSpawner;
    public GameObject DownRoomSpawner;

    // This should check if there is a room already spawned in the direction the player is exiting
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            switch (gameObject.name)
            {
                default: throw new System.Exception("unknown roomloadtrigger name");
                case "RightExit": 
                    if (RightRoomSpawner)
                    {
                        RightRoomSpawner.TryGetComponent(out RoomSpawner rs); 
                        rs.SpawnRoom("right");
                        Destroy(gameObject);
                    }
                    break;

                case "LeftExit": 

                    if (LeftRoomSpawner)
                    {
                        LeftRoomSpawner.TryGetComponent(out RoomSpawner ls); 
                        ls.SpawnRoom("left");
                        Destroy(gameObject);                        
                    }
                    break;

                case "UpExit": 
                    if (UpRoomSpawner)
                    {
                        UpRoomSpawner.TryGetComponent(out RoomSpawner us); 
                        us.SpawnRoom("up");
                        Destroy(gameObject);
                    }
                    break;

                case "DownExit":
                    if (DownRoomSpawner)
                    {
                        DownRoomSpawner.TryGetComponent(out RoomSpawner ds); 
                        ds.SpawnRoom("down");
                        Destroy(gameObject);
                    }
                    break;
            }
        }
    }
}
