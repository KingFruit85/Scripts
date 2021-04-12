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
    public string idleUp = "Ghost_Walk_Up"; // Replace this when posible
    public string idleDown = "Ghost_Idle_Front";
    public string death = "Ghost_Death";
    public float moveSpeed = 3;
    private PlayAnimations pa;
    private SpriteRenderer sr;
    public bool isPhasing;
    private bool isPlayer = false;

    void Awake()
    {
        if (transform.tag == "Player")
        {
            GetComponent<PlayerMovement>().moveSpeed = moveSpeed;
            isPlayer = true;
        }

        isPhasing = false;

        sr = GetComponent<SpriteRenderer>();

        transform.localScale = new Vector3(3.5f,3.5f,0);

        //Set the player animations/sprites to the current host creature
        pa = GetComponent<PlayAnimations>();
        pa.idleLeft = idleLeft;
        pa.idleRight = idleRight;
        pa.walkLeft = walkLeft;
        pa.walkRight = walkRight;
        pa.walkUp = walkUp;
        pa.walkDown = walkDown;
        pa.death = death;

    }


    public void GhostBolt()
    {
        // should fire towards mouseclick

            GameObject a = Instantiate
                                    (
                                        Resources.Load("Ghost_Bolt"),
                                        transform.position,
                                        transform.rotation
                                    )
                                    as GameObject;
                                    a.transform.parent = transform;
                  
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isPhasing == false && isPlayer)
        {
            sr.material.color = new Color(1f, 1f, 1f, 0.2f);
            isPhasing = true;
            GetComponent<PlayerStats>().isPhasing = true;

        }
        else if (Input.GetKeyDown(KeyCode.Space) && isPhasing == true && isPlayer)
        {
            sr.material.color = new Color(1f, 1f, 1f, 1f);
            isPhasing = false;
            GetComponent<PlayerStats>().isPhasing = false;

        }
    }
}
