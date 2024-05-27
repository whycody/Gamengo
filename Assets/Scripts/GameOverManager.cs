using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Metoda do powrotu do poprzedniej sceny
    public void Quit()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + -1);
    }
}
