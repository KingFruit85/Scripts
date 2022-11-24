using System.Collections;
using UnityEngine;


public class AIMovement : MonoBehaviour
{
    public enum State
    {
        Roaming,
        ChaseTarget,
        GoingBackToStart,
        Attacking,
    }

    private State state;
    private Vector2 startingPosition;
    private Vector2 roamingPosition;
    private float distanceApart;
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float aggroRange = 5f;
    private Vector3 playerPOS;
    private Rigidbody2D rb;
    [SerializeField]
    private bool isSlowed;

    void Awake()
    {
        state = State.Roaming;
        startingPosition = transform.position;
        roamingPosition = GetRoamingPosition();
        rb = GetComponent<Rigidbody2D>();

        switch (transform.tag)
        {
            default:
            case "Ghost": gameObject.AddComponent<Ghost>(); break;
            case "Worm": gameObject.AddComponent<Worm>(); break;
        }
    }

    private Vector3 GetRoamingPosition()
    {
        // Get random direction
        var RD = new Vector2(UnityEngine.Random.Range(-1f, 1f),
                             UnityEngine.Random.Range(-1f, 1f)
                            ).normalized;

        return startingPosition + RD * Random.Range(1f, 5f);

    }

    void Update()
    {

        playerPOS = GameObject.FindGameObjectWithTag("Player").transform.position;
        distanceApart = Vector2.Distance(transform.position, playerPOS);

        switch (state)
        {
            default: throw new System.Exception("Invalid AI movement state");

            case State.Roaming:
                Patrol();
                FindTarget();
                break;

            case State.ChaseTarget:
                MoveToPlayer();
                break;

            case State.GoingBackToStart:
                ReturnToStartPoint();
                break;

            case State.Attacking:
                Attack();
                break;
        }

        // Visual for slowed effect
        if (isSlowed)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
        }
        else if (!isSlowed)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    private void Attack() { }

    private void FindTarget()
    {
        if (distanceApart < aggroRange)
        {
            state = State.ChaseTarget;
        }
        else if (distanceApart > aggroRange)
        {
            state = State.Roaming;
        }
    }

    private void Patrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, roamingPosition, speed * Time.deltaTime);
        float distanceToTarget = Vector3.Distance(transform.position, roamingPosition);

        if (distanceToTarget <= .1f)
        {
            roamingPosition = GetRoamingPosition();
        }
    }

    public void ReturnToStartPoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, startingPosition, speed * Time.deltaTime);
        float distanceToTarget = Vector3.Distance(transform.position, startingPosition);

        if (distanceToTarget <= .5f)
        {
            state = State.Roaming;
        }
    }

    private void MoveToPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerPOS, speed * Time.deltaTime);

        if (distanceApart > 15f)
        {
            state = State.GoingBackToStart;
        }
    }

    public void KnockBack(PlayerMovement.Looking direction)
    {
        var knockDirection = "";

        if (direction == PlayerMovement.Looking.Left) knockDirection = "Left";
        if (direction == PlayerMovement.Looking.Right) knockDirection = "Right";
        if (direction == PlayerMovement.Looking.Up) knockDirection = "Up";
        if (direction == PlayerMovement.Looking.Down) knockDirection = "Down";

        StartCoroutine(Knock(knockDirection));
    }

    public IEnumerator Knock(string direction)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        var thrust = 1.5f;


        // Hit came from left so knock left
        if (direction == "Left")
        {
            rb.AddForce(-transform.right * thrust, ForceMode2D.Impulse);
        }

        // Hit came from right so knock right
        else if (direction == "Right")
        {
            rb.AddForce(transform.right * thrust, ForceMode2D.Impulse);
        }

        // Hit came from up so knock up
        else if (direction == "Up")
        {
            rb.AddForce(transform.up * thrust, ForceMode2D.Impulse);
        }

        // Hit came from down so knock down
        else if (direction == "Down")
        {
            rb.AddForce(-transform.up * thrust, ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(thrust);
        StartCoroutine(EndKnockBack());
    }

    IEnumerator EndKnockBack()
    {
        rb.velocity = new Vector2(0, 0);
        rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        transform.position = this.transform.position;
        yield return new WaitForSeconds(0.5f);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall")
        {
            state = State.GoingBackToStart;
        }
    }

    public void DazeForSeconds(int seconds)
    {
        isSlowed = true;
        StartCoroutine(SlowSpeed(seconds));
    }

    public IEnumerator SlowSpeed(int seconds)
    {
        // Half speed for provided seconds
        speed = speed / 2;
        yield return new WaitForSeconds(seconds);

        //Restore to default speed
        speed = speed * 2;
        isSlowed = false;
    }
}
