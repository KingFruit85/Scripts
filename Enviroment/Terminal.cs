using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    public float dist;
    public Vector3 playerPOS;
    public GameManager gameManager;
    public bool used = false;
    private bool inUse;

    private List<TextContent> TerminalMessages = new List<TextContent>();

    public string myTitle = "";
    public string myContent = "";

    public void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        TerminalMessages.Add(new TextContent(
            "KRVN-0 Initial discovery",
            "Description: KRVN-0 is an exactly 100ml volume of inert black non-Euclidean liquid. Samples taken from the original body of liquid quickly revert to pure water making chemical analysis difficult. The original sample was observed to recover it’s lost mass between approximately 5 - 20minutes after the failed sample was taken via some yet unknown mechanism. Dr. NAME has instructed that only no-destructive/invasive methods of analysis and experimentation are performed on KRVN-0 while further sample exploration and discovery are ongoing at site ALPHA.",
            "Object Status: Safe."
        ));
        TerminalMessages.Add(new TextContent(
            "KRVN-0 Test",
            "description: The original volume of liquid is placed in a drum centrifuge with a transparent casing. The volume is spun up to 7,500 RPM. No component separation was able to be observed, liquid was spread extremely thin but remained completely opaque to molecular observation.",
            "Object Status: Safe."
        ));
        TerminalMessages.Add(new TextContent(
            "Observation: Junior lab technician ██████",
            "Description: Junior lab technician ██████ claims that the volume of liquid displaced itself to gather at the side of its storage container nearest to ██████ when they were taking inventory at ██/██/██ ██:██:██. CCTV footage shows the liquid at its normal resting state before and after ██████ obscures the camera with their body.",
            "Object Status: Safe."
        ));
        TerminalMessages.Add(new TextContent(
            "Observation: Junior lab technician ██████",
            "Description: Random drug screening returned negative for all standard psychotropic drugs. Dr ███ reported that Junior lab technician ██████ was uncharacteristically dispondent during the testing.",
            "Object Status: Safe."
        ));

    }

    // Terminal should have perhaps two different text sections title/body
    // TerminalMessages list should be a list of lists or tupples maybe? so we can store the title/body
    void setTerminalContent(string title, string content)
    {

    }
    

    void Update()
    {

        dist = Vector3.Distance(playerPOS, transform.position);
        playerPOS = GameObject.Find("Player").transform.position;
        
        if (dist < 1.5f && Input.GetKeyDown(KeyCode.E) && !inUse)
        {
            int x = (Screen.width / 2);
            int y = (Screen.height / 2);

            // If we've run out of messages then just return the last message in the collection
            var i = (gameManager.loreIndex >= (TerminalMessages.Count -1)) ? (TerminalMessages.Count -1) : gameManager.loreIndex;
            if (!used)
            {
                myTitle = TerminalMessages[i].Title;
                myContent = TerminalMessages[i].Description;
                GameObject tWindow = Instantiate(Resources.Load("TerminalScreen"),new Vector3(x,y,0),Quaternion.identity) as GameObject;
                    tWindow.name = "tWindow";
                    tWindow.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = myTitle;
                    tWindow.transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = myContent;
                gameManager.loreIndex++;
                used = true;
            }
            else
            {
                GameObject tWindow = Instantiate(Resources.Load("TerminalScreen"),new Vector3(x,y,0),Quaternion.identity) as GameObject;
                    tWindow.name = "tWindow";
                    tWindow.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = myTitle;
                    tWindow.transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = myContent; 
            }

        }

        if (GameObject.Find("tWindow")) inUse = true;
        else inUse = false;
    }

}
