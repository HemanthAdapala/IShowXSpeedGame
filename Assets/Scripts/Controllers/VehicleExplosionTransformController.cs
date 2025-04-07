using System;
using Controllers;
using Managers;
using UnityEngine;

public class VehicleExplosionTransformController : MonoBehaviour
{
    [SerializeField] private Transform explosionParentTransform;
    [SerializeField] private GameObject explosionPrefab;

    private void Start()
    {
        GameEventManager.OnVehicleCollision += HandleVehicleCollision;
    }

    private void HandleVehicleCollision()
    {
        ParticleSystem particleSystem = explosionPrefab.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Play();
        }
        VehicleController vehicleController = GetComponentInParent<VehicleController>();
        vehicleController.StopAndDestroy();
    }

    private void OnDestroy()
    {
        GameEventManager.OnVehicleCollision -= HandleVehicleCollision;
    }
}
