using UnityEngine;

public class GoldArrow : MonoBehaviour
{
    private Rigidbody2D RB;

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        var arrowSpeed = GetComponentInParent<GoldBowPickup>().speed;

        var playerIsLooking = GameObject.Find("Player")
                               .GetComponent<PlayerMovement>()
                               .PlayerIsLooking();
        
        switch (playerIsLooking)
        {
            default: throw new System.Exception("PlayerMovement.Looking state unknown");

            case PlayerMovement.Looking.Left:
                RB.AddForce(-Vector3.right * arrowSpeed,ForceMode2D.Impulse);
                break;

            case PlayerMovement.Looking.Right: 
                RB.AddForce(Vector3.right * arrowSpeed,ForceMode2D.Impulse);
                break;

            case PlayerMovement.Looking.Up: 
                RB.AddForce(Vector3.up * arrowSpeed,ForceMode2D.Impulse);
                break;

            case PlayerMovement.Looking.Down: 
                RB.AddForce(Vector3.down * arrowSpeed,ForceMode2D.Impulse);
                break; 
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Health health) && other.tag != "Player")
        {
            other.GetComponent<Health>().TakeDamage(GetComponentInParent<GoldBowPickup>().damage, transform.parent.gameObject,"GoldArrow", false);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Items"))
        {
            return;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
