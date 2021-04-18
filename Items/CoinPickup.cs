using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField]
    private int coinCount = 1;

    private bool playerIsPhasing;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !playerIsPhasing)
        {
            var player = GameObject.Find("Player").GetComponent<PlayerStats>();
            player.AddCoins(coinCount);
            Destroy(gameObject);
        }
    }

    void Update()
    {
        playerIsPhasing = GameObject.Find("Player").GetComponent<PlayerStats>().isPhasing;
    }
}
