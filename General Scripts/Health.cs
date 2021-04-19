using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public HealthBar healthBar;
    private Rigidbody2D rb;
    public SpriteRenderer sr;
    public GameObject lastHitBy;

    public string currentHost;

    void Awake()
    {
        currentHealth = maxHealth;
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.SetMaxHealth(maxHealth);

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (gameObject.tag == "Player")
        {
            currentHost = GetComponent<PlayerStats>().currentHost;
        }
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
            if (gameObject.tag == "Player")
            {
                switch (lastHitBy.tag)
                {
                    default:FindObjectOfType<GameManager>().EndGame();break;
                    case "Ghost":SwapHost(lastHitBy, transform.gameObject);break;
                    case "Worm":SwapHost(lastHitBy, transform.gameObject);break;
                }
                
            }
            else
            {
                Die();
            }
        }
    }

    public void SwapHost(GameObject newHost, GameObject oldHost)
    {

        //Remove the current host script
        switch (currentHost)
        {
            default:throw new System.Exception("failed to remove hurrent host script, unknown host");
            case "Human" : 
                Destroy(gameObject.GetComponent<Human>());
                Destroy(GameObject.Find("SwordAim"));
                Destroy(GameObject.Find("BowAim"));
                break; 
            case "Ghost" : Destroy(gameObject.GetComponent<Ghost>());break;
            case "Worm"  : Destroy(gameObject.GetComponent<Worm>());break;
        }

        //Add the new host script
        switch (newHost.tag)
        {
            default:throw new System.Exception("Swap to new host failed, attacker host type unknown");
            case "Human" : 
                    gameObject.AddComponent<Human>();
                    gameObject.GetComponent<Animator>().Play(gameObject.GetComponent<Human>().idleDown);
                    gameObject.GetComponent<PlayAnimations>().human = GetComponent<Human>();
                    break; 

                case "Ghost" : 
                    gameObject.AddComponent<Ghost>();
                    gameObject.GetComponent<Animator>().Play(gameObject.GetComponent<Ghost>().idleDown);
                    gameObject.GetComponent<PlayAnimations>().ghost = GetComponent<Ghost>();
                    break;

                case "Worm"  : 
                    gameObject.AddComponent<Worm>();
                    gameObject.GetComponent<Animator>().Play(gameObject.GetComponent<Worm>().idleDown);
                    gameObject.GetComponent<PlayAnimations>().worm = GetComponent<Worm>();
                    break;
        }

        // Transfer health stats
        maxHealth = newHost.GetComponent<Health>().maxHealth;
        currentHealth = newHost.GetComponent<Health>().currentHealth;
        healthBar = newHost.GetComponentInChildren<HealthBar>();
        healthBar.SetHealth(currentHealth);

        //Update the current host variable
        currentHost = newHost.tag;

        //Remove attacker from the game and move to their location
        transform.position = newHost.transform.position;
        Destroy(newHost);

    }

    // Takes in a damage value to apply and the game object that caused the damage
    public void TakeDamage( int damage, GameObject attacker )
    {   
        var isPhasing = false;

        if (gameObject.tag == "Player")
        {
            isPhasing = GetComponent<PlayerStats>().isPhasing;
        }

        //Probably willneed to make this flag more generic at some point, as I'm sure other hosts/enemies may have similar abilities
        if(!isPhasing)
        {
            lastHitBy = attacker;

        //Shake screen
        if (gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Shaker>().Shake(.1f);
        }

        //Apply the damage as long as the player isn't in a dash state
        if (gameObject.tag == "Player" && GetComponent<Human>() && GetComponent<Human>().isPlayerDashing())
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
        else
        {   
            //Is immune to damage
            return;
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
