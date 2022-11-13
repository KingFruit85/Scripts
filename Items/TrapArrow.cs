using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapArrow : MonoBehaviour
{
    private Rigidbody2D RB;
    public float speed = 0.05f;
    private Vector3 left = new Vector3(-1,0,0);
    private Vector3 right = new Vector3(1,0,0);
    private Vector3 up = new Vector3(0,1,0);
    private Vector3 down = new Vector3(0,-1,0);
    public string direction;
    public int damage = 10;
    private Vector3 lastVelocity;
    public bool deflected = false;
    private GameObject player;
    private AudioManager audioManager;
    public bool ignoreWalls = false;

    void Awake()
    {
        if (transform.name.Contains("trapArrowThatIgnoresWalls"))
        {
            ignoreWalls = true;
        }
    }
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        audioManager = GameObject.FindObjectOfType<AudioManager>();


        // Check what object is firing me and fire in the appropriate direction
        if (gameObject.transform.parent.tag == "ArrowTrap")
        {
            direction = GetComponentInParent<ArrowTrap>().GetDirection();

            if (direction == "up") transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            if (direction == "down") transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
            if (direction == "left") transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            if (direction == "right") transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0)); 
        }

        if (gameObject.transform.parent.tag == "ArrowTurret")
        {
            if (direction == "up") 
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                up = new Vector3(0,0,0);
            }
        }
        Fire();
    }

    public void Fire()
    {
        if (direction == "left")
        {
            RB.AddForce(left * speed ,ForceMode2D.Force);
        }
        if (direction == "right")
        {
            RB.AddForce(right * speed ,ForceMode2D.Force);
        }
        if (direction == "up")
        {
            RB.AddForce(up * speed ,ForceMode2D.Force);
        }
        if (direction == "down")
        {
            RB.AddForce(down * speed ,ForceMode2D.Force);
        }
    }



    void Update()
    {

        // Tracked to calculate speed for deflections
        lastVelocity = RB.velocity;

        // Removes any deflected arrows that are laying about doing nothing
        if (deflected && RB.velocity.x > -5f && RB.velocity.y > - 5f) Destroy(gameObject, .5f);

        if (player.TryGetComponent(out Ghost ghost))
        {
            if (ghost.Phasing() == true)
            {
                Physics2D.IgnoreCollision(player.GetComponent<CapsuleCollider2D>(),gameObject.GetComponent<BoxCollider2D>());
            }
        }
    }   

    void OnCollisionEnter2D(Collision2D coll)
    {

        if (coll.collider.gameObject.tag == "Player")
        {
            if (!deflected)
            {
                coll.gameObject.GetComponent<Health>().TakeDamage(damage, transform.parent.gameObject, coll.collider.gameObject.tag, false);
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

        else if (coll.collider.gameObject.tag == "PlayerSword")
        {

            string[] deflects = new string[]{"SwordArrowDeflect1","SwordArrowDeflect2","SwordArrowDeflect3","SwordArrowDeflect4",
                                             "SwordArrowDeflect5","SwordArrowDeflect6","SwordArrowDeflect7","SwordArrowDeflect8"};

            int rand = Random.Range(0, deflects.Length);

            audioManager.PlayAudioClip(deflects[rand]);

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

        else if (coll.gameObject.tag == "Wall" && !ignoreWalls)
        {
            Destroy(gameObject);
        }

        else if (coll.gameObject.tag == "Wall" && ignoreWalls)
        {
            return;
        }

    }
}