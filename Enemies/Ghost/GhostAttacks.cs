using System.Collections;
using UnityEngine;

public class GhostAttacks : MonoBehaviour
{

    public GameObject ghostBolt;
    [SerializeField]
    private SpriteRenderer sr;
    private GameObject player;
    private float attackDelay = 1.0f;
    private float lastAttacked = -9999;
    private AudioManager audioManager;



    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sr = GetComponent<SpriteRenderer>();
        audioManager = GameObject.FindObjectOfType<AudioManager>();

    }

    public void FireGhostBolt()
    {

            string[] ghostBolts  = new string[]{"GhostBolt1","GhostBolt2","GhostBolt3","GhostBolt4",
                                             "GhostBolt5","GhostBolt6","GhostBolt7"};

            int rand = Random.Range(0, ghostBolts.Length);

            audioManager.PlayAudioClip(ghostBolts[rand]);
            
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
                FireGhostBolt();
                lastAttacked = Time.time;
            }            
        }
        
    }
}
