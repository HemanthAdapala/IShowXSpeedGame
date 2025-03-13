using System;
using Unity.VisualScripting;
using UnityEngine;

public class CubeCollisionDetector : MonoBehaviour
{
    private const string CenterColliderTag = "CenterCollider"; // Tag for the center collider
    private const string PlayerColliderTag = "PlayerCollider"; // Tag for the player collider

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(CenterColliderTag))
        {
            Debug.Log("Cube passed center");
            // Notify that the cube passed the center
            GameEventManager.TriggerCubePassedCenter();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(PlayerColliderTag))
        {
            Debug.Log("Cube collided with player");
            // Notify that the cube collided with the player
            GameEventManager.TriggerPlayerCollision();
        }
    }
}