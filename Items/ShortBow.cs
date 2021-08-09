using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortBow : MonoBehaviour
{
    private GameObject arrow;
    private GameObject player;
    private GameManager gameManager;

    void Start()
    {
        arrow = Resources.Load("arrow") as GameObject;
        player = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void ShootBow(Vector3 mousePosition)
    {
        if (gameManager.arrowCount > 0)
        {

        
        // Spawn arrow on top of player
        GameObject a = Instantiate(arrow,
                                new Vector3(player.transform.position.x,
                                            player.transform.position.y,
                                            player.transform.position.z), 
                                player.transform.rotation);
                                
        arrow.GetComponent<Arrow>().clickPoint = mousePosition;
        // Set arrow as child of player gameobject                     
        a.transform.parent = player.transform;

        GameObject.Find("GameManager").GetComponent<GameManager>().arrowCount --;

        // Play arrow shot sound

        }
    }
}
