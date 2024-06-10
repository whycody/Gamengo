using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;
    public int maxHp = 3;
    public int currHp;

    public int Health
    {
        get => currHp;
        set
        {
            if (currHp == value) return;
            currHp = value <= 0 ? 0 : value;
            if (currHp == 0) return;
            UpdateHp();
        }
    }

    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    private GameManager _gameManager;
    
    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);

        currHp = maxHp;
        _gameManager = GameManager.Instance;
    }


    private void Update()
    {
        UpdateHp();
    }

    private void UpdateHp()
    {
        foreach (var img in hearts)
            img.sprite = emptyHeart;

        for (var i = 0; i < currHp; i++)
            hearts[i].sprite = fullHeart;
    }
}