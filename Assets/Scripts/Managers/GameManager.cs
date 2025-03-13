using UnityEngine;
using System.Collections;
using TMPro;
using System;
using Controllers;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion

    public GameConfig GameConfig;
    public GameVehiclesConfig gameVehiclesConfig;
    public OffscreenIndicatorManager IndicatorManager;
    public TextMeshProUGUI ScoreText;
    public float Radius = 5f;

    private int score = 0;
    private float currentSpawnInterval;
    private int currentVehicleSpeed;
    private int streak = 0;

    public event EventHandler<VehicleSpawnedEventArgs> OnVehicleSpawned;
    public class VehicleSpawnedEventArgs : EventArgs
    {
        public PrometeoCarController carController;
    }

    private void Start()
    {
        currentSpawnInterval = GameConfig.InitialSpawnInterval;
        currentVehicleSpeed = GameConfig.InitialVehicleSpeed;
        StartCoroutine(SpawnVehicles());
        UpdateScoreDisplay();
    }

    IEnumerator SpawnVehicles()
    {
        while (true)
        {
            SpawnVehicle();
            yield return new WaitForSeconds(currentSpawnInterval);
        }
    }

    private void SpawnVehicle()
    {
        GameObject vehiclePrefab = gameVehiclesConfig.GetVehiclePrefabForLevel(score / 10);

        if (vehiclePrefab == null) return;

        GameObject vehicle = Instantiate(vehiclePrefab, GetRandomPositionOnCircle(Radius), Quaternion.identity, transform);

        // Ensure the PrometeoCarController is assigned and configured
        PrometeoCarController carController = vehicle.GetComponent<PrometeoCarController>();
        if (carController != null)
        {
            carController.maxSpeed = GameConfig.MaxVehicleSpeed;
            carController.accelerationMultiplier = 5;
        }
        
        VehicleController vehicleController = vehicle.GetComponent<VehicleController>();
        if (vehicleController != null)
        {
            vehicleController.offscreenIndicatorManager = IndicatorManager;
        }

        OnVehicleSpawned?.Invoke(this, new VehicleSpawnedEventArgs { carController = carController });
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

    public void IncreaseScore()
    {
        score++;
        streak++;
        AdjustDifficulty();
        UpdateScoreDisplay();
    }

    private void AdjustDifficulty()
    {
        currentSpawnInterval = Mathf.Max(GameConfig.MinSpawnInterval, currentSpawnInterval * GameConfig.SpawnIntervalReductionFactor);
        currentVehicleSpeed = Mathf.Min(GameConfig.MaxVehicleSpeed, Mathf.RoundToInt(currentVehicleSpeed * GameConfig.SpeedIncreaseFactor));
    }

    void UpdateScoreDisplay()
    {
        if (ScoreText != null)
        {
            ScoreText.text = "Score: " + score.ToString("D2");
        }
    }
}
