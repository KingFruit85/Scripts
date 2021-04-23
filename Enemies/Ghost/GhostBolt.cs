using UnityEngine;

public class GhostBolt : MonoBehaviour
{
    public float speed;
    public int damage = 10;
    private Rigidbody2D rb;
    private GameObject player;
    private Vector3 aim;
    private Vector3 playerMouseClick;

    private float born;
    private float lifeTime = 0.5f;

    public string shooter;
    private Vector3 lastVelocity;
    private bool deflected;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        playerMouseClick = player.GetComponent<PlayerCombat>().mouseClickPosition; 
        born = Time.time;
    }

    void Update()
    {
        shooter = transform.parent.tag;

        switch (shooter)
        {
            default:
            case "Ghost":ShootAtPlayer();break;
            case "Player":ShootAtEnemy();break;
        }

        if (Time.time >= born + lifeTime)
        {
            Destroy(this.gameObject);
        }

        // Tracked to calculate speed for deflections
        lastVelocity = rb.velocity;

        if (deflected)
        {
            damage = 0;
        }
    }

    private void ShootAtEnemy()
    {   
        aim = (playerMouseClick - transform.position);
        rb.AddForce(aim * speed);
    }

    private void ShootAtPlayer()
    {
        aim = (player.transform.position - transform.position).normalized;
        rb.AddForce(aim * speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if (other.tag == "Wall")
        // {
        //     //Add animation
        //     Destroy(this.gameObject);
        // }

        // PLayer logic
        // LayerMask mask = LayerMask.GetMask("enemies");

        // if (shooter == "Player" && other.gameObject.layer == 8)
        // {
        //      other.GetComponent<Health>().TakeDamage( damage, transform.parent.gameObject );
        // }


        // Ghost logic, should only do damage to player
        // if (shooter == "Ghost" && other.tag == "Player")
        // {
        //     // If player is human and dashing dont apply damage
        //     if (player.GetComponent<Human>() && player.GetComponent<Human>().isPlayerDashing())
        //     {
        //         return;
        //     }

        //     // if player not dashing apply damage
        //     else
        //     {
        //         player.GetComponent<Health>().TakeDamage( damage, transform.parent.gameObject );
        //         //add animation
        //         Destroy(this.gameObject);
        //     }

        // }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        var other = coll.collider.gameObject;

        if (other.name == "Sword")
        {
            Debug.Log("Hit!");
            // Deflect the bolt away from the sword
            float speed = lastVelocity.magnitude;
            Vector3 direction = Vector3.Reflect(lastVelocity.normalized,coll.contacts[0].normal);
            rb.velocity = direction * speed * 2;
            deflected =  true;
        }

        if (other.tag == "Wall")
        {
            //Add animation
            Destroy(this.gameObject);
        }

        // PLayer logic
        LayerMask mask = LayerMask.GetMask("enemies");

        if (shooter == "Player" && other.layer == 8)
        {
             other.GetComponent<Health>().TakeDamage( damage, transform.parent.gameObject );
        }

        // Ghost logic, should only do damage to player and if shot is deflected do no damage
        if (shooter == "Ghost" && other.tag == "Player" && !deflected)
        {
            // If player is human and dashing dont apply damage
            if (player.GetComponent<Human>() && player.GetComponent<Human>().isPlayerDashing())
            {
                return;
            }

            // if player not dashing apply damage
            else
            {
                player.GetComponent<Health>().TakeDamage( damage, transform.parent.gameObject );
                //add animation
                Destroy(this.gameObject);
            }

        }
    }

}
