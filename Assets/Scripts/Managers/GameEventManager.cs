using System;
using UnityEngine;

namespace Managers
{
    public class GameEventManager : MonoBehaviour
    {
        // Event for when a cube passes the center
        public static event Action OnCubePassedCenter;

        // Event for when a cube collides with the player
        public static event Action OnPlayerCollision;

        // Event for when the player successfully jumps
        public static event Action OnSuccessfulJump;
        
        // Event for when the player fails to jump
        public static event Action OnFailedJump;
        

        // Trigger the cube passed center event
        public static void TriggerCubePassedCenter()
        {
            Debug.Log("📢 Event Triggered: Cube Passed Center");
            OnCubePassedCenter?.Invoke();
        }

        public static void TriggerPlayerCollision()
        {
            Debug.Log("📢 Event Triggered: Player Collision");
            OnPlayerCollision?.Invoke();
        }

        public static void TriggerSuccessfulJump()
        {
            Debug.Log("📢 Event Triggered: Successful Jump");
            OnSuccessfulJump?.Invoke();
        }

        public static void TriggerFailedJump()
        {
            Debug.Log("📢 Event Triggered: Failed Jump");
            OnFailedJump?.Invoke();
        }

    }
}