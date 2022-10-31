using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    private List<string> unlockCode = new List<string>(){"Red","Green","Blue","Teal"};
    public List<string> submittedCode = new List<string>();
    private bool isBarrierDown = false;
    public GameObject redRune;
    public GameObject blueRune;
    public GameObject greenRune;
    public GameObject tealRune;
    public List<string> randomisedUnlockCode;
    public Sprite OpenSprite;

    public bool unlocksFromPuzzle = false;
    public bool unlocksFromMobKills = false;

    private AudioManager audioManager;
    public DoorController doorController;
    public SpriteRenderer sr;
    public SimpleRoom simpR;
    public bool dcFound = false;
    public bool roomTypeSet = false;


    void Awake()
    {
        var rnd = new System.Random();
        randomisedUnlockCode = unlockCode.OrderBy(item => rnd.Next()).ToList();
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        sr = GetComponent<SpriteRenderer>();


    }
    /// <summary> 1 - puzzle | 2 - mobkills </summary>
    public void SetBarrierUnlockMethod(int code)
    {
        switch (code)
        {
            case 1: unlocksFromPuzzle = true; break;
            case 2: unlocksFromMobKills = true; break;
        }
    }


    public void SubmitCode(string code)
    {
        // gate to stop same rune being submitted twice
        if (!submittedCode.Contains(code))
        {
            submittedCode.Add(code); 

            if (randomisedUnlockCode[submittedCode.Count -1] == code)
            {
                audioManager.PlayAudioClip("RuneSuccess");
                Debug.Log("correct");
            }
            else
            {
                audioManager.PlayAudioClip("RuneFailure");
                Debug.Log("false");
            }
        }
        
    }

    void Update()
    {

        if (unlocksFromMobKills && doorController.roomComplete)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            sr.sprite = OpenSprite;
        }


        if ( !dcFound && transform.parent.parent.transform.Find("DoorController").TryGetComponent(out DoorController dc))
        {
            dcFound = true;
            doorController = dc;
        };

        if (!roomTypeSet &&  transform.parent.parent.TryGetComponent(out SimpleRoom simp))
        {
            if (!unlocksFromMobKills && simp.RoomType == "Prize")
            {
                unlocksFromMobKills = true;
            }

            if (!unlocksFromPuzzle && simp.RoomType == "Puzzle")
            {
                unlocksFromPuzzle = true;
            }
        }

        if (submittedCode.Count == randomisedUnlockCode.Count() && randomisedUnlockCode.SequenceEqual(submittedCode) && !isBarrierDown)
        {
            //Play a success sound?
            gameObject.SetActive(false);
            isBarrierDown = true;  
            var simpR = transform.parent.parent.GetComponent<SimpleRoom>();


            if (simpR.RoomType == "Puzzle")
            {
                doorController.roomComplete = true;
            }          

        }

        if(submittedCode.Count == randomisedUnlockCode.Count() && !randomisedUnlockCode.SequenceEqual(submittedCode))
        {

            // Play a sound indicating failure maybe?
            // If there is a trap linked up to the rune fire it
            switch (submittedCode[submittedCode.Count-1])
            {
                default:throw new System.Exception("Rune not recognised");
                case "Red":redRune.GetComponent<Rune>().myTrap.GetComponent<ArrowTrap>().ActivateOnce();break;
                case "Green":greenRune.GetComponent<Rune>().myTrap.GetComponent<ArrowTrap>().ActivateOnce();break;
                case "Blue":blueRune.GetComponent<Rune>().myTrap.GetComponent<ArrowTrap>().ActivateOnce();break;
                case "Teal":tealRune.GetComponent<Rune>().myTrap.GetComponent<ArrowTrap>().ActivateOnce();break;
            }
            
            // Reset the runes so player can try again
            // Checks if it has a linked flamebowl and resets that also
            if (redRune)
            {
                redRune.GetComponent<Rune>().ResetRune();
                if (redRune.GetComponent<Rune>().flameBowl)
                {
                    redRune.GetComponent<Rune>().flameBowl.GetComponent<FlameBowl>().UnLight();
                }
            }
            if (blueRune)
            {
                blueRune.GetComponent<Rune>().ResetRune();  
                if (blueRune.GetComponent<Rune>().flameBowl)
                {
                    blueRune.GetComponent<Rune>().flameBowl.GetComponent<FlameBowl>().UnLight();
                }
            }
            if (greenRune)
            {
                greenRune.GetComponent<Rune>().ResetRune();
                if (greenRune.GetComponent<Rune>().flameBowl)
                {
                    greenRune.GetComponent<Rune>().flameBowl.GetComponent<FlameBowl>().UnLight();
                }   
            }
            if (tealRune)
            {
                tealRune.GetComponent<Rune>().ResetRune();
                if (tealRune.GetComponent<Rune>().flameBowl)
                {
                    tealRune.GetComponent<Rune>().flameBowl.GetComponent<FlameBowl>().UnLight();
                }    
            }

            // Resets list
            submittedCode = new List<string>();

            

        }
    }
}
