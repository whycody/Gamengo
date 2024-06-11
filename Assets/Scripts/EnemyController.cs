using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float detectionRange = 10f;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    [SerializeField] public bool horizontal;

    private Transform _player;
    private float _nextFireTime;
    private Animator _anim;
    private static readonly int Attack = Animator.StringToHash("Attack");

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _anim = GetComponent<Animator>();

    }

    private void Update()
    {
        if (_player?.tag is not "Player") return;
        var distanceToPlayer = Vector2.Distance(transform.position, _player.position);

        if (!(distanceToPlayer <= detectionRange) || !(Time.time >= _nextFireTime)) return;
        var stateInfo = _anim.GetCurrentAnimatorStateInfo(0);

        if (_anim.IsInTransition(0) || !stateInfo.IsName("Idle")) return;
        _anim.SetTrigger(Attack);
        _nextFireTime = Time.time + 1f / fireRate;
    }

    private void FireProjectile()
    {
        var projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }
}
