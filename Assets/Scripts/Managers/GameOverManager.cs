using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    #region Singleton
    public static GameOverManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion



    public int MaxLives = 3; // Configurable number of lives
    private int currentLives; // Current lives

    public int GetCurrentLives()
    {
        return currentLives;
    }

    private string gameOverScene = "GameOverScene"; // Name of the game over scene

    void Start()
    {
        currentLives = MaxLives;

        // Subscribe to the player collision event
        GameEventManager.OnPlayerCollision += HandlePlayerCollision;
    }

    void OnDisable()
    {
        // Unsubscribe from the event
        GameEventManager.OnPlayerCollision -= HandlePlayerCollision;
    }

    void HandlePlayerCollision()
    {
        currentLives--;

        if (currentLives <= 0)
        {
            GameOver();
        }
        else
        {
            Debug.Log("Life lost! Lives remaining: " + currentLives);
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
        // Reload the scene or show a game over screen
        SceneManager.LoadScene(gameOverScene);
    }
}