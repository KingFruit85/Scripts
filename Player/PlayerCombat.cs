using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private SpriteRenderer SR;
    [SerializeField]
    private string currentSprite;
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
    private PlayerMovement.Looking playerIsLooking;
    private PlayAnimations pa;
    private GameManager gameManager;


    void Start()
    {
       
        SR = GetComponent<SpriteRenderer>();
        an = GetComponent<Animator>();
        pa = GetComponent<PlayAnimations>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        shaker = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Shaker>();
        equippedWeaponName = "Short Sword";
    }

    public void SetEquippedWeaponName(string name)
    {
        equippedWeaponName = name;
    }

    public void SetRangedWeaponEquipped(bool x)
    {
        rangedWeaponEquipped = x;
        GameObject.Find("GameManager").GetComponent<GameManager>().rangedWeaponEquipped = true;
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
        switch (gameManager.currentHost)
        {
            default: throw new System.Exception("currentHost value not recognised");
            case "Human":
                if (gameManager.rangedWeaponEquipped)
                {
                    GetComponent<Human>().BowAttack(mouseClickPosition);
                }
                else
                {
                    GetComponent<Human>().SwordAttack();
                }
                break;


            case "Ghost":GetComponent<Ghost>().FireGhostBolt();break;


            case "Worm" :GetComponent<Worm>().PoisonBite();break;
        }        
    }

    public void SetRangedAttack(float speed, int damage, float attackDelay)
    {
        arrowSpeed = speed;
        arrowDamage = damage;
        rangedAttackDelay = attackDelay;
    }

    public Vector3 mouseClickPosition;

    public void Update()
    {
        playerIsLooking = GameObject.FindGameObjectWithTag("Player")
                                    .GetComponent<PlayerMovement>()
                                    .PlayerIsLooking();

        var GS = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (rangedWeaponEquipped == true && GS.GetArrowCount() > 0)
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

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }


}
