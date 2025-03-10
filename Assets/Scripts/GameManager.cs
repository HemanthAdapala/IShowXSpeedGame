using UnityEngine;
using System.Collections;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public GameConfig GameConfig; // Reference to the GameConfig ScriptableObject
    public GameObject CubePrefab; // Reference to the cube prefab
    public OffscreenIndicatorManager IndicatorManager; // Reference to the OffscreenIndicatorManager
    public TextMeshProUGUI ScoreText; // Reference to the UI score text
    public float Radius = 5f; // Radius of the circle

    private int score = 0; // Current score

    public event EventHandler<VehicleSpawnedEventArgs> OnVehicleSpawned;
    public class VehicleSpawnedEventArgs : EventArgs
    {
        public CubeController cubeController;
    }

    public event EventHandler OnVehicleDestroyed;

    private void Start()
    {
        // Start the cube spawning loop
        StartCoroutine(SpawnCubes());

        // Initialize the score display
        UpdateScoreDisplay();
    }

    IEnumerator SpawnCubes()
    {
        int currentSpawnedCount = 0;
        while (true) // Infinite loop for continuous spawning
        {
            // Get the current difficulty level based on the score
            GameConfig.DifficultyLevel difficultyLevel = GameConfig.GetDifficultyLevel(score);

            // Calculate the spawn interval and move speed
            float spawnInterval = UnityEngine.Random.Range(difficultyLevel.MinSpawnInterval, difficultyLevel.MaxSpawnInterval);
            float cubeMoveSpeed = difficultyLevel.CubeMoveSpeed;

            // Instantiate the cube at a random position on the circle
            GameObject cube = Instantiate(CubePrefab, GetRandomPositionOnCircle(Radius), Quaternion.identity, transform);
            currentSpawnedCount++;
            cube.gameObject.name = "Cube " + currentSpawnedCount;

            // Set up the cube's movement
            CubeController cubeController = cube.AddComponent<CubeController>();
            cubeController.IndicatorManager = IndicatorManager;
            cubeController.MoveSpeed = cubeMoveSpeed;
            OnVehicleSpawned?.Invoke(this, new VehicleSpawnedEventArgs { cubeController = cubeController });


            // Wait for the next spawn
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    /// <summary>
    /// Generates a random position on a horizontal circle with the specified radius.
    /// </summary>
    /// <param name="radius">The radius of the circle.</param>
    /// <returns>A Vector3 position on the circle in the xz-plane (y=0).</returns>
    private Vector3 GetRandomPositionOnCircle(float radius)
    {
        // Calculate a random angle in radians (more direct approach)
        float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);

        // Calculate the position on the circle using trigonometry
        return new Vector3(
            Mathf.Cos(angle) * radius,
            0f,
            Mathf.Sin(angle) * radius
        );
    }

    // Call this method when the player destroys a cube
    public void IncreaseScore()
    {
        score++;
        UpdateScoreDisplay();
    }

    void UpdateScoreDisplay()
    {
        if (ScoreText != null)
        {
            if (score <= 9)
            {
                ScoreText.text = "Score:  0" + score;
            }
            else
                ScoreText.text = "Score: " + score;
        }
    }

    public void DestroyCurrentTarget()
    {
        OnVehicleDestroyed?.Invoke(this, EventArgs.Empty);
    }
}