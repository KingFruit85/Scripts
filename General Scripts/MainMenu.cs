using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    private AudioManager audioManager;
    public AudioClip MainMenuMusic;

    void Awake()
    {
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        audioManager.PlayAudioClip(MainMenuMusic);
    }

    public void PlayGame()
    {
        audioManager.PlayAudioClip("PlayGameMenu");
        SceneManager.LoadScene("Main");
    }

    public void QuitGame()
    {   
        Debug.Log("Quit");
        Application.Quit();
    }
}
