using UnityEngine;

public class TeleportToNextLvl : MonoBehaviour
{
    [SerializeField] private GameObject gameManagerObject;
    [SerializeField] private int destinationLevel;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = gameManagerObject.GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        gameManager.HandleFinishingLevel();
    }
}