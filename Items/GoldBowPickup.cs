using UnityEngine;

public class GoldBowPickup : MonoBehaviour
{
    public string itemName;
    public int damage;
    public int speed;
    private GameObject player;
    private PlayerCombat playerCombat;
    private PlayerMovement.Looking playerIsLooking;
    public float attackDelay;
    public GameObject arrow;

    void Start()
    {
        itemName = "Gold Bow";
        damage = 25;
        attackDelay = 1.0f;
        speed = 10;
        player = GameObject.Find("Player");
        playerCombat = player.GetComponent<PlayerCombat>();

        playerIsLooking = GameObject.Find("Player")
                               .GetComponent<PlayerMovement>()
                               .PlayerIsLooking();
    }

    void Update()
    {
        playerIsLooking = player.GetComponent<PlayerMovement>().PlayerIsLooking();
    }

    public void ShootBow()
    {
        //Ofsets so arrow box collider doesnt clip player collider
        float x = 0;
        float y = 0;

        switch (playerIsLooking)
        {
            default: throw new System.Exception("PlayerMovement.Looking state not recognised");
            
            case PlayerMovement.Looking.Left:
                playerCombat.an.Play("Human_Attack2_Left");
                x = -.4f;
                break;

            case PlayerMovement.Looking.Right:
                playerCombat.an.Play("Human_Attack2_Right");
                x = + .4f;
                break;

            case PlayerMovement.Looking.Up:
                playerCombat.an.Play("Human_Attack2_Up");
                y = +.7f;
                break;

            case PlayerMovement.Looking.Down:
                playerCombat.an.Play("Human_Attack2_Down");
                y = -.7f;
                break;
        }

        // Spawn arrow on top of player
        GameObject a = Instantiate(arrow,
                                new Vector3(player.transform.position.x + x,
                                            player.transform.position.y + y,
                                            player.transform.position.z), 
                                player.transform.rotation);

        // Set arrow as child of player gameobject                     
        a.transform.parent = player.transform;

        //If player facing left flip the arrow sprite
        if (playerIsLooking == PlayerMovement.Looking.Left) a.transform.Rotate(new Vector3(0,180,0));  
        if (playerIsLooking == PlayerMovement.Looking.Up) a.transform.Rotate(new Vector3(0,0,90));  
        if (playerIsLooking == PlayerMovement.Looking.Down) a.transform.Rotate(new Vector3(0,0,-90));  

        //Remove 1 arrow from the player inventory
        GameObject.Find("GameManager").GetComponent<GameManager>().RemoveArrows(1);
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //Remove any pre exsisting bow scripts
            if (player.GetComponent<BowPickup>())
            {
                Destroy(player.GetComponent<BowPickup>());
            }
            
            GameObject.Find("GameManager").GetComponent<GameManager>().AddArrows(5);
            player.AddComponent<GoldBowPickup>();
            player.GetComponent<GoldBowPickup>().arrow = Resources.Load("goldarrow") as GameObject;
            player.GetComponent<PlayerCombat>().SetRangedWeaponEquipped(true);
            player.GetComponent<PlayerCombat>().rangedWeaponName = "Gold Bow";
            player.GetComponent<PlayerCombat>().SetRangedAttack(speed,damage,attackDelay);

            Destroy(this.gameObject);
        }

    } 

}