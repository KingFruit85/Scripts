using UnityEngine;

public class GhostProjectileSensor : MonoBehaviour
{   
    [SerializeField]
    private SpriteRenderer sr;
    private AudioManager audioManager;

    void Awake()
    {
        audioManager = GameObject.FindObjectOfType<AudioManager>();

    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "PlayerArrow")
        {
            if (other.GetComponent<Arrow>().isAlight)
            {
                GetComponentInParent<AIMovement>().DazeForSeconds(2);
                other.GetComponent<SpriteRenderer>().color = Color.red;
                // GetComponentInParent<GhostAttacks>().ResetAttackDelay(); 
                GetComponentInParent<Health>().setProjectileImmunity(false);
                GetComponentInParent<Health>().setMeleeImmunity(false);
            }
            else
            {
                sr.material.color = new Color(1f, 1f, 1f, 0.5f);
                other.GetComponent<SpriteRenderer>().color = Color.green;
                Physics2D.IgnoreCollision(other.GetComponent<BoxCollider2D>(),gameObject.GetComponentInParent<CapsuleCollider2D>(),true);
                // Only used for player interations, gets annoying with traps
                // GetComponentInParent<GhostAttacks>().ResetAttackDelay();

                GetComponentInParent<AIMovement>().DazeForSeconds(1);

                audioManager.PlayAudioClip("ArrowHitGhost");
            }
            
        }
        else if (other.tag == "TrapArrow")
        {
            sr.material.color = new Color(1f, 1f, 1f, 0.5f);
            Physics2D.IgnoreCollision(other.GetComponent<BoxCollider2D>(),gameObject.GetComponentInParent<CapsuleCollider2D>(),true);
            GetComponentInParent<GhostAttacks>().ResetAttackDelay();

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "TrapArrow" || other.tag == "PlayerArrow")
        {
            sr.material.color = new Color(1f, 1f, 1f, 1f);
            Physics2D.IgnoreCollision(other.GetComponent<BoxCollider2D>(),gameObject.GetComponentInParent<CapsuleCollider2D>(),false);
        }
    }

}
