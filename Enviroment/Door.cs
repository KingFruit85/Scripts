using UnityEngine;

public class Door : MonoBehaviour
{
    public bool openTrigger;
    public Sprite closedDoor;
    public Sprite openDoor;

    void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = closedDoor;
    }
    public void OpenDoor()
    {
            GetComponent<SpriteRenderer>().sprite = openDoor;
            GetComponent<BoxCollider2D>().enabled = false;
    }


}
