using System;
using Configs;
using UnityEngine;

namespace Managers
{
    public class GameEventManager : MonoBehaviour
    {
        // Event for when the player successfully jumps
        public static event Action OnSuccessfulJump;
        public static event Action<VehicleDataRewardConfig> OnSuccessfulJumpWithVehicleRewardConfig;
        // Event for when the player fails to jump
        public static event Action OnFailedJump;
        // Event for when Streak is updated
        public static event Action<int> OnStreakUpdated;
        // Event for when Multiplier is updated
        public static event Action<float> OnMultiplierUpdated;
        // Event for when Score is updated
        public static event Action<int> OnScoreUpdated;
        // Event for when the game ends
        public static event Action OnGameEnd;
        // Event for when the vehicle collides with the player
        public static event Action OnVehicleCollision;


        public static void TriggerSuccessfulJump(VehicleDataRewardConfig vehicleRewardConfig)
        {
            Debug.Log("📢 Event Triggered: Successful Jump");
            OnSuccessfulJump?.Invoke();
            OnSuccessfulJumpWithVehicleRewardConfig?.Invoke(vehicleRewardConfig);
        }

        public static void TriggerFailedJump()
        {
            Debug.Log("📢 Event Triggered: Failed Jump");
            OnFailedJump?.Invoke();
        }
        
        public static void TriggerStreakUpdated(int streakCount)
        {
            Debug.Log("📢 Event Triggered: Streak Updated");
            OnStreakUpdated?.Invoke(streakCount);
        }
        
        public static void TriggerMultiplierUpdated(float multiplier)
        {
            Debug.Log("📢 Event Triggered: Multiplier Updated");
            OnMultiplierUpdated?.Invoke(multiplier);
        }
        
        public static void TriggerScoreUpdated(int score)
        {
            Debug.Log("📢 Event Triggered: Score Updated");
            OnScoreUpdated?.Invoke(score);
        }

        public static void TriggerGameEnd()
        {
            Debug.Log("📢 Event Triggered: Game End");
            OnGameEnd?.Invoke();
        }
        public static void TriggerVehicleCollision()
        {
            Debug.Log("📢 Event Triggered: Vehicle Collision");
            OnVehicleCollision?.Invoke();
        }

    }
}