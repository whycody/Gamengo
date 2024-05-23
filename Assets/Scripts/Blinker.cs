using UnityEngine;
using System.Collections;

public class Blinker : MonoBehaviour
{
    public GameObject square;

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