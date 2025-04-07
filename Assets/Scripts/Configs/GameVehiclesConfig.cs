using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "GameVehiclesConfig", menuName = "Game/GameVehiclesConfig", order = 2)]
    public class GameVehiclesConfig : ScriptableObject
    {
        [System.Serializable]
        public class VehicleData
        {
            public GameObject prefab;
            public VehicleType type;
            public int unlockAtStreak = 0;
            public float spawnWeight = 1f;
        }

        public VehicleData[] vehicles;

        public GameObject GetVehicleForCurrentGameState(int currentStreak, float currentSpeed)
        {
            if (vehicles.Length == 0) return null;

            // Filter available vehicles based on streak
            var availableVehicles = System.Array.FindAll(vehicles, v => v.unlockAtStreak <= currentStreak);

            // Calculate total weight for probability distribution
            float totalWeight = 0;
            foreach (var vehicle in availableVehicles)
            {
                totalWeight += vehicle.spawnWeight;
            }

            // Select random vehicle with weighted probability
            float randomPoint = Random.value * totalWeight;
            for (int i = 0; i < availableVehicles.Length; i++)
            {
                if (randomPoint < availableVehicles[i].spawnWeight)
                {
                    return availableVehicles[i].prefab;
                }
                randomPoint -= availableVehicles[i].spawnWeight;
            }

            return availableVehicles[0].prefab; // fallback
        }

        public GameObject GetVehiclePrefabForLevel(int v)
        {
            Debug.Log($"GetVehiclePrefabForLevel: {v}");
            return vehicles[v].prefab;
        }

        public VehicleType GetVehicleType(GameObject vehiclePrefab)
        {
            return System.Array.Find(vehicles, v => v.prefab == vehiclePrefab).type;
        }
    }
    public enum VehicleType
    {
        Slow,
        Normal,
        Fast,
        Special
    }
}
