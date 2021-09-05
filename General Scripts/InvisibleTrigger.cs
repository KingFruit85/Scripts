using UnityEngine;

public class InvisibleTrigger : MonoBehaviour
{
    public GameObject door;
    public GameObject[] doors;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
             if (door)
             {
                 door.GetComponent<Door>().CloseDoor();
                 Destroy(gameObject);

             }

             if (doors.Length > 0)
             {
                 foreach (var door in doors)
                 {
                     door.GetComponent<Door>().CloseDoor();
                 }
                
                Destroy(gameObject);

             }
        }
    }
}
