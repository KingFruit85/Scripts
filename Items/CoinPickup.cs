using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField]
    private int coinCount = 1;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            var player = GameObject.Find("Player").GetComponent<PlayerStats>();
            player.AddCoins(coinCount);
            Destroy(gameObject);
            }
        }
}
