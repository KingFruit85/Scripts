using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private SpriteRenderer SR;
    [SerializeField]
    private string currentSprite;
    private Vector2 attackPoint;
    private Vector2 attackPointLeft;
    private Vector2 attackPointRight;
    private Vector2 attackPointUp;
    private Vector2 attackPointDown;
    public float attackRange = 0.5f;
    private LayerMask enemyLayers;
    public Animator an;
    public int attackDamage = 30;
    public string rangedWeaponName;
    private bool canShoot = false;
    private bool rangedWeaponEquipped = false;
    private float rangedCooldown = -9999;
    private float arrowSpeed = 1;
    private int arrowDamage = 10;
    private float rangedAttackDelay = 0.5f;
    private Shaker shaker;
    private bool arrowKnocked = false;
    private PlayerMovement.Looking playerIsLooking;

    public PlayerStats PS { get; private set; }

    void Start()
    {
        enemyLayers = LayerMask.GetMask("enemies");
        GameObject.Find("Player").AddComponent<Human>();
        SR = GetComponent<SpriteRenderer>();
        an = GetComponent<Animator>();
        shaker = GameObject.Find("Main Camera").GetComponent<Shaker>();
    }

    public void SetRangedWeaponEquipped(bool x)
    {
        rangedWeaponEquipped = x;
    }

    private void SetCurrentSprite()
    {
        // Gets the current player sprite with the junk text trimmed off
        currentSprite = SR.sprite.ToString();
        currentSprite = currentSprite.Substring(0,currentSprite.LastIndexOf(" " ) - 1).Trim();
    }

    public string GetCurrentSprite()
    {
        return currentSprite;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(attackPointUp, attackRange);
        Gizmos.DrawSphere(attackPointDown, attackRange);
        Gizmos.DrawSphere(attackPointLeft, attackRange);
        Gizmos.DrawSphere(attackPointRight, attackRange);
    }

    void SetAttackPoint()
    {
        var pos = transform.position;
        // Set the ranges
        attackPointLeft = new Vector2(pos.x - attackRange ,pos.y - 0.1f);
        attackPointRight = new Vector2(pos.x + attackRange ,pos.y - 0.1f);

        attackPointUp = new Vector2(pos.x - .5f + attackRange ,pos.y + 0.7f);
        attackPointDown = new Vector2(pos.x - .5f +attackRange ,pos.y - 0.7f);

        
        // Set the attackable area basd on the current posistion of the player
        switch (currentSprite)
        { 
            case "player_idle_left":
                attackPoint = attackPointLeft;
                break;
            case "player_move_left":
                attackPoint = attackPointLeft;
                break;

            case "player_idle_right":
                attackPoint = attackPointRight;
                break;
            case "player_move_right":
                attackPoint = attackPointRight;
                break;

            case "player_idle_up":
                attackPoint = attackPointUp;
                break;
            case "player_move_up":
                attackPoint = attackPointDown;
                break;
            
            case "player_idle_down":
                attackPoint = attackPointDown;
                break;
            case "player_move_down":
                attackPoint = attackPointDown;
                break;

            default:return;
        }
    }

    void Attack()
    {
        switch (playerIsLooking)
        {
            default: throw new System.Exception("PlayerMovement.Looking state not valid");
            
            case PlayerMovement.Looking.Left:
                an.Play("Human_Attack_Left"); 
                shaker.CombatShaker("Left");
                break;

            case PlayerMovement.Looking.Right:
                an.Play("Human_Attack_Right"); 
                shaker.CombatShaker("Right");
                break;

            case PlayerMovement.Looking.Up:
                an.Play("Human_Attack_Up"); 
                shaker.CombatShaker("Up");
                break;

            case PlayerMovement.Looking.Down:
                an.Play("Human_Attack_Down"); 
                shaker.CombatShaker("Down");
                break;
        }

        // Detect enemies in range of attack
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint, attackRange, enemyLayers);

            // Damage them
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Health>().TakeDamage(attackDamage);   
            }
    }

    void KnockArrow()
    {
        arrowKnocked = true;

        if (TryGetComponent(out BowPickup bp))
        {
            GetComponent<PlayAnimations>().SetPlayerKnockedAnimation();
            if (playerIsLooking == PlayerMovement.Looking.Left)
            {
                an.Play("Human_KnockArrow_Left");
            }

            if (playerIsLooking == PlayerMovement.Looking.Right)
            {
                an.Play("Human_KnockArrow_Right");
            }
        }

        if (TryGetComponent(out GoldBowPickup gbp))
        {
            GetComponent<PlayAnimations>().SetPlayerKnockedAnimation();
            if (playerIsLooking == PlayerMovement.Looking.Left)
            {
                an.Play("Human_KnockArrowGOLD_Left");
            }

            if (playerIsLooking == PlayerMovement.Looking.Right)
            {
                an.Play("Human_KnockArrowGOLD_Right");
            }
        } 
    }

    void RangedAttack()
    {
        if (canShoot == true)
        {
            canShoot = false;
            arrowKnocked = false;

            if (playerIsLooking == PlayerMovement.Looking.Left)
            {
                GetComponent<PlayAnimations>().ResetPlayerAnimations();
            }

            if (rangedWeaponName == "Short Bow")
            {
                GetComponent<BowPickup>().ShootBow();
                PS.RemoveArrows(1);

            }
            else if (rangedWeaponName == "Gold Bow")
            {
                GetComponent<GoldBowPickup>().ShootBow();
                PS.RemoveArrows(1);

            }
        }
    }

    public void setRangedAttack(float speed, int damage, float attackDelay)
    {
        arrowSpeed = speed;
        arrowDamage = damage;
        rangedAttackDelay = attackDelay;
    }

    public void Update()
    {
        playerIsLooking = GameObject.Find("Player")
                                    .GetComponent<PlayerMovement>()
                                    .playerIsLooking();

        PS = GetComponent<PlayerStats>();
        if (rangedWeaponEquipped == true && PS.getArrowCount() > 0)
        {
                if (Time.time > rangedCooldown + rangedAttackDelay)
                {
                    canShoot = true;
                    rangedCooldown = Time.time;
                }
        }
        SetCurrentSprite();
        GetCurrentSprite();
    }

    void LateUpdate()
    {
        SetAttackPoint();  
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        if(Input.GetMouseButtonDown(1) && rangedWeaponEquipped)
        {
            KnockArrow();
        }

        if (Input.GetMouseButtonUp(1) && arrowKnocked)
        {
            RangedAttack();
        }
    }
}
