using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private SpriteRenderer SR;
    private GameManager GM;
    public Sprite open;
    public Sprite closed;
    public GameObject[] treasure;
    private bool hasBeenOpened = false;
    private bool openButtonPressed = false;
    private Vector3 playerPOS;
    private float distanceBetween;
    public Door[] linkedDoors;
    public Barrier[] linkedbarriers;
    public FlameBowl[] linkedFlameBowls;

    void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
        SR.sprite = closed;
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private GameObject SpawnItem()
    {
        // If player doesn't have a bow yet, spawn a bow
        if (!GM.rangedWeaponEquipped)
        {
            return (GameObject)Resources.Load("BowPickup");
        }
        else
        {
            var R = Random.Range(0, treasure.Length);
            return treasure[R];
        }


    }

    void openChest()
    {
        SR.sprite = open;
        GameObject item = SpawnItem();
        var pos = transform.position;
                GameObject a = Instantiate
                                    (
                                        item,
                                        new Vector3(pos.x ,pos.y, pos.z),
                                        transform.rotation
                                    )
                                    as GameObject;
                //sets flag to stop item spawning if chest is closes & reopened
                hasBeenOpened = true;
                //Spawn item on top of chest
                item.GetComponent<SpriteRenderer>().sortingOrder = 3;
    }

    void CloseLinkedDoors()
    {
        foreach (var door in linkedDoors)
        {
            door.CloseDoor();
        }
    }

    void OpenLinkedBarriers()
    {
        foreach (var barrier in linkedbarriers)
        {
            barrier.gameObject.SetActive(false);
        }
    }

    void LightFlameBowls()
    {
        foreach (var flameBowl in linkedFlameBowls)
        {
            flameBowl.Light();
        }
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

            if (linkedDoors.Length > 0 && linkedDoors != null)
            {
                CloseLinkedDoors();
            }

            if (linkedbarriers.Length > 0 && linkedbarriers != null)
            {
                OpenLinkedBarriers();
            }

            if (linkedFlameBowls.Length > 0 && linkedFlameBowls != null)
            {
                LightFlameBowls();
            }
        }
    }
}
