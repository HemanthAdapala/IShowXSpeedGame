using System;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    // Event for when a cube passes the center
    public static event Action OnCubePassedCenter;

    // Event for when a cube collides with the player
    public static event Action OnPlayerCollision;

    // Trigger the cube passed center event
    public static void TriggerCubePassedCenter()
    {
        OnCubePassedCenter?.Invoke();
    }

    // Trigger the player collision event
    public static void TriggerPlayerCollision()
    {
        OnPlayerCollision?.Invoke();
    }
}