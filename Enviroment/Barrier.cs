using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    private List<int> unlockCode = new List<int>(){2,1,3};
    private List<int> submittedCode = new List<int>();
    private bool isBarrierDown = false;
    public GameObject redRune;
    public GameObject blueRune;
    public GameObject greenRune;

    public void SubmitCode(int code)
    {
        submittedCode.Add(code);
    }

    bool CheckSubmittedCodes()
    {
        bool x = unlockCode[0] == submittedCode[0];
        bool y = unlockCode[1] == submittedCode[1];
        bool z = unlockCode[2] == submittedCode[2];

        if (x && y && z) return true;
        else return false;
    }

    void Update()
    {
        if (submittedCode.Count >= 3 && CheckSubmittedCodes() && isBarrierDown == false)
        {
            //Play a success sound?
            gameObject.SetActive(false);
            isBarrierDown = true;
        }

        if(submittedCode.Count >= 3 && !CheckSubmittedCodes())
        {
            submittedCode = new List<int>();
            //Play a sound indicating failure maybe?
            redRune.GetComponent<Rune>().ResetRune();
            blueRune.GetComponent<Rune>().ResetRune();
            greenRune.GetComponent<Rune>().ResetRune();
        }
    }
}
