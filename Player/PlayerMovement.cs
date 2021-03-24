using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Vector2 movement;
    public Vector3 mousePOS;
    public Camera cam;
    public float moveSpeed;
    private Rigidbody2D rb;
    public SpriteRenderer sr;
    public bool isSlowed = false;
    public GameObject player;
    public Vector2 lookDir;
    public PlayAnimations pa;

    // private Transform aimTransform;

    public enum Looking
    {
        Up,
        Down,
        Left,
        Right
    }

    public Looking looking;

    void Awake()
    {
        // aimTransform = transform.Find("Aim");
        rb = GetComponent<Rigidbody2D>();       
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");
        pa = GetComponent<PlayAnimations>();
    }
    void Start()
    {  
        
        // sword = GameObject.FindGameObjectWithTag("PlayerSword").GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            FindObjectOfType<GameManager>().LoadNextLevel();
        }
    }

    

    void Update()
    {

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePOS = cam.ScreenToWorldPoint(Input.mousePosition);

        

        if (lookDir.x > 0f && Mathf.Abs(lookDir.x) > Mathf.Abs(lookDir.y))
        {
            looking = Looking.Right;
            pa.ChangeAnimationState(pa.idleRight);
        }
        if (lookDir.x < 0f && Mathf.Abs(lookDir.x) > Mathf.Abs(lookDir.y))
        {
            looking = Looking.Left;
            pa.ChangeAnimationState(pa.idleLeft);
        }
        if (lookDir.y > 0f && Mathf.Abs(lookDir.x) < Mathf.Abs(lookDir.y))
        {
            looking = Looking.Up;
            pa.ChangeAnimationState(pa.idleUp);
        }
        if (lookDir.y < 0f && Mathf.Abs(lookDir.x) < Mathf.Abs(lookDir.y))
        {
            looking = Looking.Down;
            pa.ChangeAnimationState(pa.idleDown);
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
        // float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg -90f;
        // sword.rotation = angle;
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

        // If in mid dash dont change directon
        if (GetComponent<Human>() && GetComponent<Human>().isPlayerDashing())
        {
            return;
        }
        else
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            // rb.velocity = new Vector2(moveX, moveY);
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

