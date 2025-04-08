using Controllers;
using Managers;
using UnityEngine;

public class CubeCollisionDetector : MonoBehaviour
{
    private const string CenterColliderTag = "CenterCollider";
    private const string PlayerColliderTag = "PlayerCollider";
    private const string VehicleColliderTag = "VehicleCollider";

    private bool _hasTriggeredCollision = false;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(CenterColliderTag))
        {
            Debug.Log("Vehicle passed center");
            var vehicleReward = GetComponent<VehicleController>().GetVehicleDataRewardConfig();
            GameEventManager.TriggerSuccessfulJump(vehicleReward);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(PlayerColliderTag) && !_hasTriggeredCollision)
        {
            _hasTriggeredCollision = true;
            Debug.Log("Vehicle collided with player");
            GameEventManager.TriggerFailedJump();

            // The vehicle will be destroyed by whatever handles the failed jump
            // No need to reset the flag since object is being destroyed
        }
        else if (other.gameObject.CompareTag(VehicleColliderTag))
        {
            Debug.Log("Vehicle collided with another vehicle");
            // Vehicle-to-vehicle collision logic
        }
    }
}