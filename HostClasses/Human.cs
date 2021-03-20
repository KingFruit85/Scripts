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
    public string knockLeft = "Human_KnockArrow_Left";
    public string knockRight = "Human_KnockArrow_Right";

    public string attackLeft = "Human_Attack_Left";
    public string attackRight = "Human_Attack_Right";
    public string attackUp = "Human_Attack_Up";
    public string attackDown = "Human_Attack_Down";

    public string knockUp;
    public string knockDown;

    public int swordDamage = 10;
    public float swordRange = 0.5f;

    private Rigidbody2D rb;
    private Animator anim;
    private Shaker shaker;

    public float moveSpeed = 5;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        shaker = GameObject.Find("Main Camera").GetComponent<Shaker>();
        GetComponent<PlayerMovement>().moveSpeed = moveSpeed;
    }

    public float dashSpeed = 10f;
    public float dashCoolDown = -9999;
    public float dashDelay = 2f;
    private bool canDash = true;
    private bool isDashing = false;

    public void Dash()
    {
        var direction = GetComponent<PlayerMovement>().looking;
        if (canDash)
        {
            canDash = false;
            StartCoroutine(ToggleIsDashingBool());

            switch (direction)
            {   
                default: throw new System.Exception("invalid dash direction provided");
                case Looking.Up:
                                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + dashSpeed); 
                                anim.Play("Human_Dash_Up");
                                shaker.CombatShaker("Up");
                                dashCoolDown = Time.time;
                                break;

                case Looking.Down:
                                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y -dashSpeed); 
                                anim.Play("Human_Dash_Down");
                                shaker.CombatShaker("Down");
                                dashCoolDown = Time.time;
                                break;

                case Looking.Left:
                                rb.velocity = new Vector2(rb.velocity.x - dashSpeed, rb.velocity.y);
                                anim.Play("Human_Dash_Left");
                                dashCoolDown = Time.time;
                                break;

                case Looking.Right:
                                rb.velocity = new Vector2(rb.velocity.x + dashSpeed, rb.velocity.y);
                                anim.Play("Human_Dash_Right");
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

    public void SwordAttack(Vector2 attackPoint, LayerMask enemyLayers)
    {
        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint, swordRange, enemyLayers);

        // Damage them
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Health>().TakeDamage(swordDamage);   
        }
    }

    public void BowAttack(string bow)
    {
        switch (bow)
        {
            default: throw new System.Exception("Supplied bow name not recognised");
            
            case "Short Bow":
                GetComponent<BowPickup>().ShootBow();
                break;

            case "Gold Bow":
                GetComponent<GoldBowPickup>().ShootBow();
                break;
        }
    }

    void Update()
    {
        if (Time.time > dashCoolDown + dashDelay)
        {
            canDash = true;
            dashCoolDown = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Dash();
        }
    }

}