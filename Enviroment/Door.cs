using UnityEngine;

public class Door : MonoBehaviour
{
    public bool open;
    public Sprite closedDoor;
    public Sprite openDoor;

    public bool startOpen, startClosed;



    void Awake()
    {
        if (startOpen)
        {
            OpenDoor();
            open = true;
        }
        else
        {
            CloseDoor();
            open = false;
        }
    }

    public void OpenDoor()
    {
            GetComponent<SpriteRenderer>().sprite = openDoor;
            GetComponent<BoxCollider2D>().enabled = false;
            open = true;
    }

    public void CloseDoor()
    {
            GetComponent<SpriteRenderer>().sprite = closedDoor;
            GetComponent<BoxCollider2D>().enabled = true;
            open = false;
    }

  




}
