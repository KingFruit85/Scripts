using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    private int coinCount = 1;
    private bool playerIsPhasing = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Ghost ghost))
        {
            playerIsPhasing = ghost.Phasing();
        }

        if (other.tag == "Player" && !playerIsPhasing)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().AddCoins(coinCount);
            Destroy(gameObject);
        }
    }


}
