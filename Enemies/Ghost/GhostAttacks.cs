using System.Collections;
using UnityEngine;

public class GhostAttacks : MonoBehaviour
{

    public GameObject ghostBolt;
    private SpriteRenderer sr;
    private GameObject player;
    private float attackDelay = 5.0f;
    private float lastAttacked = -9999;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sr = GetComponent<SpriteRenderer>();
    }

    public void fireGhostBolt()
    {
            
            // Visual warning for the player that attack is incomming
            StartCoroutine(FlashColor(Color.red));

            var pos = transform.position;

            GameObject a = Instantiate
                                    (
                                        ghostBolt,
                                        transform.position,
                                        transform.rotation
                                    )
                                    as GameObject;
                                    a.transform.parent = transform;
            
    }

    private IEnumerator FlashColor(Color color)
    {
        sr.color = color;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }

    void Update()
    {
        // Check for player in aggo range
        if (Vector3.Distance(transform.position, player.transform.position) <= 2.5f)
        {
            if (Time.time > lastAttacked + attackDelay)
            {
                fireGhostBolt();
                lastAttacked = Time.time;
            }            
        }
        
    }
}
