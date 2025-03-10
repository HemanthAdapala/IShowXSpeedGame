using System;
using UnityEngine;

public class PassingVehicleController : MonoBehaviour
{
    void Start()
    {
        GameEventManager.OnPlayerCollision += HandlePlayerCollision;
    }

    private void HandlePlayerCollision()
    {
        Destroy(gameObject);
    }

    private void IncreaseScore()
    {

    }

    void OnDisable()
    {
        GameEventManager.OnPlayerCollision -= HandlePlayerCollision;
    }
}
