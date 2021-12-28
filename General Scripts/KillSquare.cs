using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSquare : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // Player walks on tile
        if (other.tag == "Player")
        {
            other.GetComponent<Health>().TakeDamage(1000,new GameObject(),"debug",false);
            GameObject.Find("GameManager").GetComponent<GameManager>().Restart();
        }
    }
}
