using System.Collections;
using UnityEngine;
public class Human : MonoBehaviour
{
    private string idleLeft;
    private string idleRight;
    private string walkLeft;
    private string walkRight;
    private string walkUp;
    private string walkDown;
    private string idleUp;
    private string idleDown;
    private string death;
    private string knockLeft;
    private string knockRight;
    private string knockUp;
    private string knockDown;
    public int swordDamage = 10;
    public float swordRange = 0.5f;

    public Human()
    {
        idleLeft = "Human_Idle_Left";
        idleRight = "Human_Idle_Right";
        walkLeft = "Human_Walk_Left";
        walkRight = "Human_Walk_Right";    
        walkUp = "Human_Move_Up";
        walkDown = "Human_Move_Down";
        idleUp = "Human_Idle_Up";
        idleDown = "Human_Idle_Down";
        // death = "Human_Death";
        knockLeft = "Human_KnockArrow_Left";
        knockRight = "Human_KnockArrow_Right";
    }

    public float dashSpeed = 80f;
    public float dashCoolDown = -9999;
    public float dashDelay = 2f;
    private bool canDash;
    private bool isDashing;

    public void Dash()
    {
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
        // rb.isKinematic = true;
        yield return new WaitForSeconds( 0.3f );
        // rb.isKinematic = false;
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
    }

}