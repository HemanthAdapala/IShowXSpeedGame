using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "GameVehiclesConfig", menuName = "Game/GameVehiclesConfig", order = 2)]
    public class GameVehiclesConfig : ScriptableObject
    {
        public List<VehicleRawData> vehicleRawData; // Now uses VehicleRawData for structured organization

        private List<VehicleData> allVehicles; // This can be used for quick access to all vehicles if needed

        //Save all vehicle data in a list
        private void Awake()
        {
            //Cache all vehicle data in a list
            allVehicles = new List<VehicleData>();
            foreach (var raw in vehicleRawData)
            {
                allVehicles.AddRange(raw.vehicleDataConfigs.Select(config => config.vehicleData));
            }
        }

        public VehicleData GetVehicleForCurrentGameState(int currentStreak)
        {
            if (allVehicles.Count == 0) return null;

            // Filter available vehicles based on streak
            var availableVehicles = allVehicles.FindAll(v => v.unlockAtStreak <= currentStreak);

            // Weighted selection (same logic as before)
            float totalWeight = 0;
            foreach (var vehicle in availableVehicles)
            {
                totalWeight += vehicle.spawnWeight;
            }
            float randomPoint = UnityEngine.Random.value * totalWeight;
            for (int i = 0; i < availableVehicles.Count; i++)
            {
                if (randomPoint < availableVehicles[i].spawnWeight)
                {
                    return availableVehicles[i];
                }
                randomPoint -= availableVehicles[i].spawnWeight;
            }
            return availableVehicles[0];
        }

        public VehicleType GetVehicleType(VehicleData vehicleData)
        {
            foreach (var raw in vehicleRawData)
            {
                if (raw.vehicleDataConfigs.Any(config => config.vehicleData == vehicleData))
                {
                    return raw.vehicleType;
                }
            }

            Debug.LogWarning($"VehicleData not found in any raw config group: {vehicleData?.vehicleName}");
            return VehicleType.Normal; // Or a default/fallback type
        }


        public (float minSpeed, float maxSpeed) GetSpeedRange(VehicleType vehicleType)
        {
            foreach (var item in vehicleRawData)
            {
                return item.vehicleType == vehicleType
                    ? (item.minSpeed, item.maxSpeed)
                    : (0, 0);
            }
            Debug.LogWarning($"VehicleType not found: {vehicleType}");
            return (0, 0); // Or a default/fallback value
        }

    }
}