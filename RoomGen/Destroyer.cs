using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{

    private GameManager gameManager;
    public int myIndex;

    public float waitTime = 20f;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (gameManager.spawnFog)
        {
            Instantiate(Resources.Load("RoomFog"),transform.position,Quaternion.identity);
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {   
        //Stops rooms spawning over the start room, only destroys objects on the rooms layer.
        if (other.gameObject.layer == 16)
        {
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "SpawnPoint")
        {
            Destroy(other.gameObject);
        }
        // Should stop rooms spawning on top of each other
        if (other.gameObject.tag == "Destroyer")
        {
            var otherIndex = other.GetComponent<Destroyer>().myIndex;

            if (myIndex > otherIndex)
            {
                Destroy(transform.parent.parent.gameObject);
            }
            else
            {
                Destroy(other.transform.parent.parent.gameObject);
            }
        }
    }
}
