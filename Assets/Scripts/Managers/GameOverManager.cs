using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Managers
{
    public class GameOverManager : Singleton<GameOverManager>
    {
        public int maxLives = 3; // Configurable number of lives
        private int _currentLives; // Current lives

        public int GetCurrentLives()
        {
            return _currentLives;
        }

        private const string GameOverScene = "GameOverScene"; // Name of the game over scene

        void Start()
        {
            _currentLives = maxLives;

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
            _currentLives--;

            if (_currentLives <= 0)
            {
                GameOver();
            }
            else
            {
                Debug.Log("Life lost! Lives remaining: " + _currentLives);
            }
        }

        void GameOver()
        {
            Debug.Log("Game Over!");
            // Reload the scene or show a game over screen
            SceneManager.LoadScene(GameOverScene);
        }
    }
}