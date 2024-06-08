using UnityEngine;

public class TeleportToNextLvl : MonoBehaviour
{
    [SerializeField] private int destinationLevel;
    private readonly Vector3[] _lvlsPos = { new(0f, 0f, 0f), new(285, -4, 0) };

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerMovement>();
        if (player is null) return;
        player.transform.position = _lvlsPos[destinationLevel - 1];
        CoinsManager.Instance.SetLevel(destinationLevel);
    }
}