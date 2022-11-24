using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public float restartDelay = 1f;
    public int currentGameLevel;
    public Color LevelWallBaseColor;
    public Color LevelFloorBaseColor;
    public string currentHost;
    public float XP;
    public int HostStamina = 100;
    public int CravenStamina = 100;
    public int arrowCount;
    public int coinCount;
    public bool rangedWeaponEquipped = false;
    public bool miniBossKilled = false;
    public int meleeAttackBonus = 0;
    public int rangedAttackBonus = 0;
    public int healthBonus;
    public bool playerHit = false;
    public bool spawnFog = false;
    public GameObject CritMsg;
    public int loreIndex = 0;

    public bool deathTimeActive = false;
    public string GameLevelType = string.Empty;
    public float timeTillDeath = 1.0f;


    void Awake()
    {

        //If there is already a host value use that, otherwise assume new game and default to human
        currentHost = PlayerPrefs.GetString("currentHost", "Human");
        XP = PlayerPrefs.GetFloat("XP", 0);
        arrowCount = PlayerPrefs.GetInt("arrowCount", 0);
        coinCount = PlayerPrefs.GetInt("coinCount", 0);
        currentGameLevel = PlayerPrefs.GetInt("currentGameLevel", 1);
        loreIndex = PlayerPrefs.GetInt("loreIndex", 0);
        meleeAttackBonus = PlayerPrefs.GetInt("meleeAttackBonus", 0);
        healthBonus = PlayerPrefs.GetInt("healthBonus", 0);
        rangedAttackBonus = PlayerPrefs.GetInt("rangedAttackBonus", 0);

        //sets a common colour for the floor/wall tiles to reference
        LevelWallBaseColor = GetColor();
        LevelFloorBaseColor = GetColor(LevelWallBaseColor);

        if (PlayerPrefs.GetInt("rangedWeaponEquipped") == 1)
        {
            rangedWeaponEquipped = true;
        }
        else
        {
            rangedWeaponEquipped = false;
        }

        System.Timers.Timer deathTimer = new System.Timers.Timer(1000);

        // deathTimer.Elapsed +=
        // //This function decreases BoomDown every second
        // (object sender, System.Timers.ElapsedEventArgs e) => timeTillDeath--;
        Physics2D.IgnoreLayerCollision(25, 17); // TileTrap arrows ignore walls 
    }

    public void LoadHostScript(string host)
    {

        var player = GameObject.FindGameObjectWithTag("Player");

        switch (currentHost)
        {
            default: throw new System.Exception("failed to load host script, unknown currentHost value");
            case "Human":
                player.AddComponent<Human>();
                player.GetComponent<Animator>().Play(player.GetComponent<Human>().idleDown);
                player.GetComponent<PlayAnimations>().human = GetComponent<Human>();
                break;

            case "Ghost":
                player.AddComponent<Ghost>();
                player.GetComponent<Animator>().Play(player.GetComponent<Ghost>().idleDown);
                player.GetComponent<PlayAnimations>().ghost = GetComponent<Ghost>();
                break;

            case "Worm":
                player.AddComponent<Worm>();
                player.GetComponent<Animator>().Play(player.GetComponent<Worm>().idleDown);
                player.GetComponent<PlayAnimations>().worm = GetComponent<Worm>();
                break;
        }
    }


    void OnDestroy()
    {
        // Saves game data
        PlayerPrefs.SetFloat("XP", XP);
        PlayerPrefs.SetInt("arrowCount", arrowCount);
        PlayerPrefs.SetInt("coinCount", coinCount);
        PlayerPrefs.SetInt("currentGameLevel", currentGameLevel);
        PlayerPrefs.SetString("currentHost", currentHost);
        PlayerPrefs.SetInt("meleeAttackBonus", meleeAttackBonus);
        PlayerPrefs.SetInt("healthBonus", healthBonus);
        PlayerPrefs.SetInt("loreIndex", loreIndex);

        int rangedEquipped = 0;

        if (rangedWeaponEquipped) rangedEquipped = 1;
        else rangedEquipped = 0;


        PlayerPrefs.SetInt("rangedWeaponEquipped", rangedEquipped);
        PlayerPrefs.SetInt("rangedAttackBonus", rangedAttackBonus);

    }

    private void ResetAllStats()
    {
        currentGameLevel = 1;
        XP = 0;
        arrowCount = 0;
        coinCount = 0;
        currentHost = "Human";
        meleeAttackBonus = 0;
        rangedAttackBonus = 0;
        healthBonus = 0;
        rangedWeaponEquipped = false;
        loreIndex = 0;
    }


    public void EndGame()
    {
        ResetAllStats();
        PlayerPrefs.SetFloat("XP", 0);
        PlayerPrefs.SetInt("arrowCount", 0);
        PlayerPrefs.SetInt("loreIndex", 0);

        PlayerPrefs.SetInt("coinCount", 0);
        PlayerPrefs.SetInt("currentGameLevel", 1);
        PlayerPrefs.SetString("currentHost", "Human");
        currentHost = PlayerPrefs.GetString("currentHost");

        PlayerPrefs.SetInt("meleeAttackBonus", 0);
        PlayerPrefs.SetInt("healthBonus", 0);

        PlayerPrefs.SetInt("rangedWeaponEquipped", 0);
        PlayerPrefs.SetInt("rangedAttackBonus", 0);

        Invoke("Restart", restartDelay);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().maxHealth = 1000000000;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().currentHealth = 1000000000;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Human>().swordDamage = 999;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Human>().arrowDamage = 999;
        }

        if (GameLevelType == "shop")
        {
            deathTimeActive = false;
        }
        else
        {
            deathTimeActive = true;
        }

    }

    void LateUpdate()
    {

        if (deathTimeActive)
        {
            if (timeTillDeath > 0 && GameLevelType == "Main")
            {
                timeTillDeath -= Time.deltaTime;
            }

            if (timeTillDeath <= 0 && GameLevelType == "Main")
            {
                // GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().Die();
                // Debug.Log("killing player");
            }
        }

    }

    public void Restart()
    {
        // Restart back to Lab
        SceneManager.LoadScene("Main");

        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    string[] critMessages = new string[] { "FEEL NOTHING", "YOU WILL LOVE ME", "FOCUS", "YOUR MEMORIES AREN'T REAL", "KEEP DREAMING", "NO", "DON'T THINK", "HUSH" };


    private IEnumerator Hit(float duration, bool isCrit)
    {
        playerHit = true;
        // If crit
        if (isCrit)
        {
            CritMsg.GetComponent<TextMeshProUGUI>().text = critMessages[UnityEngine.Random.Range(0, critMessages.Length)];
        }
        yield return new WaitForSeconds(duration);

        CritMsg.GetComponent<TextMeshProUGUI>().text = "";

        playerHit = false;
    }

    public void SetPlayerHit(bool isCrit)
    {
        StartCoroutine(Hit(0.1f, isCrit));
    }

    public void AddMeleeAttackBonus(int bonus)
    {
        meleeAttackBonus += bonus;
        PlayerPrefs.SetInt("meleeAttackBonus", meleeAttackBonus);
    }

    public void AddRangedAttackBonus(int bonus)
    {
        rangedAttackBonus += bonus;
        PlayerPrefs.SetInt("rangedAttackBonus", rangedAttackBonus);
    }

    public void AddHealthBonus(int bonus)
    {
        healthBonus += bonus;
    }

    public void AddXP(float xp)
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

    public int GetArrowCount()
    {
        return arrowCount;
    }

    public void AddCoins(int coins)
    {
        coinCount += coins;
    }

    public Color GetColor()
    {
        List<Color> baseColors = new List<Color>()
                                                    {
                                                    Color.red,
                                                    Color.blue,
                                                    Color.cyan,
                                                    Color.gray,
                                                    Color.green,
                                                    Color.magenta,
                                                    Color.white,
                                                    Color.yellow
                                                    };


        var baseColor = UnityEngine.Random.Range(0, baseColors.Count - 1);

        return baseColors[baseColor];

    }

    public Color GetColor(Color colorToRemove)
    {
        List<Color> baseColors = new List<Color>()
                                                    {
                                                    Color.red,
                                                    Color.blue,
                                                    Color.cyan,
                                                    Color.gray,
                                                    Color.green,
                                                    Color.magenta,
                                                    Color.white,
                                                    Color.yellow
                                                    };

        baseColors.Remove(colorToRemove);

        var baseColor = UnityEngine.Random.Range(0, baseColors.Count - 1);

        return baseColors[baseColor];

    }
}
