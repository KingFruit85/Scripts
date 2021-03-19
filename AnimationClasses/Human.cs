using System.Collections;
using UnityEngine;
public class Human : MonoBehaviour
{
    private string idleLeft;
    private string idleRight;
    private string walkLeft;
    private string walkRight;
    private string walkUp;
    private string walkDown;
    private string idleUp;
    private string idleDown;
    private string death;
    private string knockLeft;
    private string knockRight;
    private string knockUp;
    private string knockDown;

    public int swordDamage = 10;
    public float swordRange = 0.5f;



    public Human()
    {
        idleLeft = "Human_Idle_Left";
        idleRight = "Human_Idle_Right";
        walkLeft = "Human_Walk_Left";
        walkRight = "Human_Walk_Right";    
        walkUp = "Human_Move_Up";
        walkDown = "Human_Move_Down";
        idleUp = "Human_Idle_Up";
        idleDown = "Human_Idle_Down";
        death = "Human_Death";

        knockLeft = "Human_KnockArrow_Left";
        knockRight = "Human_KnockArrow_Right";

        
        
    }

    public void SwordAttack(Vector2 attackPoint, LayerMask enemyLayers)
    {
        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint, swordRange, enemyLayers);

        // Damage them
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Health>().TakeDamage(swordDamage);   
        }
    }

    public void BowAttack(string bow)
    {
        switch (bow)
        {
            default: throw new System.Exception("Supplied bow name not recognised");
            
            case "Short Bow":
                GetComponent<BowPickup>().ShootBow();
                break;

            case "Gold Bow":
                GetComponent<GoldBowPickup>().ShootBow();
                break;

        }

    }

}