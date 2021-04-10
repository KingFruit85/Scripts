using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowTurret : MonoBehaviour
{
    public float shotDelay = 1f;
    public float shotCooldown = -9999;
    private GameObject arrow;
    private Vector3 rightSpawn;
    private Vector3 leftSpawn;
    private Vector3 topSpawn;
    private Vector3 bottomSpawn;

    public bool isActive;

    void Awake()
    {
        arrow = Resources.Load("trapArrow") as GameObject;
        rightSpawn = new Vector3(transform.position.x + .33f,transform.position.y,0);
        leftSpawn = new Vector3(transform.position.x - .34f,transform.position.y,0);

        topSpawn = new Vector3(transform.position.x,transform.position.y + .35f,0);
        bottomSpawn = new Vector3(transform.position.x,transform.position.y -0.341f,0);

    }
    void Update()
    {
        if (Time.time > shotCooldown + shotDelay && isActive)
        { 
            GameObject rightArrow = Instantiate(arrow,
                                       rightSpawn,
                                       Quaternion.identity)
                                       as GameObject;

            rightArrow.transform.parent = transform;
            rightArrow.name = "right";

            GameObject leftArrow = Instantiate(arrow,
                                       leftSpawn,
                                       Quaternion.identity)
                                       as GameObject;

            leftArrow.transform.parent = transform;
            leftArrow.transform.Rotate (0f, 180f, 0f);
            leftArrow.name = "left";


            GameObject upArrow = Instantiate(arrow,
                                       topSpawn,
                                       Quaternion.identity)
                                       as GameObject;

            upArrow.transform.parent = transform;
            upArrow.transform.Rotate (0f, 0f, 90f);
            upArrow.name = "up";


            GameObject downArrow = Instantiate(arrow,
                                       bottomSpawn,
                                       Quaternion.identity)
                                       as GameObject;

            downArrow.transform.parent = transform;
            downArrow.transform.Rotate (0f, 0f, -90f);
            downArrow.name = "down";


            shotCooldown = Time.time;
        }
    }
}
