using UnityEngine;

public class BossBolt : MonoBehaviour
{
    public float speed;
    public int damage = 1;
    private Rigidbody2D rb;
    private GameObject player;
    private Vector3 aim;
    private Vector3 playerMouseClick;

    private float born;
    private float lifeTime = 4.5f;

    public string shooter;
    private Vector3 lastVelocity;
    private bool deflected;
    private AudioManager audioManager;
    GameManager gameManager;




    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        playerMouseClick = player.GetComponent<PlayerCombat>().mouseClickPosition; 
        born = Time.time;
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

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
            Destroy(this.gameObject, 0.1f);
        }
    }

    private void ShootAtEnemy()
    {   
        damage = gameManager.rangedAttackBonus + player.GetComponent<Ghost>().ghostBoltDamage;
        aim = (playerMouseClick - transform.position);
        rb.AddForce(aim * speed);
    }

    private void ShootAtPlayer()
    {

        var r = Random.Range(0,5);
        var target = player.transform.position;
        var looking = player.GetComponent<PlayerMovement>().looking;
        var moving = player.GetComponent<PlayerMovement>().isMoving;

        if (moving)
        {
            switch (player.GetComponent<PlayerMovement>().looking)
            {
            case PlayerMovement.Looking.Left: 
                target.x =- r; 
                break;

            case PlayerMovement.Looking.Right:
                target.x += r;
                break;

            case PlayerMovement.Looking.Up:
                target.y += r;
                break;

            case PlayerMovement.Looking.Down:
                target.y -= r;
                break;
            }
        }
        
        

        aim = (target - transform.position).normalized;
        rb.AddForce(aim * speed);
    }

  

    void OnCollisionEnter2D(Collision2D coll)
    {
        var other = coll.collider.gameObject;

        if (other.name == "Sword")
        {
            Destroy(this.gameObject);
        }

        if (other.tag == "Wall")
        {
            var r = Random.Range(0,100);

            if (r == 99)
            {
                GameObject x = Instantiate(Resources.Load("Ghost"),transform.position,Quaternion.identity) as GameObject;
                x.GetComponent<Health>().SetSpriteColor(Color.black);
            }

            Destroy(this.gameObject);
        }

        if (other.tag == "PlayerArrow")
        {
            //Add animation
            Destroy(this.gameObject);
        }

        // PLayer logic
        LayerMask mask = LayerMask.GetMask("enemies");

        if (shooter == "Player" && other.layer == 8)
        {
             other.GetComponent<Health>().TakeDamage( damage, transform.parent.gameObject, gameObject.tag, false );
        }

        // Ghost logic, should only do damage to player and if shot is deflected do no damage
        if (other.tag == "Player" && !deflected)
        {
            // If player is human and dashing dont apply damage
            if (player.GetComponent<Human>() && player.GetComponent<Human>().isPlayerDashing())
            {
                return;
            }

            // if player not dashing apply damage
            else
            {
                player.GetComponent<Health>().TakeDamage( damage, transform.parent.gameObject, gameObject.tag, false );
                //add animation
                Destroy(this.gameObject);
            }

        }
    }

}
