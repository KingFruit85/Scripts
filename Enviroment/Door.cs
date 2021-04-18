using UnityEngine;

public class Door : MonoBehaviour
{
    public bool openTrigger;
    public Sprite closedDoor;
    public Sprite openDoor;
    public bool isCurrentlyPhasable = false;

    void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = closedDoor;
    }
    public void OpenDoor()
    {
            GetComponent<SpriteRenderer>().sprite = openDoor;
            GetComponent<BoxCollider2D>().enabled = false;
    }

    void Update()
    {
        if (isCurrentlyPhasable)
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }

        if (!isCurrentlyPhasable)
        {
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }




}
