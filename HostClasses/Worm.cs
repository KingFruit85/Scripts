using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : MonoBehaviour
{
    public string idleLeft = "worm_walk";
    public string idleRight = "worm_walk";
    public string walkLeft = "worm_walk";
    public string walkRight = "worm_walk_right";
    public string walkUp = "worm_walk";
    public string walkDown = "worm_walk";
    public string idleUp = "worm_walk";
    public string idleDown = "worm_walk";
    public string death = "worm_death";
    public string attackLeft = "worm_bite";
    public string attackRight = "worm_bite_right";
    public string attackUp = "worm_stab_up";
    public string attackDown = "worm_stab_down";

    public float moveSpeed = 10;
    private int enemyLayers;
    private PlayAnimations pa;
    private SpriteRenderer sr;
    private Animator am;
    private float attackRange = 0.4f;
    public int attackDamage = 5;

    private Vector2 attackPointLeft;
    private Vector2 attackPointRight;
    private Vector2 attackPointUp;
    [SerializeField]
    private Vector2 attackPoint;
    private PlayerMovement.Looking playerIsLooking;
    private Vector2 attackPointDown;

    void Awake()
    {
        if (gameObject.tag == "Player")
        {
            GetComponent<PlayerMovement>().moveSpeed = moveSpeed;
            enemyLayers = LayerMask.GetMask("enemies");
            GameObject.Find("GameManager").GetComponent<GameManager>().currentHost = "Worm";
        }
        else
        {
            enemyLayers = LayerMask.GetMask("Player");
        }

        transform.localScale = new Vector3(1,1,0);

        pa = GetComponent<PlayAnimations>();
        sr = GetComponent<SpriteRenderer>();
        am = GetComponent<Animator>();

        //Set the player animations/sprites to the current host creature
        pa.idleLeft = idleLeft;
        pa.idleRight = idleRight;
        pa.walkLeft = walkLeft;
        pa.walkRight = walkRight;
        pa.walkUp = walkUp;
        pa.walkDown = walkDown;
        pa.death = death;
        
    }

    void SetAttackPoint()
    {
        // Set the ranges
        attackPointLeft = new Vector2(transform.position.x - attackRange - 0.3f ,transform.position.y);
        attackPointRight = new Vector2(transform.position.x + attackRange + 0.3f ,transform.position.y);
        attackPointUp = new Vector2(transform.position.x ,transform.position.y + attackRange);
        attackPointDown = new Vector2(transform.position.x ,transform.position.y - attackRange);



        if (transform.tag == "Player")
        {
            playerIsLooking = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().looking;

            switch (playerIsLooking)
            {
                default:throw new System.Exception("looking in unknown direction");
                case PlayerMovement.Looking.Up:attackPoint = attackPointUp;break;
                case PlayerMovement.Looking.Down:attackPoint = attackPointDown;break;
                case PlayerMovement.Looking.Left:attackPoint = attackPointLeft;break;
                case PlayerMovement.Looking.Right:attackPoint = attackPointRight;break;


            }
            
        }
    }

    void Update()
    {
        SetAttackPoint();
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawSphere(attackPoint, attackRange);

    }

    private IEnumerator FlashColor(Color color)
    {
        sr.color = color;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }


    public void PoisonBite()
    {
        StartCoroutine(FlashColor(Color.red));
        
        if (attackPoint == attackPointLeft)
        {
            am.Play(attackLeft);
        }
        else if (attackPoint == attackPointRight)
        {
            am.Play(attackRight);
        }
        else if (attackPoint == attackPointUp)
        {
            am.Play(attackUp);
        }
        else if (attackPoint == attackPointDown)
        {
            am.Play(attackDown);
        }

            // Detect enemies in range of attack
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint, attackRange, enemyLayers);

            // Damage them
            if (hitEnemies.Length > 0)
            {
                foreach (Collider2D enemy in hitEnemies)
                {
                    if ( enemy != null )
                    {
                        enemy.GetComponent<Health>().TakeDamage(attackDamage, transform.gameObject, "WormPoison", false);   
                        //Need to add slow for enemies 
                        if (enemy.tag == "Player")
                        {
                            enemy.GetComponent<PlayerMovement>().DazeForSeconds(2);
                        }
                        else
                        {
                            enemy.GetComponent<AIMovement>().DazeForSeconds(2);

                        }
                    } 
                }
            }
    }

}
