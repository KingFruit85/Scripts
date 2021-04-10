﻿using System.Collections;
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

    private Vector3 lastVelocity;
    public bool deflected = false;

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();

        // Check what object is firing me and fire in the appropriate direction
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

        // Fire the arrow
        switch (direction)
        {
            default:break;
            case "left": RB.AddForce(left * speed ,ForceMode2D.Force); break;
            case "right": RB.AddForce(right * speed ,ForceMode2D.Force); break;
            case "up": RB.AddForce(up * speed ,ForceMode2D.Force); break;
            case "down": RB.AddForce(down * speed ,ForceMode2D.Force); break;
        }
    }

    void Update()
    {
        // Tracked to calculate speed for deflections
        lastVelocity = RB.velocity;

        // Removes any deflected arrows that are laying about doing nothing
        if (deflected && RB.velocity.x > -5f && RB.velocity.y > - 5f) Destroy(gameObject, .5f);
    }   

    void OnCollisionEnter2D(Collision2D coll)
    {
        
        if (coll.collider.gameObject.tag == "Player")
        {
            if (deflected == false)
            {
                coll.gameObject.GetComponent<Health>().TakeDamage(damage, transform.parent.gameObject);
                Destroy(gameObject);
            }
        }

        else if (coll.collider.gameObject.layer == LayerMask.NameToLayer("Items"))
        {
            deflected = true;
            return;
        }

        else if (coll.collider.gameObject.layer == LayerMask.NameToLayer("enemies"))
        {
            deflected = true;
            return;
        }

        else if (coll.collider.gameObject.name == "Sword")
        {
            // Deflect the arrow away from the sword
            float speed = lastVelocity.magnitude;
            Vector3 direction = Vector3.Reflect(lastVelocity.normalized,coll.contacts[0].normal);
            RB.velocity = direction * speed / 2;
            
            // tag the arrow as having been deflected
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
