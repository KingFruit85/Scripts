using UnityEngine;

public class BowPickup : MonoBehaviour
{
    private GameObject player;
    private GameObject shortBow;

    void Start()
    {
        player = GameObject.Find("Player");
        shortBow = Resources.Load("BowAim") as GameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player" && player.GetComponent<PlayerCombat>().rangedWeaponEquipped == false)
        {
            player.GetComponent<PlayerStats>().AddArrows(5);
            player.GetComponent<PlayerCombat>().SetRangedWeaponEquipped(true);

            // add shortbow game object to player game object
            GameObject a = Instantiate(shortBow,
                                new Vector3(player.transform.position.x,
                                            player.transform.position.y,
                                            player.transform.position.z), 
                                            player.transform.rotation);

            // Set arrow as child of player gameobject                     
            a.transform.parent = player.transform;
            // newly instanciated objects have "(clone)" at the end of their name, renaming for 
            a.name = "BowAim";
            //Updates the gameobject variable in <Human>
            player.GetComponent<Human>().bowAim = a;
            //Resets the scale, for some reason it spawns tiny on the player without this. Probably just need to change the scale on teh sprite but I'm being lazy
            player.GetComponent<Human>().bowAim.transform.localScale =  new Vector3(1.2f,1.2f,0);
            //Sets it as false for now as we don't want to see the sprite unless we equip it
            player.GetComponent<Human>().bowAim.SetActive(false);
            //Removes the pickup sprite from the level
            Destroy(this.gameObject);


        }
        else if (other.tag == "Player" && player.GetComponent<PlayerCombat>().rangedWeaponEquipped == true)
        {
            player.GetComponent<PlayerStats>().AddArrows(5);
            Destroy(this.gameObject);
        }

    } 
}