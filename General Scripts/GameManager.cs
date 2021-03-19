using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    // private bool gameHasEnded = false;
    public float restartDelay= 1f;

    public void EndGame()
    {
            // Restart game
            Invoke("Restart", restartDelay);
    }

    public void LoadNextLevel()
    {
        GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadNextLevelFlag = true;
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
