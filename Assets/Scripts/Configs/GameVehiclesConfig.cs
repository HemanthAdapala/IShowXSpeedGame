using UnityEngine;

[CreateAssetMenu(fileName = "GameVehiclesConfig", menuName = "Game/GameVehiclesConfig", order = 2)]
public class GameVehiclesConfig : ScriptableObject
{
    public GameObject[] VehiclePrefabs;

    public GameObject GetVehiclePrefabForLevel(int level)
    {
        if (VehiclePrefabs.Length == 0) return null;
        return VehiclePrefabs[Mathf.Min(level, VehiclePrefabs.Length - 1)];
    }
}
