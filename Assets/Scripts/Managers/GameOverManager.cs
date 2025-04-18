using System.Collections;
using Plugins;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameOverManager : Singleton<GameOverManager>
    {
        protected int maxLives = 3; // Configurable number of lives
        protected int _currentLives; // Current lives

        //Testing for immediate game over
        public bool makeGameOver = false; // Flag to prevent multiple game over triggers


        public int GetCurrentLives()
        {
            return _currentLives;
        }

        private const string GameOverScene = "GameOverScene"; // Name of the game over scene

        void Start()
        {
            _currentLives = maxLives;

            // Subscribe to the player collision event
            GameEventManager.OnFailedJump += HandlePlayerCollision;
        }

        void Update()
        {
            // For testing purposes, trigger game over immediately
            if (makeGameOver)
            {
                GameOver();
                makeGameOver = false; // Reset the flag
            }
        }

        void OnDisable()
        {
            // Unsubscribe from the event
            GameEventManager.OnFailedJump -= HandlePlayerCollision;
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
                Debug.Log("💔 Life lost! Lives remaining: " + _currentLives);
            }
        }

        void GameOver()
        {
            Debug.Log("🛑 Game Over!");
            GameManager.Instance.SaveGameSession();
            GameEventManager.TriggerGameEnd();
            StartCoroutine(LoadGameOverScene());
        }

        private IEnumerator LoadGameOverScene()
        {
            yield return new WaitForSeconds(3f);
            SceneLoader.LoadScene("GameOverScene", false);
            SceneLoader.LoadScene("RewardsUIScene", true);
        }
    }
}