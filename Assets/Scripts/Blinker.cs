using UnityEngine;
using System.Collections;

public class Blinker : MonoBehaviour
{
    public GameObject square;
    public float seconds = 2.5f;

    private void Start()
    {
        StartCoroutine(BlinkSquare());
    }

    private IEnumerator BlinkSquare()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(seconds);
            square.SetActive(true);
            yield return new WaitForSecondsRealtime(0.15f);
            square.SetActive(false);
        }
    }
}