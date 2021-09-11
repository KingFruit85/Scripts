using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCardReader : MonoBehaviour
{
    public Sprite open;
    public Sprite closed;
    public float dist;
    public GameObject myDoor;

    public Vector3 playerPOS;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = closed;
    }

    void OpenDoor()
    {
        if (myDoor.GetComponent<Door>().isLocked)
        {
            // PLay error sound
            // flash keycard reader
            StartCoroutine(Locked());
        }
        else
        {
            // Open door
            SetOpen();
            myDoor.GetComponent<BoxCollider2D>().enabled = false;
            myDoor.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private IEnumerator Locked()
    {
        GetComponent<SpriteRenderer>().sprite = open;
        yield return new WaitForSeconds( .5f );
        GetComponent<SpriteRenderer>().sprite = closed;
    }

    public void SetOpen()
    {
        GetComponent<SpriteRenderer>().sprite = open;
    }

    public void SetClosed()
    {
        GetComponent<SpriteRenderer>().sprite = closed;
    }



    void Update()
    {
        playerPOS = GameObject.Find("Player").transform.position;
        dist = Vector3.Distance(playerPOS, transform.position);
        
        if (dist < 1.5f && Input.GetKeyDown(KeyCode.E))
        {
            if (GetComponent<SpriteRenderer>().sprite = closed)
            {
                OpenDoor();
            }
        }
    }

}
