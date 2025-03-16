using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Handlers
{
    public class ScoreHandler : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText; // UI reference for score display
        [FormerlySerializedAs("streakManager")] [SerializeField] private StreakHandler streakHandler; // Reference to StreakManager
        
        private int _currentScore = 0; // Player's score

        private void OnEnable()
        {
            if (streakHandler is null)
            {
                streakHandler = FindAnyObjectByType<StreakHandler>();
            }
            GameEventManager.OnSuccessfulJump += IncreaseScore;
            if (scoreText != null)
            {
                scoreText.text = _currentScore.ToString("D2");
            }
        }

        private void OnDisable()
        {
            GameEventManager.OnSuccessfulJump -= IncreaseScore;
        }

        private void IncreaseScore()
        {
            if (streakHandler == null) return; // Safety check

            // Fetch streak multiplier **AFTER** it updates
            float multiplier = streakHandler.GetMultiplier();  
            _currentScore += Mathf.RoundToInt(100 * multiplier); // Apply multiplier
            
            UpdateScoreDisplay();
        }

        private void UpdateScoreDisplay()
        {
            if (scoreText != null)
            {
                scoreText.text = _currentScore.ToString("D2");
            }
        }
    }
}