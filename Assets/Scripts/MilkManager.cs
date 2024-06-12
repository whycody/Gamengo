using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkManager : MonoBehaviour
{

    [SerializeField] private AudioSource swallowSound;
    private GameObject _gameManagerObject;
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManagerObject = GameObject.FindGameObjectWithTag("GameManager");
        _gameManager = _gameManagerObject.GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (!collision.CompareTag("Player")) return;
            _gameManager.HandleAddingHp();
            swallowSound.Play();
            Destroy(gameObject);
        }
    }
}
