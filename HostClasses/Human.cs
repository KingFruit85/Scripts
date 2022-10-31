using System.Collections;
using UnityEngine;
using static PlayerMovement;

public class Human : MonoBehaviour
{

    public ParticleSystem dust;
    public string idleLeft = "Human_Idle_Left";
    public string idleRight = "Human_Idle_Right";

    public string walkLeft = "Human_Walk_Left";
    public string walkRight = "Human_Walk_Right";
    public string walkUp = "Human_Move_Up";
    public string walkDown = "Human_Move_Down";


    public string walkLeftNonBloodied = "Human_Walk_Left";
    public string walkRightNonBloodied = "Human_Walk_Right";
    public string walkUpNonBloodied = "Human_Move_Up";
    public string walkDownNonBloodied = "Human_Move_Down";

    public string walkDownBloodied = "Human_Move_Down_Bloodied";
    public string walkUpBloodied = "Human_Move_Up_Bloodied";
    public string walkLeftBloodied = "Human_Move_Left_Bloodied";
    public string walkRightBloodied = "Human_Move_Right_Bloodied";

    public string idleUp = "Human_Idle_Up";
    public string idleDown = "Human_Idle_Down";
    public string death = "Human_Death";
    public string attackLeft = "Sword_Stab_Left";
    public string attackRight = "Sword_Stab_Right";
    public string attackUp = "Human_Attack_Up";
    public string attackDown = "Human_Attack_Down";
    

    private Rigidbody2D rb;
    private Animator playerAnim;
    private PlayAnimations pa;
    public Animator swordAnim;
    private Shaker shaker;
    public GameObject swordAim;
    public GameObject bowAim;
    public GameObject sword;
    public GameObject bow;
    public GameObject player;
    private AudioManager audioManager;
    private GameManager gameManager;

    public bool SwordEquipped = true;
    public bool BowEquipped = false;
    public float moveSpeed = 5;
    public int swordDamage = 30;
    public float swordRange = 0.5f;

    public int arrowDamage = 10;
    public int arrowSpeed = 10;
    public int critModifier = 2;

    void Awake()
    {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            dust = GameObject.Find("dustPS").GetComponent<ParticleSystem>();

            if (gameManager.rangedWeaponEquipped)
            {
                var BowPickup = Resources.Load("BowPickup") as GameObject;
                GameObject x = Instantiate(BowPickup,transform.position,Quaternion.identity);

                x.GetComponent<BowPickup>().AddBowToPlayer();
                Destroy(x);
            }
    }

    void Start()
    {
        gameManager.currentHost = "Human";

        rb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        shaker = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Shaker>();
        player = GameObject.FindGameObjectWithTag("Player");
        audioManager = GameObject.FindObjectOfType<AudioManager>();


        //This is the current set up i'm using for the weapons, needs improvement
        GameObject swordLoad = 
                            Instantiate(
                                Resources.Load("SwordAim"),
                                new Vector2(transform.position.x - 0.04f,transform.position.y + 0.05f),
                                Quaternion.identity)
                            as GameObject;
                            swordLoad.transform.parent = transform;;
                            swordLoad.name = "SwordAim";

        swordAim = gameObject.transform.Find("SwordAim").gameObject;

        sword = swordAim.transform.GetChild(0).gameObject;                           
        swordAim.SetActive(true);
        swordAnim = sword.GetComponent<Animator>();

        transform.localScale = new Vector3(2.5f,2.5f,0);
        
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

    void CreateDust()
    {
        dust.Play();
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

            CreateDust();
            gameManager.HostStamina --;
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
                // swordAnim.Play(attackLeft);
                // shaker.CombatShaker("Left");
                break;
                
            case PlayerMovement.Looking.Right:
                // swordAnim.Play(attackRight);
                // shaker.CombatShaker("Right");
                break;

            case PlayerMovement.Looking.Up:
                // swordAnim.Play(attackUp);
                // shaker.CombatShaker("Up");
                break;

            case PlayerMovement.Looking.Down:
                // swordAnim.Play(attackDown);
                // shaker.CombatShaker("Down");
                break;
        }   
        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(sword.transform.position, swordRange, LayerMask.GetMask("enemies"));

        // Damage them
        foreach (Collider2D enemy in hitEnemies)
        {
            bool isCrit = false;

            if (enemy.GetComponent<Health>())
            {
                int dammageToApply = swordDamage + gameManager.meleeAttackBonus;
                var random = Random.Range(0,11);

                // Check if critical hit
                if (random == 10)
                {
                    dammageToApply *= critModifier;
                    isCrit = true;
                }
                   

                enemy.GetComponent<Health>().TakeDamage(dammageToApply, transform.gameObject,"PlayerSword", isCrit);  
                
                string[] swordHits = new string[]{"SwordHit","SwordHit1","SwordHit2","SwordHit3","SwordHit4","SwordHit5","SwordHit6"};
                int rand = Random.Range(0, swordHits.Length);

                audioManager.PlayAudioClip(swordHits[rand]);

            }
            else
            {
                audioManager.PlayAudioClip("SwordMiss");
            }
        }
    }

