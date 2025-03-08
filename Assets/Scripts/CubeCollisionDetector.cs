using UnityEngine;

public class CubeCollisionDetector : MonoBehaviour
{
    public string CenterColliderTag = "CenterCollider"; // Tag for the center collider
    public string PlayerColliderTag = "PlayerCollider"; // Tag for the player collider

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(CenterColliderTag))
        {
            Debug.Log("Cube passed center");
            // Notify that the cube passed the center
            GameEventManager.TriggerCubePassedCenter();
        }
        else if (other.gameObject.CompareTag(PlayerColliderTag))
        {
            Debug.Log("Cube collided with player");
            // Notify that the cube collided with the player
            GameEventManager.TriggerPlayerCollision();
        }
    }
}