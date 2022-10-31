using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 movement;
    public Vector3 mousePOS;
    public Camera cam;
    public float moveSpeed;
    [SerializeField]
    private bool canMove = true;
    private Rigidbody2D rb;
    public SpriteRenderer sr;
    public bool isSlowed = false;
    public GameObject player;
    public Vector2 lookDir;
    public Looking looking;
    public bool isMoving;


    public enum Looking
    {
        Up,
        Down,
        Left,
        Right
    }


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();       
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x == 0 && movement.y == 0)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }

        mousePOS = cam.ScreenToWorldPoint(Input.mousePosition);

        if (lookDir.x > 0f && Mathf.Abs(lookDir.x) > Mathf.Abs(lookDir.y))
        {
            looking = Looking.Right;
        }
        if (lookDir.x < 0f && Mathf.Abs(lookDir.x) > Mathf.Abs(lookDir.y))
        {
            looking = Looking.Left;
        }
        if (lookDir.y > 0f && Mathf.Abs(lookDir.x) < Mathf.Abs(lookDir.y))
        {
            looking = Looking.Up;
        }
        if (lookDir.y < 0f && Mathf.Abs(lookDir.x) < Mathf.Abs(lookDir.y))
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

    void FixedUpdate()
    {
        Move();
        lookDir = mousePOS - player.transform.position;
    }

    public void StopPlayerMovement()
    {
         canMove = false;
    }

    public void StartPlayerMovement()
    {
         canMove = true;
    }

    public Looking PlayerIsLooking()
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
        if (canMove)
        {
            float moveX = movement.x * moveSpeed;
            float moveY = movement.y * moveSpeed;

            // If in mid dash dont change directon
            if (GetComponent<Human>() && GetComponent<Human>().isPlayerDashing())
            {
                return;
            }
            else
            {
                rb.velocity = new Vector2(moveX, moveY);
            }
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
        moveSpeed = moveSpeed / 8;
        yield return new WaitForSeconds(seconds);

        //Restore to default speed
        moveSpeed = moveSpeed * 8;
        isSlowed = false;
    }

    }

