using Controllers;
using Managers;
using UnityEngine;

public class CubeCollisionDetector : MonoBehaviour
{
    private const string CenterColliderTag = "CenterCollider"; // Tag for the center collider
    private const string PlayerColliderTag = "PlayerCollider"; // Tag for the player collider

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(CenterColliderTag))
        {
            Debug.Log("vehicle passed center");
            // Notify that the vehicle passed the center
            var vehicleRewardConfig = this.gameObject.GetComponent<VehicleController>().GetVehicleDataRewardConfig();
            GameEventManager.TriggerSuccessfulJump(vehicleRewardConfig);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(PlayerColliderTag))
        {
            Debug.Log("vehicle collided with player");
            // Notify that the vehicle collided with the player
            GameEventManager.TriggerFailedJump();
        }
    }
}