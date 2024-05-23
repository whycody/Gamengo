using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject square;
    public AudioSource audioSource;

    public void PlayGame()
    {
        PlaySound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        PlaySound();
        Debug.Log("Quit");
        Application.Quit();
    }

    public void PlaySound()
    {
        audioSource.Play();
    }
}
