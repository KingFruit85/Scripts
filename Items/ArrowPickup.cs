using UnityEngine;

public class ArrowPickup : MonoBehaviour
{
    [SerializeField]
    private int arrowCount = 3;
    private bool canPickUp;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            var player = GameObject.Find("Player").GetComponent<PlayerStats>();
            player.AddArrows(arrowCount);
            Destroy(gameObject);
            }
        }
    }
