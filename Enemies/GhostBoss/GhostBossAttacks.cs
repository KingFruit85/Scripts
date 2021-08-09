using System.Collections;
using UnityEngine;

public class GhostBossAttacks : MonoBehaviour
{

    public GameObject bossBolt;
    private GameObject boss;
    public GameObject[] aoeSpawnPoints;
    public SpriteRenderer sr;
    private GameObject player;
    private GameObject[] activeBossBoltsInScene;
    public bool canShoot = false;
    private float attackDelay = 2.5f;
    private float lastAttacked = -9999;
    private AudioManager audioManager;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sr = GetComponent<SpriteRenderer>();
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        bossBolt = Resources.Load("Boss_Bolt") as GameObject;
        boss = GameObject.FindGameObjectWithTag("GhostBoss");

    }

    public void AOEAttack()
    {
        foreach (var point in aoeSpawnPoints)
        {
            GameObject a = Instantiate
                                    (
                                        bossBolt,
                                        point.transform.position,
                                        Quaternion.identity)

                                    as GameObject;
                                    a.transform.parent = boss.transform;

        }
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
        activeBossBoltsInScene = GameObject.FindGameObjectsWithTag("BossBolt");

        if (activeBossBoltsInScene.Length < 1)
        {
            canShoot = true;
        }
        else
        {
            canShoot = false;
        }

        if (Vector3.Distance(transform.position, player.transform.position) <= 2.5f)
        {
           sr.material.color = new Color(1f, 1f, 1f, 0.5f); 
           canShoot = false;
        }
        else
        {
            sr.material.color = new Color(1f, 1f, 1f, 1f);
            canShoot = true;
        }
        // Check for player in aggo range
        if (Vector3.Distance(transform.position, player.transform.position) <= 5.0f)
        {
            if (canShoot && Time.time > lastAttacked + attackDelay)
            {
                AOEAttack();
                lastAttacked = Time.time;
            }            
        }



        
    }
}
