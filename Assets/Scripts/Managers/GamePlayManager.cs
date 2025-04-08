using System;
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

        [Header("Configurations")]
        public GameConfig gameConfig;
        public GameVehiclesConfig gameVehiclesConfig;

        [Header("References")]
        public OffscreenIndicatorManager indicatorManager;
        public VehicleSpeedManager speedManager;

        [Header("Spawn Settings")]
        [SerializeField] private float radius = 5f;
        [SerializeField] private bool shouldSpawn = true;
        [SerializeField] private float spawnPositionVariance = 0.5f;

        private float _currentSpawnInterval;
        private float _nextSpawnTime;

        public event Action<VehicleController> OnVehicleSpawned;

        private void Start()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            _currentSpawnInterval = gameConfig.initialSpawnInterval;
            _nextSpawnTime = Time.time + _currentSpawnInterval;
            speedManager.Initialize(gameConfig);
        }

        private void Update()
        {
            if (Time.time >= _nextSpawnTime && shouldSpawn)
            {
                SpawnVehicle();
                UpdateSpawnInterval();
                _nextSpawnTime = Time.time + _currentSpawnInterval;
            }
        }

        private void SpawnVehicle()
        {
            var vehicleData = gameVehiclesConfig.GetVehicleForCurrentGameState(GameSessionManager.Instance.GetCurrentStreak());
            if (vehicleData == null) return;
            GameObject vehiclePrefab = vehicleData.prefab;

            if (vehiclePrefab == null) return;

            bool isScareCar = UnityEngine.Random.value < gameConfig.scareCarChance;
            Vector3 spawnPosition = GetRandomPositionOnCircle(radius);

            GameObject vehicle = Instantiate(vehiclePrefab, spawnPosition, Quaternion.identity, transform);
            ConigureVehicle(vehicle, vehicleData, spawnPosition, isScareCar);

            var vehicleController = vehicle.GetComponent<VehicleController>();
            OnVehicleSpawned?.Invoke(vehicleController);
        }

        private void ConigureVehicle(GameObject vehicle,VehicleData vehicleData,Vector3 spawnPosition,bool isScareCar)
        {
            if(vehicle == null) return;
            if(vehicle.TryGetComponent<PrometeoCarController>(out var carController))
            {
                VehicleType type = gameVehiclesConfig.GetVehicleType(vehicleData);
                //Get min and max speed from GameVehiclesConfig
                var minSpeed = gameVehiclesConfig.GetSpeedRange(type);

                carController.maxSpeed = (int)speedManager.CurrentSpeed;
                carController.accelerationMultiplier = gameConfig.accelerationMultiplier;

                // Configure behavior
                if (vehicle.TryGetComponent<VehicleController>(out var vehicleController))
                {
                    vehicleController.offscreenIndicatorManager = indicatorManager;
                    vehicleController.vehicleData = vehicleData;

                    if (isScareCar)
                    {
                        Vector3 normalDestination = -spawnPosition.normalized * radius;
                        Vector3 scarePoint = GetScareOffsetFromCenter();
                        vehicleController.SetScareCarPath(spawnPosition, scarePoint, normalDestination);
                    }
                    else
                    {
                        vehicleController.SetTarget(-spawnPosition.normalized * radius);
                    }

                    vehicleController.OnVehicleDestroyed += HandleVehicleDestroyed;
                    vehicleController.OnVehicleReachedDestination += HandleVehiclePassed;
                }
            }
        }

        //private void ConfigureVehicle(GameObject vehicle, Vector3 spawnPosition, bool isScareCar)
        //{
        //    // Get vehicle data reference
        //    VehicleData vehicleData = gameVehiclesConfig.vehicleRawData
        //        .Find(raw => raw.vehicleDataConfigs.Exists(v => v.prefab == vehicle))
        //        ?.vehicleDataConfigs.Find(v => v.prefab == vehicle);

        //    // Configure physics
        //    if (vehicle.TryGetComponent<PrometeoCarController>(out var carController))
        //    {
        //        carController.maxSpeed = (int)speedManager.CurrentSpeed;
        //        carController.accelerationMultiplier = gameConfig.accelerationMultiplier;

        //        // Optional: Apply type-specific speed modifiers
        //        VehicleType type = gameVehiclesConfig.GetVehicleType(vehicle);
        //        if (type == VehicleType.Slow) carController.maxSpeed *= 0.8f;
        //        if (type == VehicleType.Fast) carController.maxSpeed *= 1.2f;
        //    }

        //    // Configure behavior
        //    if (vehicle.TryGetComponent<VehicleController>(out var vehicleController))
        //    {
        //        vehicleController.offscreenIndicatorManager = indicatorManager;

        //        if (isScareCar)
        //        {
        //            Vector3 normalDestination = -spawnPosition.normalized * radius;
        //            Vector3 scarePoint = GetScareOffsetFromCenter();
        //            vehicleController.SetScareCarPath(spawnPosition, scarePoint, normalDestination);
        //        }
        //        else
        //        {
        //            vehicleController.SetTarget(-spawnPosition.normalized * radius);
        //        }

        //        vehicleController.OnVehicleDestroyed += HandleVehicleDestroyed;
        //        vehicleController.OnVehicleReachedDestination += HandleVehiclePassed;
        //    }
        //}

        private Vector3 GetScareOffsetFromCenter()
        {
            float lateralOffset = UnityEngine.Random.Range(
                gameConfig.scareCarMinOffset,
                gameConfig.scareCarMaxOffset
            );
            return new Vector3(
                UnityEngine.Random.value > 0.5f ? lateralOffset : -lateralOffset,
                0f,
                UnityEngine.Random.value > 0.5f ? lateralOffset : -lateralOffset
            );
        }

        private Vector3 GetRandomPositionOnCircle(float radius)
        {
            float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
            float variance = UnityEngine.Random.Range(-spawnPositionVariance, spawnPositionVariance);
            return new Vector3(
                Mathf.Cos(angle) * (radius + variance),
                0f,
                Mathf.Sin(angle) * (radius + variance)
            );
        }

        private void UpdateSpawnInterval()
        {
            _currentSpawnInterval = Mathf.Max(
                gameConfig.minSpawnInterval,
                _currentSpawnInterval * Mathf.Pow(gameConfig.spawnIntervalReductionFactor, GameSessionManager.Instance.GetCurrentStreak())
            );
        }

        private void HandleVehicleDestroyed()
        {
            speedManager.OnJumpFailed();
        }

        private void HandleVehiclePassed()
        {
            speedManager.OnSuccessfulJump();
        }

        private void OnDestroy()
        {
            // Clean up any remaining vehicles
            var vehicles = FindObjectsOfType<VehicleController>();
            foreach (var vehicle in vehicles)
            {
                vehicle.OnVehicleDestroyed -= HandleVehicleDestroyed;
                vehicle.OnVehicleReachedDestination -= HandleVehiclePassed;
            }
        }

        public GameSessionData GetGameSessionData()
        {
            return GameSessionManager.Instance.GameSessionData;
        }
    }
}