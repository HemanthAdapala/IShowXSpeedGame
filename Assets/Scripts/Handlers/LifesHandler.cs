using System;
using Managers;
using UnityEngine;

public class LifesHandler : MonoBehaviour
{
    [SerializeField] private Transform[] lifes; // Array of life icons

    void Start()
    {
        GameEventManager.OnFailedJump += HandlePlayerCollision;
    }

    private void HandlePlayerCollision()
    {
        int currentLives = GameOverManager.Instance.GetCurrentLives();
        Debug.Log("Life lost! Lives remaining: ");
        Debug.Log("Player collision detected_LifesHandler!");
        lifes[currentLives - 1].gameObject.SetActive(false);
    }

    void OnDisable()
    {
        GameEventManager.OnFailedJump -= HandlePlayerCollision;
    }
}
