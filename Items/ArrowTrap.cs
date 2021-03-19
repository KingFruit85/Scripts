using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    public GameObject arrow;
    private float shotCooldown = -9999;
    public float shotDelay = 2f;
    public bool shootUp, shootDown, shootLeft, shootRight;
    private string direction;
    private Vector3 spawnPOS;
    public bool active;

    void Start()
    {
        active = false;

        if (shootUp) direction = "up"; spawnPOS = new Vector3(0,1); 
        if (shootDown) direction = "down"; spawnPOS = new Vector3(0,-1);
        if (shootLeft) direction = "left"; spawnPOS = new Vector3(-1,0);
        if (shootRight) direction = "right"; spawnPOS = new Vector3(1,0);
    }

    public bool isActive()
    {
        return active;
    }

    public void Activate()
    {
        active = true;
    }

    public string GetDirection()
    {
        return direction;
    }
    
    void Update()
    {
        if (Time.time > shotCooldown + shotDelay && isActive())
        {
            GameObject a = Instantiate(arrow,
                                       transform.position,
                                       Quaternion.identity)
                                       as GameObject;

            a.transform.parent = transform;
            shotCooldown = Time.time;
        }
    }
}
