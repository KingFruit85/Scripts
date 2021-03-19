using System.Collections.Generic;
using UnityEngine;

public class PuzzleChest : MonoBehaviour
{
    private List<int> secretCode = new List<int>(){2,1,3};
    public List<int> playerCode = new List<int>();
    private bool barrierDown;
    public GameObject redRune;
    public GameObject blueRune;
    public GameObject greenRune;

    public void SubmitRune(int runeCode)
    {
        playerCode.Add(runeCode);
    }

    bool CheckPlayerCodes()
    {
        bool one = secretCode[0] == playerCode[0];
        bool two = secretCode[1] == playerCode[1];
        bool three = secretCode[2] == playerCode[2];

        if (one && two && three)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Update()
    {
        if (playerCode.Count >= 3 && CheckPlayerCodes() && barrierDown == false)
        {
            GameObject.Find("Barrier").SetActive(false);
            barrierDown = true;
        }
        if(playerCode.Count >= 3 && !CheckPlayerCodes())
        {
            playerCode = new List<int>();
            redRune.GetComponent<Rune>().ResetRune();
            blueRune.GetComponent<Rune>().ResetRune();
            greenRune.GetComponent<Rune>().ResetRune();
        }
    }
}
