using UnityEngine;

public class TeleportToNextLvl : MonoBehaviour
{
    [SerializeField] private int destinationLevel;
    private Vector3[] _lvlsPos = new[] { new Vector3(0f, 0f, 0f), new Vector3(285, -4, 0) };

    private void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.collider.GetComponent<PlayerMovement>();
        player.transform.position = _lvlsPos[destinationLevel - 1];
    }
}