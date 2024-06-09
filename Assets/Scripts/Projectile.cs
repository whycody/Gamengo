using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;
    private Transform _player;
    private Vector2 _direction;

    public void Awake()
    {
        if(!GameObject.FindGameObjectWithTag("Player")) return;
        _player = GameObject.FindGameObjectWithTag("Player").transform.Find("ProjectileTarget");
        Destroy(gameObject, lifeTime);
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
        Destroy(gameObject);
    }
}
