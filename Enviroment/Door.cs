using UnityEngine;

public class Door : MonoBehaviour
{
    public bool open;
    public Sprite closedDoor;
    public Sprite openDoor;
    public bool isLocked;
    public bool startOpen, startClosed;
    public GameObject myKeyCardReader;
    public DoorController DC;

    void Update()
    {
        // if (DC.player && DC.enemies.Length > 0)
        // {
        //     CloseDoor();
        // }

        // if (DC.player && DC.enemies.Length == 0)
        // {
        //     OpenDoor();
        // }


    }

    void Awake()
    {

        DC = transform.parent.parent.Find("DoorController").GetComponent<DoorController>();

        if (startOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    public void UnlockDoor()
    {
        isLocked = false;
        if (myKeyCardReader)
        {
            myKeyCardReader.GetComponent<KeyCardReader>().SetOpen();
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