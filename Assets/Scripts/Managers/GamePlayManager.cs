using System;
using System.Collections;
using Configs;
using Controllers;
using Data;
using UnityEngine;

namespace Managers
{
    public class GamePlayManager : MonoBehaviour
    {
        #region Singleton

        private static GamePlayManager _instance;
        public static GamePlayManager Instance => _instance;
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion

        public GameConfig gameConfig;
        public GameVehiclesConfig gameVehiclesConfig;
        public OffscreenIndicatorManager indicatorManager;
        public float radius = 5f;

        private int _score = 0;
        private float _currentSpawnInterval;

        [SerializeField] private bool shouldSpawn = true;


        public event EventHandler<VehicleSpawnedEventArgs> OnVehicleSpawned;
        public class VehicleSpawnedEventArgs : EventArgs
        {
            public VehicleController CarController;
        }

        private void Start()
        {
            _currentSpawnInterval = gameConfig.initialSpawnInterval;
            StartCoroutine(SpawnVehicles());
        }

        IEnumerator SpawnVehicles()
        {
            while (true)
            {
                if (shouldSpawn)
                {
                    SpawnVehicle();
                }
                yield return new WaitForSeconds(_currentSpawnInterval);
            }
        }

        private void SpawnVehicle()
        {
            GameObject vehiclePrefab = gameVehiclesConfig.GetVehiclePrefabForLevel(_score / 10);
            if (vehiclePrefab == null) return;

            bool isScareCar = UnityEngine.Random.value < gameConfig.scareCarChance; // Configurable probability

            Vector3 spawnPosition = GetRandomPositionOnCircle(radius);
            Vector3 targetPosition = isScareCar ? GetScareCarTarget(spawnPosition) : Vector3.zero;

            GameObject vehicle = Instantiate(vehiclePrefab, spawnPosition, Quaternion.identity, transform);

            ConfigureVehicle(vehicle, targetPosition, isScareCar);
            var vehicleController = vehicle.GetComponent<VehicleController>();
            OnVehicleSpawned?.Invoke(this, new VehicleSpawnedEventArgs { CarController = vehicleController });
        }

        private void ConfigureVehicle(GameObject vehicle, Vector3 targetPosition, bool isScareCar)
        {
            PrometeoCarController carController = vehicle.GetComponent<PrometeoCarController>();
            if (carController != null)
            {
                carController.maxSpeed = gameConfig.maxVehicleSpeed;
                carController.accelerationMultiplier = gameConfig.accelerationMultiplier;
            }

            VehicleController vehicleController = vehicle.GetComponent<VehicleController>();
            if (vehicleController != null)
            {
                vehicleController.offscreenIndicatorManager = indicatorManager;
                vehicleController.SetTarget(targetPosition);
                if (isScareCar)
                {
                    vehicleController.SetScareMode();
                }
            }
        }

        private Vector3 GetRandomPositionOnCircle(float radius)
        {
            float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
            return new Vector3(
                Mathf.Cos(angle) * radius,
                0f,
                Mathf.Sin(angle) * radius
            );
        }

        private Vector3 GetScareCarTarget(Vector3 spawnPosition)
        {
            Vector3 directionToCenter = (Vector3.zero - spawnPosition).normalized;

            // Move the target slightly to the side instead of directly to the player
            float lateralOffset = UnityEngine.Random.Range(gameConfig.scareCarMinOffset, gameConfig.scareCarMaxOffset);
            Vector3 perpendicularOffset = Vector3.Cross(directionToCenter, Vector3.up) * lateralOffset;

            // Adjust the target position so it passes slightly near the center, but not directly at it
            return Vector3.zero + perpendicularOffset;
        }

        public GameSessionData GetGameSessionData()
        {
            GameSessionData sessionData = GameSessionManager.Instance.GameSessionData;
            return sessionData;
        }

    }
}
