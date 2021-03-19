using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public HealthBar healthBar;
    private Rigidbody2D rb;
    public SpriteRenderer sr;


    void Awake()
    {
        currentHealth = maxHealth;
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.SetMaxHealth(maxHealth);

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void AddHealth(int amount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += amount;
        }
    }

    public void RemoveHealth(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();

            if (gameObject.tag == "Player")
            {
                FindObjectOfType<GameManager>().EndGame();
            }
        }
    }

    public void TakeDamage( int damage )
    {
        //Shake screen
        if (gameObject.tag == "Player")
        {
            GameObject.Find("Main Camera").GetComponent<Shaker>().Shake(.1f);
        }


        //Apply the damage as long as the player isn't in a dash state
        if (gameObject.tag == "Player" && GameObject.Find("Player").GetComponent<Dash>().isPlayerDashing())
        {
            return;
        }
        else
        {
            //Display the damage as a popup
            DamagePopup.Create( transform.position, damage );
            
            // Flash Red to confirm hit
            StartCoroutine(FlashColor( Color.gray, 0.3f ));

            RemoveHealth(damage);
            
            healthBar.SetHealth(currentHealth);

            var playerIsLooking = GameObject.Find( "Player" )
                                   .GetComponent<PlayerMovement>()
                                   .playerIsLooking();


            //Get knocked back in the opposite direction of the attack
            if (TryGetComponent(out AIMovement move))
            {
                switch (playerIsLooking)
                {
                    default: throw new System.Exception("PlayerMovement.Looking state unknown");
                    case PlayerMovement.Looking.Left: move.KnockBack("Left");break;
                    case PlayerMovement.Looking.Right: move.KnockBack("Right");break;
                    case PlayerMovement.Looking.Up: move.KnockBack("Up");break;
                    case PlayerMovement.Looking.Down: move.KnockBack("Down");break;
                }   
            }
        }

        
         
    }

    private IEnumerator FlashColor(Color color, float duration)
    {
        sr.color = color;
        yield return new WaitForSeconds( duration );
        sr.color = Color.white;
    }

    private bool isDead = false;
    void Die()
    {
        if (!isDead && gameObject.name != "Player")
        {
            isDead = true;

            if (TryGetComponent(out AIMovement aim))
            {
                aim.enabled = false;
            }

            if (TryGetComponent(out GhostAttacks ga))
            {
                ga.enabled = false;
            }

            if (TryGetComponent(out CapsuleCollider2D cc))
            {
                cc.enabled = false;
            }

            if (TryGetComponent(out BoxCollider2D bc))
            {
                bc.enabled = false;
            }
            
            //Add player XP
            GameObject.Find( "Player" ).GetComponent<PlayerStats>()
                                 .AddXP( 100 );

            //Play death animation                         
            StartCoroutine(GetComponent<PlayAnimations>().Kill());

            //Drop loot
            GetComponent<DropLoot>().SpawnLoot();
        }

        else
        {
            // insert code that handles the asset swap to the killers sprites
        }
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }
}
