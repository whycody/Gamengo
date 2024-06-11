using UnityEngine;

public class Projectile : MonoBehaviour
{
    private const float Speed = 8f;
    public float lifeTime = 4f;
    private Transform _player;
    private Vector2 _direction;
    private GameObject _gameManagerObject;
    private GameManager _gameManager;
    private AudioSource _attackSound;

    public void Awake()
    {
        _attackSound = GameObject.FindGameObjectWithTag("AttackSound").GetComponent<AudioSource>();
        _attackSound.Play();
        _gameManagerObject = GameObject.FindGameObjectWithTag("GameManager");
        _gameManager = _gameManagerObject.GetComponent<GameManager>();
        if (!GameObject.FindGameObjectWithTag("Player")) return;
        _player = GameObject.FindGameObjectWithTag("Player").transform.Find("ProjectileTarget");
        _direction = (_player.position - transform.position).normalized;
        Destroy(gameObject, lifeTime);
    }

    public void Update()
    {
        transform.Translate(_direction * (Speed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        _gameManager.HandleAttack();
        Destroy(gameObject);
    }
}