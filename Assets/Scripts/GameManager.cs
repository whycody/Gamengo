using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource winMusic;
    [SerializeField] private AudioSource loseSound;
    [SerializeField] private AudioSource resumeSound;

    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject levelCompleteScreen;
    [SerializeField] private GameObject player;

    private readonly Vector3[] _lvlsPos = { new(97f, -2.78f, 4.55f), new(285, -4, 0), new(320, -4, 0) };

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
            _currentLevel = value;
            resumeSound.Play();
            PlayerPrefs.SetInt("ChosenLevel", value);
            if (CoinsManager.Instance is not null)
                CoinsManager.Instance.SetLevel(value);
            player.transform.position = _lvlsPos[value];
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameOverScreen.activeSelf || levelCompleteScreen.activeSelf) return;
            if (IsPaused) ResumeGame();
            else PauseGame();
        }
            
    }

    public void HandleFinishingLevel()
    {
        IsPaused = true;
        winMusic.Play();
        levelCompleteScreen.SetActive(true);
        backgroundMusic.volume *= 0.1f;
    }

    public void StartNextLevel()
    {
        IsPaused = false;
        CurrentLevel++;
        backgroundMusic.volume *= 10f;
        levelCompleteScreen.SetActive(false);
    }

    public void HandleDeath()
    {
        IsPaused = true;
        loseSound.Play();
        gameOverScreen.SetActive(true);
        backgroundMusic.volume *= 0.2f;
    }

    private void PauseGame()
    {
        IsPaused = true;
        Time.timeScale = 0f;
        pauseScreen.SetActive(true);
    }

    private void ResumeGame()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        pauseScreen.SetActive(false);
    }

    private void Awake()
    {
        CurrentLevel = PlayerPrefs.GetInt("ChosenLevel");
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        } 
    }
}
