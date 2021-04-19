using System.Collections;
using UnityEngine;
using static PlayerMovement;

public class Human : MonoBehaviour
{
    public string idleLeft = "Human_Idle_Left";
    public string idleRight = "Human_Idle_Right";
    public string walkLeft = "Human_Walk_Left";
    public string walkRight = "Human_Walk_Right";
    public string walkUp = "Human_Move_Up";
    public string walkDown = "Human_Move_Down";
    public string idleUp = "Human_Idle_Up";
    public string idleDown = "Human_Idle_Down";
    public string death;
    public string attackLeft = "Sword_Stab_Left";
    public string attackRight = "Sword_Stab_Right";
    public string attackUp = "Human_Attack_Up";
    public string attackDown = "Human_Attack_Down";
    public int swordDamage = 10;
    public float swordRange = 0.5f;

    private Rigidbody2D rb;
    private Animator playerAnim;
    private PlayAnimations pa;
    public Animator swordAnim;
    private Shaker shaker;
    public float moveSpeed = 5;
    public GameObject swordAim;
    public GameObject bowAim;
    public GameObject sword;
    public GameObject bow;
    public GameObject player;
    public bool SwordEquipped;
    public bool BowEquipped = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        shaker = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Shaker>();
        player = GameObject.Find("Player");

        //This is the current set up i'm using for the weapons, 
        swordAim = transform.GetChild(1).gameObject;
                                   
        sword = swordAim.transform.GetChild(0).gameObject;
        swordAim.SetActive(true);

        swordAnim = sword.GetComponent<Animator>();

        transform.localScale = new Vector3(2.5f,2.5f,0);

        bowAim = new GameObject();
        bow = new GameObject();
        
        GetComponent<PlayerMovement>().moveSpeed = moveSpeed;

        //Set the player animations/sprites to the current host creature
        pa = GetComponent<PlayAnimations>();

