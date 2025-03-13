using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Game/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    public float InitialSpawnInterval = 2.0f;
    public float MinSpawnInterval = 0.5f;
    public float SpawnIntervalReductionFactor = 0.95f;

    public int InitialVehicleSpeed = 5;
    public int MaxVehicleSpeed = 20;
    public float SpeedIncreaseFactor = 1.05f;
}
