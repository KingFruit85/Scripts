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
    public string attackLeft = "";
    public string attackRight = "";
    public string attackUp = "";
    public string attackDown = "";

    public string death = "Ghost_Death";

    public float moveSpeed = 1;

    void Awake()
    {
        if (transform.tag == "Player")
        {
            GetComponent<PlayerMovement>().moveSpeed = moveSpeed;
        }

        

        GetComponent<PlayAnimations>().idleLeft = idleLeft;
        GetComponent<PlayAnimations>().idleRight = idleRight;
        GetComponent<PlayAnimations>().walkLeft = walkLeft;
        GetComponent<PlayAnimations>().walkRight = walkRight;
        GetComponent<PlayAnimations>().walkUp = walkUp;
        GetComponent<PlayAnimations>().walkDown = walkDown;
        GetComponent<PlayAnimations>().death = death;


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
                                    a.name = "a";
                                    a.transform.parent = transform;


            
                           
    }
}
