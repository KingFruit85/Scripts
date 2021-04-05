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
    private PlayAnimations pa;

    void Awake()
    {
        if (transform.tag == "Player")
        {
            GetComponent<PlayerMovement>().moveSpeed = moveSpeed;
        }

        transform.localScale = new Vector3(1,1,0);

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

    public void PoisonBite()
    {
        return;
    }

}


