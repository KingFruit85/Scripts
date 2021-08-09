using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;
    public Animator transition;
    public float transitionTime = 1f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }


    public void LoadNextLevel()
    {   
        // Get next level by incrementing the current level's index
        // StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));

        //Can also request a specific scene as a string
        SceneManager.LoadScene("PCG");

        // Or as a scene index
        // SceneManager.LoadScene(1);
    }

    public void LoadPCG()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadNextLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    IEnumerator LoadLevel(int LevelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(LevelIndex);
    }


}
