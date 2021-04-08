using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapArrow : MonoBehaviour
{
    private Rigidbody2D RB;
    private float speed = 0.05f;
    private Vector2 left = new Vector2(-1,0);
    private Vector2 right = new Vector2(1,0);
    private Vector2 up = new Vector2(0,1);
    private Vector2 down = new Vector2(0,-1);
    public string direction;
    private int damage = 10;

    public Vector3 lastVelocity;
    public bool deflected = false;

    public GameObject[] trapArrows;


    
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        GetComponent<SpriteRenderer>().sortingOrder = 3;

        if (gameObject.transform.parent.tag == "ArrowTrap")
        {
            direction = GetComponentInParent<ArrowTrap>().GetDirection();

            if (direction == "up") transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            if (direction == "down") transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
            if (direction == "left") transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            if (direction == "right") transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));  

            
        }

        else if (gameObject.transform.parent.name == "arrowTurret")
        {
            direction = transform.name;
        }


        switch (direction)
            {
                default:break;
                case "left": RB.AddForce(left * speed ,ForceMode2D.Force); break;
                case "right": RB.AddForce(right * speed ,ForceMode2D.Force); break;
                case "up": RB.AddForce(up * speed ,ForceMode2D.Force); break;
                case "down": RB.AddForce(down * speed ,ForceMode2D.Force); break;
            }

        trapArrows = GameObject.FindGameObjectsWithTag("TrapArrow");


        

        
    }

    void Update()
    {
        lastVelocity = RB.velocity;
        trapArrows = GameObject.FindGameObjectsWithTag("TrapArrow");

        foreach (var arrow in trapArrows)
        {
            Physics2D.IgnoreCollision(arrow.GetComponent<BoxCollider2D>(),gameObject.GetComponent<BoxCollider2D>());
        }
        
    }

    void OnCollisionEnter2D(Collision2D coll)
    {

        if (coll.collider.gameObject.tag == "Player")
        {
            if (deflected == false)
            {
                coll.gameObject.GetComponent<Health>().TakeDamage(damage);
                Destroy(gameObject);
            }
        }

        else if (coll.collider.gameObject.layer == LayerMask.NameToLayer("Items"))
        {
            return;
        }

        else if (coll.collider.gameObject.layer == LayerMask.NameToLayer("enemies"))
        {
            return;
        }

        else if (coll.collider.gameObject.tag == "TrapArrow")
        {
            Physics2D.IgnoreCollision(coll.collider.gameObject.GetComponent<BoxCollider2D>(),gameObject.GetComponent<BoxCollider2D>());
        }

        else if (coll.collider.gameObject.name == "Sword")
        {
            var speed = lastVelocity.magnitude;
            var direction = Vector3.Reflect(lastVelocity.normalized,coll.contacts[0].normal);
            RB.velocity = direction * speed / 2;
            deflected = true;
        }
        else if (coll.collider.gameObject.tag == "PlayerArrow")
        {
            var speed = lastVelocity.magnitude;
            var direction = Vector3.Reflect(lastVelocity.normalized,coll.contacts[0].normal);
            RB.velocity = direction * speed * 2;
        }
        else if (coll.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }


    }



    
}
