using System;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private int value = 1;
    private bool _hasTriggered;

    private CoinsManager _coinManager;

    private void Start()
    {
        _coinManager = CoinsManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || _hasTriggered) return;
        _hasTriggered = true;
        _coinManager.ChangeCoins(value);
        audioSource.Play();
        Destroy(gameObject);
    }
}
