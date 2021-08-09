using UnityEngine;
using UnityEngine.UI;


public class CoinDisplay : MonoBehaviour
{
    private int remainingCoins = 0;
    public Text coinText;

    void Update()
    {
        remainingCoins = GameObject.Find("GameManager").GetComponent<GameManager>().getArrowCount();
        coinText.text = "x" + remainingCoins;
    }
}
