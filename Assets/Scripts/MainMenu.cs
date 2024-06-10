using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject square;
    public AudioSource audioSource;

    public void PlayGame(int level)
    {
        PlaySound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        PlayerPrefs.SetFloat("volume", AudioManager.volume);
        PlayerPrefs.SetInt("ChosenLevel", level);
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
