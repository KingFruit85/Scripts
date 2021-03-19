using UnityEngine;
public class RoomMonitor : MonoBehaviour
{
    public GameObject topLeftCorner;
    public GameObject bottomRightCorner;
    public Collider2D[] enemies;
    public LayerMask enemyLayer;
    public GameObject myDoor;

    
    void Update()
    {
        enemies = Physics2D.OverlapAreaAll(topLeftCorner.transform.position, bottomRightCorner.transform.position, enemyLayer);

        if (enemies.Length <= 0)
        {
            myDoor.GetComponent<Door>().OpenDoor();
        }

    }
}
