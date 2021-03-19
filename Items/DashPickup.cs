using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPickup : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameObject.Find("Player").GetComponent<Dash>().enabled = true;
            Destroy(gameObject,0.2f);
        }
    }
}
