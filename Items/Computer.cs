using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Computer : MonoBehaviour
{
    public float dist;
    public Vector3 playerPOS;
    public GameObject myDoor;

    public Action unlockMyDoor;
    public Action displayErrorMsg;

    public bool inUse = false;



    void Start()
    {
        unlockMyDoor = () => {
            myDoor.GetComponent<Door>().UnlockDoor();
        };

        displayErrorMsg = () => {
            // Create a UI Element and set some text in it
            Debug.Log("Unable to unlock door, insufficient access");
        };
    }


    public void LoadTerminal()
    {
        TerminalPopUp popup = UIManager.Instance.CreateTerminal();
            // Canvas, btn1txt, btn2txt, btn3txt, action1, action2, 

            popup.PopUpTerminal(
                                this.gameObject,
                                UIManager.Instance.MainCanvas, 
                                "Unlock North Door", 
                                "Unlock West Door", 
                                "Close Terminal", 
                                unlockMyDoor, 
                                displayErrorMsg);
                                }



    void Update()
    {
        playerPOS = GameObject.Find("Player").transform.position;
        dist = Vector3.Distance(playerPOS, transform.position);
        
        if (dist < 1.5f && Input.GetKeyDown(KeyCode.E) && !inUse)
        {
            inUse = true;
            LoadTerminal();
        }

    }
}
