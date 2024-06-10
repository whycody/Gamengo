using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public bool IsStarted { get; set; } = false;
    public bool IsPaused { get; set; } = false;
    public bool IsInputDisabled { get; set; } = false;
    private int _currentLevel = 1;

    private HealthManager _healthManager;
    public int CurrentLevel
    {
        get => _currentLevel;
        set
        {
            if (_currentLevel == value) return;
            _currentLevel = value;
            if (CoinsManager.Instance is not null)
                CoinsManager.Instance.SetLevel(value);
        }
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        _healthManager = HealthManager.Instance;
    }

    public void AddHp(int value)
    {
        _healthManager.Health += value;
    }
    
    public int GetHp()
    {
        return _healthManager.Health;
    }
}
