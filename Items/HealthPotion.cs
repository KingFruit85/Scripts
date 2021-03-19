using UnityEngine;
public class HealthPotion : MonoBehaviour
{
    [SerializeField]
    private int HealthAmount = 50;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            var player = GameObject.Find("Player")
                                   .GetComponent<Health>();

            if (player.currentHealth < player.maxHealth)
            {
                if ((player.currentHealth + HealthAmount) > player.maxHealth)
                {
                    player.currentHealth = player.maxHealth;
                }
                else
                {
                    player.AddHealth(HealthAmount); 
                }
                Destroy(gameObject);
            }
        }
    }
}
