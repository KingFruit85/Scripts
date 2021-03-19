using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    public bool LoadNextLevelFlag = false;

    void Update()
    {
        if(LoadNextLevelFlag)
        {
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {   
        LoadNextLevelFlag = false;
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));

        //Can also request a specific scene as a string
        // SceneManager.LoadScene("Level 1");

        // Or as a scene index
        // SceneManager.LoadScene(1);

    }

    IEnumerator LoadLevel(int LevelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(LevelIndex);
    }
}
