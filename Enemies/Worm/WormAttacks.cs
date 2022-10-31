using System.Collections;
using UnityEngine;

public class WormAttacks : MonoBehaviour
{

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private GameObject player;
    private Animator am;
    [SerializeField]
    private float attackDelay = 2.5f;
    [SerializeField]
    private int attackDamage = 5;
    private float lastAttacked = -9999;
    private LayerMask enemyLayers;
    private Vector2 attackPoint;
    private Vector2 attackPointLeft;
    private Vector2 attackPointRight;
    private float attackRange = 0.4f;
    [SerializeField]
    private string currentSprite;


    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyLayers = LayerMask.GetMask("Player");

        am = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void SetCurrentSprite()
    {
        // Gets the current sprite with the junk text trimmed off
        currentSprite = sr.sprite.ToString();
        currentSprite = currentSprite.Substring(0,currentSprite.LastIndexOf(" " ) - 1).Trim();
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawSphere(attackPointRight, .5f);
        Gizmos.DrawSphere(attackPointLeft, .5f);

    }

    public void ResetAttackDelay()
    {
        lastAttacked = Time.time;
    }

    void SetAttackPoint()
    {
        // Set the ranges
        attackPointLeft = new Vector2(rb.position.x - attackRange - 0.3f ,rb.position.y);
        attackPointRight = new Vector2(rb.position.x + attackRange + 0.3f ,rb.position.y);

        
        // Set the attackable area
        switch (currentSprite)
        { 
            case "worm_walk":
                attackPoint = attackPointLeft;
                break;
            case "worm_walk_right":
                attackPoint = attackPointRight;
                break;

            case "worm_bite":
                attackPoint = attackPointLeft;
                break;
            case "worm_bite_right":
                attackPoint = attackPointRight;
                break;

            case "worm_death":
                gameObject.SetActive(false);
                break;

            default:throw new System.Exception("supplied sprite not recognised");
        }
}

    public void PoisonBite(Collider2D enemy, int damagePerSecond)
    {
        StartCoroutine(poisonBite(enemy,damagePerSecond));
    }

    private IEnumerator poisonBite(Collider2D enemy, int damage)
    {
        for (int i = 0; i <= 5; i++)
        {
            enemy.GetComponent<Health>().TakeDamage(1, transform.gameObject, "WormPoison", false);    
            StartCoroutine(FlashColor(Color.red));
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void Bite()
    {
            StartCoroutine(FlashColor(Color.red));

            if (attackPoint == attackPointLeft)
            {   
                am.Play("worm_bite");
            }
            else if (attackPoint == attackPointRight)
            {  
                am.Play("worm_bite_right");
            }

            // Detect enemies in range of attack
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint, attackRange, enemyLayers);

            // Damage them
            if (hitEnemies.Length > 0)
            {
                foreach (Collider2D enemy in hitEnemies)
                {
                    bool isCrit = false;
                    if (Random.Range(0,21) == 20)
                    {
                        isCrit = true;
                        attackDamage += (attackDamage*2);
                    }
                    if ( enemy != null )
                    {
                        enemy.GetComponent<Health>().TakeDamage(attackDamage, transform.gameObject, "WormPoison", isCrit);    
                        enemy.GetComponent<PlayerMovement>().DazeForSeconds(2);
                        PoisonBite(enemy,1);
                    } 
                }
            }
    }

    private IEnumerator FlashColor(Color color)
    {
        sr.color = color;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }

    void Update()
    {
        SetCurrentSprite();
        SetAttackPoint();

        // Check for player in aggo range
        if (Vector3.Distance(transform.position, player.transform.position) <= 1.0f)
        {
            if (Time.time > lastAttacked + attackDelay)
            {
                Bite();
                lastAttacked = Time.time;
            }
        }
    }
}
