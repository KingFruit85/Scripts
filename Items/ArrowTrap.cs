using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    public GameObject trapArrow;
    private float shotCooldown = -9999;
    public float shotDelay = 5f;
    public float startDelay = 0f;

    public bool shootUp, shootDown, shootLeft, shootRight;
    private string direction;
    private Vector3 spawnPOS;
    public bool active;
    private bool canShoot;

    void Start()
    {
        if (active)
        {
            Invoke("Activate",startDelay);
        }

        if (shootUp) direction = "up"; spawnPOS = new Vector3(0,1); 
        if (shootDown) direction = "down"; spawnPOS = new Vector3(0,-1);
        if (shootLeft) direction = "left"; spawnPOS = new Vector3(-1,0);
        if (shootRight) direction = "right"; spawnPOS = new Vector3(1,0);
    }

    public void Activate()
    {
        canShoot = true;
    }

    public void ActivateOnce()
    {
        active = true;

        //just shoot one arrow for traps and stuff
        GameObject a = Instantiate(trapArrow,
                                       transform.position,
                                       Quaternion.identity)
                                       as GameObject;

        a.transform.parent = transform;

        active = false;
    }

    public void ShootAtSpecificLocation(Vector3 tilePOS)
    {
        var arrowThatDoesntCollideWithWalls = Resources.Load("trapArrowThatIgnoresWalls") as GameObject;
        GameObject a = Instantiate(arrowThatDoesntCollideWithWalls,
                                       transform.position,
                                       Quaternion.identity)
                                       as GameObject;

        a.transform.parent = transform;

        var rb = a.GetComponent<Rigidbody2D>();

        var aim = (tilePOS - transform.position).normalized;
        rb.AddForce(aim * 0.1f);
    }

    public string GetDirection()
    {
        return direction;
    }
    
    void Update()
    {
        if (Time.time > shotCooldown + shotDelay && canShoot)
        {
            GameObject a = Instantiate(trapArrow,
                                       transform.position,
                                       Quaternion.identity)
                                       as GameObject;

            a.transform.parent = transform;
            shotCooldown = Time.time;
        }
    }
}
