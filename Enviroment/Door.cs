using UnityEngine;

public class Door : MonoBehaviour
{
    public bool openTrigger;
    public Sprite closedDoor;
    public Sprite openDoor;

    public bool startOpen, startClosed;



    void Awake()
    {
        if (startOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }
    public void OpenDoor()
    {
            GetComponent<SpriteRenderer>().sprite = openDoor;
            GetComponent<BoxCollider2D>().enabled = false;
    }

    public void CloseDoor()
    {
            GetComponent<SpriteRenderer>().sprite = closedDoor;
            GetComponent<BoxCollider2D>().enabled = true;
    }

  




}
