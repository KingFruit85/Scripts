using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public string idleLeft = "Ghost_Idle_Left";
    public string idleRight = "Ghost_Idle_Right";
    public string walkLeft = "Ghost_Walk_Left";
    public string walkRight = "Ghost_Walk_Right";
    public string walkUp = "Ghost_Walk_Up";
    public string walkDown = "Ghost_Walk_Down";
    public string idleUp = "Ghost_Walk_Up"; // Replace this when posable
    public string idleDown = "Ghost_Idle_Front";
    public string death = "Ghost_Death";
    public float moveSpeed = 5;
    private PlayAnimations pa;
    private SpriteRenderer sr;
    private bool isPhasing;
    private bool isPlayer = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        pa = GetComponent<PlayAnimations>();

        if (gameObject.tag == "Player")

        {
            isPlayer = true;
            GetComponent<PlayerMovement>().moveSpeed = moveSpeed;
            //Due to the sprite scaling when you change from a human to a ghost the capsule collider is too large to move horizontally though 1 unit tall corridors
            GetComponent<CapsuleCollider2D>().size = new Vector2(0.1f, 0.2f);
            GameObject.Find("GameManager").GetComponent<GameManager>().currentHost = "Ghost";
        }

        isPhasing = false;
        transform.localScale = new Vector3(3.5f, 3.5f, 0);

        //Set the player animations/sprites to the current host creature
        pa.idleLeft = idleLeft;
        pa.idleRight = idleRight;
        pa.walkLeft = walkLeft;
        pa.walkRight = walkRight;
        pa.walkUp = walkUp;
        pa.walkDown = walkDown;
        pa.death = death;

    }

    public bool Phasing()
    {
        return isPhasing;
    }

    public void FireGhostBolt()
    {

        if (!isPhasing)
        {
            GameObject a = Instantiate
                                    (
                                        Resources.Load("Ghost_Bolt"),
                                        transform.position,
                                        transform.rotation
                                    )
                                    as GameObject;
            a.transform.parent = transform;
        }

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && isPhasing == false && isPlayer)
        {
            sr.material.color = new Color(1f, 1f, 1f, 0.5f);
            isPhasing = true;
            //Can pass through enemies
            Physics2D.IgnoreLayerCollision(12, 8, true);
            //Does not pick up items
            Physics2D.IgnoreLayerCollision(12, 10, true);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isPhasing == true && isPlayer)
        {
            sr.material.color = new Color(1f, 1f, 1f, 1f);
            isPhasing = false;
            Physics2D.IgnoreLayerCollision(12, 8, false);
            Physics2D.IgnoreLayerCollision(12, 10, false);
        }
    }
}
