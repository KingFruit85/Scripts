using UnityEngine;

public class GhostProjectileSensor : MonoBehaviour
{   
    [SerializeField]
    private SpriteRenderer sr;
    void Awake()
    {
        // sr = GetComponentInParent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerArrow")
        {
            sr.material.color = new Color(1f, 1f, 1f, 0.5f);
            other.GetComponent<SpriteRenderer>().color = Color.green;
            Physics2D.IgnoreCollision(other.GetComponent<BoxCollider2D>(),gameObject.GetComponentInParent<CapsuleCollider2D>(),true);
            // Only used for player interations, gets annoying with traps
            GetComponentInParent<AIMovement>().DazeForSeconds(1);
        }
        else if (other.tag == "TrapArrow")
        {
            sr.material.color = new Color(1f, 1f, 1f, 0.5f);
            Physics2D.IgnoreCollision(other.GetComponent<BoxCollider2D>(),gameObject.GetComponentInParent<CapsuleCollider2D>(),true);
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
