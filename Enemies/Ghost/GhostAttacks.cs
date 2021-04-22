using System.Collections;
using UnityEngine;

public class GhostAttacks : MonoBehaviour
{

    public GameObject ghostBolt;
    [SerializeField]
    private SpriteRenderer sr;
    private GameObject player;
    private float attackDelay = 5.0f;
    private float lastAttacked = -9999;
    [SerializeField]
    private GameObject[] trapArrows;
    private GameObject[] playerArrows;

    [SerializeField]
    private float distanceToTrapArrow;
    [SerializeField]

    private float distanceToPlayerArrow;

    [SerializeField]
    private bool isPhasing;

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

    public void ResetAttackDelay()
    {
        lastAttacked = Time.time;
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

        // trapArrows = GameObject.FindGameObjectsWithTag("TrapArrow");
        // playerArrows = GameObject.FindGameObjectsWithTag("PlayerArrow");


        // for (int i = 0; i < trapArrows.Length; i++)
        // {
        //     distanceToTrapArrow = Vector2.Distance(trapArrows[i].transform.position,transform.position);
        //     distanceToPlayerArrow = Vector2.Distance(playerArrows[i].transform.position,transform.position);

        //     if (distanceToTrapArrow < 3.5f || distanceToPlayerArrow < 3.5)
        //     {
        //         isPhasing = true;
        //         sr.material.color = new Color(1f, 1f, 1f, 0.5f);
        //         gameObject.GetComponent<CapsuleCollider2D>().isTrigger = true;
        //     }
        //     else
        //     {
        //         isPhasing = false;
        //         sr.material.color = new Color(1f, 1f, 1f, 1f);
        //         gameObject.GetComponent<CapsuleCollider2D>().isTrigger = false;

        //     }


        // }
        
    }
}
