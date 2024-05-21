using System;
using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed;
    [SerializeField] private float checkDistance = 0.05f;

    private Transform _targetWaypoint;
    private int _currentWaypointIndex = 0;

    private void Start()
    {
        _targetWaypoint = waypoints[0];
    }


    private void Update()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            _targetWaypoint.position,
            speed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, _targetWaypoint.position) < checkDistance)
            _targetWaypoint = GetNextWaypoint();
    }

    private Transform GetNextWaypoint()
    {
        _currentWaypointIndex++;
        if (_currentWaypointIndex >= waypoints.Length)
            _currentWaypointIndex = 0;

        return waypoints[_currentWaypointIndex];
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.collider.GetComponent<PlayerMovement>();
        player?.SetParent(transform);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        var player = other.collider.GetComponent<PlayerMovement>();
        player?.ResetParent();
    }
}