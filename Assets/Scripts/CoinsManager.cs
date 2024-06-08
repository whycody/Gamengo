using TMPro;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    public static CoinsManager Instance;

    private int _coins;
    private int _totalCoinsOnLevel;
    [SerializeField] private TMP_Text coinsDisplay;

    private int _currentLevel;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        UpdateTotalCoinsOnLevel();
        UpdateCoinsDisplay();
    }
    
    private void Start()
    {
        _currentLevel = GameManager.Instance.CurrentLevel;
        UpdateTotalCoinsOnLevel();
        UpdateCoinsDisplay();
    }

    public void ChangeCoins(int amount)
    {
        _coins += amount;
        UpdateCoinsDisplay();
    }

    public void SetLevel(int level)
    {
        _currentLevel = level;
        UpdateTotalCoinsOnLevel();
        UpdateCoinsDisplay();
    }

    private void UpdateTotalCoinsOnLevel()
    {
        var levelNode = GameObject.Find("CoinsOnLvl" + _currentLevel);
        _totalCoinsOnLevel = levelNode ? levelNode.transform.childCount : 0;
        _coins = 0;
    }

    private void UpdateCoinsDisplay()
    {
        coinsDisplay.text = $"{_coins}/{_totalCoinsOnLevel}";
    }

    private void OnGUI()
    {
        UpdateCoinsDisplay();
    }
}
