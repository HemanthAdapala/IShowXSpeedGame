using System;
using Configs;
using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Handlers
{
    public class StreakHandler : MonoBehaviour
    {
        public static event Action<int> OnStreakUpdated;  
        public static event Action<float> OnStreakMilestone; 
        public static event Action OnStreakReset; 

        [SerializeField] private StreakConfig streakConfig;
        [SerializeField] private TextConfig textConfig;

        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI streakText;
        [SerializeField] private Slider streakSlider;

        private int _streakCount = 0;
        private float _initialScoreMultiplier = 1.0f;
        private float _scoreMultiplier = 1.0f;
        private float _sliderValue = 0f;

        private void OnEnable()
        {
            GameEventManager.OnSuccessfulJump += IncreaseStreak;
            GameEventManager.OnFailedJump += ResetStreak;
            ResetUIData();
        }

        private void OnDisable()
        {
            GameEventManager.OnSuccessfulJump -= IncreaseStreak;
            GameEventManager.OnFailedJump -= ResetStreak;
        }

        private void IncreaseStreak()
        {
            _streakCount++;
            _scoreMultiplier = streakConfig.GetMultiplier(_streakCount);

            OnStreakUpdated?.Invoke(_streakCount);

            if (!Mathf.Approximately(_scoreMultiplier, _initialScoreMultiplier))
            {
                _initialScoreMultiplier = _scoreMultiplier;
                OnStreakMilestone?.Invoke(_scoreMultiplier);
            }

            SetUIData();
            Debug.Log($"🔥 Streak: {_streakCount} | Multiplier: {_scoreMultiplier}");
        }

        private void SetUIData()
        {
            _sliderValue = Mathf.Clamp(_streakCount / 10f, 0f, 1f);
            streakSlider.DOValue(_sliderValue, 0.5f).SetEase(Ease.OutQuad);
        }

        private void ResetStreak()
        {
            _streakCount = 0;
            _scoreMultiplier = 1.0f;

            OnStreakReset?.Invoke();
            ResetUIData();
            Debug.Log("Streak reset!");
        }

        private void ResetUIData()
        {
            streakSlider.DOValue(0f, 0.3f).SetEase(Ease.OutQuad);
        }

        public float GetMultiplier()
        {
            return _scoreMultiplier;
        }
    }
}
