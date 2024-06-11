using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource winMusic;
    [SerializeField] private AudioSource loseSound;
    [SerializeField] private AudioSource resumeSound;
    [SerializeField] private AudioSource hurtSound;

    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject levelCompleteScreen;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject healthContainer;

    [SerializeField] private AudioClip[] levelsClips;
//285 -4 0
    private readonly Vector3[] _lvlsPos = { new(-6.5f, -2.4f, 0), new(285f, -4f, 0), new(465f, -0.34f, 0) };

    public bool IsStarted { get; set; } = false;
    public bool IsPaused { get; set; } = false;
    public bool IsInputDisabled { get; set; } = false;
    private int _currentLevel = 1;

    private HealthManager _healthManager;
    private PlayerMovement _playerMovement;
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
            StartCoroutine(changeCurrentLevelMusic(value));
        }
    }

    private IEnumerator changeCurrentLevelMusic(int level)
    {
        yield return new WaitForSeconds(0.75f);
        backgroundMusic.clip = levelsClips[level];
        backgroundMusic.Play();
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
        _playerMovement.ResetParams();
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
        _healthManager.Health = _healthManager.maxHp;
    }

    public void HandleDeath()
    {
        _playerMovement.ResetParams();
        IsPaused = true;
        loseSound.Play();
        gameOverScreen.SetActive(true);
        backgroundMusic.volume *= 0.2f;
    }

    public void HandleAttack()
    {
        if (_healthManager.Health > 0) hurtSound.Play();
        _healthManager.Health--;
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
        _playerMovement = player.GetComponent<PlayerMovement>();
        _healthManager = healthContainer.GetComponent<HealthManager>();
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
