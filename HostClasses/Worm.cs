using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : MonoBehaviour
{
    public string idleLeft = "worm_walk";
    public string idleRight = "worm_walk";
    public string walkLeft = "worm_walk";
    public string walkRight = "worm_walk";
    public string walkUp = "worm_walk";
    public string walkDown = "worm_walk";
    public string idleUp = "worm_walk";
    public string idleDown = "worm_walk";
    public string death = "worm_death";
    public string attackLeft = "worm_bite_left";
    public string attackRight = "worm_bite_right";
    public string attackUp = "worm_bite_left";
    public string attackDown = "worm_bite_left";

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

    public void PoisonBite()
    {
        return;
    }

}


