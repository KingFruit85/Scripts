using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTile : MonoBehaviour
{
    public LevelLoader levelLoader;
    public bool isShopLevel;
    void Awake()
    {
        if (SceneManager.GetActiveScene().name == "shop")
        {
            isShopLevel = true;
        }
        
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        var gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Player walks on win tile
        if (other.tag == "Player" && this.tag == "WinGameExit")
        {
            other.GetComponent<PlayerMovement>().moveSpeed = 0;
                levelLoader.LoadNextLevel("PlaceholderWinScreen");
        }

        // PLayer walks on next level tile
        if (other.tag == "Player" && this.tag == "Exit")
        {
            if (isShopLevel && gameManager.currentGameLevel == 2)
            {
                levelLoader.LoadNextLevel("FirstBoss");
                gameManager.currentGameLevel ++;
            }
            else if (isShopLevel)
            {
                levelLoader.LoadNextLevel("PCG");
                gameManager.currentGameLevel ++;
            }
            else
            {
                levelLoader.LoadNextLevel("shop");
            }
        }
        }
    }
