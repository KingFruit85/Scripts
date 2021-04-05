using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb;
    private int damage = 10;
    private int arrowSpeed = 10;

    public Vector3 clickPoint;
    public Vector3 aim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        aim = (clickPoint - transform.position).normalized;
        Debug.Log("Aim: "+ aim);
        Debug.Log("MouseClick " + clickPoint);
        // transform.LookAt(clickPoint);
        rb.AddForce(aim * arrowSpeed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("enemies"))
        {
            other.GetComponent<Health>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Items"))
        {
            return;
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            return;
        }
        else
        {
            Destroy(gameObject);
        }  
    }
}
