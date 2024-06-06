using System;
using UnityEngine;

public class HangingPlatformsScript : MonoBehaviour
{
    private Vector3 _lastPosition;

    public void Start()
    {
        _lastPosition = transform.position;
    }

    private void Update()
    {
        var deltaPosition = transform.position - _lastPosition;
        _lastPosition = transform.position;
        
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Player"))
            {
                child.position += deltaPosition;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerMovement>();
        player?.SetParent(transform);
    }
    

    private void OnTriggerExit2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerMovement>();
        player?.SetParent(null);
    }
}

