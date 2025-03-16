using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Game/GameConfig", order = 1)]
    public class GameConfig : ScriptableObject
    {
        [Header("Vehicle Settings")]
        public float initialSpawnInterval = 2.0f; // Starting spawn rate
        public float minSpawnInterval = 0.5f; // Minimum allowed spawn rate
        public float spawnIntervalReductionFactor = 0.95f; // % reduction per streak

        public int initialVehicleSpeed = 20; // Base speed for vehicles
        public int maxVehicleSpeed = 100; // Maximum vehicle speed
        public float speedIncreaseFactor = 1.05f; // % increase per streak

        [Header("Scare Car Settings")]
        [Range(0f, 1f)] public float scareCarChance = 0.1f; // 10% default chance
        public float scareCarMinOffset = 2f; // Minimum offset from center
        public float scareCarMaxOffset = 5f; // Maximum offset from center

        [Header("Vehicle Acceleration")]
        public int accelerationMultiplier = 5;
    }
}