        pa.idleLeft = idleLeft;
        pa.idleRight = idleRight;
        pa.walkLeft = walkLeft;
        pa.walkRight = walkRight;
        pa.walkUp = walkUp;
        pa.walkDown = walkDown;
        pa.death = death;
        pa.attackLeft = attackLeft;
        pa.attackRight = attackRight;
        pa.attackUp = attackUp;
        pa.attackDown = attackDown;
    }


    public float dashSpeed = 20f;
    private float dashCoolDown = -9999;
    public float dashDelay = .5f;
    private bool canDash = true;
    private bool isDashing = false;


    public void Dash()
    {
        var direction = GetComponent<PlayerMovement>().looking;
        if (canDash)
        {
            // Set users in dashing state/invincibility state for a frame or two
            canDash = false;
            StartCoroutine(ToggleIsDashingBool());

            switch (direction)
            {   
                default: throw new System.Exception("invalid dash direction provided");
                case Looking.Up:
                                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + dashSpeed); 
                                playerAnim.Play("Human_Dash_Up");
                                shaker.CombatShaker("Up");
                                dashCoolDown = Time.time;
                                break;

                case Looking.Down:
                                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y -dashSpeed); 
                                playerAnim.Play("Human_Dash_Down");
                                shaker.CombatShaker("Down");
                                dashCoolDown = Time.time;
                                break;

                case Looking.Left:
                                rb.velocity = new Vector2(rb.velocity.x - dashSpeed, rb.velocity.y);
                                playerAnim.Play("Human_Dash_Left");
                                shaker.CombatShaker("Left");
                                dashCoolDown = Time.time;
                                break;

                case Looking.Right:
                                rb.velocity = new Vector2(rb.velocity.x + dashSpeed, rb.velocity.y);
                                playerAnim.Play("Human_Dash_Right");
                                shaker.CombatShaker("Right");
                                dashCoolDown = Time.time;
                                break;
            }
        }
    }

    private IEnumerator ToggleIsDashingBool()
    {
        isDashing = true;
        yield return new WaitForSeconds( 0.1f );
        isDashing = false;
    }

    public bool isPlayerDashing()
    {
        return isDashing;
    }

    public PlayerMovement.Looking playerIsLooking;

    public void SwordAttack()
    {

        switch (playerIsLooking)
        {
            default: throw new System.Exception("PlayerMovement.Looking state not valid");
            case PlayerMovement.Looking.Left:
                swordAnim.Play(attackLeft);
                shaker.CombatShaker("Left");
                break;
                
            case PlayerMovement.Looking.Right:
                swordAnim.Play(attackRight);
                shaker.CombatShaker("Right");
                break;

            case PlayerMovement.Looking.Up:
                swordAnim.Play(attackUp);
                shaker.CombatShaker("Up");
                break;

            case PlayerMovement.Looking.Down:
                swordAnim.Play(attackDown);
                shaker.CombatShaker("Down");
                break;
        }   
        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(sword.transform.position, swordRange, LayerMask.GetMask("enemies"));

        // Damage them
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.GetComponent<Health>())
            enemy.GetComponent<Health>().TakeDamage(swordDamage, transform.gameObject);   
        }
    }

    public void BowAttack(string bow)
    {
        switch (bow)
        {
            default: throw new System.Exception("Supplied bow name not recognised");
            
            case "Short Bow":
                // GetComponent<BowPickup>().ShootBow();
                break;

            case "Gold Bow":
                GetComponent<GoldBowPickup>().ShootBow();
                break;
        }
    }

    void Update()
    {

        playerIsLooking = GetComponent<PlayerMovement>().playerIsLooking();

        switch (playerIsLooking)
        {
            default:return;
            case PlayerMovement.Looking.Left: 
                if (SwordEquipped == true ) swordAim.transform.localPosition = new Vector3(0.054f,0.057f,180f);
                if (SwordEquipped == true ) sword.GetComponent<SpriteRenderer>().sortingOrder = 3;

                break;
                
            case PlayerMovement.Looking.Right:
                if (SwordEquipped == true ) swordAim.transform.localPosition = new Vector3(-0.05f,0.043f,0);
                if (SwordEquipped == true ) sword.GetComponent<SpriteRenderer>().sortingOrder = 3;
                // if (BowEquipped == true ) bow.GetComponent<SpriteRenderer>().sortingOrder = 1;

                break;

            case PlayerMovement.Looking.Up:
                if (SwordEquipped == true ) swordAim.transform.localPosition = new Vector3(0.052f,0.055f,0);
                if (SwordEquipped == true ) sword.GetComponent<SpriteRenderer>().sortingOrder = 1;
                // if (BowEquipped == true ) bow.GetComponent<SpriteRenderer>().sortingOrder = 1;


                break;

            case PlayerMovement.Looking.Down:
                if (SwordEquipped == true ) swordAim.transform.localPosition = new Vector3(-0.0287f,0.0485f,0);
                if (SwordEquipped == true ) sword.GetComponent<SpriteRenderer>().sortingOrder = 3;         

                break;
        }

        if (Time.time > dashCoolDown + dashDelay)
        {
            canDash = true;
            dashCoolDown = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Dash();
        }

        ///WEAPON SELECTION SHOULD PROBABLY BE MOVED TO PlayerCombat.CS

        // If the "1" button is presssed equip the sword
        if (Input.GetKeyDown(KeyCode.Alpha1) && SwordEquipped == false)
        {
            SwordEquipped = true;
            swordAim.SetActive(true);
            sword.SetActive(true);
            player.GetComponent<PlayerCombat>().setEquippedWeaponName("Short Sword");

            //Deactivate bow if it currently is held by the player
            if (GetComponent<PlayerCombat>().rangedWeaponEquipped)
            {
                bowAim.SetActive(false);
                bow.SetActive(false);
                BowEquipped = false;
            }  
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && BowEquipped == false)
        {
            SwordEquipped = false;
            swordAim.SetActive(false);
            sword.SetActive(false);

            if (GetComponent<PlayerCombat>().rangedWeaponEquipped)
            {
                BowEquipped = true;
                bowAim.SetActive(true);
                bow.SetActive(true);
                player.GetComponent<PlayerCombat>().setEquippedWeaponName("Short Bow");
            }   
        }


        Vector3 mousePOS = Camera.main.ScreenToWorldPoint (Input.mousePosition);
        mousePOS.z = 0f;

        Vector3 aimDirection = (mousePOS - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        swordAim.transform.eulerAngles = new Vector3(0,0,angle);

        if (GetComponent<PlayerCombat>().rangedWeaponEquipped)
        {
            bowAim.transform.eulerAngles = new Vector3(0,0,angle);
        }

    }

    


}