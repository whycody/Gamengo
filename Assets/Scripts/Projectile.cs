using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed = 8f;
    public float lifeTime = 4f;
    private Transform _player;
    private Vector2 _direction;
    private GameObject gameManagerObject;
    private GameManager gameManager;

    public void Awake()
    {
        gameManagerObject = GameObject.FindGameObjectWithTag("GameManager");
        gameManager = gameManagerObject.GetComponent<GameManager>();
        if (!GameObject.FindGameObjectWithTag("Player")) return;
        _player = GameObject.FindGameObjectWithTag("Player").transform.Find("ProjectileTarget");
        _direction = (_player.position - transform.position).normalized;
        Destroy(gameObject, lifeTime);
    }

    public void Update()
    {
        transform.Translate(_direction * (speed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameManager.HandleAttack();
            Destroy(gameObject);
        }
    }
}