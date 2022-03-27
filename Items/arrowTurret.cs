using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowTurret : MonoBehaviour
{
    public float shotDelay = 1f;
    public float shotCooldown = -9999;
    public GameObject arrow;
    public GameObject rightSpawn;
    public GameObject leftSpawn;
    public GameObject topSpawn;
    public GameObject bottomSpawn;

    public bool isActive = true;



    public void RotateTurret()
    {
        transform.Rotate (0f, 0f, 45f);
    }

    void Update()
    {
        if (Time.time > shotCooldown + shotDelay && isActive)
        { 
            RotateTurret();

            GameObject rightArrow = Instantiate(arrow,
                                       rightSpawn.transform.position,
                                       Quaternion.identity)
                                       as GameObject;

            rightArrow.transform.parent = transform;

            GameObject leftArrow = Instantiate(arrow,
                                       leftSpawn.transform.position,
                                       Quaternion.identity)
                                       as GameObject;

            leftArrow.transform.parent = transform;

            GameObject upArrow = Instantiate(arrow,
                                       topSpawn.transform.position,
                                       Quaternion.identity)
                                       as GameObject;

            upArrow.transform.parent = transform;

            GameObject downArrow = Instantiate(arrow,
                                       bottomSpawn.transform.position,
                                       Quaternion.identity)
                                       as GameObject;

            downArrow.transform.parent = transform;

            shotCooldown = Time.time;
        }
    }
}
