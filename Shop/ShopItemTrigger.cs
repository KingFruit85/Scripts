using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemTrigger : MonoBehaviour
{
    public bool sword;
    public int swordDamageIncrease = 10;

    public bool health;
    public int healthIncrease = 10;

    public bool bow;
    public int bowDamageIncrease = 10;

    string idleAnimation;
    string activeAnimation;
    
    [SerializeField]
    GameObject highlightedObject;
    [SerializeField]
    GameManager gameManager;
    [SerializeField]
    private bool itemSelected;

    void Awake()
    {

        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();

        if (sword)
        {
            activeAnimation = "SwordAttackLevelUp";
            idleAnimation = "SwordAttackLevelUp_Idle";
            highlightedObject = GameObject.Find("SwordAttackLevelUp");
        }

        else if (health)
        {
            activeAnimation = "HealthLevelUp";
            idleAnimation = "HealthLevelUp_idle";
            highlightedObject = GameObject.Find("HealthLevelUp");

        }

        else if (bow)
        {
            activeAnimation = "BowAttackLevelUp";
            idleAnimation = "BowAttackLevelUp_idle";
            highlightedObject = GameObject.Find("BowAttackLevelUp");

        }

        else
        {
            activeAnimation = null;
            idleAnimation = null;  
        }
    }

    void Update()
    {
        if (itemSelected && Input.GetKeyDown(KeyCode.E))
            {
                // add highlighted items bonus to player
                switch (highlightedObject.name)
                {
                    default:return;
                    case "SwordAttackLevelUp":
                        gameManager.AddMeleeAttackBonus(swordDamageIncrease);
                        // Add a function to the human/sword class that manages the increase in damage
                        break;


                    case "HealthLevelUp":
                        gameManager.AddHealthBonus(healthIncrease);
                        GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().ChangeMaxHealth(10);
                        break;


                    case "BowAttackLevelUp":
                        gameManager.AddRangedAttackBonus(bowDamageIncrease);
                        break;
                }

                // Remove all the items from teh table

                Destroy(GameObject.Find("SwordAttackLevelUp"));
                Destroy(GameObject.Find("HealthLevelUp"));
                Destroy(GameObject.Find("BowAttackLevelUp"));

                Destroy(GameObject.Find("SwordFloorTrigger"));
                Destroy(GameObject.Find("HealthFloorTrigger"));
                Destroy(GameObject.Find("BowFloorTrigger"));


                transform.parent.gameObject.GetComponent<AddRoom>().SpawnExit();

            }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            highlightedObject.GetComponent<Animator>().Play(activeAnimation);  

            itemSelected = true;

            
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            highlightedObject.GetComponent<Animator>().Play(idleAnimation);

            itemSelected = false;

        }
    }
}
