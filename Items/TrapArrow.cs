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
    private string direction;
    public int damage = 10;

    
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        direction = GetComponentInParent<ArrowTrap>().GetDirection();

        if (direction == "up") transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        if (direction == "down") transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
        if (direction == "left") transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        if (direction == "right") transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        switch (direction)
        {
            default:break;
            case "left": RB.AddForce(left * speed ,ForceMode2D.Force); break;
            case "right": RB.AddForce(right * speed ,ForceMode2D.Force); break;
            case "up": RB.AddForce(up * speed ,ForceMode2D.Force); break;
            case "down": RB.AddForce(down * speed ,ForceMode2D.Force); break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Health health))
        {
            other.GetComponent<Health>().TakeDamage(damage);
        }
        else if (other.tag == "HealthPotion")
        {
            return;
        }
        else 
        {
            Destroy(this.gameObject);
        }
    }
    
}
