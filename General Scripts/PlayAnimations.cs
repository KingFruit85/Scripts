using System;
using System.Collections;
using UnityEngine;

public class PlayAnimations : MonoBehaviour
{
    public string idleLeft, idleRight, walkLeft, walkRight, 
                   death, currentState, lastFacingDirection,
                   walkDown,walkUp,idleDown,idleUp,attackLeft,attackRight,
                   attackUp,attackDown;

    public Animator animator;
    public Vector2 direction;
    public Vector2 previousDirection;
    private Rigidbody2D rb;
    private Vector2 movement;
    private AIMovement AIMovement;  

    public Human human;
    public Ghost ghost;
    public Worm worm;


    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        human = GetComponent<Human>();


        if (TryGetComponent(out AIMovement aim))
        {
            AIMovement = GetComponent<AIMovement>();
        }

        // if (gameObject.tag == "Ghost")
        // {
        //     ghost = GetComponent<Ghost>();

        //     idleLeft = ghost.idleLeft;
        //     idleRight = ghost.idleRight;
        //     walkLeft = ghost.walkLeft;
        //     walkRight = ghost.walkRight;
        //     walkUp = ghost.walkUp;
        //     walkDown = ghost.walkDown;
        //     idleUp = ghost.idleUp;
        //     idleDown = ghost.idleDown;
        //     death = ghost.death;
        //     attackLeft = ghost.attackLeft;
        //     attackRight = ghost.attackRight;
        //     attackUp = ghost.attackUp;
        //     attackDown = ghost.attackDown;
        // }
        
        if (gameObject.tag == "Player")
        {
            idleLeft = human.idleLeft;
            idleRight = human.idleRight;
            walkLeft = human.walkLeft;
            walkRight = human.walkRight;
            walkUp = human.walkUp;
            walkDown = human.walkDown;
            idleUp = human.idleUp;
            idleDown = human.idleDown;
            attackLeft = human.attackLeft;
            attackRight = human.attackRight;
            attackUp = human.attackUp;
            attackDown = human.attackDown;
        }

        if (gameObject.tag == "Worm")
        {
            idleLeft = worm.idleLeft;
            idleRight = worm.idleRight;
            walkLeft = worm.walkLeft;
            walkRight = worm.walkRight;
            walkUp = worm.walkUp;
            walkDown = worm.walkDown;
            death = worm.death;
            attackLeft = worm.attackLeft;
            attackRight = worm.attackRight;
            attackUp = worm.attackUp;
            attackDown = worm.attackDown;
        }
    }


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

    public void ResetHumanAnimations()
    {
        idleLeft = human.idleLeft;
        idleRight = human.idleRight;
        walkLeft = human.walkLeft;
        walkRight = human.walkRight;  
    }

    public void ResetGhostAnimations()
    {
        idleLeft = ghost.idleLeft;
        idleRight = ghost.idleRight;
        walkLeft = ghost.walkLeft;
        walkRight = ghost.walkRight;  
    }

    public void ResetWormAnimations()
    {
        idleLeft = worm.idleLeft;
        idleRight = worm.idleRight;
        walkLeft = worm.walkLeft;
        walkRight = worm.walkRight;  
    }

    public void ResetPlayerAnimations()
    {
        switch (GetComponent<PlayerStats>().currentHost)
        {
            default:
            case "Human": ResetHumanAnimations();break;
            case "Ghost": ResetGhostAnimations();break;
            case "Worm" : ResetWormAnimations();break;
        }
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

    // Sets the active sprite to an idle sprite if the host stops moving
    // private void SetSpriteDirection()
    // {   
    //     string ch = GetComponent<PlayerStats>().currentHost;

    //     if (rb.velocity.x == 0 && rb.velocity.y == 0)
    //     {
    //         if (lastFacingDirection == "right")
    //         {
    //             switch (ch)
    //             {
    //                 default:
    //                 case "Human":ChangeAnimationState(human.idleRight);break;
    //                 case "Ghost":ChangeAnimationState(ghost.idleRight);break;
    //                 case "Worm" :ChangeAnimationState(worm.idleRight);break;
    //             }
    //         }

    //         else if (lastFacingDirection == "left")
    //         {
    //             switch (ch)
    //             {
    //                 default:
    //                 case "Human":ChangeAnimationState(human.idleLeft);break;
    //                 case "Ghost":ChangeAnimationState(ghost.idleLeft);break;
    //                 case "Worm" :ChangeAnimationState(worm.idleLeft);break;
    //             }
    //         }

    //         else if (lastFacingDirection == "up")
    //         {
    //             switch (ch)
    //             {
    //                 default:
    //                 case "Human":ChangeAnimationState(human.idleUp);break;
    //                 case "Ghost":ChangeAnimationState(ghost.idleUp);break;
    //                 case "Worm" :ChangeAnimationState(worm.idleUp);break;
    //             }
    //         }

    //         else if (lastFacingDirection == "down")
    //         {
    //             switch (ch)
    //             {
    //                 default:
    //                 case "Human":ChangeAnimationState(human.idleDown);break;
    //                 case "Ghost":ChangeAnimationState(ghost.idleDown);break;
    //                 case "Worm" :ChangeAnimationState(worm.idleDown);break;
    //             }
    //         }
    //     }
    // }

    public IEnumerator Kill()
    {
            ChangeAnimationState(death);

            yield return new WaitForSeconds(1.6f);
            Destroy(this.gameObject);
    }
}
