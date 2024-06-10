using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;
    private Transform _player;
    private Vector2 _direction;
    private bool _horizontal;

    public void Awake()
    {
        if (!GameObject.FindGameObjectWithTag("Player")) return;
        _player = GameObject.FindGameObjectWithTag("Player").transform.Find("ProjectileTarget");
        Destroy(gameObject, lifeTime);
        _horizontal = _player.position.y + 0.5 < transform.position.y;
        if (_horizontal) 
            _direction = _player.position.x < transform.position.x ? Vector2.left : Vector2.right;
        else
            _direction = (_player.position - transform.position).normalized;
    }

    public void Update()
    {
        transform.Translate(_direction * (speed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Zniszczenie pocisku po kolizji
        // Tutaj możesz dodać dodatkową logikę, np. zadającą obrażenia graczowi
        HealthManager.Instance.Health -= 1;
        print(HealthManager.Instance.Health);
        Destroy(gameObject);
    }
}