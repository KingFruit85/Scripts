using System.Collections;
using UnityEngine;
using static PlayerMovement;

public class Dash : MonoBehaviour
{
    public Rigidbody2D rb;
    public float dashSpeed = 80f;
    public float dashCoolDown = -9999;
    public float dashDelay = 2f;
    private Animator anim;
    private Shaker shaker;
    private bool canDash;
    private bool isDashing;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        shaker = GameObject.Find("Main Camera").GetComponent<Shaker>();
    }

    void Update()
    {
        if (Time.time > dashCoolDown + dashDelay)
        {
            canDash = true;
            dashCoolDown = Time.time;
        }
    }

    public bool isPlayerDashing()
    {
        return isDashing;
    }

    private IEnumerator ToggleIsDashingBool()
    {
        isDashing = true;
        // rb.isKinematic = true;
        yield return new WaitForSeconds( 0.3f );
        // rb.isKinematic = false;
        isDashing = false;
    }

    public void DashAbility(Looking direction)
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
                                // shaker.CombatShaker("Up");
                                dashCoolDown = Time.time;
                                break;

                case Looking.Down:
                                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y -dashSpeed); 
                                anim.Play("Human_Dash_Down");
                                // shaker.CombatShaker("Down");
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
                                // shaker.CombatShaker("Right");
                                dashCoolDown = Time.time;
                                break;
            }

        }
        
    }
}
