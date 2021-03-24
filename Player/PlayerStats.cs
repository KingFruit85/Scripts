using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int XP;
    [SerializeField]
    private int arrowCount;
    private int coinCount;
    private GameObject player;
    public string currentHost;

    void Awake()
    {
        // Player always starts as human
        gameObject.AddComponent<Human>();
        currentHost = "Human";
    }

    void ChangeHostBody(string newHost)
    {

        if (newHost != currentHost)
        {
            // Remove the current host script
            switch (currentHost)
            {
                default: throw new System.Exception("Unable to remove currentHost");

                case "Human" : Destroy(gameObject.GetComponent<Human>());break; 
                case "Ghost" : Destroy(gameObject.GetComponent<Ghost>());break;
                case "Worm"  : Destroy(gameObject.GetComponent<Worm>());break;
            }

            // Add new host script
            switch (newHost)
            {
                default: throw new System.Exception("newHost not recognised");

                case "Human" : 
                    gameObject.AddComponent<Human>();
                    gameObject.GetComponent<Animator>().Play(gameObject.GetComponent<Human>().idleDown);
                    gameObject.GetComponent<PlayAnimations>().human = GetComponent<Human>();
                    break; 

                case "Ghost" : 
                    gameObject.AddComponent<Ghost>();
                    gameObject.GetComponent<Animator>().Play(gameObject.GetComponent<Ghost>().idleDown);
                    gameObject.GetComponent<PlayAnimations>().ghost = GetComponent<Ghost>();
                    break;

                case "Worm"  : 
                    gameObject.AddComponent<Worm>();
                    gameObject.GetComponent<Animator>().Play(gameObject.GetComponent<Worm>().idleDown);
                    gameObject.GetComponent<PlayAnimations>().worm = GetComponent<Worm>();
                    break;
            }
            currentHost = newHost;   
        }    
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeHostBody("Worm");
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            ChangeHostBody("Ghost");
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            ChangeHostBody("Human");
        }
    }

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
