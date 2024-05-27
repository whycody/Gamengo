using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBlinker : MonoBehaviour
{
    public GameObject square;
    public float seconds = 2.5f;

    void Start()
    {
        StartCoroutine(BlinkSquare());
    }

    IEnumerator BlinkSquare()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(seconds);
            square.SetActive(true);
            yield return new WaitForSecondsRealtime(seconds);
            square.SetActive(false);
        }
    }
}
