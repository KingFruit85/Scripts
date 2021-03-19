using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAI : MonoBehaviour
{
    public enum State
    {
        Roaming,
        ChaseTarget,
        GoingBackToStart,
        Attacking,
    }

    private Vector2 startingPosition;
    private Vector2 roamingPosition;
    private float distanceApart = 0f;
    public float speed = 1f;
    public State state;
    private Vector3 playerPOS;
    private bool facingLeft;

    public bool isFacingLeft()
    {
        return facingLeft;
    }

    void Awake()
    {
        state = State.Roaming;
    }

    void Start()
    {
        startingPosition = transform.position;
        roamingPosition = GetRoamingPosition();
    }

    void Update()
    {

        playerPOS = GameObject.FindGameObjectWithTag("Player").transform.position;
        distanceApart = Vector2.Distance(transform.position, playerPOS);


        switch (state)
        {
            default:

            case State.Roaming: MoveTo("Patrol"); FindTarget();
            break; 

            case State.ChaseTarget: MoveTo("Player");
            break;

            case State.GoingBackToStart:MoveTo("Start"); 
            break;

            case State.Attacking:Attack();
            break;

        }

    }

    private void Attack()
    {

    }

    private Vector3 GetRoamingPosition()
    {
        // Get random direction
        var RD = new Vector2(UnityEngine.Random.Range(-1f,1f), UnityEngine.Random.Range(-1f,1f)).normalized;

        return startingPosition + RD * Random.Range(1f,5f);

    }

    private void MoveTo(string target)
    {
        if (target == "Patrol")
        {
            transform.position= Vector3.MoveTowards(transform.position, roamingPosition, speed * Time.deltaTime);

            float reachedPositionDistance = .1f;
            float d = Vector3.Distance(transform.position, roamingPosition);

            if ( d <= reachedPositionDistance)
            {
                roamingPosition = GetRoamingPosition();
            }  
        }

        if (target == "Start")
        {
            transform.position = Vector3.MoveTowards(transform.position, startingPosition, speed * Time.deltaTime);

            float reachedPositionDistance = .5f;
            float d = Vector3.Distance(transform.position, startingPosition);

            if ( d <= reachedPositionDistance)
            {
                state = State.Roaming;
            }  
        }

        if (target == "Player")
        {
            var bottomLeftSide = new Vector3(playerPOS.x -1,playerPOS.y -1,0);
            var bottomRightSide = new Vector3(playerPOS.x +1,playerPOS.y -1,0);


            var topLeftSide = new Vector3(playerPOS.x -1,playerPOS.y +1,0);
            var topRightSide = new Vector3(playerPOS.x +1,playerPOS.y +1,0);

            var PC = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<PlayerMovement>();
            
            facingLeft = PC.PlayerIsLookingLeft();

            if (transform.position.y > playerPOS.y && facingLeft)
            {
                speed = 2f;
                transform.position= Vector3.MoveTowards(transform.position, topLeftSide, speed * Time.deltaTime);
            }
            else if (transform.position.y < playerPOS.y && facingLeft)
            {
                speed = 2f;
                transform.position= Vector3.MoveTowards(transform.position, bottomLeftSide, speed * Time.deltaTime);
            }
            else if (transform.position.y > playerPOS.y && !facingLeft)
            {
                speed = 2f;
                transform.position= Vector3.MoveTowards(transform.position, topRightSide, speed * Time.deltaTime);
            }
            else if (transform.position.y < playerPOS.y && !facingLeft)
            {
                speed = 2f;
                transform.position= Vector3.MoveTowards(transform.position, bottomRightSide, speed * Time.deltaTime);
            }

            speed = 1f;

            float stopChaseDistance = 5f;

            if (distanceApart > stopChaseDistance)
            {
                state = State.GoingBackToStart;
            }
        }
    }


    private void FindTarget()
    {
        float targetRange = 5f;
        if (distanceApart < targetRange)
        {
            // Player is in target range
            state = State.ChaseTarget;            

        }
        else if (distanceApart > targetRange)
        {
            state = State.Roaming;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Wall")
        {
            Debug.Log("I've hit a bloody wall!");
            state = State.GoingBackToStart;
            
        }

        if (other.tag == "Player")
        {
            Debug.Log("I've hit a bloody player!");
            // GetComponent<GhostAttacks>().TakeDamage(0, true);
            
        }

    }



}
