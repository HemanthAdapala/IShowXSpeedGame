using System;
using Managers;
using UnityEngine;

public class LifesHandler : MonoBehaviour
{
    [SerializeField] private Transform[] lifes; // Array of life icons

    void Start()
    {
        GameEventManager.OnPlayerCollision += HandlePlayerCollision;
    }

    private void HandlePlayerCollision()
    {
        int currentLives = GameOverManager.Instance.GetCurrentLives();
        Debug.Log("Life lost! Lives remaining: ");
        lifes[currentLives - 1].gameObject.SetActive(false);
    }

    void OnDisable()
    {
        GameEventManager.OnPlayerCollision -= HandlePlayerCollision;
    }
}
