using UnityEngine;

public class GhostBolt : MonoBehaviour
{
    public float speed;
    public int damage = 10;
    private Rigidbody2D rb;
    private GameObject player;
    private Vector3 aim;
    private Vector3 enemyLoc;

    void Awake()
    {
        player = GameObject.Find("Player");
        rb = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        aim = (player.transform.position - transform.position).normalized;
        enemyLoc = GameObject.FindGameObjectWithTag("Ghost").transform.position;

        rb.AddForce(aim * speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall")
        {
            //Add animation
            Destroy(this.gameObject);
        }

        if (other.tag == "Player")
        {
            if (player.GetComponent<Dash>().isPlayerDashing())
            {
                return;
            }
            else
            {
                player.GetComponent<Health>().TakeDamage(damage);
                GameObject.Find("Main Camera").GetComponent<Shaker>().Shake(.1f);
                //add animation
                Destroy(this.gameObject);
            }
            
        }
    }
}
