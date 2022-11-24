using UnityEngine;

public class Arrow : MonoBehaviour
{
    private int damage;
    private int arrowSpeed;

    public Sprite flameArrow;
    public bool isAlight = false;

    public Vector3 clickPoint;
    public Vector3 aim;
    private Vector2 lastVelocity;

    private Rigidbody2D rb;
    private AudioManager audioManager;
    private SpriteRenderer spriteRenderer;

    private Human player;

    void Start()
    {
        aim = (clickPoint - transform.position).normalized;
        //Points the arrow the direction we're shooting
        float angle = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Human>();
        damage = player.arrowDamage;
        arrowSpeed = player.arrowSpeed;


        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "FlameBowl")
        {
            SetAlight();
        }
    }

    void SetAlight()
    {
        spriteRenderer.sprite = flameArrow;
        isAlight = true;
        damage += 10;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.gameObject.layer == LayerMask.NameToLayer("Items"))
        {
            return;
        }

        else if (coll.collider.gameObject.layer == LayerMask.NameToLayer("enemies"))
        {
            if (isAlight)
            {
                coll.gameObject.GetComponent<Health>().TakeDamage(damage, transform.parent.gameObject, "FlamingPlayerArrow", false);
            }
            else
            {
                coll.gameObject.GetComponent<Health>().TakeDamage(damage, transform.parent.gameObject, gameObject.tag, false);
            }

            Destroy(gameObject);
        }

        else if (coll.collider.gameObject.tag == "TrapArrow")
        {
            var speed = lastVelocity.magnitude;
            var direction = Vector3.Reflect(lastVelocity.normalized, coll.contacts[0].normal);
            rb.velocity = direction * speed * 2;
        }

        else if (coll.gameObject.tag == "Wall")
        {
            //maybe have the arrow get imbedded in the wall?
            // audioManager.PlayAudioClip("ArrowHitWall");
            Destroy(gameObject);
        }


    }
}
