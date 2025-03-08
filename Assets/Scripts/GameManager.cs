using UnityEngine;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameConfig GameConfig; // Reference to the GameConfig ScriptableObject
    public GameObject CubePrefab; // Reference to the cube prefab
    public OffscreenIndicatorManager IndicatorManager; // Reference to the OffscreenIndicatorManager
    public TextMeshProUGUI ScoreText; // Reference to the UI score text
    public float Radius = 5f; // Radius of the circle

    private int score = 0; // Current score

    void Start()
    {
        // Start the cube spawning loop
        StartCoroutine(SpawnCubes());

        // Initialize the score display
        UpdateScoreDisplay();
    }

    IEnumerator SpawnCubes()
    {
        while (true) // Infinite loop for continuous spawning
        {
            // Get the current difficulty level based on the score
            GameConfig.DifficultyLevel difficultyLevel = GameConfig.GetDifficultyLevel(score);

            // Calculate the spawn interval and move speed
            float spawnInterval = Random.Range(difficultyLevel.MinSpawnInterval, difficultyLevel.MaxSpawnInterval);
            float cubeMoveSpeed = difficultyLevel.CubeMoveSpeed;

            // Instantiate the cube at a random position on the circle
            GameObject cube = Instantiate(CubePrefab, GetRandomPositionOnCircle(Radius), Quaternion.identity, transform);

            // Set up the cube's movement
            CubeController cubeController = cube.AddComponent<CubeController>();
            cubeController.IndicatorManager = IndicatorManager;
            cubeController.MoveSpeed = cubeMoveSpeed;

            // Wait for the next spawn
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    Vector3 GetRandomPositionOnCircle(float radius)
    {
        // Calculate a random angle in radians
        float angle = Random.Range(0, 360) * Mathf.Deg2Rad;

        // Calculate the position on the circle using trigonometry
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        return new Vector3(x, 0, z);
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
}