    public void BowAttack(Vector3 mouseClickPosition)
    {
        player.transform.GetComponentInChildren<ShortBow>().ShootBow(mouseClickPosition);
    }

    public void SetBloodiedSprites(bool x)
    {
        if (x)
        {
            walkLeft = walkLeftBloodied;
            walkRight = walkRightBloodied;
            walkUp = walkUpBloodied;
            walkDown = walkDownBloodied;
        }
        else
        {
            walkLeft = walkLeftNonBloodied;
            walkRight = walkRightNonBloodied;
            walkUp = walkUpNonBloodied;
            walkDown =  walkDownNonBloodied;
        }
    }

    public void SetMeleeAsActiveWeapon()
    {
        SwordEquipped = true;
        swordAim.SetActive(true);
        sword.SetActive(true);
        player.GetComponent<PlayerCombat>().SetEquippedWeaponName("Short Sword");

        //Deactivate bow if it currently is held by the player
        if (GetComponent<PlayerCombat>().rangedWeaponEquipped)
        {
            bowAim.SetActive(false);
            bow.SetActive(false);
            BowEquipped = false;
            gameManager.rangedWeaponEquipped = false;
        } 
    }

    public void SetRangedAsActiveWeapon()
    {
        SwordEquipped = false;
        swordAim.SetActive(false);
        sword.SetActive(false);

        if (GetComponent<PlayerCombat>().rangedWeaponEquipped)
        {
            BowEquipped = true;
            gameManager.rangedWeaponEquipped = true;
            bowAim.SetActive(true);
            bow.SetActive(true);
            player.GetComponent<PlayerCombat>().SetEquippedWeaponName("Short Bow");
        }   
    }

    // This update function if really chonky, probably needs a look over
    void Update()
    {
        if (GetComponent<Health>().isBloodied)
        {
            SetBloodiedSprites(true);
        }

        if (!GetComponent<Health>().isBloodied)
        {
            SetBloodiedSprites(false);
        }

        playerIsLooking = GetComponent<PlayerMovement>().PlayerIsLooking();
        if (BowEquipped)
        {
            bowAim.transform.localScale = new Vector2(-transform.localScale.x + 1.5f,transform.localScale.y - 1.5f);
        }

        switch (playerIsLooking)
        {
            default:return;
            case PlayerMovement.Looking.Left: 
                if (SwordEquipped != null && SwordEquipped == true ) swordAim.transform.localPosition = new Vector3(0.054f,0.057f,180f);
                if (SwordEquipped != null && SwordEquipped == true ) sword.GetComponent<SpriteRenderer>().sortingOrder = 3;
                if (BowEquipped == true ) 
                {
                    bow.GetComponent<SpriteRenderer>().sortingOrder = 3;
                } 
                break;
                
            case PlayerMovement.Looking.Right:
                if (SwordEquipped != null && SwordEquipped == true ) swordAim.transform.localPosition = new Vector3(-0.05f,0.043f,0);
                if (SwordEquipped != null && SwordEquipped == true ) sword.GetComponent<SpriteRenderer>().sortingOrder = 3;
                
                if (BowEquipped == true ) 
                {
                    bow.GetComponent<SpriteRenderer>().sortingOrder = 3;
                } 

                break;

            case PlayerMovement.Looking.Up:
                if (SwordEquipped != null && SwordEquipped == true ) swordAim.transform.localPosition = new Vector3(0.052f,0.055f,0);
                if (SwordEquipped != null && SwordEquipped == true ) sword.GetComponent<SpriteRenderer>().sortingOrder = 1;  
                if (BowEquipped == true ) 
                {
                    bow.GetComponent<SpriteRenderer>().sortingOrder = 1;
                } 
                break;

            case PlayerMovement.Looking.Down:
                if (SwordEquipped != null && SwordEquipped == true ) swordAim.transform.localPosition = new Vector3(-0.0287f,0.0485f,0);
                if (SwordEquipped != null && SwordEquipped == true ) sword.GetComponent<SpriteRenderer>().sortingOrder = 3;  

                if (BowEquipped == true ) 
                {
                    bow.GetComponent<SpriteRenderer>().sortingOrder = 3;
                } 
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
            SetMeleeAsActiveWeapon();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && BowEquipped == false)
        {
            SetRangedAsActiveWeapon();
        }


        Vector3 mousePOS = Camera.main.ScreenToWorldPoint (Input.mousePosition);
        mousePOS.z = 0f;

        Vector3 aimDirection = (mousePOS - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        if (SwordEquipped)
        {      
            swordAim.transform.eulerAngles = new Vector3(0,0,angle);
        }

        if (GetComponent<PlayerCombat>().rangedWeaponEquipped)
        {
            bowAim.transform.eulerAngles = new Vector3(0,0,angle);
        }

    }
}