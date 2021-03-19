using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Vector2 movement;
    public float moveSpeed;
    private Rigidbody2D rb;
    public SpriteRenderer sr;
    public bool isSlowed = false;
    public enum Looking
    {
        Up,
        Down,
        Left,
        Right
    }

    public Looking looking;


    void Start()
    {  
        rb = GetComponent<Rigidbody2D>();       
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            FindObjectOfType<GameManager>().LoadNextLevel();
        }
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Dash>().DashAbility(looking);
        }
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        Move();

        if (movement.x == -1 && movement.y == 0)
        {
            looking = Looking.Left;
        }
        if (movement.x == 1 && movement.y == 0)
        {
            looking = Looking.Right;
        }
        if (movement.x == 0 && movement.y == 1)
        {
            looking = Looking.Up;
        }
        if (movement.x == 0 && movement.y == -1)
        {
            looking = Looking.Down;
        }

        

        // Visual for slowed effect
        if (isSlowed)
        {
            sr.color = Color.green;
        }
        else if (!isSlowed)
        {
            sr.color = Color.white;
        }
    }

    public Looking playerIsLooking()
    {
        return looking;
    }

    public bool PlayerIsLookingLeft()
    {
        if (looking == Looking.Left)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Move()
    {
        float moveX = movement.x * moveSpeed;
        float moveY = movement.y * moveSpeed;

        if (GameObject.Find("Player").GetComponent<Dash>().isPlayerDashing())
        {
            return;
        }
        else
        {
            rb.velocity = new Vector2(moveX, moveY);
        }
    }

    public void DazeForSeconds(int seconds)
    {
        isSlowed = true;
        StartCoroutine(SlowSpeed(seconds));  
    }

    private IEnumerator SlowSpeed(int seconds)
    {   
        // Half player speed for provided seconds
        moveSpeed = moveSpeed / 2;
        yield return new WaitForSeconds(seconds);

        //Restore to default speed
        moveSpeed = moveSpeed * 2;
        isSlowed = false;
    }

    }

