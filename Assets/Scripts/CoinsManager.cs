using System;
using TMPro;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    public static CoinsManager Instance;

    private int _coins;
    [SerializeField] private TMP_Text coinsDisplay;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    public void ChangeCoins(int amount)
    {
        _coins += amount; 
    }

    private void OnGUI()
    {
        coinsDisplay.text = _coins.ToString();
    }
}
