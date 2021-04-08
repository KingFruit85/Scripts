using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private SpriteRenderer SR;
    public Sprite open;
    public Sprite closed;
    public GameObject treasure;
    private bool hasBeenOpened = false;
    private bool openButtonPressed = false;
    private Vector3 playerPOS;
    private float distanceBetween;

    void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
        SR.sprite = closed;
    }

    void openChest()
    {
        SR.sprite = open;

        var pos = transform.position;
                GameObject a = Instantiate
                                    (
                                        treasure,
                                        new Vector3(pos.x ,pos.y, pos.z),
                                        transform.rotation
                                    )
                                    as GameObject;
                //sets flag to stop item spawning if chest is closes & reopened
                hasBeenOpened = true;
                //Spawn item on top of chest
                treasure.GetComponent<SpriteRenderer>().sortingOrder = 3;
    }


    void Update()
    {
        playerPOS = GameObject.Find("Player").transform.position;
        distanceBetween = Vector2.Distance(transform.position, playerPOS);

        if (distanceBetween < 1f && Input.GetKeyDown(KeyCode.E))
        {
           openButtonPressed  = true; 
        }
        else
        {
            openButtonPressed  = false; 
        }

        if (openButtonPressed == true && hasBeenOpened == false)
        {
            openChest();
        }
    }
}
