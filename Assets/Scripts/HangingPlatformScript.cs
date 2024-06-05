using System;
using UnityEngine;

public class HangingPlatformsScript : MonoBehaviour
{
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
