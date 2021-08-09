using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBoss : MonoBehaviour
{
    
    public string walkLeft = "ghostBoss_Walk_Left";
    public string idleUp = "ghostBoss_Idle_Front"; 
    public string walkRight = "ghostBoss_Walk_Right";
    public string walkDown = "ghostBoss_Idle_Front";
    public string idleDown = "ghostBoss_Idle_Front";
    public string walkUp = "ghostBoss_Idle_Front";

    public string idleLeft = "Ghost_Idle_Left";
    public string idleRight = "Ghost_Idle_Right";
    public string death = "Ghost_Death";

    private PlayAnimations pa;
    private SpriteRenderer sr;
    private bool isPhasing = false;


    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

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

    public bool Phasing()
    {
        return isPhasing;
    }

    
}
