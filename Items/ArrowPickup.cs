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
            GameObject.Find("GameManager").GetComponent<GameManager>().AddArrows(arrowCount);
            Destroy(gameObject);
            }
        }
    }
