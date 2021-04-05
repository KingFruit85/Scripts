using UnityEngine;

public class Arrow : MonoBehaviour
{
    private int damage = 10;
    private int arrowSpeed = 10;

    public Vector3 clickPoint;
    public Vector3 aim;

    void Start()
    {
        aim = (clickPoint - transform.position).normalized;
        //Points the arrow the direction we're shooting
        float angle = Mathf.Atan2(aim.y,aim.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0,0,angle));
    }

    public void SetArrowDamage(int dmg)
    {
        damage = dmg;
    }

    void FixedUpdate()
    {
        //Adds force in the direction of the mouseclick to the arrow
        transform.position += aim * arrowSpeed * Time.deltaTime;
    }

    //This Function deals with handling collisions, don't love it as is.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("enemies"))
        {
            other.GetComponent<Health>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Items"))
        {
            return;
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            return;
        }
        else
        {
            Destroy(gameObject);
        }  
    }
}
