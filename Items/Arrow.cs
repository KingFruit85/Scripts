using UnityEngine;

public class Arrow : MonoBehaviour
{
    private int damage = 10;
    private int arrowSpeed = 10;

    public Vector3 clickPoint;
    public Vector3 aim;
    private Vector2 lastVelocity;

    private Rigidbody2D rb;

    void Start()
    {
        aim = (clickPoint - transform.position).normalized;
        //Points the arrow the direction we're shooting
        float angle = Mathf.Atan2(aim.y,aim.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0,0,angle));

        rb = GetComponent<Rigidbody2D>();
    }

    public void SetArrowDamage(int dmg)
    {
        damage = dmg;
    }

    void FixedUpdate()
    {
        //Adds force in the direction of the mouseclick to the arrow
        transform.position += aim * arrowSpeed * Time.deltaTime;

        lastVelocity = rb.velocity;
    }

    void FinalUpdate()
    {
        if (rb.velocity.x == 0 && rb.velocity.y == 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {

        if (coll.collider.gameObject.layer == LayerMask.NameToLayer("Items"))
        {
            return;
        }

        else if (coll.collider.gameObject.layer == LayerMask.NameToLayer("enemies"))
        {
            coll.gameObject.GetComponent<Health>().TakeDamage(damage, transform.parent.gameObject);
            Destroy(gameObject);
        }
        
        else if (coll.collider.gameObject.tag == "TrapArrow")
        {
            var speed = lastVelocity.magnitude;
            var direction = Vector3.Reflect(lastVelocity.normalized,coll.contacts[0].normal);
            rb.velocity = direction * speed / 2;
        }
        
        else if (coll.gameObject.tag == "Wall")
        {
            //maybe have the arrow get imbedded in the wal?
            Destroy(gameObject);
        }


    }
}
