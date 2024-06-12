using UnityEngine;

public class TeleportToNextLvl : MonoBehaviour
{
    [SerializeField] private GameObject gameManagerObject;
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = gameManagerObject.GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _gameManager.HandleFinishingLevel();
    }
}