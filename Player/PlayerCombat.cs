using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private SpriteRenderer SR;
    [SerializeField]
    private string currentSprite;
    public float attackRange = 0.5f;
    public Animator an;
    public string rangedWeaponName;
    public string equippedWeaponName;
    private bool canShoot = false;
    public bool rangedWeaponEquipped = false;
    private float rangedCooldown = -9999;
    private float arrowSpeed = 1;
    private int arrowDamage = 10;
    private float rangedAttackDelay = 0.5f;
    private Shaker shaker;
    private bool arrowKnocked = false;
    private PlayerMovement.Looking playerIsLooking;
    private PlayAnimations pa;

    public PlayerStats PS { get; private set; }

    void Start()
    {
       
        SR = GetComponent<SpriteRenderer>();
        an = GetComponent<Animator>();
        pa = GetComponent<PlayAnimations>();
        shaker = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Shaker>();
        equippedWeaponName = "Short Sword";
    }

    public void setEquippedWeaponName(string name)
    {
        equippedWeaponName = name;
    }

    public void SetRangedWeaponEquipped(bool x)
    {
        rangedWeaponEquipped = x;
    }

    private void SetCurrentSprite()
    {
        // Gets the current player sprite with the junk text trimmed off
        currentSprite = SR.sprite.ToString();
        currentSprite = currentSprite.Substring(0,currentSprite.LastIndexOf(" " ) - 1).Trim();
    }

    public string GetCurrentSprite()
    {
        return currentSprite;
    }

    void Attack()
    {
        switch (GetComponent<Health>().currentHost)
        {
            default: throw new System.Exception("currentHost value not recognised");
            case "Human":GetComponent<Human>().SwordAttack();break;
            case "Ghost":GetComponent<Ghost>().GhostBolt();break;
            case "Worm" :GetComponent<Worm>().PoisonBite();break;
        }        
    }

    void KnockArrow()
    {
        arrowKnocked = true;

        if (TryGetComponent(out BowPickup bp))
        {
            GetComponent<PlayAnimations>().SetPlayerKnockedAnimation();
            if (playerIsLooking == PlayerMovement.Looking.Left)
            {
                an.Play("Human_KnockArrow_Left");
            }

            if (playerIsLooking == PlayerMovement.Looking.Right)
            {
                an.Play("Human_KnockArrow_Right");
            }
        }

        if (TryGetComponent(out GoldBowPickup gbp))
        {
            GetComponent<PlayAnimations>().SetPlayerKnockedAnimation();
            if (playerIsLooking == PlayerMovement.Looking.Left)
            {
                an.Play("Human_KnockArrowGOLD_Left");
            }

            if (playerIsLooking == PlayerMovement.Looking.Right)
            {
                an.Play("Human_KnockArrowGOLD_Right");
            }
        } 
    }

    void RangedAttack()
    {
        if (canShoot == true)
        {
            canShoot = false;
            arrowKnocked = false;

            switch (equippedWeaponName)
            {
                default: throw new System.Exception("ranged weapon not recognised");

                case "Short Bow":
                //Finds the bow gameobject on the player and triggers its shoot function
                GameObject.Find("Player").transform.GetComponentInChildren<ShortBow>().ShootBow(mouseClickPosition);
                break;



                case "Gold Bow" :GetComponent<GoldBowPickup>().ShootBow();break; 
            }

            GetComponent<PlayAnimations>().ResetPlayerAnimations();
        }
    }

    public void setRangedAttack(float speed, int damage, float attackDelay)
    {
        arrowSpeed = speed;
        arrowDamage = damage;
        rangedAttackDelay = attackDelay;
    }

    public Vector3 mouseClickPosition;

    public void Update()
    {
        playerIsLooking = GameObject.Find("Player")
                                    .GetComponent<PlayerMovement>()
                                    .playerIsLooking();

        PS = GetComponent<PlayerStats>();
        if (rangedWeaponEquipped == true && PS.getArrowCount() > 0)
        {
                if (Time.time > rangedCooldown + rangedAttackDelay)
                {
                    canShoot = true;
                    rangedCooldown = Time.time;
                }
        }
        SetCurrentSprite();
        GetCurrentSprite();

        mouseClickPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
    }


    void LateUpdate()
    {

        if (GetComponent<Health>().currentHost == "Human" && Input.GetMouseButtonDown(0))
        {
            switch (equippedWeaponName)
            {
                default:throw new System.Exception("equipped weapon not regognised");
                case "Short Sword":Attack();break;

                case "Short Bow":
                    GameObject.Find("Player").transform.GetComponentInChildren<ShortBow>().ShootBow(mouseClickPosition);
                    break;
            }
        }

        if (GetComponent<Health>().currentHost == "Ghost" && Input.GetMouseButtonDown(0))
        {
            
            GetComponent<Ghost>().GhostBolt();
        }

    }
}
