using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject square;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    void Start()
    {
        StartCoroutine(BlinkSquare());
    }

    IEnumerator BlinkSquare()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.5f);
            square.SetActive(true);
            yield return new WaitForSeconds(0.15f);
            square.SetActive(false);
        }
    }
}
