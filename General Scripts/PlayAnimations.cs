using System;
using System.Collections;
using UnityEngine;

public class PlayAnimations : MonoBehaviour
{
    private string idleLeft, idleRight, walkLeft, walkRight, 
                   death, currentState, lastFacingDirection,
                   walkDown,walkUp,idleDown,idleUp;
    public Animator animator;
    public Vector2 direction;
    public Vector2 previousDirection;
    private Rigidbody2D rb;
    private Vector2 movement;
    private AIMovement AIMovement;  

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (TryGetComponent(out AIMovement aim))
        {
            AIMovement = GetComponent<AIMovement>();
        }

        if (gameObject.tag == "Ghost")
        {
            idleLeft = "Ghost_Idle_Left";
            idleRight = "Ghost_Idle_Right";
            walkLeft = "Ghost_Walk_Left";
            walkRight = "Ghost_Walk_Right";
            walkDown = "Ghost_Walk_Down";
            walkUp = "Ghost_Walk_Up";
            death = "Ghost_Death";
        }
        
        if (gameObject.tag == "Player")
        {
            idleLeft = "Human_Idle_Left";
            idleRight = "Human_Idle_Right";
            walkLeft = "Human_Walk_Left";
            walkRight = "Human_Walk_Right";    
            walkUp = "Human_Move_Up";
            walkDown = "Human_Move_Down";
            idleUp = "Human_Idle_Up";
            idleDown = "Human_Idle_Down";
        }

        if (gameObject.tag == "Worm")
        {
            idleLeft = "worm_walk";
            idleRight = "worm_walk_right";
            walkLeft = "worm_walk";
            walkRight = "worm_walk_right";
            death = "worm_death";
        }
    }

    // public void LoadAnimations(string type)
    // {
    //     // here i need to be able to dynamically load differnt sprite/animations to the player based on whatever killed them

    //     switch (type)
    //     {
    //         default:
    //         case "Human": return 
    //     }
    // }

    public void SetPlayerKnockedAnimation()
    {
        if (gameObject.tag == "Player" && TryGetComponent(out BowPickup bp))
        {
            idleLeft = "Human_KnockArrow_Left";
            idleRight = "Human_KnockArrow_Right";
            walkLeft = "Human_KnockArrow_Left";
            walkRight = "Human_KnockArrow_Right"; 
        }

        if (gameObject.tag == "Player" && TryGetComponent(out GoldBowPickup gbp))
        {
            idleLeft = "Human_KnockArrowGOLD_Left";
            idleRight = "Human_KnockArrowGOLD_Right";
            walkLeft = "Human_KnockArrowGOLD_Left";
            walkRight = "Human_KnockArrowGOLD_Right"; 
        }
    }

    public void ResetPlayerAnimations()
    {
        idleLeft = "Human_Idle_Left";
        idleRight = "Human_Idle_Right";
        walkLeft = "Human_Walk_Left";
        walkRight = "Human_Walk_Right";  
    }

    public void resetBoolTriggers()
    {
        animator.SetBool("WalkLeft", false);
        animator.SetBool("WalkRight", false);
    }

    public Vector3 previousPosition;
    public Vector3 currentMovementDirection;
    public int signX;
    public int signY;

    public void Update()
    {

        //Get AI movement direction
        if(previousPosition != transform.position && gameObject.tag != "Player") 
        {
            currentMovementDirection = (previousPosition - transform.position);
            signX = Math.Sign(currentMovementDirection.x);
            signY = Math.Sign(currentMovementDirection.y);
            previousPosition = transform.position;
        }

        
        direction = transform.position;

        if (gameObject.tag != "Player")
        {
            if (signX == -1)
            {
                ChangeAnimationState(walkLeft);
                previousDirection = direction;
            }

            else if (signX == 1)
            {
                ChangeAnimationState(walkRight);
                previousDirection = direction;
            }

            else if (signY == 1)
            {
                ChangeAnimationState(walkUp);
                previousDirection = direction;
            }

            else if (signY == -1)
            {
                ChangeAnimationState(walkDown);
                previousDirection = direction;
            }
        }
        //Player
        else
        {
            if (movement.x == 1 && movement.y == 0)
            {
                ChangeAnimationState(walkRight);
                lastFacingDirection = "right";
            }
            else if (movement.x == -1 && movement.y == 0)
            {
                ChangeAnimationState(walkLeft);
                lastFacingDirection = "left";
            }
            else if (movement.x == 0 && movement.y == 1)
            {
                ChangeAnimationState(walkUp);
                lastFacingDirection = "up";
            }
            else if (movement.x == 0 && movement.y == -1)
            {
                ChangeAnimationState(walkDown);
                lastFacingDirection = "down";
            }
        }

        //If player/AI stops moving, set it's sprite to the idle animaton of it's prevoud movement direction
        SetSpriteDirection();
        
    }

    public void FixedUpdate()
    {
        // add if condition in animation logic to chck if player or AI
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    public void ChangeAnimationState(string newState)
    {
        // Stop the animation from playing over itself
        if (currentState == newState) return;

        // Play the animation
        animator.Play(newState);

        // Reassign the current state
        currentState = newState;
    }

    private void SetSpriteDirection()
    {
        if (rb.velocity.x == 0 && rb.velocity.y == 0)
        {
            if (lastFacingDirection == "right")
            {    
                ChangeAnimationState(idleRight);
            }
            else if (lastFacingDirection == "left")
            {
                ChangeAnimationState(idleLeft);
            }
            else if (lastFacingDirection == "up")
            {
                ChangeAnimationState(idleUp);
            }
            else if (lastFacingDirection == "down")
            {
                ChangeAnimationState(idleDown);
            }
        }
    }

    public IEnumerator Kill()
    {
            ChangeAnimationState(death);

            yield return new WaitForSeconds(1.6f);
            Destroy(this.gameObject);
    }
}
