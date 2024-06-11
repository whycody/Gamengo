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
    [SerializeField] private GameObject[] levelsBackgrounds;

    private readonly Vector3[] _lvlsPos = { new(-6.5f, -2.4f, 0), new(285f, -4f, 0), new(580f, -4.3f, 0) };
    private bool[] _completedLvl = new[] { false, false, false };

    public bool IsPaused { get; set; }
    public bool IsSlowed { get; set; }
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
            StartCoroutine(ChangeCurrentLevelMusic(value));
            ChangeCurrentLevelBackground(value);
        }
    }

    private void Awake()
    {
        _playerMovement = player.GetComponent<PlayerMovement>();
        _healthManager = healthContainer.GetComponent<HealthManager>();
        CurrentLevel = PlayerPrefs.GetInt("ChosenLevel");

        if (!Instance) Instance = this;
        else Destroy(gameObject);
    }

    private IEnumerator ChangeCurrentLevelMusic(int level)
    {
        yield return new WaitForSeconds(0.75f);
        backgroundMusic.clip = levelsClips[level];
        backgroundMusic.Play();
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        if (gameOverScreen.activeSelf || levelCompleteScreen.activeSelf) return;
        if (IsPaused) ResumeGame();
        else PauseGame();
    }

    public void HandleFinishingLevel()
    {
        _playerMovement.ResetParams();
        IsPaused = true;
        winMusic.Play();
        _completedLvl[_currentLevel] = true;
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
        _healthManager.Health = 0;
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

    private void ChangeCurrentLevelBackground(int lvl)
    {
        for (var i = 0; i < levelsBackgrounds.Length; i++)
            levelsBackgrounds[i].SetActive(i == lvl);
    }

    public void SlowPlayer(float duration)
    {
        if (!IsSlowed)
            StartCoroutine(SlowPlayerCoroutine(duration));
    }

    private IEnumerator SlowPlayerCoroutine(float duration)
    {
        IsSlowed = true;
        yield return new WaitForSeconds(duration);
        IsSlowed = false;
    }
}