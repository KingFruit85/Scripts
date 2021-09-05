using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    private AudioManager audioManager;

    void Awake()
    {
        audioManager = GameObject.FindObjectOfType<AudioManager>();
    }

    public void PlayGame()
    {
        audioManager.Play("PlayGameMenu");
        SceneManager.LoadScene("Lab");
    }

    public void QuitGame()
    {   
        Debug.Log("Quit");
        Application.Quit();
    }
}
