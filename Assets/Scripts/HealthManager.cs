using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] public int maxHp;
    [SerializeField] GameObject gameManagerObject;
    private GameManager _gameManager;
    public int currHp;

    public int Health
    {
        get => currHp;
        set
        {
            if (currHp == value) return;
            currHp = value;
            if (value == 0) _gameManager.HandleDeath();
            UpdateHp();
        }
    }
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Transform heartsContainer;

    private List<Image> _hearts = new List<Image>();

    private void Awake()
    {
        _gameManager = gameManagerObject.GetComponent<GameManager>();
        currHp = maxHp;
        InstantiateHearts();
    }

    private void InstantiateHearts()
    {
        foreach (Transform child in heartsContainer)
            Destroy(child.gameObject);

        _hearts.Clear();
        
        for (var i = 0; i < maxHp; i++)
        {
            var heartInstance = Instantiate(heartPrefab, heartsContainer);
            var heartImage = heartInstance.GetComponent<Image>();
            _hearts.Add(heartImage);
        }
    }


    private void Update()
    {
        UpdateHp();
    }

    private void UpdateHp()
    {
        foreach (var img in _hearts)
            img.sprite = emptyHeart;

        for (var i = 0; i < currHp; i++)
            _hearts[i].sprite = fullHeart;
    }
}