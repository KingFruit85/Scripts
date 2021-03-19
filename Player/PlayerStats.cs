using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int XP;
    [SerializeField]
    private int arrowCount;
    private int coinCount;

    public void AddXP(int xp)
    {
        XP += xp;
    }

    public void AddArrows(int arrows)
    {
        arrowCount += arrows;
    }

    public void RemoveArrows(int arrows)
    {
        arrowCount -= arrows;
    }

    public int getArrowCount()
    {
        return arrowCount;
    }

    public void AddCoins(int coins)
    {
        coinCount += coins;
    }

    public void RemoveCoins(int coins)
    {
        coinCount -= coins;
    }

    public int getCoinCount()
    {
        return coinCount;
    }

    
    
}
