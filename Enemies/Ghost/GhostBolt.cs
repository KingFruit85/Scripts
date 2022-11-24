using UnityEngine;

public class GhostBolt : MonoBehaviour, IProjectile
{
    private Rigidbody2D rb;
    private GameObject player;
    private Vector3 aim;
    private Vector3 playerMouseClick;

    private float born;
    private float lifeTime = 1.5f;

    public string shooter;
    private Vector3 lastVelocity;
    public bool deflected;
    private AudioManager audioManager;
    GameManager gameManager;

    public float speed { get; set; }
    public int damage { get; set; }

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        playerMouseClick = player.GetComponent<PlayerCombat>().mouseClickPosition;
        born = Time.time;
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        speed = 0.001f;
        damage = 10;
    }

    void Start()
    {
        shooter = transform.parent.tag;

        // Stops the bolt colliding with whoever is firing it
        if (shooter == "Player")
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), player.GetComponent<CapsuleCollider2D>());
        }
        else
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), transform.parent.GetComponent<CapsuleCollider2D>());
        }
    }

    void Update()
    {
        switch (shooter)
        {
            case "Ghost": ShootAtPlayer(); break;
            case "Player": ShootAtEnemy(); break;
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
        damage = gameManager.rangedAttackBonus + damage;
        aim = (playerMouseClick - transform.position);
        rb.AddForce(aim * (speed * 5));
    }

    private void ShootAtPlayer()
    {
        aim = (player.transform.position - transform.position).normalized;
        rb.AddForce(aim * speed);
    }



    void OnCollisionEnter2D(Collision2D coll)
    {
        var other = coll.collider.gameObject;

        if (other.name == "Sword")
        {
            // Deflect the bolt away from the sword

            string[] deflects = new string[]{"SwordGhostBoltDeflect1","SwordGhostBoltDeflect2","SwordGhostBoltDeflect3","SwordGhostBoltDeflect4",
                                             "SwordGhostBoltDeflect5","SwordGhostBoltDeflect6","SwordGhostBoltDeflect7"};

            int rand = Random.Range(0, deflects.Length - 1);

            audioManager.PlayAudioClip(deflects[rand]);

            float speed = lastVelocity.magnitude;
            Vector3 direction = Vector3.Reflect(lastVelocity.normalized, coll.contacts[0].normal);
            rb.velocity = direction * speed * 2;
            deflected = true;
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
            other.GetComponent<Health>().TakeDamage(damage, transform.parent.gameObject, gameObject.tag, false);
        }

        // Ghost logic, should only do damage to player and if shot is deflected do no damage
        if (shooter == "Ghost" && other.tag == "Player" && !deflected)
        {
            // If player is human and dashing don't apply damage
            if (player.GetComponent<Human>() && player.GetComponent<Human>().isPlayerDashing())
            {
                return;
            }

            // if player not dashing apply damage
            else
            {
                bool isCrit = false;
                if (Random.Range(0, 11) == 10)
                {
                    isCrit = true;
                    damage += (damage * 2);
                }
                player.GetComponent<Health>().TakeDamage(damage, transform.parent.gameObject, gameObject.tag, isCrit);
                //add animation
                Destroy(this.gameObject);
            }

        }
    }

}
