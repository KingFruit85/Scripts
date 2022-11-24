using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public string currentHost;
    public bool isBloodied;

    // public HealthBar healthBar;
    public SpriteRenderer sr;
    public GameObject lastHitBy;
    public GameObject currentRoom;
    public GameObject player;

    public bool isBoss = false;
    public bool isImmuneToMeleeDamage = false;
    public bool isImmuneToProjectileDamage = false;
    public bool isImmuneToAllDamage = false;

    private AudioManager audioManager;
    private Shaker cameraShaker;
    private GameManager gameManager;
    private int currentGameLevel;
    private float XP;
    private bool isDead = false;
    private bool isUsingNonDefaultColor = false;
    private Color nonDefaultColor;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        // Get the current level, used to modify npc stats
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentGameLevel = gameManager.currentGameLevel;
        player = GameObject.FindGameObjectWithTag("Player");
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        cameraShaker = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Shaker>();

        if (gameObject.tag == "Player")
        {
            ChangeMaxHealth(gameManager.healthBonus);
        }

        // Sets some immunities based on the type of monster
        if (gameObject.tag == "GhostBoss") isImmuneToMeleeDamage = true;
        if (gameObject.tag == "GhostBoss") isImmuneToProjectileDamage = true;

    }

    void Start()
    {
        if (gameObject.tag != "Player")
        {
            SetHealth();
            SetXP();
        }
        else
        {
            currentHost = GameObject.Find("GameManager").GetComponent<GameManager>().currentHost;
            gameManager.LoadHostScript(currentHost);
            currentHealth = maxHealth;
        }
    }

    private void SetHealth()
    {
        // Just a placeholder, will need to refine after testing
        if (currentGameLevel > 1)
        {
            maxHealth = maxHealth * (currentGameLevel / 1.5f);
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth = maxHealth;
        }
    }

    public void ChangeMaxHealth(int increaseAmount)
    {
        maxHealth += increaseAmount;
        AddHealth(increaseAmount);
    }

    private void SetXP()
    {
        if (currentGameLevel > 1)
        {
            XP = XP * (currentGameLevel / 1.5f);
        }
        else
        {
            XP = 100;
        }
    }

    public void AddHealth(int amount)
    {
        if (currentHealth + amount < maxHealth)
        {
            currentHealth += amount;
        }
        else
        {
            currentHealth = maxHealth;
        }

        // healthBar.SetHealth(currentHealth);
    }

    public void RemoveHealth(int amount)
    {

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            if (gameObject.tag == player.tag)
            {
                int chance = Random.Range(0, 11);
                switch (lastHitBy.tag)
                {
                    default: Die(); break;

                    case "Ghost":
                        if (chance == 10)
                        {
                            SwapHost(lastHitBy);
                            break;
                        }
                        else
                        {
                            Die(); break;
                        }
                    case "Worm":
                        if (chance == 10)
                        {
                            SwapHost(lastHitBy);
                            break;
                        }
                        else
                        {
                            Die(); break;
                        }
                }
            }
            else
            {
                Die();
            }
        }
    }

    public void SwapHost(GameObject newHost)
    {

        gameManager.currentHost = newHost.tag;

        //Remove the old host script
        switch (currentHost)
        {
            default: throw new System.Exception("failed to remove current host script, unknown host");
            case "Human":
                Destroy(gameObject.GetComponent<Human>());
                Destroy(GameObject.Find("SwordAim"));
                Destroy(GameObject.Find("BowAim"));
                break;
            case "Ghost": Destroy(gameObject.GetComponent<Ghost>()); break;
            case "Worm": Destroy(gameObject.GetComponent<Worm>()); break;
        }

        //Add the new host script
        switch (newHost.tag)
        {
            default: throw new System.Exception("Swap to new host failed, attacker host type unknown");
            case "Human":
                gameObject.AddComponent<Human>();
                gameObject.GetComponent<Animator>().Play(gameObject.GetComponent<Human>().idleDown);
                gameObject.GetComponent<PlayAnimations>().human = GetComponent<Human>();
                break;

            case "Ghost":
                gameObject.AddComponent<Ghost>();
                gameObject.GetComponent<Animator>().Play(gameObject.GetComponent<Ghost>().idleDown);
                gameObject.GetComponent<PlayAnimations>().ghost = GetComponent<Ghost>();
                break;

            case "Worm":
                gameObject.AddComponent<Worm>();
                gameObject.GetComponent<Animator>().Play(gameObject.GetComponent<Worm>().idleDown);
                gameObject.GetComponent<PlayAnimations>().worm = GetComponent<Worm>();
                break;
        }

        // Transfer health stats
        maxHealth = newHost.GetComponent<Health>().maxHealth;
        currentHealth = newHost.GetComponent<Health>().currentHealth;
        // gameObject.GetComponentInChildren<HealthBar>().SetMaxHealth(maxHealth);
        // gameObject.GetComponentInChildren<HealthBar>().SetHealth(currentHealth);


        //Update the current host variable
        currentHost = newHost.tag;

        //Remove attacker from the game and move to their location
        transform.position = newHost.transform.position;
        Destroy(newHost);

    }

    public void setProjectileImmunity(bool isImmune)
    {
        if (isImmune == false)
        {
            StartCoroutine(DisableProjectileImmunity(2f));
        }
        else
        {
            isImmuneToProjectileDamage = true;
        }
    }

    public void setMeleeImmunity(bool isImmune)
    {
        if (isImmune == false)
        {
            StartCoroutine(DisableMeleeImmunity(2f));
        }
        else
        {
            isImmuneToMeleeDamage = true;
        }
    }

    private IEnumerator DisableProjectileImmunity(float duration)
    {
        sr.color = Color.red;
        isImmuneToProjectileDamage = false;
        yield return new WaitForSeconds(duration);
        isImmuneToProjectileDamage = true;
        sr.color = Color.white;
    }

    private IEnumerator DisableMeleeImmunity(float duration)
    {
        sr.color = Color.red;
        isImmuneToMeleeDamage = false;
        yield return new WaitForSeconds(duration);
        isImmuneToMeleeDamage = true;
        sr.color = Color.white;
    }

    // Takes in a damage value to apply and the game object that caused the damage
    public void TakeDamage(int damage, GameObject attacker, string damageType, bool isCrit)
    {

        if (TryGetComponent(out Ghost ghost)) isImmuneToProjectileDamage = ghost.Phasing();
        if (TryGetComponent(out Human human)) isImmuneToAllDamage = human.isPlayerDashing();


        // If the host is phasing and the damage is a projectile, return
        // If the host is immune to melee, return
        if (isImmuneToMeleeDamage || damageType == "melee") return;
        // If the host is immune to all damage, return
        if (isImmuneToAllDamage) return;


        ////////////////
        //Hit connects//
        ////////////////
        if (gameObject.tag == "Player" && isCrit)
        {
            cameraShaker.Shake(.3f, 3.0f);
            gameManager.SetPlayerHit(isCrit);

        }
        // Log attacker
        lastHitBy = attacker;
        // Display the damage as a popup
        DamagePopup.Create(transform.position, damage, isCrit);
        // Flash Red to confirm hit
        StartCoroutine(FlashColor(Color.gray, 0.3f));
        // Remove the health
        RemoveHealth(damage);
        // If enemy, apply knockback and reset attack delay value
        if (gameObject.layer == 8)
        {
            TriggerKnockBack();
            TriggerAttackDelayReset();
        }
    }

    private void TriggerKnockBack()
    {
        // Bosses can't be knocked back
        if (!isBoss)
        {
            var playerIsLooking = player.GetComponent<PlayerMovement>().PlayerIsLooking();
            var movement = TryGetComponent(out AIMovement move);
            move.KnockBack(playerIsLooking);
        }
    }

    private void TriggerAttackDelayReset()
    {
        switch (gameObject.tag)
        {
            default: throw new System.Exception("unknown recipient of damage");
            case "Ghost": gameObject.GetComponent<GhostAttacks>().ResetAttackDelay(); break;
            case "MiniBoss": gameObject.GetComponent<GhostAttacks>().ResetAttackDelay(); break;
            case "Worm": gameObject.GetComponent<WormAttacks>().ResetAttackDelay(); break;
                // case "GhostBoss" : gameObject.GetComponent<GhostBossAttacks>().ResetAttackDelay();break;
        }
    }

    private IEnumerator FlashColor(Color color, float duration)
    {
        sr.color = color;
        yield return new WaitForSeconds(duration);
        sr.color = Color.white;
    }

    public void Die()
    {
        // Player death
        if (gameObject.tag == "Player")
        {
            // Stops player from being able to move on death and plays death animation 
            GetComponent<PlayerMovement>().StopPlayerMovement();
            StartCoroutine(GetComponent<PlayAnimations>().Kill());

            if (TryGetComponent(out Human human))
            {
                human.enabled = false;
            }
            // Removes any weapons the host may have
            foreach (Transform child in gameObject.transform)
            {
                if (child.name == "SwordAim")
                {
                    Destroy(child.gameObject);
                }
            }

            // Triggers game restart
            gameManager.EndGame();

        }

        if (gameObject.tag == "GhostBoss")
        {
            // Take player to game over screen for the time being
            currentRoom.GetComponent<SimpleRoom>().SpawnExitTile();
            // gameManager.TemporaryGameComplete();

        }

        if (isBoss && gameObject.tag == "MiniBoss")
        {
            currentRoom.GetComponent<SimpleRoom>().UnlockExitTile();
            gameManager.miniBossKilled = true;
        }

        if (!isDead && gameObject.name != player.tag)
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
                Physics2D.IgnoreCollision(cc, player.GetComponent<CapsuleCollider2D>());
            }

            if (TryGetComponent(out BoxCollider2D bc))
            {
                Physics2D.IgnoreCollision(bc, player.GetComponent<CapsuleCollider2D>());
            }

            //Add player XP
            if (isBoss)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().AddXP(XP + 500);
            }
            else
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().AddXP(XP);
            }


            //Play death animation                         
            StartCoroutine(GetComponent<PlayAnimations>().Kill());

            //Drop loot
            if (!isBoss)
            {
                // Need to add boss drops
                GetComponent<DropLoot>().SpawnLoot();
            }
        }

    }


    public void SetSpriteColor(Color color)
    {
        isUsingNonDefaultColor = true;
        nonDefaultColor = color;
    }

    void Update()
    {

        if (currentHealth <= maxHealth / 2)
        {
            isBloodied = true;
        }
        else
        {
            isBloodied = false;
        }
        if (isBoss)
        {
            sr.color = new Color(156, 21, 21, 255);
            // currentRoom = transform.parent.gameObject;
        }
        if (gameObject.tag == "MiniBoss")
        {
            sr.color = new Color(156, 21, 21, 255);
            currentRoom = transform.parent.gameObject;
        }

        if (isUsingNonDefaultColor)
        {
            GetComponent<SpriteRenderer>().color = nonDefaultColor;
        }

    }
}
